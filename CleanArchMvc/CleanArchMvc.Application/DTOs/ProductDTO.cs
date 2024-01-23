using CleanArchMvc.Domain.Entities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CleanArchMvc.Application.DTOs
{
    public class ProductDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The Name is Required")]
        [MinLength(3)]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required(ErrorMessage = "The Description is Required")]
        [MinLength(5)]
        [MaxLength(200)]
        public string Description { get; set; }

        [Required(ErrorMessage = "The Price is Required")]
        [Column(TypeName = "decimal(18,2)")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        [DataType(DataType.Currency)]
        [DisplayName("Price")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "The Stock is Required")]
        [Range(0, 9999)]
        [DisplayName("Stock")]
        public int Stock {  get; set; }

        [MaxLength(200)]
        [DisplayName("Product Image")]
        public string Image { get; set; }

        public Category Category { get; set; }

        [DisplayName("Categories")]
        public int CategoryId { get; set; }
    }
}