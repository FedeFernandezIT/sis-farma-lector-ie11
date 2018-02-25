using System;
using System.Diagnostics;
using System.Reflection;
using System.Security.Principal;
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
            base.OnStartup(e);
        }

        private bool IsRunAsAdministrator()
        {
            var wi = WindowsIdentity.GetCurrent();
            var wp = new WindowsPrincipal(wi);

            return wp.IsInRole(WindowsBuiltInRole.Administrator);
        }
    }
}
