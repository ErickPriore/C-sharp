namespace Exercicio.SolidPoo
{
    using System.Collections.Generic;

    namespace BibliotecaApp.Refatorado
    {
        public class NotificadorComposto : INotificador
        {
            private readonly IEnumerable<INotificador> _notificadores;

            public NotificadorComposto(IEnumerable<INotificador> notificadores)
            {
                _notificadores = notificadores;
            }

            public void Notificar(Usuario usuario, string assunto, string mensagem)
            {
                foreach (var notificador in _notificadores)
                {
                    notificador.Notificar(usuario, assunto, mensagem);
                }
            }
        }
    }
}
