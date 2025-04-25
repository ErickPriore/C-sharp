namespace Exercicio.SolidPoo
{
    using System;

    namespace BibliotecaApp.Refatorado
    {
        public class SMSNotificador : INotificador
        {
            public void Notificar(Usuario usuario, string assunto, string mensagem)
            {
                // Simulação de envio de SMS
                Console.WriteLine($"SMS enviado para {usuario.Nome}: {mensagem}");
            }
        }
    }
}
