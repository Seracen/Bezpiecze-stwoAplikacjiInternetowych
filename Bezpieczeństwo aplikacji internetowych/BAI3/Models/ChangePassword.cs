using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BAI3.Models
{
    public class ChangePassword
    {
        public string haslo { get; set; }
        [RegularExpression("[a-zA-Z0-9]{8,16}", ErrorMessage = "Hasło powinno składać się od 8 do 16 znaków.")]
        public string Nowehaslo { get; set; }
        public ChangePassword() { }
    }
}
