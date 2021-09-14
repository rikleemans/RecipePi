using System;
using System.Collections.Generic;
using System.Text;

namespace Recipe_Pi.Models
{
    public class RecipeProductModel
    {
        public int FPProductID { get; set; }
        public int FPRecipeID { get; set; }
        public string ProductName { get; set; }
        public string ExpireDate { get; set; }
        public int ProductCount { get; set; }
        public bool IsExpired
        {
            get
            {
                if (ExpireDate.Equals(string.Empty))
                    return false;
                return Convert.ToDateTime(ExpireDate) < DateTime.Now;
            }
        }
    }
}
