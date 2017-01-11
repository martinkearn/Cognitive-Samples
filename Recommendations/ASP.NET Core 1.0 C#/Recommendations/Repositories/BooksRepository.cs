using Microsoft.AspNetCore.Hosting;
using Recommendations.Interfaces;
using Recommendations.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recommendations.Repositories
{
    public class BooksRepository : IBooksRepository
    {
        private readonly IEnumerable<Book> _books;

        public BooksRepository(IHostingEnvironment environment)
        {
            var books = new List<Book>();
            var rootPath = environment.ContentRootPath;
            var storeFilePath = rootPath + "/bookscatalog.txt";
            using (var fileStream = new FileStream(storeFilePath, FileMode.Open))
            {
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                {
                    string line;
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        var cells = line.Split(',');
                        var book = new Book()
                        {
                            Id = cells[0],
                            Title = cells[1],
                            Type = cells[2],
                            Author = cells[3].Substring(cells[3].IndexOf('=') + 1),
                            Publisher = cells[4].Substring(cells[4].IndexOf('=') + 1),
                            Year = cells[5].Substring(cells[5].IndexOf('=') + 1)
                        };
                        books.Add(book);
                    }
                }
            }
            _books = books.AsEnumerable();
        }

        public IEnumerable<Book> GetBooks()
        {
            return _books;
        }
    }
}
