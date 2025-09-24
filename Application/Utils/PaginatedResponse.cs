using Newtonsoft.Json;

namespace krov_nad_glavom_api.Application.Utils
{
    public class PaginatedResponse<T>
    {
        public List<T> Items { get; set; }
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }

        public PaginatedResponse(List<T> items, int totalCount, int pageNumber, int pageSize, int totalPages)
        {
            Items = items;
            TotalCount = totalCount;
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalPages = totalPages;
        }

        public string getMetadata()
        {
            var metadata = new
            {
                this.TotalPages,
                this.TotalCount,
                this.PageSize,
                this.PageNumber,
            };
            return JsonConvert.SerializeObject(metadata);
        }
    }
}