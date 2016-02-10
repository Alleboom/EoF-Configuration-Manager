using Caliburn.Micro;
using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace EoF_Configuration_Manager.ViewModels.UserControls
{
    public class MachinesViewModel : PropertyChangedBase
    {

        public event EventHandler MachineInfoChanged = new EventHandler(doNothing);
        private ObservableCollection<EoF_Configuration_Manager.Model.EoF_Configuration_Database_TEST_REGIONDataSet.MachinesRow> _machinesList = new ObservableCollection<Model.EoF_Configuration_Database_TEST_REGIONDataSet.MachinesRow>();
        private Model.EoF_Configuration_Database_TEST_REGIONDataSet.MachinesRow _selectedMachine;

        private static void doNothing(object sender, EventArgs e)
        {
            
        }

        /// <summary>
        /// All the machines in the database
        /// </summary>
        public ObservableCollection<Model.EoF_Configuration_Database_TEST_REGIONDataSet.MachinesRow> Machines
        {
            get
            {
                _machinesList.Clear();
                try
                {
                    using (var conn = new Model.EoF_Configuration_Database_TEST_REGIONDataSetTableAdapters.MachinesTableAdapter())
                    {
                        foreach (var machine in conn.GetData())
                        {
                            _machinesList.Add(machine);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                return _machinesList;
            }


        }

        /// <summary>
        /// The selected machine
        /// </summary>
        public Model.EoF_Configuration_Database_TEST_REGIONDataSet.MachinesRow SelectedMachine
        {
            get
            {
                return _selectedMachine;
            }
            set
            {
                _selectedMachine = value;
                NotifyOfPropertyChange(() => SelectedMachine);
            }
        }

        /// <summary>
        /// Adds a machine to our database
        /// </summary>
        public void AddMachine()
        {
            try
            {
                using (var conn = new Model.EoF_Configuration_Database_TEST_REGIONDataSetTableAdapters.MachinesTableAdapter())
                {
                    conn.Insert(Helpers.Globals.DEFAULT_MACHINE_NAME, Helpers.Globals.DEFAULT_MACHINE_ASSETTAG, Helpers.Globals.DEFAULT_MACHINE_GROUP, Helpers.Globals.DEFAULT_MACHINE_EPNAME, Helpers.Globals.DEFAULT_MACHINE_IP);
                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            MachineInfoChanged.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// Saves the changes made in the table to the machines
        /// </summary>
        public void SaveChanges()
        {
                try
                {
                    using (var conn = new Model.EoF_Configuration_Database_TEST_REGIONDataSetTableAdapters.MachinesTableAdapter())
                    {
                        foreach (var machine in _machinesList)
                        {
                            conn.Update(machine);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                MachineInfoChanged.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// Removes a selected Machine from the database
        /// </summary>
        public void RemoveMachine()
        {
            if (Helpers.CommonPrompts.YesNoDialog.PromptForDeletion() == MessageBoxResult.Yes && SelectedMachine != null)
            {
                try
                {
                    using (var conn = new Model.EoF_Configuration_Database_TEST_REGIONDataSetTableAdapters.MachinesTableAdapter())
                    {
                        conn.DeleteQuery(SelectedMachine.ID);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Please select a machine");
            }
            MachineInfoChanged.Invoke(this, new EventArgs());
        }
    }
}
