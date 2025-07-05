using BackTransaccionesLogicStudio.Models.Dtos;

namespace BackTransaccionesLogicStudio.Services.Interfaces
{
    public interface ITransactionService
    {
        Task<IEnumerable<TipoTransaccionDto>> GetAllTransactionTypes();

        Task<bool> Generate(TransaccionDto transaccion);

        Task<IEnumerable<TransaccionDto>> GetAllTransactions();

        Task<TransaccionDto> Update(int idTransaccion, TransaccionDto transaccion);

        Task<TransaccionDto> GetTransaccion(int idTransaccion);
    }
}
