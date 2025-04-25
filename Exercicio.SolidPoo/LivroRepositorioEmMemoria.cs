namespace Exercicio.SolidPoo
{
    using System.Collections.Generic;

    namespace BibliotecaApp.Refatorado
    {
        public class LivroRepositorioEmMemoria : ILivroRepositorio
        {
            private readonly List<Livro> _livros = new List<Livro>();

            public void Adicionar(Livro livro)
            {
                _livros.Add(livro);
            }

            public Livro BuscarPorISBN(string isbn)
            {
                return _livros.Find(l => l.ISBN == isbn);
            }

            public IEnumerable<Livro> BuscarTodos()
            {
                return _livros;
            }
        }
    }
}
