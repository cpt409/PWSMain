using System;
using System.Collections.Generic;

namespace EFConsole
{
    public partial class NameRepoJunction
    {
        public int NameRepoId { get; set; }
        public int NameRef { get; set; }
        public int RepoRef { get; set; }

        public virtual Names NameRefNavigation { get; set; }
        public virtual Repo RepoRefNavigation { get; set; }
    }
}
