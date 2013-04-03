using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TodoSite
{
    public class TaskModel
    {
        public string Description { get; set; }
        public DateTime Date { get; set; }
    }

    public class TaskInputModel : TaskModel
    {
        public int Index { get; set; }
    }
}