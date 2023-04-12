using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ProjecPRN.Models
{
    public partial class Movie
    {
        public Movie()
        {
            Rates = new HashSet<Rate>();
        }

        public int MovieId { get; set; }
        [Required(ErrorMessage = "Title not empty")]
        [Display(Name = "Title")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Year not empty")]
        [Display(Name = "Year")]
        public int? Year { get; set; }
        [Required(ErrorMessage = "Image not empty")]
        [Display(Name = "Image")]

        public string? Image { get; set; }
        [Required(ErrorMessage = "Description not empty")]
        [Display(Name = "Description")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Genres not empty")]
        [Display(Name = "Genres")]
        public int? GenreId { get; set; }

        public virtual Genre Genre { get; set; }
        public virtual ICollection<Rate> Rates { get; set; }
    }
}
