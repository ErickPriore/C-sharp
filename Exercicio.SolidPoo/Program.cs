namespace Exercicio.SolidPoo
{
    using System;

    namespace BibliotecaApp.Refatorado
    {
        public class Program
        {
            static void Main(string[] args)
            {
                // Criar serviços
                var servicoLivro = BibliotecaFactory.CriarServicoLivro();
                var servicoUsuario = BibliotecaFactory.CriarServicoUsuario();
                var servicoEmprestimo = BibliotecaFactory.CriarServicoEmprestimo();

                // Adicionar livros
                servicoLivro.AdicionarLivro("Clean Code", "Robert C. Martin", "978-0132350884");
                servicoLivro.AdicionarLivro("Design Patterns", "Erich Gamma", "978-0201633610");

                // Adicionar usuários
                servicoUsuario.AdicionarUsuario("João Silva", 1);
                servicoUsuario.AdicionarUsuario("Maria Oliveira", 2);

                // Realizar empréstimo
                servicoEmprestimo.RealizarEmprestimo(1, "978-0132350884", 7);

                // Realizar devolução (com atraso simulado)
                var resultado = servicoEmprestimo.RealizarDevolucao("978-0132350884", 1);

                if (resultado.Sucesso)
                    Console.WriteLine($"Multa por atraso: R$ {resultado.Multa}");
                else
                    Console.WriteLine($"Erro: {resultado.MensagemErro}");

                Console.ReadLine();
            }
        }
    }
}
