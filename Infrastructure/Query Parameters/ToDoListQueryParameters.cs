

namespace ToDoListeWeb.Infrastructure.QueryParameters
{
    public class ToDoListQueryParameters
    {
        const int _maxSize = 100;
        private int _siteSize = 20;
        public int Page { get; set; }
        public int SiteSize { 
            get
            {
                return _siteSize;
            } 
            set
            {
                _siteSize = System.Math.Min(_maxSize, value);
            } 
        }
        public string Name { get; set; }
        public int Id { get; set; }
        public string SortBy { get; set; } = "Id";
        private string _sortOrder = "asc";
        public string SortOrder 
        { 
            get
            {
                return _sortOrder;
        } 
            set
            {
                if(value == "desc" || value == "asc")
                {
                    _sortOrder = value;
                }
        }   }
    }
}