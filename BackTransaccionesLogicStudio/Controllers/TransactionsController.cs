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
    }
}
