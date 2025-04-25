namespace Exercicio.SolidPoo
{
    using System.Collections.Generic;

    namespace BibliotecaApp.Refatorado
    {
        public class BibliotecaFactory
        {
            public static ServicoLivro CriarServicoLivro()
            {
                return new ServicoLivro(
                    new LivroRepositorioEmMemoria(),
                    new EmailNotificador());
            }

            public static ServicoUsuario CriarServicoUsuario()
            {
                return new ServicoUsuario(
                    new UsuarioRepositorioEmMemoria(),
                    new NotificadorComposto(new List<INotificador>
                    {
                    new EmailNotificador(),
                    new SMSNotificador()
                    }));
            }

            public static ServicoEmprestimo CriarServicoEmprestimo()
            {
                return new ServicoEmprestimo(
                    new LivroRepositorioEmMemoria(),
                    new UsuarioRepositorioEmMemoria(),
                    new EmprestimoRepositorioEmMemoria(),
                    new CalculadorMultaPadrao(),
                    new NotificadorComposto(new List<INotificador>
                    {
                    new EmailNotificador(),
                    new SMSNotificador()
                    }));
            }
        }
    }
}
