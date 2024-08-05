using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Recipes
{
    class Program
    {

        public class ApiResponse
        {
            public Recipe[] Results { get; set; }
        }
        public class Recipe
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Image { get; set; }
        }

        public class RecipeService
        {
            private readonly HttpClient _httpClient;
            private readonly string _apiKey = "7c41c830713541419aff43e16bb545fb"; 

            public RecipeService(HttpClient httpClient)
            {
                _httpClient = httpClient;
            }

            public async Task<ApiResponse> GetRecipesAsync(string query)
            {
                string url = $"https://api.spoonacular.com/recipes/complexSearch?query={query}&apiKey={_apiKey}";
                HttpResponseMessage response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ApiResponse>(responseBody);
            }
        }

        static async Task Main(string[] args)
        {
            var httpClient = new HttpClient();
            var recipeService = new RecipeService(httpClient);

            Console.WriteLine("Digite o termo para buscar receitas:");
            string query = Console.ReadLine();

            try
            {
                var response = await recipeService.GetRecipesAsync(query);
                Console.WriteLine($"Receitas encontradas para '{query}':");
                foreach (var recipe in response.Results)
                {
                    Console.WriteLine($"ID: {recipe.Id}, Título: {recipe.Title}");
                    Console.WriteLine($"Imagem: {recipe.Image}");
                    Console.WriteLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex.Message}");
            }
        }
    }
}
