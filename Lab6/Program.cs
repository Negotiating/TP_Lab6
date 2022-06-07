using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab6
{
    internal class Program
    {
        
        interface ILibrary
        {
            void AddBook(Book book);
            void RemoveBook(int index);
            Book InfoByName(string name);
            List<Book> AllBooksByAuthor(Author author);
            void CurrentListOfBooks();
        }
        class Library : ILibrary
        {
            private List<Book> books;
            public List<Book> Books { get => books; }
            public Library(List<Book> _book)
            {
                books = _book;
            }

            public void AddBook(Book book) {
                infoEvent.Invoke("AddBook", DateTime.Now);
                books.Add(book);
                Event.Invoke($"В библиотеку добавлена книга {book}");
            }

            public void RemoveBook(int index) {
                infoEvent.Invoke("RemoveBook", DateTime.Now);
                books.RemoveAt(index);
                Event.Invoke($"Удалена книга под номером {index}");
            }
                
            public Book InfoByName(string name) =>
                books.FirstOrDefault(x => x.Name == name);
            public List<Book> AllBooksByAuthor(Author author) =>
                books.Where(x => x.Author == author).ToList();

            public void CurrentListOfBooks()
            {
                infoEvent.Invoke("CurrentListOfBooks", DateTime.Now);
                Console.WriteLine("\nТекущий список книг в библиотеке: \n");
                foreach (var book in Books)
                {
                    Console.WriteLine(book);
                    Console.WriteLine();
                }
            }

            public delegate void Message(string message);
            public event Message Event;

            public delegate void Info(string met, DateTime t);
            public event Info infoEvent;

        }
        public struct Author
        {
            private string lastname;
            private string name;

            public Author(string _lastname, string _name)
            {
                lastname = _lastname;
                name = _name;
            }

            public string Lastname { get => lastname; }
            public string Name { get => name; }

            public override string ToString() => $"{Lastname} {Name}";
            public static bool operator ==(Author a1, Author a2) => a1.Lastname == a2.Lastname && a1.Name == a2.Name;
            public static bool operator !=(Author a1, Author a2) => !(a1 == a2);
        }


        class Book
        {
            private Author author;
            private string name;
            public string Name { get => name; }
            public Author Author { get => author; }

            public Book(Author _author, string _name)
            {
                author = _author;
                name = _name;
            }
            public override string ToString()
            {
                return $"{Author} \"{Name}\"";
            }
        }
        
        static void Main(string[] args)
        {
            var books = new List<Book>()
            {
                 new Book(new Author("Леру", "Гастон"), "Призрак оперы"),
                 new Book(new Author("Остин", "Джейн"), "Гордость и предубеждение")
            };
            var library = new Library(books);

            library.Event += DisplayMessage;
            library.infoEvent += InfoMessage;
            
            library.CurrentListOfBooks();

            Console.Write("Введите название книги: ");
            string newBookName = Console.ReadLine();
            Console.Write("Введите фамилию и имя автора: ");
            var newAutor = Console.ReadLine().Split(' ').ToArray();

            library.AddBook(new Book(new Author(newAutor[0], newAutor[1]), newBookName));
            library.CurrentListOfBooks();

            Console.WriteLine($"Книги автора {new Author(newAutor[0], newAutor[1])}\n");
            foreach (var book in library.AllBooksByAuthor(new Author(newAutor[0], newAutor[1])))
                Console.WriteLine(book);

            library.RemoveBook(0);
            Console.WriteLine("Список книг после удаления 0 элемента из списка");
            library.CurrentListOfBooks();

            Console.ReadLine();
        }
        static void DisplayMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n"+message+"\n");
            Console.ResetColor();
        }

        static void InfoMessage(string method, DateTime time)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"Вызван метод {method}, время вызова функции: {time}");
            Console.ResetColor();
        }
    }
}
