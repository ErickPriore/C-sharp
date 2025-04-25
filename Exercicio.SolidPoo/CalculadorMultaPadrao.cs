namespace Exercicio.SolidPoo
{
    using System;

    namespace BibliotecaApp.Refatorado
    {
        public class CalculadorMultaPadrao : ICalculadorMulta
        {
            private readonly double _valorDiario;

            public CalculadorMultaPadrao(double valorDiario = 1.0)
            {
                _valorDiario = valorDiario;
            }

            public double CalcularMulta(Emprestimo emprestimo)
            {
                if (!emprestimo.EstaAtrasado())
                    return 0;

                DateTime dataReferencia = emprestimo.DataDevolucaoEfetiva ?? DateTime.Now;
                TimeSpan atraso = dataReferencia - emprestimo.DataDevolucaoPrevista;
                return Math.Max(0, atraso.Days * _valorDiario);
            }
        }
    }
}
