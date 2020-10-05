using System;
using System.Collections.Generic;

namespace EFConsole.Models
{
    public partial class Names
    {
        public Names()
        {
            NameRepoJunction = new HashSet<NameRepoJunction>();
        }

        public int NameId { get; set; }
        public string Name { get; set; }
        public bool? TopBool { get; set; }
        public int Wins { get; set; }
        public int TopCount { get; set; }

        public virtual ICollection<NameRepoJunction> NameRepoJunction { get; set; }
    }
}
