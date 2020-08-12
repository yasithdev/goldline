using System.Windows;
using Core.Employees.Users;

namespace Presentation
{
    /// <summary>
    ///     Interaction logic for App
    /// </summary>
    public partial class App : Application
    {
        private static User _currentUser;

        public static User CurrentUser
        {
            get { return _currentUser; }
            set
            {
                _currentUser = value;
                UserPermissions.SetUserType(_currentUser.TypeName);
            }
        }

        public static string DefaultPassword => "defaultPW";
    }
}