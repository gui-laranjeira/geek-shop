using GeekShopping.ProductAPI.Data.ValueObjects;
using GeekShopping.ProductAPI.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace GeekShopping.ProductAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _repository;

        public ProductController(IProductRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpGet]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Get all products")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(IEnumerable<ProductVO>))]
        [SwaggerResponse(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<ProductVO>>> FindAll()
        {
            var product = await _repository.FindAll();

            return Ok(product);
        }

        [HttpGet("{id}")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Get product by ID")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ProductVO))]
        [SwaggerResponse(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<ProductVO>> FindByID(long id)
        {
            var product = await _repository.FindByID(id);

            if (product.Id <= 0) 
                return NotFound();

            return Ok(product);
        }

        [HttpPost]
        [Produces("application/json")]
        [Consumes("application/json")]
        [SwaggerOperation(Summary = "Create new product")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ProductVO))]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProductVO>> Create([FromBody] ProductVO productVo)
        {
            if (productVo == null)
                return BadRequest();

            await _repository.Create(productVo);

            return Ok(productVo);
        }

        //TODO Get product id by uri and not body
        [HttpPut]
        [Produces("application/json")]
        [Consumes("application/json")]
        [SwaggerOperation(Summary = "Update product")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ProductVO))]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProductVO>> Update([FromBody] ProductVO productVo)
        {
            if (productVo == null)
                return BadRequest();

            await _repository.Update(productVo);

            return Ok(productVo);
        }

        [HttpDelete("{id}")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Delete product")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(bool))]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Delete(long id)
        {
            var isDeleted = await _repository.Delete(id);
            if (!isDeleted)
                return BadRequest(isDeleted);

            return Ok(isDeleted);
        }
    }
}
