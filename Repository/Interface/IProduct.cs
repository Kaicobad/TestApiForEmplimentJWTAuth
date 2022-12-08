using testapi.Model;

namespace testapi.Repository.Interface
{
    public interface IProduct
    {
        public Task<List<Product>> GetProducteDetails();
        public Task<Product> GetProductDetails(int id);
        public void AddProduct(Product product);
        public void UpdateProduct(Product product);
        public Task<Product> DeleteProduct(int id);
        public bool CheckProduct(int id);
    }
}
