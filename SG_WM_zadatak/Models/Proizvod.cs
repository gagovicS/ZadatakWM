using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SG_WM_zadatak.Models
{
    public class Proizvod
    {
        public int proizvodID { get; set; }
        [Required]
        [StringLength(50,ErrorMessage ="Ne sme biti duzi od 50 karaktera")]
        public string naziv { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "Ne sme biti duzi od 50 karaktera")]
        public string opis { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "Ne sme biti duzi od 50 karaktera")]
        public string kategorija { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "Ne sme biti duzi od 50 karaktera")]
        public string proizvodjac { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "Ne sme biti duzi od 50 karaktera")]
        public string dobavljac { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        public double cena { get; set; }

        public static implicit operator List<object>(Proizvod v)
        {
            throw new NotImplementedException();
        }
    }
}