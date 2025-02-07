using System.ComponentModel.DataAnnotations;

namespace ClothDomain;

public enum OrderStatus
{
    [Display(Name = "Собирается")]
    СОБИРАЕТСЯ,
    [Display(Name = "В пути")]
    В_ПУТИ,
    [Display(Name = "Ожидает в пункте выдачи")]
    ОЖИДАЕТ_В_ПУНКТЕ_ВЫДАЧИ,
    [Display(Name = "Доставлен")]
    ДОСТАВЛЕН
}
