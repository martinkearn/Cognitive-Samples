using Recommendations.Models;

namespace Recommendations.ViewModels
{
    public class HomeBookViewModel
    {
        public Book Book { get; set; }
        public RecommendedItems ITIItems { get; set; }
        public RecommendedItems FBTItems { get; set; }
    }
}
