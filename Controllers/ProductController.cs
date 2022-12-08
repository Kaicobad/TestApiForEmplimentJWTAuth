using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using testapi.Repository.Interface;

namespace testapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProduct _product;

        public ProductController(IProduct product)
        {
           _product = product;
        }
        [HttpGet,Route("getallproducts")]
        public async Task<IActionResult> GetProducts()
        {
            var Products = _product.GetProducteDetails();
            if (Products != null)
            {
                try
                {

                    return Ok(new
                    {
                        status = 200,
                        message = "success",
                        data = Products
                    });
                }
                catch (Exception ex)
                {

                   return BadRequest(new
                   {
                       status = 400,
                       message = ex.Message,
                       data = string.Empty
                   });
                }


            }
            else
            {
                return BadRequest(new
                {
                    status = 400,
                    message = "Somrthing wen Wrong",
                    data = string.Empty
                });
            }
        }
    }
}
