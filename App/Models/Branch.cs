using System.ComponentModel.DataAnnotations;

namespace App.Models
{

    //public enum BranchStatus
    //{
    //    Closed, Open
    //}
    public class Branch
    {
        [Key]
        public int BranchID { get; set; }

        [Required]
        [Display(Name = "Branch Name")]
        public string BranchName { get; set; }

        [Required]
        public string Address { get; set; }
        //public long SortCode { get; set; }
        //public BranchStatus Status { get; set; }
    }
}
