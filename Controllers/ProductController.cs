using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using testapi.Model;
using testapi.Repository.Interface;
using testapi.Repository.Service;

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
        [Authorize]
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
        [HttpPost, Route("addproduct")]
        [Authorize]
        public async Task<IActionResult> AddProduct(testapi.Model.Product product)
        {
            try
            {
                _product.AddProduct(product);
                return Ok(new
                {
                    status = 200,
                    message = " added successfully",
                    data = product
                });
            }
            catch (Exception ex)
            {

                return BadRequest(new
                {
                    status = 400,
                    message = ex.InnerException,
                    data = ""
                });
            }
            
        }
        [HttpPost,Route("checkproduct")]
        [Authorize]
        public async Task<IActionResult> CheckProduct(int id)
        {
            try
            {
                var prd = _product.CheckProduct(id);

                if (prd == true)
                {
                    return Ok(new
                    {
                        status = 200,
                        message = "success",
                        data = prd
                    });
                }
                else
                {
                    return NotFound();
                }

            }
            catch (Exception ex)
            {

                return BadRequest(new
                {
                    status = 400,
                    message = ex.InnerException,
                    data = ""
                });
            }
        }
    }
}
