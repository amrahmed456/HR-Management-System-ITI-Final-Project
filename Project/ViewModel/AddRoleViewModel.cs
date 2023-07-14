using System.ComponentModel.DataAnnotations;

namespace FinalProject.ViewModel
{
    public class AddRoleViewModel
    {
        [Required, StringLength(10)]
        public string RoleName { get; set; }
        public List<CheckBoxViewModel> RoleCalims { get; set; }

    }
}
