namespace Exercicio.SolidPoo
{
    namespace BibliotecaApp.Refatorado
    {
        public class ResultadoDevolucao
        {
            public bool Sucesso { get; private set; }
            public double Multa { get; private set; }
            public string MensagemErro { get; private set; }

            private ResultadoDevolucao()
            {
            }

            public static ResultadoDevolucao ResultadoSucesso(double multa)
            {
                return new ResultadoDevolucao
                {
                    Sucesso = true,
                    Multa = multa
                };
            }

            public static ResultadoDevolucao EmprestimoNaoEncontrado()
            {
                return new ResultadoDevolucao
                {
                    Sucesso = false,
                    MensagemErro = "Empréstimo não encontrado."
                };
            }

            public static ResultadoDevolucao Erro(string mensagem)
            {
                return new ResultadoDevolucao
                {
                    Sucesso = false,
                    MensagemErro = mensagem
                };
            }
        }
    }
}
