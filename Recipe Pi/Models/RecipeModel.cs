using System;
using System.Collections.Generic;
using System.Text;

namespace Recipe_Pi.Models
{    
    public class RecipeModel
    {
        public int RecipeId { get; set; }
        public string RecipeName { get; set; }

        /// <author>Siem</author>
        /// <summary>
        /// Override ToString method to show RecipeName.
        /// </summary>
        /// <returns>Recipename when recipe is selected/returns>
        public override string ToString()
        {
            return RecipeName;
        }
    }
}
