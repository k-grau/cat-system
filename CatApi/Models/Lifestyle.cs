using System;
using System.ComponentModel.DataAnnotations;

namespace CatApi.Models
{
    public class Lifestyle
    {
        [Required]
        [Range(1, 3)]
        public int Livsstil_Id { get; set; }
        public string Beskrivning { get; set; }
    }
}