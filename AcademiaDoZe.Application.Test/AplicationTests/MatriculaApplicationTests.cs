//João Coelho

using AcademiaDoZe.Application.Tests;
using AcademiaDoZe.Application.DTOs;
using AcademiaDoZe.Application.Enums;
using AcademiaDoZe.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace AcademiaDoZe._Application.Tests.AplicationTests
{
    public class MatriculaApplicationTests
    {
        const string connectionString = "Server=localhost,1433;Database=db_academia_do_ze;User Id=sa;Password=abcBolinhas62;TrustServerCertificate=True;Encrypt=True;"; // Substitua pela sua string de conexão de teste
        const EAppDatabaseType databaseType = EAppDatabaseType.SqlServer;

        [Fact(Timeout = 60000)]
        public async Task MatriculaService_Integracao_Adicionar_Obter_Atualizar_Remover()
        {
            var services = DependencyInjection.ConfigureServices(connectionString, databaseType);
            var provider = DependencyInjection.BuildServiceProvider(services);

            var matriculaService = provider.GetRequiredService<IMatriculaService>();
            var alunoService = provider.GetRequiredService<IAlunoService>();
            var logradouroService = provider.GetRequiredService<ILogradouroService>();
            var planoService = provider.GetRequiredService<IPlanoService>();

            var aluno = await alunoService.ObterPorIdAsync(1);
            var plano = await planoService.ObterPorIdAsync(1008);
            var cpfUnico = new Random().Next(100000000, 999999999).ToString("D11");
            var cepUnico = new Random().Next(99500000, 99599999).ToString();

            MatriculaDTO? matriculaCriada = null;

            try
            {
                var matriculaDto = new MatriculaDTO
                {
                    AlunoMatricula = aluno,
                    Plano = plano,
                    DataInicio = DateOnly.FromDateTime(DateTime.Now),
                    DataFim = DateOnly.FromDateTime(DateTime.Now.AddYears(1)),
                    Objetivo = "Hipertrofia",
                    RestricoesMedicas = EAppMatriculaRestricoes.None,
                };

                matriculaCriada = await matriculaService.AdicionarAsync(matriculaDto);
                Assert.NotNull(matriculaCriada);
                Assert.True(matriculaCriada.Id > 0);
                Assert.Equal("Hipertrofia", matriculaCriada.Objetivo);

                var obtida = await matriculaService.ObterPorIdAsync(matriculaCriada.Id);
                Assert.NotNull(obtida);
                Assert.Equal(aluno.Id, obtida.AlunoMatricula.Id);
                Assert.Equal(plano.Id, obtida.Plano.Id);

                var atualizarDto = obtida; 
                atualizarDto.Objetivo = "Definição Muscular";
                atualizarDto.ObservacoesRestricoes = "obs_restricao";

                var atualizada = await matriculaService.AtualizarAsync(atualizarDto);
                Assert.NotNull(atualizada);
                Assert.Equal("Definição Muscular", atualizada.Objetivo);
                Assert.Equal("Nenhuma observação.", atualizada.ObservacoesRestricoes);

                var removido = await matriculaService.RemoverAsync(matriculaCriada.Id);
                Assert.True(removido);

                var aposRemocao = await matriculaService.ObterPorIdAsync(matriculaCriada.Id);
                Assert.Null(aposRemocao);
            }
            finally
            {
                if (matriculaCriada is not null)
                {
                    try { await matriculaService.RemoverAsync(matriculaCriada.Id); } catch { }
                }
            }
        }
    }
}