namespace Exercicio.SolidPoo
{
    using System.Collections.Generic;

    namespace BibliotecaApp.Refatorado
    {
        public interface IUsuarioRepositorio
        {
            void Adicionar(Usuario usuario);
            Usuario BuscarPorId(int id);
            IEnumerable<Usuario> BuscarTodos();
        }
    }
}
