using BackTransaccionesLogicStudio.Models;
using BackTransaccionesLogicStudio.Models.Dtos;
using BackTransaccionesLogicStudio.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Transactions;

namespace BackTransaccionesLogicStudio.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly DbtestLogicStudioContext _ctx;
        private readonly HttpClient _productsApi;
        private readonly int ID_VENTA = 2;

        public TransactionService(DbtestLogicStudioContext ctx,
                              IHttpClientFactory httpFactory) {
            _ctx = ctx;
            _productsApi = httpFactory.CreateClient("ProductsApi");
        } 

        public async Task<bool> Generate(TransaccionDto transaccion)
        {
            
            try
            {
                #region Validación del stock en caso de venta
                foreach (var trans in transaccion.TransaccionDetalles)
                {
                    ProductDto productOld = null;

                    if (transaccion.IdTipoTransaccion == ID_VENTA)
                    {
                        var resp = await _productsApi.GetAsync(
                            $"/api/Products/GetProduct/{trans.IdProducto}");

                        if (resp != null && (int)resp.StatusCode == StatusCodes.Status200OK)
                        {
                            productOld = await resp.Content.ReadFromJsonAsync<ProductDto>();

                            if (productOld != null)
                            {
                                if (productOld.Stock < trans.Cantidad)
                                {
                                    throw new Exception(
                                        $"Err003: No existe el stock necesario de {productOld.Nombre} para completar esta transaccion"
                                        );
                                }
                            }
                            else
                            {
                                throw new Exception(
                                    $"Err002: Error al convertir el producto Id: {trans.IdProducto}"
                                    );
                            }
                        }
                        else
                        {
                            throw new Exception(
                                $"Err001: El producto Id: {trans.IdProducto} no ha podido ser encontrado"
                                );
                        }
                    }
                }
                #endregion

                TransaccionCabecera transaccionCabecera = new TransaccionCabecera
                {
                    Fecha = transaccion.Fecha,
                    IdTipoTransaccion = transaccion.IdTipoTransaccion
                };

                _ctx.TransaccionCabeceras.Add(transaccionCabecera);

                await _ctx.SaveChangesAsync();

                foreach (var trans in transaccion.TransaccionDetalles)
                {
                    ProductDto productOld = null;
                    #region Validacion de stock en caso de venta

                    var resp = await _productsApi.GetAsync(
                            $"/api/Products/GetProduct/{trans.IdProducto}");

                    if (resp != null && (int)resp.StatusCode == StatusCodes.Status200OK)
                    {
                        productOld = await resp.Content.ReadFromJsonAsync<ProductDto>();

                        if (productOld == null)
                        {
                            throw new Exception(
                                "Err002: Error al convertir el producto"
                                );
                        }
                    }
                    else
                    {
                        throw new Exception(
                            "Err001: El producto no ha podido ser encontrado"
                            );
                    }

                    #endregion

                    TransaccionDetalle transaccionDetalle = new TransaccionDetalle
                    {
                        IdTransaccionCabecera = transaccionCabecera.Id,
                        IdProducto = trans.IdProducto,
                        Cantidad = trans.Cantidad,
                        PrecioUnitario = trans.PrecioUnitario,
                        PrecioTotal = trans.PrecioTotal,
                        Detalle = trans.Detalle
                    };

                    _ctx.TransaccionDetalles.Add(transaccionDetalle);
                    await _ctx.SaveChangesAsync();

                    #region Actualizacion del stock

                    var updateDto = new ProductUpdateDto
                    {
                        Stock = transaccion.IdTipoTransaccion == ID_VENTA ? productOld.Stock - trans.Cantidad : productOld.Stock + trans.Cantidad
                    }
                    ;

                    var responseUpdate = await _productsApi.PutAsJsonAsync(
                        $"/api/Products/UpdateProduct/{trans.IdProducto}",updateDto);

                    if (responseUpdate != null && (int)responseUpdate.StatusCode == StatusCodes.Status200OK)
                    {
                        await _ctx.SaveChangesAsync();
                    }
                    else
                    {
                        throw new Exception(
                            $"Err004: El producto Id: {trans.IdProducto} no ha podido ser actualizado en stock"
                            );
                    }

                    #endregion

                }


                return true;
            }
            catch (Exception ex)
            {
                throw;
            }               
            
        }

        public async Task<IEnumerable<TransaccionDto>> GetAllTransactions()
        {
            List<TransaccionDto> transacciones = new List<TransaccionDto>();

            transacciones = await _ctx.TransaccionCabeceras
                             .Include(c => c.TransaccionDetalles)
                             .OrderByDescending(c => c.Fecha)
                             .Select(c => new TransaccionDto
                             {
                                 Id = c.Id,
                                 IdTipoTransaccion = c.IdTipoTransaccion,
                                 Fecha = c.Fecha,
                                 TransaccionDetalles = c.TransaccionDetalles
                                     .Select(d => new DetalleTransaccionDto
                                     {
                                         Id = d.Id,
                                         IdTransaccionCabecera = d.IdTransaccionCabecera,
                                         IdProducto = d.IdProducto,
                                         Cantidad = d.Cantidad,
                                         PrecioUnitario = d.PrecioUnitario,
                                         PrecioTotal = d.PrecioTotal,
                                         Detalle = d.Detalle
                                     })
                                     .ToList()
                             })
                             .ToListAsync();

            return transacciones;
        }

        public async Task<IEnumerable<TipoTransaccionDto>> GetAllTransactionTypes()
        {
            List<TipoTransaccionDto> tipoTransaccions = new List<TipoTransaccionDto> ();

            tipoTransaccions = await _ctx.TipoTransaccions
                                .Where(t => t.Activo)
                                .OrderBy(t => t.Id)
                                .Select(t => new TipoTransaccionDto
                                {
                                    Id = t.Id,
                                    Nombre = t.Nombre
                                }
                                ).ToListAsync();

            return tipoTransaccions;
        }

        public async Task<TransaccionDto> GetTransaccion(int idTransaccion)
        {
            TransaccionDto transaccionDto = new TransaccionDto();

            transaccionDto = await _ctx.TransaccionCabeceras
                             .Include(c => c.TransaccionDetalles)
                             .Where(c => c.Id == idTransaccion)
                             .Select(c => new TransaccionDto
                             {
                                 Id = c.Id,
                                 IdTipoTransaccion = c.IdTipoTransaccion,
                                 Fecha = c.Fecha,
                                 TransaccionDetalles = c.TransaccionDetalles
                                     .Select(d => new DetalleTransaccionDto
                                     {
                                         Id = d.Id,
                                         IdTransaccionCabecera = d.IdTransaccionCabecera,
                                         IdProducto = d.IdProducto,
                                         Cantidad = d.Cantidad,
                                         PrecioUnitario = d.PrecioUnitario,
                                         PrecioTotal = d.PrecioTotal,
                                         Detalle = d.Detalle
                                     })
                                     .ToList()
                             }).FirstOrDefaultAsync();

            return transaccionDto;
        }

        public async Task<TransaccionDto> Update(int idTransaccion, TransaccionDto transaccion)
        {
            try
            {
                #region Reverso de la anterior Transaccion
                var listadoDetallesTransacciones = await _ctx.TransaccionDetalles
                                                    .Where(d => d.IdTransaccionCabecera == idTransaccion)
                                                    .ToListAsync();

                if (!(transaccion.IdTipoTransaccion == ID_VENTA))
                {
                    foreach (var detTransaccion in listadoDetallesTransacciones)
                    {
                        var resp = await _productsApi.GetAsync(
                                $"/api/Products/GetProduct/{detTransaccion.IdProducto}");

                        if (resp != null && (int)resp.StatusCode == StatusCodes.Status200OK)
                        {
                            var productOldReverse = await resp.Content.ReadFromJsonAsync<ProductDto>();

                            if (productOldReverse != null)
                            {
                                if (productOldReverse.Stock < detTransaccion.Cantidad)
                                {
                                    throw new Exception(
                                        $"Err007: No existe el stock necesario de {productOldReverse.Nombre} para completar esta transaccion"
                                        );
                                }
                            }
                            else
                            {
                                throw new Exception(
                                    $"Err006: Error al convertir el producto Id: {detTransaccion.IdProducto}"
                                    );
                            }
                        }
                        else
                        {
                            throw new Exception(
                                $"Err005: El producto Id: {detTransaccion.IdProducto} no ha podido ser encontrado"
                                );
                        }
                    }
                }
                

                foreach (var detTransaccion in listadoDetallesTransacciones)
                {
                    var resp = await _productsApi.GetAsync(
                            $"/api/Products/GetProduct/{detTransaccion.IdProducto}");
                    var productOldReverse = await resp.Content.ReadFromJsonAsync<ProductDto>();

                    var updateDto = new ProductUpdateDto
                    {
                        Stock = transaccion.IdTipoTransaccion == ID_VENTA ? productOldReverse.Stock + detTransaccion.Cantidad : productOldReverse.Stock - detTransaccion.Cantidad
                    };

                    var responseUpdate = await _productsApi.PutAsJsonAsync(
                        $"/api/Products/UpdateProduct/{detTransaccion.IdProducto}", updateDto);

                    _ctx.TransaccionDetalles.Remove(detTransaccion);
                }

                await _ctx.SaveChangesAsync();

                var entityTransaccion = await _ctx.TransaccionCabeceras.FindAsync(idTransaccion);

                _ctx.TransaccionCabeceras.Remove(entityTransaccion);

                await _ctx.SaveChangesAsync();

                #endregion

                #region Generacion de la nueva transaccion
                #region Validación del stock en caso de venta
                foreach (var trans in transaccion.TransaccionDetalles)
                {
                    ProductDto productOld = null;

                    if (transaccion.IdTipoTransaccion == ID_VENTA)
                    {
                        var resp = await _productsApi.GetAsync(
                            $"/api/Products/GetProduct/{trans.IdProducto}");

                        if (resp != null && (int)resp.StatusCode == StatusCodes.Status200OK)
                        {
                            productOld = await resp.Content.ReadFromJsonAsync<ProductDto>();

                            if (productOld != null)
                            {
                                if (productOld.Stock < trans.Cantidad)
                                {
                                    throw new Exception(
                                        $"Err003: No existe el stock necesario de {productOld.Nombre} para completar esta transaccion"
                                        );
                                }
                            }
                            else
                            {
                                throw new Exception(
                                    $"Err002: Error al convertir el producto Id: {trans.IdProducto}"
                                    );
                            }
                        }
                        else
                        {
                            throw new Exception(
                                $"Err001: El producto Id: {trans.IdProducto} no ha podido ser encontrado"
                                );
                        }
                    }
                }
                #endregion

                TransaccionCabecera transaccionCabecera = new TransaccionCabecera
                {
                    Fecha = transaccion.Fecha,
                    IdTipoTransaccion = transaccion.IdTipoTransaccion
                };

                _ctx.TransaccionCabeceras.Add(transaccionCabecera);

                await _ctx.SaveChangesAsync();

                foreach (var trans in transaccion.TransaccionDetalles)
                {
                    ProductDto productOld = null;
                    #region Validacion de stock en caso de venta

                    var resp = await _productsApi.GetAsync(
                            $"/api/Products/GetProduct/{trans.IdProducto}");

                    if (resp != null && (int)resp.StatusCode == StatusCodes.Status200OK)
                    {
                        productOld = await resp.Content.ReadFromJsonAsync<ProductDto>();

                        if (productOld == null)
                        {
                            throw new Exception(
                                "Err002: Error al convertir el producto"
                                );
                        }
                    }
                    else
                    {
                        throw new Exception(
                            "Err001: El producto no ha podido ser encontrado"
                            );
                    }

                    #endregion

                    TransaccionDetalle transaccionDetalle = new TransaccionDetalle
                    {
                        IdTransaccionCabecera = transaccionCabecera.Id,
                        IdProducto = trans.IdProducto,
                        Cantidad = trans.Cantidad,
                        PrecioUnitario = trans.PrecioUnitario,
                        PrecioTotal = trans.PrecioTotal,
                        Detalle = trans.Detalle
                    };

                    _ctx.TransaccionDetalles.Add(transaccionDetalle);
                    await _ctx.SaveChangesAsync();

                    #region Actualizacion del stock

                    var updateDto = new ProductUpdateDto
                    {
                        Stock = transaccion.IdTipoTransaccion == ID_VENTA ? productOld.Stock - trans.Cantidad : productOld.Stock + trans.Cantidad
                    }
                    ;

                    var responseUpdate = await _productsApi.PutAsJsonAsync(
                        $"/api/Products/UpdateProduct/{trans.IdProducto}", updateDto);

                    if (responseUpdate != null && (int)responseUpdate.StatusCode == StatusCodes.Status200OK)
                    {
                        await _ctx.SaveChangesAsync();
                    }
                    else
                    {
                        throw new Exception(
                            $"Err004: El producto Id: {trans.IdProducto} no ha podido ser actualizado en stock"
                            );
                    }

                    #endregion

                }

                #endregion

                return await GetTransaccion(transaccionCabecera.Id);

            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
