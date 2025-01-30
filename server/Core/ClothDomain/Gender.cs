using System.ComponentModel.DataAnnotations;

namespace ClothDomain;

public enum Gender
{
    [Display(Name = "Мужской")]
    МУЖСКОЙ,
    [Display(Name = "Женский")]
    ЖЕНСКИЙ
}
