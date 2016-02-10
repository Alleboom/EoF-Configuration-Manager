using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EoF_Configuration_Manager.ViewModels
{
    public class MainWindowViewModel : PropertyChangedBase
    {
        public MainWindowViewModel()
        {
            MachinesVM = new ViewModels.UserControls.MachinesViewModel();
            SoftwareVM = new ViewModels.UserControls.SoftwareViewModel();
            SelectedMachineSoftwareVM = new ViewModels.UserControls.SelectedMachineSoftwareViewModel();

            MachinesVM.MachineInfoChanged += MachinesVM_MachineInfoChanged;
            SoftwareVM.SoftwareInfoChanged += SoftwareVM_PropertyChanged;
        }

        private void SoftwareVM_PropertyChanged(object sender, EventArgs e)
        {
            SoftwareVM.Refresh();
            SelectedMachineSoftwareVM.Refresh();
        }

        void MachinesVM_MachineInfoChanged(object sender, EventArgs e)
        {
            MachinesVM.Refresh();
            SelectedMachineSoftwareVM.Refresh();
        }

        public UserControls.MachinesViewModel MachinesVM { get; set; }

        public UserControls.SoftwareViewModel SoftwareVM { get; set; }

        public UserControls.SelectedMachineSoftwareViewModel SelectedMachineSoftwareVM { get; set; }
    }
}
