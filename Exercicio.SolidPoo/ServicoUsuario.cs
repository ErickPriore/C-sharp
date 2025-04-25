namespace Exercicio.SolidPoo
{
    using System;
    using System.Collections.Generic;

    namespace BibliotecaApp.Refatorado
    {
        public class ServicoUsuario
        {
            private readonly IUsuarioRepositorio _usuarioRepositorio;
            private readonly INotificador _notificador;

            public ServicoUsuario(IUsuarioRepositorio usuarioRepositorio, INotificador notificador)
            {
                _usuarioRepositorio = usuarioRepositorio;
                _notificador = notificador;
            }

            public void AdicionarUsuario(string nome, int id)
            {
                var usuario = new Usuario(nome, id);
                _usuarioRepositorio.Adicionar(usuario);

                _notificador.Notificar(usuario, "Bem-vindo à Biblioteca",
                    "Você foi cadastrado em nosso sistema!");

                Console.WriteLine($"Usuário adicionado: {nome}");
            }

            public IEnumerable<Usuario> ListarTodos()
            {
                return _usuarioRepositorio.BuscarTodos();
            }
        }
    }
}
