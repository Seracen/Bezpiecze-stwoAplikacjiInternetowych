using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BAI3.Models
{
    public class LoginAttepmts
    {
        public int Id { get; set; }
        public int attempt { get; set; } = 0;

        public LoginAttepmts() { }
        public LoginAttepmts(int i)
        {
            this.attempt = i;
        }
    }
}
