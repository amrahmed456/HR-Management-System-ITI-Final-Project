using Humanizer;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace FinalProject.Models
{
    public class PaginatedList<T> : List<T>
    {
        public int PageNumber { get; private set; }
        public int TotalPages { get; private set; }

        public int PageSize { get; private set; } = 10;


        public bool HasPreviousPage { get; private set; }

        public bool HasNextPage { get; private set; }
        public PaginatedList(List<T> items, int pageNumber, int pageSize, int totalRecords, int totalPages , bool hasPreviousPage , bool hasNextPage)
        {
            PageNumber = pageNumber;
            TotalPages = totalPages;
            HasPreviousPage = hasPreviousPage;
            HasNextPage = hasNextPage;

		}
    }
}
