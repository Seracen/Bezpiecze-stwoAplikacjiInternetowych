using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BAI3.Models
{
    public class UserView : User
    {
        public string partialPassword { get; set; }
        public UserView() { }
    }
}
