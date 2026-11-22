using System.Windows;
using System.Security.Principal;

namespace WindowsOptimizer.Gui
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var isAdmin = System.Security.Principal.WindowsIdentity.GetCurrent()?
                .Owner?
                .IsWellKnown(System.Security.Principal.WellKnownSidType.BuiltinAdministratorsSid) ?? false;
            if (!isAdmin)
            {
                MessageBox.Show("Программа должна быть запущена от имени администратора для корректной работы твиков.", 
                                "Требуются права администратора", 
                                MessageBoxButton.OK, 
                                MessageBoxImage.Error);
                Shutdown();
            }
        }
    }
}