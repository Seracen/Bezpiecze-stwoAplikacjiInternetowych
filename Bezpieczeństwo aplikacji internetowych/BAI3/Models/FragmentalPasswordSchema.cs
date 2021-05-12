using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BAI3.Models
{
    public class FragmentalPasswordSchema
    {
        [Key]
        public int Id { get; set; }
        public int passwordSice { get; set; }
        public int userId { get; set; }
        public string schema { get; set; }
        public int actualPasswordSchema { get; set; }
        public FragmentalPasswordSchema()
        {

        }
        public FragmentalPasswordSchema(int u, string s,int p)
        {
            this.userId = u;
            this.schema = s;
            this.passwordSice = p;
            actualPasswordSchema = 0;
        }
    }
}
