namespace krov_nad_glavom_api.Application.Utils
{
    public class QueryStringParameters
    {
        const int maxPageSize = 100;
        private int _pageNumber = 1;
        public int PageNumber
        {
            get => (_pageNumber > 0) ? _pageNumber : 1;
            set 
            {
                _pageNumber = (value > 0) ? value : 1;
            }
        }
        private int _pageSize = 10;
        public int PageSize 
        {
            get => _pageSize;
            set 
            {
                _pageSize = (value > maxPageSize) ? maxPageSize : value;
            }
        }
        public string SearchText { get; set; } = "";
        public string SortProperty { get; set; }
        public string SortType { get; set; } = "asc";
        public string City { get; set; }
        public string Address { get; set; }
        public int? Area { get; set; }
        public int? RoomCount { get; set; }
        public int? BalconyCount { get; set; }
        public int? Floor { get; set; }
        public string Orientation { get; set; }

        public void checkOverflow(int count) {
            if (((int)Math.Ceiling(count / (double)PageSize)) < PageNumber) {
                PageNumber = (int)Math.Ceiling(count / (double)PageSize);
            }
        }
    }
}