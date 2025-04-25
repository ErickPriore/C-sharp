# Gabarito: SOLID e Clean Code em C#

## 1. Identificação das Violações (Parte 1)

### Violação 1: Single Responsibility Principle (SRP)
- **Onde ocorre**: Classe `GerenciadorBiblioteca`
- **Descrição**: A classe tem múltiplas responsabilidades - gerenciar livros, usuários, empréstimos, calcular multas e enviar notificações.
- **Justificativa**: Cada classe deve ter apenas uma única responsabilidade. A classe atual faz muitas coisas diferentes, o que dificulta a manutenção e aumenta o acoplamento.

### Violação 2: Dependency Inversion Principle (DIP)
- **Onde ocorre**: Métodos `EnviarEmail` e `EnviarSMS` na classe `GerenciadorBiblioteca`
- **Descrição**: A classe implementa diretamente o envio de notificações em vez de depender de abstrações.
- **Justificativa**: Classes de alto nível não devem depender de detalhes de implementação, mas de abstrações. A forma como as notificações são enviadas deveria ser injetada como dependência.

### Violação 3: Open/Closed Principle (OCP)
- **Onde ocorre**: Método `RealizarDevolucao` na classe `GerenciadorBiblioteca`
- **Descrição**: O cálculo de multa está diretamente implementado no método, tornando difícil estender ou modificar a lógica de cálculo.
- **Justificativa**: O código deve ser aberto para extensão, mas fechado para modificação. Se a política de multas mudar, será necessário modificar este método.

### Violação 4: Interface Segregation Principle (ISP)
- **Onde ocorre**: Ausência de interfaces
- **Descrição**: Não há interfaces definidas, forçando dependências em classes concretas.
- **Justificativa**: Clientes não devem ser forçados a depender de interfaces que não utilizam. Interfaces específicas são melhores que uma única interface genérica.

### Violação 5: Clean Code - Código Duplicado e Mistura de Responsabilidades
- **Onde ocorre**: Métodos `RealizarEmprestimo` e `RealizarDevolucao`
- **Descrição**: Lógica de negócio, notificação e cálculo de multa estão misturados nos mesmos métodos.
- **Justificativa**: Funções devem fazer uma única coisa e fazê-la bem. A lógica de notificação e cálculo deve estar separada da lógica de empréstimo/devolução.

### Violação 6: Clean Code - Tratamento de Erros Inadequado
- **Onde ocorre**: Método `RealizarDevolucao`
- **Descrição**: Retorna -1 para indicar erro.
- **Justificativa**: Códigos mágicos (-1) são difíceis de entender e manter. Usar exceções ou tipos de retorno mais expressivos seria mais adequado.

### Violação 7: Clean Code - Má Organização do Código
- **Onde ocorre**: Em toda a estrutura do projeto
- **Descrição**: Falta de separação em camadas e módulos (repositórios, serviços, etc.).
- **Justificativa**: Código bem organizado separa preocupações em camadas diferentes, facilitando a manutenção e o entendimento.

## 2. Código Refatorado (Parte 2)

Aqui está uma possível refatoração do código que corrige as violações identificadas:

```csharp
using System;
using System.Collections.Generic;

namespace BibliotecaApp.Refatorado
{
    #region Entidades

    public class Livro
    {
        public string Titulo { get; private set; }
        public string Autor { get; private set; }
        public string ISBN { get; private set; }
        public bool Disponivel { get; private set; }

        public Livro(string titulo, string autor, string isbn)
        {
            Titulo = titulo;
            Autor = autor;
            ISBN = isbn;
            Disponivel = true;
        }

        public void Emprestar()
        {
            if (!Disponivel)
                throw new InvalidOperationException("Livro já está emprestado.");
                
            Disponivel = false;
        }

        public void Devolver()
        {
            Disponivel = true;
        }
    }

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

    public class Emprestimo
    {
        public Livro Livro { get; private set; }
        public Usuario Usuario { get; private set; }
        public DateTime DataEmprestimo { get; private set; }
        public DateTime DataDevolucaoPrevista { get; private set; }
        public DateTime? DataDevolucaoEfetiva { get; private set; }

        public Emprestimo(Livro livro, Usuario usuario, int diasEmprestimo)
        {
            Livro = livro;
            Usuario = usuario;
            DataEmprestimo = DateTime.Now;
            DataDevolucaoPrevista = DateTime.Now.AddDays(diasEmprestimo);
        }

        public void Devolver()
        {
            if (DataDevolucaoEfetiva.HasValue)
                throw new InvalidOperationException("Empréstimo já foi devolvido.");
                
            DataDevolucaoEfetiva = DateTime.Now;
            Livro.Devolver();
        }

        public bool EstaAtrasado()
        {
            if (DataDevolucaoEfetiva.HasValue)
                return DataDevolucaoEfetiva.Value > DataDevolucaoPrevista;
                
            return DateTime.Now > DataDevolucaoPrevista;
        }
    }

    #endregion

    #region Interfaces

    public interface ILivroRepositorio
    {
        void Adicionar(Livro livro);
        Livro BuscarPorISBN(string isbn);
        IEnumerable<Livro> BuscarTodos();
    }

    public interface IUsuarioRepositorio
    {
        void Adicionar(Usuario usuario);
        Usuario BuscarPorId(int id);
        IEnumerable<Usuario> BuscarTodos();
    }

    public interface IEmprestimoRepositorio
    {
        void Adicionar(Emprestimo emprestimo);
        Emprestimo BuscarEmprestimoAberto(string isbn, int usuarioId);
        IEnumerable<Emprestimo> BuscarTodos();
    }

    public interface ICalculadorMulta
    {
        double CalcularMulta(Emprestimo emprestimo);
    }

    public interface INotificador
    {
        void Notificar(Usuario usuario, string assunto, string mensagem);
    }

    #endregion

    #region Implementações

    public class LivroRepositorioEmMemoria : ILivroRepositorio
    {
        private readonly List<Livro> _livros = new List<Livro>();

        public void Adicionar(Livro livro)
        {
            _livros.Add(livro);
        }

        public Livro BuscarPorISBN(string isbn)
        {
            return _livros.Find(l => l.ISBN == isbn);
        }

        public IEnumerable<Livro> BuscarTodos()
        {
            return _livros;
        }
    }

    public class UsuarioRepositorioEmMemoria : IUsuarioRepositorio
    {
        private readonly List<Usuario> _usuarios = new List<Usuario>();

        public void Adicionar(Usuario usuario)
        {
            _usuarios.Add(usuario);
        }

        public Usuario BuscarPorId(int id)
        {
            return _usuarios.Find(u => u.ID == id);
        }

        public IEnumerable<Usuario> BuscarTodos()
        {
            return _usuarios;
        }
    }

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

    public class CalculadorMultaPadrao : ICalculadorMulta
    {
        private readonly double _valorDiario;

        public CalculadorMultaPadrao(double valorDiario = 1.0)
        {
            _valorDiario = valorDiario;
        }

        public double CalcularMulta(Emprestimo emprestimo)
        {
            if (!emprestimo.EstaAtrasado())
                return 0;

            DateTime dataReferencia = emprestimo.DataDevolucaoEfetiva ?? DateTime.Now;
            TimeSpan atraso = dataReferencia - emprestimo.DataDevolucaoPrevista;
            return Math.Max(0, atraso.Days * _valorDiario);
        }
    }

    public class EmailNotificador : INotificador
    {
        public void Notificar(Usuario usuario, string assunto, string mensagem)
        {
            // Simulação de envio de e-mail
            Console.WriteLine($"E-mail enviado para {usuario.Nome}. Assunto: {assunto}");
        }
    }

    public class SMSNotificador : INotificador
    {
        public void Notificar(Usuario usuario, string assunto, string mensagem)
        {
            // Simulação de envio de SMS
            Console.WriteLine($"SMS enviado para {usuario.Nome}: {mensagem}");
        }
    }

    public class NotificadorComposto : INotificador
    {
        private readonly IEnumerable<INotificador> _notificadores;

        public NotificadorComposto(IEnumerable<INotificador> notificadores)
        {
            _notificadores = notificadores;
        }

        public void Notificar(Usuario usuario, string assunto, string mensagem)
        {
            foreach (var notificador in _notificadores)
            {
                notificador.Notificar(usuario, assunto, mensagem);
            }
        }
    }

    #endregion

    #region Serviços

    public class ServicoLivro
    {
        private readonly ILivroRepositorio _livroRepositorio;
        private readonly INotificador _notificador;

        public ServicoLivro(ILivroRepositorio livroRepositorio, INotificador notificador)
        {
            _livroRepositorio = livroRepositorio;
            _notificador = notificador;
        }

        public void AdicionarLivro(string titulo, string autor, string isbn)
        {
            var livro = new Livro(titulo, autor, isbn);
            _livroRepositorio.Adicionar(livro);
            Console.WriteLine($"Livro adicionado: {titulo}");
        }

        public IEnumerable<Livro> ListarTodos()
        {
            return _livroRepositorio.BuscarTodos();
        }
    }

    public class ServicoUsuario
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly INotificador _notificador;

        public ServicoUsuario(IUsuarioRepositorio usuarioRepositorio, INotificador notificador)
        {
            _usuarioRepositorio = usuarioRepositorio;
            _notificador = notificador;
        }

        public void AdicionarUsuario(string nome, int id)
        {
            var usuario = new Usuario(nome, id);
            _usuarioRepositorio.Adicionar(usuario);
            
            _notificador.Notificar(usuario, "Bem-vindo à Biblioteca", 
                "Você foi cadastrado em nosso sistema!");
                
            Console.WriteLine($"Usuário adicionado: {nome}");
        }

        public IEnumerable<Usuario> ListarTodos()
        {
            return _usuarioRepositorio.BuscarTodos();
        }
    }

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
                
                return ResultadoDevolucao.Sucesso(multa);
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

    public class ResultadoDevolucao
    {
        public bool Sucesso { get; private set; }
        public double Multa { get; private set; }
        public string MensagemErro { get; private set; }

        private ResultadoDevolucao()
        {
        }

        public static ResultadoDevolucao Sucesso(double multa)
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

    #endregion

    #region Factory para DI

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

    #endregion

    class ProgramRefatorado
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
```

## 3. Explicação das Melhorias

### 1. Single Responsibility Principle (SRP)
- Criamos classes de serviço separadas: `ServicoLivro`, `ServicoUsuario`, e `ServicoEmprestimo`
- Implementamos repositórios para lidar com o armazenamento de dados
- Separamos a notificação em uma interface dedicada com implementações específicas
- Isolamos o cálculo de multa em uma classe específica

### 2. Open/Closed Principle (OCP)
- A lógica de cálculo de multa está encapsulada na interface `ICalculadorMulta`
- Novas estratégias de cálculo podem ser implementadas sem alterar o código existente
- O sistema de notificação pode ser estendido com novos tipos de notificadores

### 3. Liskov Substitution Principle (LSP)
- Todas as implementações de repositórios podem ser substituídas sem afetar o comportamento do sistema
- Todas as implementações de notificadores seguem o mesmo contrato e podem ser substituídas entre si

### 4. Interface Segregation Principle (ISP)
- Interfaces pequenas e focadas: `ILivroRepositorio`, `IUsuarioRepositorio`, `IEmprestimoRepositorio`
- Interface `INotificador` com um único método para notificação
- Interface `ICalculadorMulta` com um único método para cálculo

### 5. Dependency Inversion Principle (DIP)
- Serviços dependem de abstrações (interfaces) e não de implementações concretas
- Implementamos uma `BibliotecaFactory` para injeção de dependências
- Classes de alto nível (serviços) estão desacopladas das implementações de baixo nível

### 6. Clean Code - Tratamento de Erros Adequado
- Substituímos o código mágico (-1) por uma classe `ResultadoDevolucao` que encapsula o resultado
- Utilizamos exceções com try/catch para tratamento de erros
- Mensagens de erro significativas e específicas

### 7. Clean Code - Melhor Organização
- Código organizado em regiões (#region) para facilitar a navegação
- Separação clara entre entidades, repositórios, serviços e fábricas
- Nomes significativos e coerentes

## 4. Conclusão

O código refatorado agora segue os princípios SOLID e boas práticas de Clean Code. As principais melhorias são:

1. **Separação de responsabilidades**: Cada classe tem um propósito único e bem definido
2. **Flexibilidade**: O sistema pode ser facilmente estendido com novas funcionalidades
3. **Testabilidade**: A injeção de dependências facilita a criação de testes unitários
4. **Manutenção**: O código é mais fácil de entender e modificar
5. **Robustez**: Melhor tratamento de erros e situações excepcionais

Essas melhorias resultam em um código de maior qualidade, mais fácil de manter e expandir no futuro, demonstrando claramente a aplicação dos princípios SOLID e Clean Code em um sistema real.