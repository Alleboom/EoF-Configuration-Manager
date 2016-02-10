using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;

namespace EoF_Configuration_Manager.ViewModels.UserControls
{
    public class SoftwareViewModel : PropertyChangedBase
    {
        private ObservableCollection<Model.EoF_Configuration_Database_TEST_REGIONDataSet.SoftwareRow> _allSoftware = new ObservableCollection<Model.EoF_Configuration_Database_TEST_REGIONDataSet.SoftwareRow>();

        /// <summary>
        /// All of the software in the database
        /// </summary>
        public ObservableCollection<Model.EoF_Configuration_Database_TEST_REGIONDataSet.SoftwareRow> Softwares
        {
            get
            {
                try
                {
                    _allSoftware.Clear();
                    using (var conn = new Model.EoF_Configuration_Database_TEST_REGIONDataSetTableAdapters.SoftwareTableAdapter())
                    {
                        foreach (var software in conn.GetData())
                        {
                            _allSoftware.Add(software);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                return _allSoftware;
            }
        }

        /// <summary>
        /// Adds a software data object
        /// </summary>
        public void Add()
        {
            try
            {
                using (var conn = new Model.EoF_Configuration_Database_TEST_REGIONDataSetTableAdapters.SoftwareTableAdapter())
                {
                    conn.Insert(Helpers.Globals.DEFAULT_SOFTWARE_NAME, Helpers.Globals.DEAFULT_SOFTWARE_VENDOR, 
                        Helpers.Globals.DEFAULT_SOFTWARE_VERSION, Helpers.Globals.DEFAULT_SOFTWARE_DESCRIPTION, Helpers.Globals.DEFAULT_SOFTWARE_PURPOSE);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            SoftwareInfoChanged.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// Saves the changes made in data grid etc.
        /// </summary>
        public void Save()
        {
            try
            {
                using (var conn = new Model.EoF_Configuration_Database_TEST_REGIONDataSetTableAdapters.SoftwareTableAdapter())
                {
                    foreach (var software in _allSoftware)
                    {
                        conn.Update(software);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            SoftwareInfoChanged.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// Removes the row from the database
        /// </summary>
        public void Remove()
        {
            try
            {
                using (var conn = new Model.EoF_Configuration_Database_TEST_REGIONDataSetTableAdapters.SoftwareTableAdapter())
                {
                    conn.DeleteQuery(SelectedSoftware.ID);    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            SoftwareInfoChanged.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// The selected piece of software
        /// </summary>
        public Model.EoF_Configuration_Database_TEST_REGIONDataSet.SoftwareRow SelectedSoftware
        {
            get
            {
                return _selectedSoftware;
            }
            set
            {
                _selectedSoftware = value;
                NotifyOfPropertyChange(() => SelectedSoftware);
            }
        }

        public event EventHandler SoftwareInfoChanged;
        private Model.EoF_Configuration_Database_TEST_REGIONDataSet.SoftwareRow _selectedSoftware;

        public SoftwareViewModel()
        {
            SoftwareInfoChanged = new EventHandler(DoNothing);
        }

        private void DoNothing(object sender, EventArgs e)
        {
            
        }


    }
}
