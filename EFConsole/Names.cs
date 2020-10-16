using System;
using System.Collections.Generic;

namespace EFConsole
{
    public partial class Names
    {
        public Names()
        {
            NameRepoJunction = new HashSet<NameRepoJunction>();
            WinHistory = new HashSet<WinHistory>();
        }

        public int NameId { get; set; }
        public string Name { get; set; }
        public bool? TopBool { get; set; }
        public int Wins { get; set; }
        public int TopCount { get; set; }
        public DateTime? DateWin { get; set; }

        public virtual ICollection<NameRepoJunction> NameRepoJunction { get; set; }
        public virtual ICollection<WinHistory> WinHistory { get; set; }
    }
}
