using BackTransaccionesLogicStudio.Models;
using BackTransaccionesLogicStudio.Models.Dtos;
using BackTransaccionesLogicStudio.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BackTransaccionesLogicStudio.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionsController: ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionsController(ITransactionService transactionService) => _transactionService = transactionService;

        /// <summary>
        /// Registra una transacción (venta o compra) y actualiza el stock en el microservicio de productos.
        /// </summary>
        [HttpPost("GenerateTransaction")]
        public async Task<IActionResult> Generate([FromBody] TransaccionDto dto)
        {
            try
            {
                await _transactionService.Generate(dto);
                return Ok(new { message = "Transacción registrada correctamente" });
            }
            catch (Exception ex) {
                return BadRequest(new { message = ex.Message });
            }

        }

        [HttpGet("GetTransactionList")]
        public async Task<ActionResult<IEnumerable<TransaccionDto>>> GetTransactionList()
        {
            var transactions = await _transactionService.GetAllTransactions();

            return transactions.Count() > 0 ? Ok(transactions) : NoContent();
        }

        [HttpGet("GetTransactionTypes")]
        public async Task<ActionResult<IEnumerable<TipoTransaccionDto>>> GetTransactionTypes()
        {
            var transactionsTypes = await _transactionService.GetAllTransactionTypes();

            return transactionsTypes.Count() > 0 ? Ok(transactionsTypes) : NoContent();
        }

        [HttpPut("UpdateTransaction/{id:int}")]
        public async Task<ActionResult<TransaccionDto>> UpdateTransaction(int id, TransaccionDto dto)
        {
            var updated = await _transactionService.Update(id, dto);

            return updated is null ? NotFound() : Ok(updated);
        }

        [HttpGet("GetTransaction/{id:int}")]
        public async Task<ActionResult<TransaccionDto>> GetById(int id)
        {
            var transaccion = await _transactionService.GetTransaccion(id);

            return transaccion is null ? NotFound() : Ok(transaccion);
        }
    }
}
