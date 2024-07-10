using System;
using System.Collections.Generic;

namespace BookStorage
{
    internal class Program
    {
        static void Main(string[] args)
        {
            BookStorage library = new BookStorage();
            library.Work();
        }
    }

    class BookStorage
    {
        private List<Book> _books;
        private int _lastBookId;

        public BookStorage()
        {
            _books = CreateBooks();
        }

        public void Work()
        {
            const string CommandShowAllBooks = "1";
            const string CommandAddBook = "2";
            const string CommandDeleteBook = "3";
            const string CommandSearchBook = "4";
            const string CommandExit = "exit";

            bool isRunning = true;

            while (isRunning)
            {
                Console.WriteLine($"Введите {CommandShowAllBooks}, чтобы просмотреть все книги.\n" +
                    $"Введите {CommandAddBook}, чтобы добавить книгу.\n" +
                    $"Введите {CommandDeleteBook}, чтобы удалить книгу.\n" +
                    $"Введите {CommandSearchBook}, чтобы найти книгу.\n" +
                    $"Введите {CommandExit}, чтобы выйти.\n");
                string userInput = Console.ReadLine();

                Console.Clear();

                switch (userInput)
                {
                    case CommandShowAllBooks:
                        ShowAllBooks();
                        break;

                    case CommandAddBook:
                        AddBook();
                        break;

                    case CommandDeleteBook:
                        DeleteBook();
                        break;

                    case CommandSearchBook:
                        SearchBook();
                        break;

                    case CommandExit:
                        isRunning = false;
                        break;

                    default:
                        ShowMessageCommandNotFound();
                        break;
                }

                Console.WriteLine();
            }

            Console.WriteLine("Программа завершена.");
        }

        private void ShowAllBooks()
        {
            foreach (Book book in _books)
                book.ShowStats();
        }

        private List<Book> CreateBooks()
        {
            List<Book> books = new List<Book>();

            books.Add( new Book(GetBookId(), "Анна Каренина", new Author("Толстой", "Лев", "Николаевич"), 2023));
            books.Add( new Book(GetBookId(), "Евгений Онегин", new Author("Пушкин", "Александр", "Сергеевич"), 1950));
            books.Add( new Book(GetBookId(), "Братья Карамазовы", new Author("Достоевсикй", "Фёдор", "Михайлович"), 1984));
            books.Add( new Book(GetBookId(), "Чайка", new Author("Чехов", "Антон", "Павлович"), 2012));
            books.Add( new Book(GetBookId(), "Доктор Живаго", new Author("Пастернак", "Борис", "Леонидович"), 2016));
            books.Add( new Book(GetBookId(), "Война и мир", new Author("Толстой", "Лев", "Николаевич"), 2006));

            return books;
        }

        private void AddBook()
        {
            _books.Add(ReadBook());
        }

        private Book ReadBook()
        {
            int id = GetBookId();
            int yearRelease;

            string name;

            Author author;

            Book book;

            do
            {
                Console.WriteLine("Введите название книги:");
                name = Console.ReadLine();

                author = CreateAuthor();

                yearRelease = ReadYear();

                book = new Book(id, name, author, yearRelease);
            }
            while (IsInputBookCorrect(book));

            return book;
        }

        private bool IsInputBookCorrect(Book book)
        {
            string commandExit = "1";

            Console.WriteLine($"Если данные верны нажмите {commandExit}.");
            book.ShowStats();

            return Console.ReadLine() != commandExit;
        }

        private Author CreateAuthor()
        {
            Console.WriteLine("Введите фамилию:");
            string lastname = Console.ReadLine();

            Console.WriteLine("Введите имя:");
            string firstname = Console.ReadLine();

            Console.WriteLine("Введите отчество:");
            string surname = Console.ReadLine();

            return new Author(lastname, firstname, surname);
        }

        private int ReadYear()
        {
            int minYear = 1900;
            int maxYear = 2024;

            int year;

            do
            {
                Console.WriteLine($"Введите год издания от {minYear} до {maxYear}:");
                year = ReadInt();
            }
            while (IsYearCorrect(year, minYear, maxYear) == false);

            return year;
        }

        private bool IsYearCorrect(int number, int minYear, int maxYear)
        {
            bool isCorrectYear = number >= minYear && number <= maxYear;

            if (isCorrectYear == false)
                Console.WriteLine("Эту книга надо сдать в музей или она ещё не вышла.");

            return isCorrectYear;
        }

        private int ReadInt()
        {
            int number;

            while (int.TryParse(Console.ReadLine(), out number) == false)
                Console.WriteLine("Это не число.");

            return number;
        }

        private void DeleteBook()
        {
            if (_books.Count == 0)
            {
                Console.WriteLine("Библиотека пуста.");
            }
            else
            {
                ShowAllBooks();

                if (TryGetBookById(out Book book))
                    _books.Remove(book);
            }
        }

        private bool TryGetBookById(out Book foundBook)
        {
            foundBook = null;

            Console.WriteLine("Введите идентификатор:");
            int id = ReadInt();

            foreach (var book in _books)
            {
                if (id == book.Id)
                {
                    foundBook = book;
                    return true;
                }
            }

            Console.WriteLine("Это не идентификатор.");

            return false;
        }

        private void SearchBook()
        {
            const string CommandSearchId = "1";
            const string CommandSearchName = "2";
            const string CommandSearchAuthor = "3";
            const string CommandSearchYearRelease = "4";

            Console.WriteLine($"Введите {CommandSearchId}, чтобы найти книгу по id.\n" +
                $"Введите {CommandSearchName}, чтобы найти книгу по названию.\n" +
                $"Введите {CommandSearchAuthor}, чтобы найти книгу по автору.\n" +
                $"Введите {CommandSearchYearRelease}, чтобы найти книгу по году.");
            string userInput = Console.ReadLine();

            switch (userInput)
            {
                case CommandSearchId:
                    SearchId();
                    break;

                case CommandSearchName:
                    SearchName();
                    break;

                case CommandSearchAuthor:
                    SearchAuthor();
                    break;

                case CommandSearchYearRelease:
                    SearchYearRelease();
                    break;

                default:
                    ShowMessageCommandNotFound();
                    break;
            }
        }

        private void SearchId()
        {
            if (TryGetBookById(out Book book))
                book.ShowStats();
        }

        private void SearchName()
        {
            if (TryGetBookByName(out List<Book> books))
                ShowBooks(books);
        }

        private void SearchAuthor()
        {
            if (TryGetBookByAuthor(out List<Book> books))
                ShowBooks(books);
        }

        private void SearchYearRelease()
        {
            if (TryGetYearRelease(out List<Book> books))
                ShowBooks(books);
        }

        private void ShowBooks(List<Book> books)
        {
            foreach (Book book in books)
                book.ShowStats();
        }

        private bool TryGetBookByName(out List<Book> books)
        {
            bool isFound = false;

            books = new List<Book>();

            Console.WriteLine("Введите название книги:");
            string name = Console.ReadLine();

            foreach (var book in _books)
            {
                if (name.ToLower() == book.Name.ToLower())
                {
                    books.Add(book);
                    isFound = true;
                }
            }

            if (isFound == false)
                Console.WriteLine("Книги с таким названием не найдено.");

            return isFound;
        }

        private bool TryGetBookByAuthor(out List<Book> books)
        {
            bool isFound = false;

            books = new List<Book>();

            Console.WriteLine("Введите фамилию автора:");
            string lastname = Console.ReadLine();

            foreach (var book in _books)
            {
                if (lastname.ToLower() == book.Author.Lastname.ToLower())
                {
                    books.Add(book);
                    isFound = true;
                }
            }

            if (isFound == false)
                Console.WriteLine("Книги с таким автором не найдено.");

            return isFound;
        }

        private bool TryGetYearRelease(out List<Book> books)
        {
            bool isFound = false;

            books = new List<Book>();

            int year = ReadYear();

            foreach (var book in _books)
            {
                if (year == book.YearRelease)
                {
                    books.Add(book);
                    isFound = true;
                }
            }

            if (isFound == false)
                Console.WriteLine("Книг с таким годом выпуска не найдено.");

            return isFound;
        }

        private int GetBookId()
        {
            return _lastBookId++;
        }

        private void ShowMessageCommandNotFound()
        {
            Console.WriteLine("Такой команды нет.");
        }
    }

    class Book
    {
        public Book(int id, string name, Author author, int yearRelease)
        {
            Id = id;
            Name = name;
            Author = author;
            YearRelease = yearRelease;
        }

        public string Name { get; private set; }
        public Author Author { get; private set; }
        public int YearRelease { get; private set; }
        public int Id { get; private set; }

        public void ShowStats(string separator = "-")
        {
            Console.WriteLine($"{Id}{separator}{Name}{separator}{Author.GetFullName()}{separator}{YearRelease}");
        }
    }

    class Author
    {
        private string _firstname;
        private string _surname;

        public Author(string lastname, string firstname, string surname)
        {
            Lastname = lastname;
            _firstname = firstname;
            _surname = surname;
        }

        public string Lastname { get; private set; }

        public string GetFullName()
        {
            string separator = " - ";
            string fullName = $"{Lastname}{separator}{_firstname}{separator}{_surname}";

            return fullName;
        }

    }
}
