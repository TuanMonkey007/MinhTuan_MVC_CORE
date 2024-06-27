using Microsoft.AspNetCore.Mvc;
using MinhTuan.Domain.Core.DTO;
using MinhTuan.Domain.DTOs.ApiPythonDTO;
using MinhTuan.Domain.Helper.ImageSearch;
using MinhTuan.Service.Services.ImageSearch;
using Newtonsoft.Json;
using System.Numerics;
using System.Text;
using MathNet.Numerics.LinearAlgebra;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Net.Http;

namespace MinhTuan.API.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class ImageSearchController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpClientFactory _httpClientFactory;
        public ImageSearchController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _httpClient = _httpClientFactory.CreateClient();

            _httpClient.BaseAddress = new Uri("http://weaviate:8080");
           //  _httpClient.BaseAddress = new Uri("http://localhost:8080");
        }
        [HttpPost("reload-image-to-model")]
        public async Task<IActionResult> ReloadData()
        {
            var imagesDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "Product");
            if (!Directory.Exists(imagesDirectory))
            {
                return NotFound("Không tìm thấy thư mục chứa ảnh directory not found.");
            }

            var imgFiles = Directory.GetFiles(imagesDirectory);
            var promises = new List<Task>();

            foreach (var imgFile in imgFiles)
            {
                var imageBytes = System.IO.File.ReadAllBytes(imgFile);
                var imageBase64 = Convert.ToBase64String(imageBytes);
                var fileName = Path.GetFileName(imgFile);

                var data = new
                {
                    @class = "ProductImage", // Chú ý: "class" không phải "Class"
                    properties = new
                    {
                        image = imageBase64,
                        path = fileName
                    }
                };

                var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");

                promises.Add(Task.Run(async () =>
                {
                    var request = new HttpRequestMessage(HttpMethod.Post, "http://weaviate:8080/v1/objects") { Content = content };
                   // var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost:8080/v1/objects") { Content = content };
                    var response = await _httpClient.SendAsync(request);

                    if (!response.IsSuccessStatusCode)
                    {
                        var errorContent = await response.Content.ReadAsStringAsync();
                        Console.WriteLine($"Error adding image {fileName}: {errorContent}"); // Hoặc xử lý lỗi theo cách khác
                    }
                }));
            }

            await Task.WhenAll(promises);


            return Ok("Đã load thành công dữ liệu.");
        }
        [HttpGet("config-schema-module")]
        public async Task<IActionResult> ConfigureSchemaAndModule()
        {

            var schemaConfig = new
            {
                Class = "ProductImage",
                vectorizer = "img2vec-neural",
                vectorIndexType = "hnsw",
                moduleConfig = new
                {
                    img2vec_neural = new
                    {
                        imageFields = new[] { "image" }
                    }
                },
                properties = new[]
                {
                    new { name = "image", dataType = new[] { "blob" } },
                    new { name = "path", dataType = new[] { "string" } }
                }
            };
            string schemaJson = JsonConvert.SerializeObject(schemaConfig);
            // Sử dụng schemaJson cho các yêu cầu API
            // Ví dụ: Gửi yêu cầu POST để tạo schema
            var request = new HttpRequestMessage(HttpMethod.Post, "/v1/schema");
            request.Content = new StringContent(schemaJson, Encoding.UTF8, "application/json");
            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                // Xử lý khi thành công
                return Ok("Schema configured successfully.");
            }
            else
            {
                // Xử lý khi không thành công
                return Ok(await response.Content.ReadAsStringAsync());
            }

        }

        [HttpDelete("delete-schema")] // Sử dụng HTTP DELETE
        public async Task<IActionResult> DeleteSchema()
        {
            string className = "ProductImage";
            // Kiểm tra xem lớp có tồn tại không
            var schemaRes = await _httpClient.GetAsync("/v1/schema"); // Hoặc bất kỳ cách nào bạn đang dùng để lấy schema
            var schemaJson = await schemaRes.Content.ReadAsStringAsync();
            var existingSchema = JsonConvert.DeserializeObject<dynamic>(schemaJson);

            var classExists = existingSchema?.classes != null && existingSchema.classes.Count > 0 && (string)existingSchema.classes[0].@class == className;

            if (classExists)
            {
                // Xóa schema
                var request = new HttpRequestMessage(HttpMethod.Delete, $"/v1/schema/{className}");
                var response = await _httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    return Ok($"Schema '{className}' deleted successfully.");
                }
                else
                {
                    // Xử lý lỗi từ Weaviate (ví dụ: schema không tồn tại)
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return BadRequest($"Error deleting schema: {errorContent}");
                }
            }
            else
            {
                return NotFound($"Schema '{className}' not found.");
            }
        }
        private async Task<bool> deleteSchema()
        {
            string className = "ProductImage";
            // Kiểm tra xem lớp có tồn tại không
            var schemaRes = await _httpClient.GetAsync("/v1/schema"); // Hoặc bất kỳ cách nào bạn đang dùng để lấy schema
            var schemaJson = await schemaRes.Content.ReadAsStringAsync();
            var existingSchema = JsonConvert.DeserializeObject<dynamic>(schemaJson);

            var classExists = existingSchema?.classes != null && existingSchema.classes.Count > 0 && (string)existingSchema.classes[0].Class == className;

            if (classExists)
            {
                // Xóa schema
                var request = new HttpRequestMessage(HttpMethod.Delete, $"/v1/schema/{className}");
                var response = await _httpClient.SendAsync(request);

            }
            return true;
        }
        private async Task<bool> addSchema()
        {
            var schemaConfig = new
            {


                Class = "ProductImage",
                vectorizer = "img2vec-neural",
                vectorIndexType = "hnsw",
                moduleConfig = new
                {
                    img2vec_neural = new
                    {
                        imageFields = new[] { "image" }
                    }
                },
                properties = new[]
              {
                    new { name = "image", dataType = new[] { "blob" } },
                    new { name = "path", dataType = new[] { "string" } }
                }
            };
            string schemaJson = JsonConvert.SerializeObject(schemaConfig);
            // Sử dụng schemaJson cho các yêu cầu API
            // Ví dụ: Gửi yêu cầu POST để tạo schema
            var request = new HttpRequestMessage(HttpMethod.Post, "/v1/schema");
            request.Content = new StringContent(schemaJson, Encoding.UTF8, "application/json");
            var response = await _httpClient.SendAsync(request);
            return true;
        }

        [HttpGet("get-schema")]
        public async Task<IActionResult> GetSchema()
        {
            var response = await _httpClient.GetAsync("http://localhost:8080/v1/schema");

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
            }

            var schema = await response.Content.ReadAsStringAsync();
            return Ok(schema);
        }



    }

}
