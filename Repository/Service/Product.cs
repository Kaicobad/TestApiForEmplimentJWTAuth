using Microsoft.EntityFrameworkCore;
using testapi.DataLayer;
using testapi.Repository.Interface;

namespace testapi.Repository.Service
{
    public class Product : IProduct
    {
        private readonly ApplicationDbContext applicationDbContext;
        public Product(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        public void AddProduct(Model.Product product)
        {
            try
            {
                applicationDbContext.products.Add(product);
                applicationDbContext.SaveChanges();
            }
            catch (Exception ex)
            {

                throw ex.InnerException;
            }
        }

        public bool CheckProduct(int id)
        {
            try
            {
                var productCheck = applicationDbContext.products.Where(c => c.Id == id).FirstOrDefault();
                if (productCheck != null)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                if (ex.Message != null)
                {
                    return false;
                }
            }
            return false;
        }

        public async Task<Model.Product> DeleteProduct(int id)
        {
            try
            {
                Model.Product product = await applicationDbContext.products.FindAsync(id);
                applicationDbContext.products.Remove(product);
                applicationDbContext.SaveChangesAsync();
                return product;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Model.Product> GetProductDetails(int id)
        {
            try
            {
                Model.Product product = await applicationDbContext.products.FindAsync(id);
                return product;
            }
            catch (Exception ex)
            {

                throw ex.InnerException;
            }
        }

        public async Task<List<Model.Product>> GetProducteDetails()
        {
            try
            {
                var productlist = await applicationDbContext.products.ToListAsync();
                return productlist;
            }
            catch (Exception ex)
            {
                return null;
            }
            
        }

        public void UpdateProduct(Model.Product product)
        {
            try
            {
                var prd = applicationDbContext.products.Find(product.Id);
                applicationDbContext.products.Update(prd);
                applicationDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
