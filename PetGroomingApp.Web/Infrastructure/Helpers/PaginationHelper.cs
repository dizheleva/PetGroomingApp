namespace PetGroomingApp.Web.Infrastructure.Helpers
{
    using PetGroomingApp.Web.ViewModels;

    public static class PaginationHelper
    {
        public static PaginationViewModel CreatePagination(int currentPage, int totalItems, int pageSize = 6)
        {
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            
            if (currentPage < 1)
                currentPage = 1;
            if (currentPage > totalPages && totalPages > 0)
                currentPage = totalPages;

            return new PaginationViewModel
            {
                CurrentPage = currentPage,
                TotalPages = totalPages,
                PageSize = pageSize,
                TotalItems = totalItems
            };
        }

        public static IEnumerable<T> Paginate<T>(IEnumerable<T> items, int page, int pageSize)
        {
            return items.Skip((page - 1) * pageSize).Take(pageSize);
        }
    }
}

