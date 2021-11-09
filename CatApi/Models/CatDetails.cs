using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


namespace CatApi.Models
{
    public class CatDetails
    {
        public CatDetails() { }

        public int Id { get; set; }
        [Required]
        [MinLength(2)]
        public string Namn { get; set; }
        [Required]
        [Range(2000, 2020)]
        public int Fodd { get; set; }
        [Required]
        [MinLength(2)]
        public string Farg { get; set; }
        [Required]
        [MinLength(2)]
        public string Sort { get; set; }
        [JsonIgnore]
        public int Lever_som { get; set; }
        [Required]
        public Lifestyle Livsstil {get; set;}
        public List<CatHabits> Ovanor { get; set; }
    }
}