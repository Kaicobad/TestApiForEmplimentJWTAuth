using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
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
        private readonly IMemoryCache _memoryCache;

        public ProductController(IProduct product, IMemoryCache memoryCache)
        {
           _product = product;
            _memoryCache = memoryCache;
        }
        [HttpGet,Route("getallproducts")]
        [Authorize]
        public async Task<IActionResult> GetProducts()
        {
            //List<Model.Product> product = new List<Model.Product>();
            if (!_memoryCache.TryGetValue("Employees", out List<Model.Product> product))
            { 
                try
                {
                    product = await _product.GetProducteDetails(); // Get the data from database
                    var cacheEntryOptions = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpiration = DateTime.Now.AddMinutes(5),
                        SlidingExpiration = TimeSpan.FromMinutes(2),
                        Size = 1024,
                    };
                    _memoryCache.Set("Employees", product, cacheEntryOptions);
                    return Ok(new
                    {
                        status = 200,
                        message = "success",
                        data = product
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
                var products = _memoryCache.Get("Employees");
                try
                {
                    if (products != null)
                    {
                        return Ok(new
                        {
                            status = 200,
                            message = "success",
                            data = products
                        });
                    }
                    else
                    {
                        return BadRequest(new
                        {
                            status = 401,
                            message = "Data not Stored in Memory!",
                            data = string.Empty
                        });
                    }
                    
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

            //var Products = _product.GetProducteDetails();
            //if (Products != null)
            //{
            //    try
            //    {

            //        return Ok(new
            //        {
            //            status = 200,
            //            message = "success",
            //            data = Products
            //        });
            //    }
            //    catch (Exception ex)
            //    {

            //       return BadRequest(new
            //       {
            //           status = 400,
            //           message = ex.Message,
            //           data = string.Empty
            //       });
            //    }


            //}
            //else
            //{
            //    return BadRequest(new
            //    {
            //        status = 400,
            //        message = "Somrthing wen Wrong",
            //        data = string.Empty
            //    });
            //}
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
