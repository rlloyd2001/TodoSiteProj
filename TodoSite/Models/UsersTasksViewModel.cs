using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TodoSite
{
    public interface IUsersTasksViewModel
    {        
        //select a different user model with this setting
        long UserID { set; }

        //create or select a different user model with this setting
        UserInputModel UserInput { set; }

        //return the selected user model or null
        UserModel User { get; }
        
        //return a list of tasks that are part of the selected user
        IReadOnlyList<TaskModel> Tasks { get; }

        //return a list of all the users
        IReadOnlyList<UserModel> Users { get; }
        
        //replace task at index with new TaskModel
        void UpdateTask(int index, TaskModel taskModel);

        //index for task that was last updated
        int EditTaskIndex { get; set; }

        //add new task
        void AddTask(TaskModel taskModel);

        //index for task that was last added
        int AddedTaskIndex { get; }

        //delete task at index
        void DeleteTask(int index);
    }

    public class UsersTasksViewModel : IUsersTasksViewModel
    {
        public UsersTasksViewModel()
        {
            LoadSession();
        }      

        private static string SessionKey = "SimpleTaskMgr";

        private Dictionary<long, UserTasksModel> userTasks;
        private int addedTaskIndex = -1;
        private int editTaskIndex = -1;
        private long userID = -1;

        public DateTime LoginTime { get; set; }

        public long UserID
        {
            set
            {
                userID = value;
            }
        }

        public UserInputModel UserInput
        {
            set
            {
                UserInputModel uInput = value;
                //check if user exists
                if (uInput == null) {
                    return;
                }
                LoginTime = uInput.CurrentTime;
                //find user
                var result = from l in userTasks.Values
                             where l.User.FirstName.Equals(uInput.FirstName, StringComparison.CurrentCultureIgnoreCase)
                             && l.User.LastName.Equals(uInput.LastName, StringComparison.CurrentCultureIgnoreCase)
                             select l;
                if (result.Count() > 0) {
                    userID = result.ElementAt(0).User.ID;
                }
                else {
                    //new user
                    var maxResult = userTasks.Values.Max(n => n.User.ID);
                    userID = maxResult++;
                    long v = 0;
                    while (userTasks.ContainsKey(userID)) {
                        userID++;
                        v++; if (v == long.MaxValue) { throw new SystemException("userID loop error in UserTaskListModel"); }
                    }
                    UserModel userModel = new UserModel { ID = userID, FirstName = uInput.FirstName, LastName = uInput.LastName };
                    userTasks[userID] = new UserTasksModel(userModel);
                    SaveSession();
                }
            }
        }

        public UserModel User
        {
            get
            {
                if (userTasks.ContainsKey(userID)) {
                    return userTasks[userID].User;
                }
                return null;
            }
        }

        public IReadOnlyList<TaskModel> Tasks
        {
            get
            {
                if (userTasks.ContainsKey(userID)) {
                    return userTasks[userID].List.AsReadOnly();
                }
                return null;
            }
        }
        
        public IReadOnlyList<UserModel> Users
        {
            get
            {
                var result =
                    from m in userTasks.Values
                    select m.User;
                return result.ToList<UserModel>().AsReadOnly();
            }
        }

        public int EditTaskIndex { get { return editTaskIndex; } set { editTaskIndex = value; } }

        public int AddedTaskIndex
        {
            get { return addedTaskIndex; }
        }

        public void UpdateTask(int index, TaskModel taskModel)
        {
            if (userTasks.ContainsKey(userID)) {
                userTasks[userID].List[index] = taskModel;
            }
        }

        public void AddTask(TaskModel taskModel)
        {
            if (userTasks.ContainsKey(userID)) {
                userTasks[userID].List.Add(taskModel);
                addedTaskIndex = userTasks[userID].List.Count - 1;
            }
        }

        public void DeleteTask(int index)
        {
            if (userTasks.ContainsKey(userID)) {
                if (index < 0) { return; }
                if (userTasks[userID].List.Count <= index) { return; }
                userTasks[userID].List.RemoveAt(index);
            }
        }

        private void CreateUserTaskListData()
        {
            long id = 1;
            userTasks[id] = new UserTasksModel
            {
                User = new UserModel
                {
                    FirstName = "Frank",
                    ID = id,
                    LastName = "O"
                },
                List = new List<TaskModel>
                {
                    new TaskModel {
                        Description = "Take out garbage",
                        Date = DateTime.Now
                    },                    
                    new TaskModel {
                        Description = "Wash the car",
                        Date = DateTime.Now
                    }
                }
            };
            id++;
            userTasks[id] = new UserTasksModel
            {
                User = new UserModel
                {
                    FirstName = "Billy",
                    ID = id,
                    LastName = "Jean"
                },
                List = new List<TaskModel>
                {
                    new TaskModel {
                        Description = "Wash car",
                        Date = DateTime.Now
                    }
                }
            };
            id++;
            userTasks[id] = new UserTasksModel
            {
                User = new UserModel
                {
                    FirstName = "Darth",
                    ID = id,
                    LastName = "Vader"
                },
                List = new List<TaskModel>
                {
                    new TaskModel { Description = "Brush Teeth", Date = DateTime.Now },
                    new TaskModel { Description = "Kidnap Princess Leah", Date = DateTime.Now.AddHours(2) },
                    new TaskModel { Description = "Eat Lunch", Date = DateTime.Now.AddHours(3) },
                    new TaskModel { Description = "Light Saber Battle", Date = DateTime.Now.AddHours(5) },
                    new TaskModel { Description = "Attend Comicon", Date = DateTime.Now.AddDays(2) },
                    new TaskModel { Description = "Win Oscar", Date = DateTime.Now.AddYears(1).AddHours(53) }
                }
            };
            SaveSession();
        }

        private void LoadSession()
        {
            if (HttpContext.Current.Session[SessionKey] != null) {
                userTasks = (Dictionary<long, UserTasksModel>)HttpContext.Current.Session[SessionKey];
                LoginTime = (DateTime) HttpContext.Current.Session["LoginTime"];
            }
            else {
                userTasks = new Dictionary<long, UserTasksModel>();
                CreateUserTaskListData(); //default entries in data for testing
                SaveSession();
            }
        }

        private void SaveSession()
        {
            HttpContext.Current.Session[SessionKey] = userTasks;
            HttpContext.Current.Session["LoginTime"] = LoginTime;
        }
    }
}