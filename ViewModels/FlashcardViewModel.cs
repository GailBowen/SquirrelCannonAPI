using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace SquirrelCannon.ViewModels
{
    public class FlashcardViewModel
    {
        public string Question { get; set; }
        public string Answer { get; set; }

        [Display(Name = "Subject")]
        public int SubjectId { get; set; }

        [ValidateNever]
        public List<SelectListItem> Subjects { get; set; }
    }
}
