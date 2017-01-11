using Recommendations.Models;
using System.Collections.Generic;

namespace Recommendations.Interfaces
{
    public interface IBooksRepository
    {
        IEnumerable<Book> GetBooks();
    }
}
