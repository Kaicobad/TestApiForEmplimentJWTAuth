using System.ComponentModel.DataAnnotations;

namespace testapi.Model
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string  ProductCode{ get; set; }
    }
}
