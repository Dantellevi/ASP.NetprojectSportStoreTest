using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SportsStore.Domain.Entities
{
       public class Product
    {
        [HiddenInput(DisplayValue=false)]
        public int ProductID { get; set; }

        [Required(ErrorMessage ="Пожалуйста введите название товара!!!")]
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage ="Пожалуйста введите описание товара!!!")]
        public string description { get; set; }
        [Required]
        [Range(0.01,double.MaxValue,ErrorMessage ="Введите правильную цену!!Не отрицательную!!!")]
        public decimal price { get; set; }

        [Required(ErrorMessage ="Введите необходимую категорию товара!!!")]
        public string Category { get; set; }


        public byte[] ImageData { get; set; }
        public string ImageMimeType { get; set; }



    }
}
