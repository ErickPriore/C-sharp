namespace Exercicio.SolidPoo
{
    using System;

    namespace BibliotecaApp.Refatorado
    {
        public class Emprestimo
        {
            public Livro Livro { get; private set; }
            public Usuario Usuario { get; private set; }
            public DateTime DataEmprestimo { get; private set; }
            public DateTime DataDevolucaoPrevista { get; private set; }
            public DateTime? DataDevolucaoEfetiva { get; private set; }

            public Emprestimo(Livro livro, Usuario usuario, int diasEmprestimo)
            {
                Livro = livro;
                Usuario = usuario;
                DataEmprestimo = DateTime.Now;
                DataDevolucaoPrevista = DateTime.Now.AddDays(diasEmprestimo);
            }

            public void Devolver()
            {
                if (DataDevolucaoEfetiva.HasValue)
                    throw new InvalidOperationException("Empréstimo já foi devolvido.");

                DataDevolucaoEfetiva = DateTime.Now;
                Livro.Devolver();
            }

            public bool EstaAtrasado()
            {
                if (DataDevolucaoEfetiva.HasValue)
                    return DataDevolucaoEfetiva.Value > DataDevolucaoPrevista;

                return DateTime.Now > DataDevolucaoPrevista;
            }
        }
    }
}
