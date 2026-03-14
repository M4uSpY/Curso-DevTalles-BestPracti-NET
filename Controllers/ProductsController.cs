using ApiEcommerce.Models;
using ApiEcommerce.Models.DTOs;
using ApiEcommerce.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiEcommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public ProductsController(IProductRepository productRepository, IMapper mapper, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        public IActionResult GetProducts()
        {
            var products = _productRepository.GetProducts();
            var productsDto = _mapper.Map<List<ProductDTO>>(products);
            return Ok(productsDto);
        }

        [HttpGet("{id:int}", Name = "GetProduct")]
        public IActionResult GetProduct(int id)
        {
            var product = _productRepository.GetProduct(id);
            if (product is null)
            {
                return NotFound($"El producto con el id {id} no existe");
            }
            var productDto = _mapper.Map<ProductDTO>(product);
            return Ok(productDto);
        }

        [HttpPost]
        public IActionResult CreateProduct([FromBody] CreateProductDTO createProductDTO)
        {
            if (createProductDTO is null)
            {
                return BadRequest(ModelState);
            }
            if (_productRepository.ProductExists(createProductDTO.Name))
            {
                ModelState.AddModelError("CustomError", "El producto ya existe");
                return BadRequest(ModelState);
            }
            if (!_categoryRepository.CategoryExists(createProductDTO.CategoryId))
            {
                ModelState.AddModelError("CustomError", $"La categoria con el id {createProductDTO.CategoryId} no existe");
                return BadRequest(ModelState);
            }

            var product = _mapper.Map<Product>(createProductDTO);
            if (!_productRepository.CreateProduct(product))
            {
                ModelState.AddModelError("CustomError", $"Algo salio mal al guardar el registro {product.Name}");
                return StatusCode(500, ModelState);
            }
            var createdProduct = _productRepository.GetProduct(product.ProductID);
            var productDto = _mapper.Map<ProductDTO>(createdProduct);
            return CreatedAtRoute("GetProduct", new { id = product.ProductID }, productDto);
        }
    }
}
// VIDEO 65
