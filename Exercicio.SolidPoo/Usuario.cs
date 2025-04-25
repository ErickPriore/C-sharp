namespace Exercicio.SolidPoo
{
    namespace BibliotecaApp.Refatorado
    {
        public class Usuario
        {
            public string Nome { get; private set; }
            public int ID { get; private set; }

            public Usuario(string nome, int id)
            {
                Nome = nome;
                ID = id;
            }
        }
    }
}
