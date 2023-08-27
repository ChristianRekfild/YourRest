using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourRest.DAL.Postgre.Entities
{
    public class Customer
    {
        [Key]
        public string Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }
        [Required]
        [MaxLength(50)]
        public string MiddleName { get; set; }
        [Required]
        public DateTime DateOFBirth { get; set; }
        [Required]
        [MaxLength(50)]
        public string Login { get; set; }

        [Required]
        [Comment("Тут хранится хэш пароля")] // могу докинуть кстати класс для его формирования, ибо есть, откуда "дёрнуть"
        [MaxLength(150)] // увеличил поле, так как при моих попытках запихать хэш в 50 символов всё закончилось расширением длинны поля
        public string Password { get; set; }

        [Required]
        [Comment("Активен ли пользователь. Если нет - значит он накосячил и мы его заблочили")]
        public bool IsActive { get; set; }

        public List<CustomerPhone> Phones { get; set; }
    }
}
