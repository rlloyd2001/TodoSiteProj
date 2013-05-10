using FubuMVC.Core;

namespace TodoSite.TaskList
{
    public interface ITaskInput
    {
        int Index { get; set; }
    }

    public class EditTaskInputModel : ITaskInput
    {
        public int Index { get; set; }
    }

    public class DeleteTaskInputModel : ITaskInput
    {
        public int Index { get; set; }
    }
}