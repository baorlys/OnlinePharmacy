namespace OnlinePharmacy.Models
{
    public class SearchResultsViewModel
    {
        public string Query { get; set; }
        public List<Product> ProductResults { get; set; }
        public List<ProductCategory> CategoryResults { get; set; }
        public List<Blog> BlogResults { get; set; }
    }

}
