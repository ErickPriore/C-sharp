namespace Exercicio.SolidPoo
{
    using System;

    namespace BibliotecaApp.Refatorado
    {
        public class Livro
        {
            public string Titulo { get; private set; }
            public string Autor { get; private set; }
            public string ISBN { get; private set; }
            public bool Disponivel { get; private set; }

            public Livro(string titulo, string autor, string isbn)
            {
                Titulo = titulo;
                Autor = autor;
                ISBN = isbn;
                Disponivel = true;
            }

            public void Emprestar()
            {
                if (!Disponivel)
                    throw new InvalidOperationException("Livro já está emprestado.");

                Disponivel = false;
            }

            public void Devolver()
            {
                Disponivel = true;
            }
        }
    }
}
