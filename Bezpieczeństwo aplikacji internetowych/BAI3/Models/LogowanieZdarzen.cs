using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BAI3.Models
{
    public class LogowanieZdarzen
    {
        [Key]
        public int logowanieZdarzenId { get; set; }
        public string Login { get; set; }
        public string haslo { get; set; }
        public int iloscNieudanychLogowan { get; set; }
        public DateTime ostatniaProbaLogowania { get; set; }

        public LogowanieZdarzen() { }
        public LogowanieZdarzen(string l, string h, int i, DateTime d)
        {
            this.Login = l;
            this.haslo = h;
            this.iloscNieudanychLogowan = i;
            this.ostatniaProbaLogowania = d;
        }
    }
}
