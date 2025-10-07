//João Coelho
using AcademiaDoZe.Application.DTOs;
using AcademiaDoZe.Domain.Entities;
using AcademiaDoZe.Domain.ValueObjects;

namespace AcademiaDoZe.Application.Mappings
{
    public static class MatriculaMappings
    {
        public static MatriculaDTO ToDto(this Matricula matricula)
        {
            return new MatriculaDTO
            {
                Id = matricula.Id,
                AlunoMatricula = matricula.Aluno.ToDto(),
                Plano = matricula.Plano.ToDto(),
                DataInicio = matricula.DataInicio,
                DataFim = matricula.DataFim,
                Objetivo = matricula.Objetivo,
                RestricoesMedicas = matricula.Restricoes.ToApp(),
                ObservacoesRestricoes = matricula.ObservacoesRestricoes,
                LaudoMedico = matricula.LaudoMedico != null ? new ArquivoDTO { Conteudo = matricula.LaudoMedico.Conteudo } : null, 
            };
        }
        public static Matricula ToEntity(this MatriculaDTO matriculaDto)
        {
            return Matricula.Criar(
            matriculaDto.Id,
            matriculaDto.AlunoMatricula.ToEntityMatricula(), 
            matriculaDto.Plano.ToEntity(),
            matriculaDto.DataInicio,
            matriculaDto.DataFim,
            matriculaDto.Objetivo,
            matriculaDto.RestricoesMedicas.ToDomain(),
            matriculaDto.ObservacoesRestricoes!,
            (matriculaDto.LaudoMedico?.Conteudo != null) ? Arquivo.Criar(matriculaDto.LaudoMedico.Conteudo,"png") : null! 
            );
        }
        public static Matricula UpdateFromDto(this Matricula matricula, MatriculaDTO matriculaDto)
        {
            return Matricula.Criar(
            matriculaDto.Id,
            matriculaDto.AlunoMatricula.ToEntityMatricula() ?? matricula.Aluno,
            matriculaDto.Plano != default ? matriculaDto.Plano.ToEntity() : matricula.Plano,
            matriculaDto.DataInicio != default ? matriculaDto.DataInicio : matricula.DataInicio,
            matriculaDto.DataFim != default ? matriculaDto.DataFim : matricula.DataFim,
            matriculaDto.Objetivo ?? matricula.Objetivo,
            matriculaDto.RestricoesMedicas != default ? matriculaDto.RestricoesMedicas.ToDomain() : matricula.Restricoes,
            matriculaDto.ObservacoesRestricoes ?? matricula.ObservacoesRestricoes,
            (matriculaDto.LaudoMedico?.Conteudo != null) ? Arquivo.Criar(matriculaDto.LaudoMedico.Conteudo,"png") : matricula.LaudoMedico 
            );
        }
    }
}