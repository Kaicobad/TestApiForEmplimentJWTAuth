using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Collections;
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
        [HttpGet, Route("getallproducts")]
        [Authorize]
        public async Task<IActionResult> GetProducts()
        {
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
                    var prdmemCatch = _memoryCache.Set("Employees", product, cacheEntryOptions);
                    return Ok(new
                    {
                        status = 200,
                        message = "success",
                        data = prdmemCatch
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
                var dbProd = await _product.GetProducteDetails();
                var products = _memoryCache.Get("Employees");

                var prdcount = products as List<Model.Product>;
                int cacheCountinit = 0;

                if (prdcount != null)
                {
                    var cacheCount = prdcount.Cast<object>().Count();
                    cacheCountinit = cacheCount;
                }

                if (dbProd.Count > cacheCountinit)
                {
                    var cacheEntryOptions = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpiration = DateTime.Now.AddMinutes(5),
                        SlidingExpiration = TimeSpan.FromMinutes(2),
                        Size = 1024,
                    };
                    var prdcache = _memoryCache.Set("Employees", dbProd, cacheEntryOptions);
                    try
                    {
                        if (prdcache != null)
                        {
                            return Ok(new
                            {
                                status = 200,
                                message = "success",
                                data = prdcache
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
                else
                {
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
        [HttpPost, Route("checkproduct")]
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
