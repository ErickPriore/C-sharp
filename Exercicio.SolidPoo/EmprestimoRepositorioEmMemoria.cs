namespace Exercicio.SolidPoo
{
    using System.Collections.Generic;

    namespace BibliotecaApp.Refatorado
    {
        public class EmprestimoRepositorioEmMemoria : IEmprestimoRepositorio
        {
            private readonly List<Emprestimo> _emprestimos = new List<Emprestimo>();

            public void Adicionar(Emprestimo emprestimo)
            {
                _emprestimos.Add(emprestimo);
            }

            public Emprestimo BuscarEmprestimoAberto(string isbn, int usuarioId)
            {
                return _emprestimos.Find(e =>
                    e.Livro.ISBN == isbn &&
                    e.Usuario.ID == usuarioId &&
                    e.DataDevolucaoEfetiva == null);
            }

            public IEnumerable<Emprestimo> BuscarTodos()
            {
                return _emprestimos;
            }
        }
    }
}
