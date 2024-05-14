namespace Introduction_HomeTask.Configurations
{
    public class ProductOptions
    {
        public const string ConfigName = "Product";
        public string ConnectionString { get; set; } = string.Empty;
        public int MaxAmountOfProducts { get; set; }
    }
}
