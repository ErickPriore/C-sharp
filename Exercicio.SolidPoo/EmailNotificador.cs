namespace Exercicio.SolidPoo
{
    using System;

    namespace BibliotecaApp.Refatorado
    {
        public class EmailNotificador : INotificador
        {
            public void Notificar(Usuario usuario, string assunto, string mensagem)
            {
                // Simulação de envio de e-mail
                Console.WriteLine($"E-mail enviado para {usuario.Nome}. Assunto: {assunto}");
            }
        }
    }
}
