//João Coelho

using AcademiaDoZe.Application.DTOs;
namespace AcademiaDoZe.Application.Interfaces
    public interface IPlanoService
    {

        Task<PlanoDTO> ObterPorIdAsync(int id);
        Task<IEnumerable<PlanoDTO>> ObterTodosAsync();
        Task<IEnumerable<PlanoDTO>> ObterPlanosAtivosAsync();
        Task<PlanoDTO> AdicionarAsync(PlanoDTO planoDto);
        Task<PlanoDTO> AtualizarAsync(PlanoDTO planoDto);
        Task<bool> RemoverAsync(int id);
    }
}