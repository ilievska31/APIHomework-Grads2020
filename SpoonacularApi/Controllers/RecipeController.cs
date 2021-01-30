using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SpoonacularApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SpoonacularApi.Controllers
{
    public class RecipeController : Controller
    {
        private readonly String apiURL = "https://api.spoonacular.com/recipes";
        private readonly String apiID = "apiKey=51c719f6a4674569aef3e2ef6f81fdf0";

        public RecipeController()
        {
            APIHelper.InitializeClient();
        }
        public IActionResult Index()
        {
            return View();
        }


        public async Task<IActionResult> GetRecipes(string name = "")
        {
            string url = apiURL + "/complexSearch?&query=" + name.Trim().Replace(" ", "+") +"&"+ apiID;
            using (HttpResponseMessage response = await APIHelper.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    Response resObject = JsonConvert.DeserializeObject<Response>(json);


                    return View(resObject.results);
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<IActionResult> GetDetails(string id = "") 
        {
            string url = apiURL + "/" + id + "/information?" + apiID;
            using (HttpResponseMessage response = await APIHelper.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    RecipeDetails resObject = JsonConvert.DeserializeObject<RecipeDetails>(json);



                    return View(resObject);
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
    }
}
