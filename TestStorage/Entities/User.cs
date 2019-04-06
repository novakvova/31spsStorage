using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestStorage.Entities
{
    [Table("tblUsers")]
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required, StringLength(maximumLength: 75)]
        public string Email { get; set; }
        [Required, StringLength(maximumLength: 75)]
        public string FirstName { get; set; }
        [Required, StringLength(maximumLength: 75)]
        public string LastName { get; set; }
        public int Age { get; set; }
        public bool Sex { get; set; }
        public virtual ICollection<UserImage> UserImages { get; set; }
    }
}
