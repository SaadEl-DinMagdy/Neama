namespace Neama.Api.Helper
{
    public class Pagination<T> where T : class
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int Count { get; set; }
        public IReadOnlyList<T> Data { get; set; }

        public Pagination(int pageindex, int pagesize, int count, IReadOnlyList<T> data)
        {
            PageIndex = pageindex;
            PageSize = pagesize;
            Count = count;
            Data = data;
        }

    }
}
