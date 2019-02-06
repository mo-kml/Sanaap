namespace Sanaap.Dto
{
    public partial class FilterNewsDto
    {
        public int? Month { set; get; }
        public int? Year { set; get; }
        public string SearchKey { set; get; }
        public int Page { set; get; }
    }
}
