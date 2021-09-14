using System;
using System.Collections.Generic;
using System.Text;

namespace Recipe_Pi.Models
{
    public class ProductModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductBarcode { get; set; }
        public int ProductCount { get; set; }
        public string PurchaseDate { get; set; }
        public string ExpireDate { get; set; }
        
        public bool IsExpired
        {
            get
            {
                if (ExpireDate.Equals(string.Empty))
                    return false;
                return Convert.ToDateTime(ExpireDate) < DateTime.Now;
            }
        }

        /// <author>Anas</author>
        /// <summary>
        /// Overrride om te vragen of de gebruiker zeker weet of hij/zij de juiste informatie heeft toegevoegd.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string productname = ProductName;
            string productcount = ProductCount.ToString();
            string expiredate = ExpireDate;
            return productname + " " + productcount + " " + expiredate;
        }
    }
}
