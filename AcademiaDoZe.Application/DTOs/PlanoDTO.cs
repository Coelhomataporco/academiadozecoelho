//João Coelho

namespace AcademiaDoZe.Application.DTOs
{
    public class PlanoDTO
    {
        public int Id { get; set; }
        public required string Tipo { get; set; }
        public required string Descricao { get; set; }
        public required decimal Preco { get; set; }
        public required int DuracaoEmDias { get; set; }
        public required bool Ativo { get; set; }
    }
}
