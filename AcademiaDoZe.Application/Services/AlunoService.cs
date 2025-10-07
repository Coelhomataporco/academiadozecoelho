//João Coelho

using AcademiaDoZe.Application.DTOs;
using AcademiaDoZe.Application.Interfaces;
using AcademiaDoZe.Application.Mappings;
using AcademiaDoZe.Domain.Repositories;

namespace AcademiaDoZe.Application.Services
{
    public class AlunoService : IAlunoService
    {
        private readonly Func<IAlunoRepository> _repoFactory;
        public AlunoService(Func<IAlunoRepository> repoFactory)
        {
            _repoFactory = repoFactory ?? throw new ArgumentNullException(nameof(repoFactory));
        }

        public async Task<AlunoDTO> AdicionarAsync(AlunoDTO alunoDto)
        {
            
            if (await _repoFactory().CpfJaExiste(alunoDto.Cpf))

            {
                throw new InvalidOperationException($"Já existe um colaborador cadastrado com o CPF {alunoDto.Cpf}.");
            }
            var colaborador = alunoDto.ToEntity();
            await _repoFactory().Adicionar(colaborador);
            return colaborador.ToDto();
        }

        public async Task<AlunoDTO> AtualizarAsync(AlunoDTO alunoDto)
        {

            var alunoExistente = await _repoFactory().ObterPorId(alunoDto.Id) ?? throw new KeyNotFoundException($"Colaborador ID {alunoDto.Id} não encontrado.");


            if (await _repoFactory().CpfJaExiste(alunoDto.Cpf, alunoDto.Id))

            {
                throw new InvalidOperationException($"Já existe outro colaborador cadastrado com o CPF {alunoDto.Cpf}.");
            }
            var alunoAtualizado = alunoExistente.UpdateFromDto(alunoDto);
            await _repoFactory().Atualizar(alunoAtualizado);
            return alunoAtualizado.ToDto();
        }

        public async Task<bool> CpfJaExisteAsync(string cpf, int? id = null)
        {
            return await _repoFactory().CpfJaExiste(cpf, id);
        }

        public async Task<AlunoDTO> ObterPorCpfAsync(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf))
                throw new ArgumentException("CPF não pode ser vazio.", nameof(cpf));
            cpf = new string([.. cpf.Where(char.IsDigit)]);
            var aluno = await _repoFactory().ObterPorCpf(cpf);
            return (aluno != null) ? aluno.ToDto() : null!;
        }

        public async Task<AlunoDTO> ObterPorIdAsync(int id)
        {
            var aluno = await _repoFactory().ObterPorId(id);
            return (aluno != null) ? aluno.ToDto() : null!;
        }

        public async Task<IEnumerable<AlunoDTO>> ObterTodosAsync()
        {
            var alunos = await _repoFactory().ObterTodos();
            return [.. alunos.Select(c => c.ToDto())];
        }

        public async Task<bool> RemoverAsync(int id)
        {
            var aluno = await _repoFactory().ObterPorId(id);

            if (aluno == null)

            {
                return false;
            }
            await _repoFactory().Remover(id);

            return true;
        }
    }
}
