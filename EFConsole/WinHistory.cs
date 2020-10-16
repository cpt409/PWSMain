using System;
using System.Collections.Generic;

namespace EFConsole
{
    public partial class WinHistory
    {
        public int WinHistoryId { get; set; }
        public int NameId { get; set; }
        public DateTime? WinDate { get; set; }

        public virtual Names Name { get; set; }
    }
}
