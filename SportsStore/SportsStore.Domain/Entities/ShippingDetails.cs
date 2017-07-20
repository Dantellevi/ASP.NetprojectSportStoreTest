using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SportsStore.Domain.Entities
{
   public class ShippingDetails
    {
        [Required(ErrorMessage ="Введите имя!!")]//валидация модели-имя
        public string Name { get; set; }

        [Required(ErrorMessage ="Введите строку адреса")]//валидация-адреса
        [Display(Name="Строка адреса 1")]//атрибут для чтения модели представлением
        public string Line1 { get; set; }
        [Display(Name = "Строка адреса 2")]
        public string Line2 { get; set; }
        [Display(Name = "Строка адреса 3")]
        public string Line3 { get; set; }


        [Required(ErrorMessage ="Введите свой город")]//валидация города
        [Display(Name = "Город")]
        public string city { get; set; }

        [Required(ErrorMessage ="Введите областной район для отправки")]//валидация -области
        [Display(Name = "Региональная область")]
        public string State { get; set; }
        [Display(Name = "Почтовый индекс")]
        public string Zip { get; set; }

        [Required(ErrorMessage = "Введите название страны")]
        [Display(Name = "Страна")]
        public string Country { get; set; }
        public bool GiftWrap { get; set; }

        

    }
}
