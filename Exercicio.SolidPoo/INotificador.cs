namespace Exercicio.SolidPoo
{
    namespace BibliotecaApp.Refatorado
    {
        public interface INotificador
        {
            void Notificar(Usuario usuario, string assunto, string mensagem);
        }
    }
}
