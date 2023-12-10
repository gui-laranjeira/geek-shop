using GeekShopping.Web.Models;
using GeekShopping.Web.Services.IServices;
using GeekShopping.Web.Utils;
using System.Net.Http.Headers;

namespace GeekShopping.Web.Services
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _client;

        public const string BasePath = "api/v1/product"; 
        public ProductService(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(HttpClient));
        }

        public async Task<IEnumerable<ProductModel>> FindAllProducts(string token)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _client.GetAsync(BasePath);
            return await response.ReadContentAsync<List<ProductModel>>();
        }
        public async Task<ProductModel> FindProductByID(long id, string token)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _client.GetAsync($"{BasePath}/{id}");
            return await response.ReadContentAsync<ProductModel>();
        }

        public async Task<ProductModel> CreateProduct(ProductModel model, string token)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _client.PostAsJson($"{BasePath}", model);
            if (!response.IsSuccessStatusCode)
                throw new Exception("Something went wrong when calling API.");

            return await response.ReadContentAsync<ProductModel>();
        }

        public async Task<ProductModel> UpdateProduct(ProductModel model, string token)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _client.PutAsJson($"{BasePath}", model);
            if (!response.IsSuccessStatusCode)
                throw new Exception("Something went wrong when calling API.");

            return await response.ReadContentAsync<ProductModel>();
        }

        public async Task<bool> DeleteProductByID(long id, string token)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _client.DeleteAsync($"{BasePath}/{id}");
            if (!response.IsSuccessStatusCode)
                throw new Exception("Something went w rong shen calling API.");

            return await response.ReadContentAsync<bool>();
        }
    }
}
