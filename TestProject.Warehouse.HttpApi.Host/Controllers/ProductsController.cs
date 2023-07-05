using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TestProject.Application.Dtos;
using TestProject.Core; 

namespace TestProject.HttpApi.Host.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private IProductRepository _productRepository;
        private IMapper _mapper;

        public ProductsController(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var products = (await _productRepository.GetAllAsync())
                .Select(aa => _mapper.Map<ProductDto>(aa)).ToArray();
            return Ok(products);
        }

        [HttpPost]
        public async Task<IActionResult> GetList([FromBody] IEnumerable<int> ids)
        {
            var products = (await _productRepository.GetProductList(ids))
                .Select(aa => _mapper.Map<ProductDto>(aa)).ToArray();
            return Ok(products);
        }

        [HttpGet("with-quantity")]
        public async Task<IActionResult> GetProducts()
        {
            var products = (await _productRepository.GetAllAsync())
                .Select(aa => _mapper.Map<ProductWithQuantityDto>(aa)).ToArray();
            return Ok(products);
        }
    }
}
