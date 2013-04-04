using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TodoSite
{
    public class UserInputModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class UserModel : UserInputModel
    {
        public long ID { get; set; }
    }
}