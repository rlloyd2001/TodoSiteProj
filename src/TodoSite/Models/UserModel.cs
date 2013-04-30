using System;
using System.Linq;
using FubuCore.Reflection;
using FubuLocalization;
using FubuMVC.Core.Registration;
using FubuMVC.Validation;
using FubuMVC.Validation.Remote;
using FubuValidation;

namespace TodoSite
{
    public class UserInputModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime CurrentTime { get; set; }

        public string FullName
        {
            get { return FirstName + " " + LastName; }
        }
    }

    public class UserInputModelOverrides : OverridesFor<UserInputModel>
    {
        public UserInputModelOverrides()
        {
            Property(x => x.FirstName).Required();
            Property(x => x.LastName).Required();

            Property(x => x.FirstName).Add<CheckLengthName>();
            Property(x => x.LastName).Add<CheckLengthName>();
        }
    }

    public class CheckLengthName : IRemoteFieldValidationRule
    {
        public StringToken Token { get; set; }

        public void Validate(Accessor accessor, ValidationContext context)
        {
            var name = context.GetFieldValue<string>(accessor);
            
            if (name.Length < 2)
            {
                context
                    .Notification
                    .RegisterMessage(accessor, StringToken.FromKeyString("Name:CheckLength", "Name {name} too short"),
                                     TemplateValue.For("name", name));
            }
        }
    }

    public class UserModel : UserInputModel
    {
    }
}