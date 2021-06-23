using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace leave_management.Data
{
    public class LeaveRequest
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        [ForeignKey("ApplicationUserId")]
        public ApplicationUser ApplicationUser { get; set; }
        public string ApplicationUserId { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public DateTime DateRequested { get; set; }
        public bool ApprovedByAdmin { get; set; } = false;
        public bool ApprovedByManager { get; set; } = false;
        public bool Cancelled { get; set; }
    }
}
