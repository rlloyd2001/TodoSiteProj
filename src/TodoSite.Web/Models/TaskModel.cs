using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FubuMVC.Core.Registration;
using FubuMVC.Validation;

//using FubuMVC.Validation;

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

    public class TaskInputModelOverrides : OverridesFor<TaskInputModel>
    {
        public TaskInputModelOverrides()
        {
            Property(x => x.Description).Required();
            Property(x => x.Date).Required();
        }
    }
}