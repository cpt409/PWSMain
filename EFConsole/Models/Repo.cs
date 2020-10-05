using System;
using System.Collections.Generic;

namespace EFConsole.Models
{
    public partial class Repo
    {
        public Repo()
        {
            NameRepoJunction = new HashSet<NameRepoJunction>();
        }

        public int RepoId { get; set; }
        public string RepoName { get; set; }

        public virtual ICollection<NameRepoJunction> NameRepoJunction { get; set; }
    }
}
