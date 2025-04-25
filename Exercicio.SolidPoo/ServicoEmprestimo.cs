namespace Exercicio.SolidPoo
{
    using System;
    using System.Collections.Generic;

    namespace BibliotecaApp.Refatorado
    {
        public class ServicoEmprestimo
        {
            private readonly ILivroRepositorio _livroRepositorio;
            private readonly IUsuarioRepositorio _usuarioRepositorio;
            private readonly IEmprestimoRepositorio _emprestimoRepositorio;
            private readonly ICalculadorMulta _calculadorMulta;
            private readonly INotificador _notificador;

            public ServicoEmprestimo(
                ILivroRepositorio livroRepositorio,
                IUsuarioRepositorio usuarioRepositorio,
                IEmprestimoRepositorio emprestimoRepositorio,
                ICalculadorMulta calculadorMulta,
                INotificador notificador)
            {
                _livroRepositorio = livroRepositorio;
                _usuarioRepositorio = usuarioRepositorio;
                _emprestimoRepositorio = emprestimoRepositorio;
                _calculadorMulta = calculadorMulta;
                _notificador = notificador;
            }

            public bool RealizarEmprestimo(int usuarioId, string isbn, int diasEmprestimo)
            {
                var livro = _livroRepositorio.BuscarPorISBN(isbn);
                var usuario = _usuarioRepositorio.BuscarPorId(usuarioId);

                if (livro == null || usuario == null || !livro.Disponivel)
                    return false;

                try
                {
                    livro.Emprestar();
                    var emprestimo = new Emprestimo(livro, usuario, diasEmprestimo);
                    _emprestimoRepositorio.Adicionar(emprestimo);

                    _notificador.Notificar(usuario, "Empréstimo Realizado",
                        $"Você pegou emprestado o livro: {livro.Titulo}");

                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao realizar empréstimo: {ex.Message}");
                    return false;
                }
            }

            public ResultadoDevolucao RealizarDevolucao(string isbn, int usuarioId)
            {
                var emprestimo = _emprestimoRepositorio.BuscarEmprestimoAberto(isbn, usuarioId);

                if (emprestimo == null)
                    return ResultadoDevolucao.EmprestimoNaoEncontrado();

                try
                {
                    emprestimo.Devolver();

                    double multa = _calculadorMulta.CalcularMulta(emprestimo);

                    if (multa > 0)
                    {
                        _notificador.Notificar(emprestimo.Usuario, "Multa por Atraso",
                            $"Você tem uma multa de R$ {multa}");
                    }

                    return ResultadoDevolucao.ResultadoSucesso(multa);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao realizar devolução: {ex.Message}");
                    return ResultadoDevolucao.Erro(ex.Message);
                }
            }

            public IEnumerable<Emprestimo> ListarTodos()
            {
                return _emprestimoRepositorio.BuscarTodos();
            }
        }
    }
}
