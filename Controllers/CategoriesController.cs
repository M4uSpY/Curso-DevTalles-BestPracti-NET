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
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoriesController(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        // Producir la respuesta
        public IActionResult GetCategories()
        {
            var categories = _categoryRepository.GetCategories();
            var categoriesDto = new List<CategoryDTO>();
            foreach (var category in categories)
            {
                categoriesDto.Add(_mapper.Map<CategoryDTO>(category));
            }
            return Ok(categoriesDto);
        }

        [HttpGet("{id:int}", Name = "GetCategory")]
        public IActionResult GetCategory(int id)
        {
            var category = _categoryRepository.GetCategory(id);
            if (category is null)
            {
                return NotFound($"La categoria con el id {id} no existe");
            }
            var categoryDto = _mapper.Map<CategoryDTO>(category);
            return Ok(categoryDto);
        }

        [HttpPost]
        public IActionResult CreateCategory([FromBody] CreateCategoryDTO createCategoryDTO)
        {
            if (createCategoryDTO is null)
            {
                return BadRequest(ModelState);
            }
            if (_categoryRepository.CategoryExists(createCategoryDTO.Name))
            {
                ModelState.AddModelError("CustomError", "La categoria ya existe");
                return BadRequest(ModelState);
            }

            var category = _mapper.Map<Category>(createCategoryDTO);
            if (!_categoryRepository.CreateCategory(category))
            {
                ModelState.AddModelError("CustomError", $"Algo salio mal al guardar el registro {category.Name}");
                return StatusCode(500, ModelState);
            }
            return CreatedAtRoute("GetCategory", new { id = category.Id }, category);
        }

        [HttpPatch("{id:int}", Name = "UpdateCategory")]
        public IActionResult UpdateCategory(int id, [FromBody] CreateCategoryDTO updateCategoryDto)
        {
            if (!_categoryRepository.CategoryExists(id))
            {
                return NotFound($"La categoria con el id {id} no existe");
            }
            if (updateCategoryDto is null)
            {
                return BadRequest(ModelState);
            }
            if (_categoryRepository.CategoryExists(updateCategoryDto.Name))
            {
                ModelState.AddModelError("CustomError", "La categoria ya existe");
                return BadRequest(ModelState);
            }

            var category = _mapper.Map<Category>(updateCategoryDto);
            category.Id = id;
            if (!_categoryRepository.UpdateCategory(category))
            {
                ModelState.AddModelError("CustomError", $"Algo salio mal al actualizar el registro {category.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

        [HttpDelete("{id:int}", Name = "DeleteCategory")]
        public IActionResult DeleteCategory(int id)
        {
            if (!_categoryRepository.CategoryExists(id))
            {
                return NotFound($"La categoria con el id {id} no existe");
            }
            var category = _categoryRepository.GetCategory(id);
            if (category is null)
            {
                return NotFound($"La categoria con el id {id} no existe");
            }
            if (!_categoryRepository.DeleteCategory(category))
            {
                ModelState.AddModelError("CustomError", $"Algo salio mal al eliminar el registro {category.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();

        }

    }
}
