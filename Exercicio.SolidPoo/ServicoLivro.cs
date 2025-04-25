namespace Exercicio.SolidPoo
{
    using System;
    using System.Collections.Generic;

    namespace BibliotecaApp.Refatorado
    {
        public class ServicoLivro
        {
            private readonly ILivroRepositorio _livroRepositorio;
            private readonly INotificador _notificador;

            public ServicoLivro(ILivroRepositorio livroRepositorio, INotificador notificador)
            {
                _livroRepositorio = livroRepositorio;
                _notificador = notificador;
            }

            public void AdicionarLivro(string titulo, string autor, string isbn)
            {
                var livro = new Livro(titulo, autor, isbn);
                _livroRepositorio.Adicionar(livro);
                Console.WriteLine($"Livro adicionado: {titulo}");
            }

            public IEnumerable<Livro> ListarTodos()
            {
                return _livroRepositorio.BuscarTodos();
            }
        }
    }
}
