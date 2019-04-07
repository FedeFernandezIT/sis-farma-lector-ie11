using Lector.Sharp.Wpf.Helpers;
using System;
using System.Deployment.Application;
using System.Diagnostics;
using System.Reflection;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Lector.Sharp.Wpf
{
    /// <summary>
    /// Lógica de interacción para App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            if (!ApplicationDeployment.IsNetworkDeployed)
            {
                MessageBox.Show("Sisfarma Lector debe instalarse como Administrador.");
                Application.Current.Shutdown();
            }

            if (!IsRunAsAdministrator())
            {
                var processInfo = new ProcessStartInfo(Assembly.GetExecutingAssembly().CodeBase);

                // Configurar Lector como Administrador
                processInfo.UseShellExecute = true;
                processInfo.Verb = "runas";

                // Ejecutar Lector
                try
                {
                    Process.Start(processInfo);
                }
                catch (Exception ex)
                {
                    // El usuario no permite que Lector se ejecute como Administrador
                    MessageBox.Show("Sisfarma Lector, sólo puede ejecutarse como Administrador.");
                }
                // Cerrar Lector (el primero que se ejecuta sin permisos)
                Application.Current.Shutdown();
            }

            if (IsRunAsAdministrator() && !Current.SetSingleInstance())
            {
                MessageBox.Show("Application is already running!",
                                "ALREADY ACTIVE",
                                MessageBoxButton.OK,
                                MessageBoxImage.Exclamation);
                Current.Shutdown(-1);
            }
            
            base.OnStartup(e);
        }

        private bool IsRunAsAdministrator()
        {
            var wi = WindowsIdentity.GetCurrent();
            var wp = new WindowsPrincipal(wi);

            return wp.IsInRole(WindowsBuiltInRole.Administrator);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            // last change for cleanup code here!            

            // clear to exit app
            base.OnExit(e);
            
            // only restart if user requested, not an unhandled app exception...
            Current.RestartIfRequired();
        }
    }
}
