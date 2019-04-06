using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestStorage.Entities
{
    [Table("tblUserImages")]
    public class UserImage
    {
        [Key]
        public int Id { get; set; }
        [Required, StringLength(maximumLength: 75)]
        public string Name { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        public virtual User User { get; set; }

    }
}
