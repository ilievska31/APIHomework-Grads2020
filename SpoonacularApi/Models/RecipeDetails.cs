using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpoonacularApi.Models
{
    public class RecipeDetails
    {
        public String Title { get; set; }
        public String Instructions { get; set; }
        public List<ExtendedIngredients> ExtendedIngredients { get; set; }
        public String Image { get; set; }
    }
}
