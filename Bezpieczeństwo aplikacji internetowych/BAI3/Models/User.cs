using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BAI3.Models
{
    public class User
    {
        [Key]
        public int userId { get; set; }
        [RegularExpression("^[a-zA-Z]{3,8}", ErrorMessage = "Login powinien składać się z minimum 3 znaków.")]
        public string login { get; set; }
        [RegularExpression("[a-zA-Z0-9]{8,16}", ErrorMessage = "Hasło powinno składać się od 8 do 16 znaków.")]
        public string haslo { get; set; }
        public string imie { get; set; }
        public string nazwisko { get; set; }
        public int iloscNieUdanychProbLogowania { get; set; } = 0;
        public string wlaczenieBlokadyKonta { get; set; } = "false";
        public string blokada { get; set; } = "false";
        public DateTime dataOstatniegoNieudanegoLogowania { get; set; }
        public DateTime dataOstatniegoUdanegoLogowania { get; set; }

        public User() { }
        public User(string l, string h, string i, string n)
        {
            this.login = l;
            this.haslo = h;
            this.imie = i;
            this.nazwisko = n;
        }
    }
}
