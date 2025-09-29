// Joao Vitor Coelho de Souza

namespace AcademiaDoZe.Application.DTOs
{
    public class ArquivoDTO
    {
        /// <summary>
        /// Conteúdo binário do arquivo.
        /// </summary>
        public byte[]? Conteudo { get; set; }

        /// <summary>
        /// Tipo MIME do arquivo (ex.: image/png, application/pdf).
        /// </summary>
        public string? ContentType { get; set; }
    }
}
