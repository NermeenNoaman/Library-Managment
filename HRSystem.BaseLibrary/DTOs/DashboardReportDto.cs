using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSystem.BaseLibrary.DTOs
{
    public class DashboardReportDto
    {
        public int TotalBooks { get; set; }
        public int TotalCategories { get; set; }
        public int TotalMembers { get; set; }

        public int TotalBorrowingCount { get; set; } 
        public int TotalActiveBorrowings { get; set; } 

    }
}
