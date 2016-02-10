using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EoF_Configuration_Manager
{
    public class AppBootStrapper : BootstrapperBase
    {

        public AppBootStrapper()
        {
            Initialize();
        }

        protected override void OnStartup(object sender, System.Windows.StartupEventArgs e)
        {
            DisplayRootViewFor<ViewModels.MainWindowViewModel>();
        }

    }
}
