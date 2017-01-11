using Recommendations.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recommendations.Interfaces
{
    public interface IBooksRepository
    {
        IEnumerable<Book> GetBooks();
    }
}
