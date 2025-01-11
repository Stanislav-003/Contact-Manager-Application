using System.ComponentModel.DataAnnotations;

namespace CsvParser.Web.Models;

public class CsvViewModel
{
    public Guid Id { get; set; }
    
    [Required(ErrorMessage = "Name is required")]
    [StringLength(30, ErrorMessage = "Name cannot be longer than 30 characters")]
    public required string Name { get; set; } 
    
    [Required(ErrorMessage = "Birth date is required")]
    [Display(Name = "Birth Date")]
    [DataType(DataType.Date)]
    public DateTime BirthDate { get; set; }

    [Display(Name = "Married")]
    public bool IsMarried { get; set; }

    [Required(ErrorMessage = "Phone is required")]
    [StringLength(10, ErrorMessage = "Phone cannot be longer than 10 characters")]
    public required string Phone { get; set; }


    [Required(ErrorMessage = "Salary is required")]
    [Range(500, double.MaxValue, ErrorMessage = "Salary must be at least $500")]
    [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
    public decimal Salary { get; set; }
}