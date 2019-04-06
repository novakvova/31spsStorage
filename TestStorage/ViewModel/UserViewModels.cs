using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestStorage.ViewModel
{
    public class UserViewModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public bool UserSex { get; set; }
        public string UserEmail { get; set; }
        public int UserAge { get; set; }
        public List<UserImageViewModel> Images { get; set; }
    }

    public class UserImageViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
