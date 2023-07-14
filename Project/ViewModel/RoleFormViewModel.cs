using System.ComponentModel.DataAnnotations;

namespace FinalProject.ViewModel
{
    public class RoleFormViewModel
    {
        [Required, StringLength(10)]
        public string Name { get; set; }
    }
}

