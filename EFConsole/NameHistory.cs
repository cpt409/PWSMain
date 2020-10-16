using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;

namespace EFConsole
{
    class NameHistory
    {
        public NameHistory()
        {
            Names = "";
            WinDates = null;
        }

        public List<DateTime?> WinDates { get; set; }

        public string Names { get; set; }

        public void InsertWinDate(DateTime dt)
        {
            this.WinDates.Add(dt);
        }

        public DateTime? GetRecentWinDate
        {
            get
            {
                return WinDates.First();
            }
        }

        public DateTime? GetOldestWinDate
        {
            get
            {
                return WinDates.Last();
            }
        }

    }
}
