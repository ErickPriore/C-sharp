namespace Exercicio.SolidPoo
{
    using System.Collections.Generic;

    namespace BibliotecaApp.Refatorado
    {
        public interface ILivroRepositorio
        {
            void Adicionar(Livro livro);
            Livro BuscarPorISBN(string isbn);
            IEnumerable<Livro> BuscarTodos();
        }
    }
}
