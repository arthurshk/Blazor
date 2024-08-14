using Blazor.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Blazor.Services
{
    public class BookService
    {
        private const string FilePath = "books.json";
        private List<Book> books;

        public BookService()
        {
            //initialize with a list.
            books = LoadBooks();
        }
        
        public List<Book> GetBooks()
        {
            //return list of books
            return books;
        }

        public void AddBook(Book book)
        {
            //add another Id to the list of Id's or 1 if it is the first
            book.Id = books.Any() ? books.Max(b => b.Id) + 1 : 1;
            books.Add(book);
            SaveBooks();
        }

        public void UpdateBook(Book updatedBook)
        {
            //book update through the book paramater that is passed.
            var book = books.FirstOrDefault(b => b.Id == updatedBook.Id);
            if (book != null)
            {
                book.Title = updatedBook.Title;
                book.Author = updatedBook.Author;
                SaveBooks();
            }
        }

        public void DeleteBook(int id)
        { 
            //Finds the book and then deletes it based on ID
            var book = books.FirstOrDefault(b => b.Id == id);
            if (book != null)
            {
                books.Remove(book);
                SaveBooks();
            }
        }

        private void SaveBooks()
        {
            //Creates a file after the first operation done on it
            var json = JsonSerializer.Serialize(books);
            File.WriteAllText(FilePath, json);
        }

        private List<Book> LoadBooks()
        {
            //Bring the response back as a list after deserializing the json
            if (File.Exists(FilePath))
            {
                var json = File.ReadAllText(FilePath);
                return JsonSerializer.Deserialize<List<Book>>(json);
            }
            //if no file exists yet just return the default list
            return new List<Book>
            {
                new Book { Id = 1, Title = "Animal Farm", Author = "George Orwell" },
                new Book { Id = 2, Title = "The Picture of Dorian Gray", Author = "Oscar Wilde" },
                new Book { Id = 3, Title = "The Shining", Author = "Stephen King" }
            };
        }
    }
}
