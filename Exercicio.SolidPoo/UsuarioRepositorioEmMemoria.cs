namespace Exercicio.SolidPoo
{
    using System.Collections.Generic;

    namespace BibliotecaApp.Refatorado
    {
        public class UsuarioRepositorioEmMemoria : IUsuarioRepositorio
        {
            private readonly List<Usuario> _usuarios = new List<Usuario>();

            public void Adicionar(Usuario usuario)
            {
                _usuarios.Add(usuario);
            }

            public Usuario BuscarPorId(int id)
            {
                return _usuarios.Find(u => u.ID == id);
            }

            public IEnumerable<Usuario> BuscarTodos()
            {
                return _usuarios;
            }
        }
    }
}
