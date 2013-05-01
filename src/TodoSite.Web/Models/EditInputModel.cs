using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using FubuMVC.Core;

namespace TodoSite
{
    public class EditTaskInputModel
    {
        [RouteInput]
        public int Index { get; set; }
    }

    public class DeleteTaskInputModel
    {
        [RouteInput]
        public int Index { get; set; }
    }
}