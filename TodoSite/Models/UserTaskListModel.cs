using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TodoSite
{
    public interface IUserTaskListModel
    {
        //user ID belonging to the current user
        //determine in the constructor
        long UserID { get; }

        IReadOnlyList<TaskModel> Tasks { get; }

        IReadOnlyList<UserModel> Users { get; }

        string FullName { get; }
    }

    public class UserTaskListModel
    {
        public UserTaskListModel()
        {
            Setup(null, 0);
        }

        public UserTaskListModel(long userID)
        {
            Setup(null, userID);
        }

        public UserTaskListModel(UserModel userModel)
        {
            Setup(userModel, 0);
        }
        
        private void Setup(UserModel userModel, long userID)
        {
            LoadSession();            
            //check if user exists
            if (userModel == null) {
                if (userTaskList.ContainsKey(userID) == false) {
                    userModel = new UserModel { ID = userID, FirstName = "", LastName = "" };
                    userTaskList[userID] = new TaskListModel(userModel);
                }
                else {
                    userModel = userTaskList[userID].User;
                }
            }
            //find user
            var result = from l in userTaskList.Values
                         where l.User.FirstName.Equals(userModel.FirstName, StringComparison.CurrentCultureIgnoreCase)
                         && l.User.LastName.Equals(userModel.LastName, StringComparison.CurrentCultureIgnoreCase)
                         select l;
            if (result.Count() > 0) {
                this.userID = result.ElementAt(0).User.ID;
            }
            else {
                //new user
                var maxResult = userTaskList.Values.Max(n => n.User.ID);
                userID = maxResult++;
                long v = 0;
                while (userTaskList.ContainsKey(userID)) {
                    userID++;
                    v++; if (v == long.MaxValue) { throw new SystemException("userID loop error in UserTaskListModel"); }
                }
                userModel.ID = userID;
                userTaskList[userID] = new TaskListModel(userModel);
                this.userID = userID;
                SaveSession();
            }
        }

        private static string SessionKey = "SimpleTaskMgr";

        private Dictionary<long, TaskListModel> userTaskList;
        private long userID;
        private int editTaskIndex = -1;
        private int addTaskIndex = -1;

        public long UserID
        {
            get
            {
                return userID;
            }
        }

        public IReadOnlyList<TaskModel> List
        {
            get
            {
                return userTaskList[userID].List.AsReadOnly();
            }
        }

        public IReadOnlyList<TaskListModel> TaskUserList
        {
            get
            {
                return (new List<TaskListModel>(
                    userTaskList.Values.AsEnumerable<TaskListModel>())
                    ).AsReadOnly();
            }
        }

        public string FullName
        {
            get
            {
                return userTaskList[userID].User.FirstName + " " +
                    userTaskList[userID].User.LastName;
            }
        }

        public int EditTaskIndex
        {
            get { return editTaskIndex; }
            set { editTaskIndex = value; }
        }

        public int AddTaskIndex
        {
            get { return addTaskIndex; }
            set { addTaskIndex = value; }
        }

        public void UpdateTask(int index, TaskModel taskModel)
        {
            userTaskList[userID].List[index] = taskModel;
        }

        public void AppendTask(string description, DateTime date)
        {
            userTaskList[userID].List.Add(
                new TaskModel { Description = description, Date = date });
        }
        public bool DeleteTask(int index)
        {
            if (index < 0) { return false; }
            if (userTaskList[userID].List.Count <= index) { return false; }
            userTaskList[userID].List.RemoveAt(index);
            return true;
        }

        private void CreateUserTaskListData()
        {
            long id = 1;
            userTaskList[id] = new TaskListModel
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
            userTaskList[id] = new TaskListModel
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
            userTaskList[id] = new TaskListModel
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
                userTaskList = (Dictionary<long, TaskListModel>)HttpContext.Current.Session[SessionKey];
            }
            else {
                userTaskList = new Dictionary<long, TaskListModel>();
                CreateUserTaskListData(); //default entries in data for testing
                SaveSession();
            }
        }

        private void SaveSession()
        {
            HttpContext.Current.Session[SessionKey] = userTaskList;
        }
    }
}