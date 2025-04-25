namespace Exercicio.SolidPoo
{
    using System.Collections.Generic;

    namespace BibliotecaApp.Refatorado
    {
        public interface IEmprestimoRepositorio
        {
            void Adicionar(Emprestimo emprestimo);
            Emprestimo BuscarEmprestimoAberto(string isbn, int usuarioId);
            IEnumerable<Emprestimo> BuscarTodos();
        }
    }
}
