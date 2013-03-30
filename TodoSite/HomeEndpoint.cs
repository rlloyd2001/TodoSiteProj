using System.Web.Routing;
using Bottles;
using FubuMVC.Core;
using FubuMVC.StructureMap;

using System.Collections.Generic;

namespace TodoSite
{
    public class UserInputModel    
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

	public class HomeModel
	{
		public string Message { get; set; }
	}

    public class HomeListModel
    {
        public List<HomeModel> TextList = new List<HomeModel> 
        {
            new HomeModel { Message = "abc" },            
            new HomeModel { Message = "efg" }
        };
    }
	
	// Fubu's default policies look for classes suffixed with "Endpoint" or "Endpoints"
    public class HomeEndpoint
	{
		// Fubu will use HomeEndpoint.Index as the default "home" route
        public UserInputModel Index()
		{
            return new UserInputModel();
		}

        public HomeListModel ListTodo(UserInputModel inputModel)
        {
            return new HomeListModel();
        }
	}

}