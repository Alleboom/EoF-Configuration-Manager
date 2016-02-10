using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Windows;

namespace EoF_Configuration_Manager.ViewModels.UserControls
{
    public class SelectedMachineSoftwareViewModel : PropertyChangedBase
    {
        private Model.EoF_Configuration_Database_TEST_REGIONDataSet.MachinesRow _selectedMachine;
        private Model.EoF_Configuration_Database_TEST_REGIONDataSet.SoftwareRow _selectedRegisteredSoftware;
        private Model.EoF_Configuration_Database_TEST_REGIONDataSet.SoftwareRow _selectedAvailableSoftware;

        /// <summary>
        /// All the machines in the database
        /// </summary>
        public ObservableCollection<Model.EoF_Configuration_Database_TEST_REGIONDataSet.MachinesRow> Machines
        {
            get
            {
                var _ltr = new ObservableCollection<Model.EoF_Configuration_Database_TEST_REGIONDataSet.MachinesRow>();

                try
                {
                    using (var conn = new Model.EoF_Configuration_Database_TEST_REGIONDataSetTableAdapters.MachinesTableAdapter())
                    {
                        foreach (var machine in conn.GetData())
                        {
                            _ltr.Add(machine);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                return _ltr;
            }
        }

        /// <summary>
        /// The selected machine we want to query against
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
                NotifyOfPropertyChange(() => RegisteredSoftware);
                NotifyOfPropertyChange(() => AvailableSoftware);
            }
        }

        /// <summary>
        /// Returns all the software tied to a machine
        /// </summary>
        public ObservableCollection<Model.EoF_Configuration_Database_TEST_REGIONDataSet.SoftwareRow> RegisteredSoftware
        {
            get
            {
                var _ltr = new ObservableCollection<Model.EoF_Configuration_Database_TEST_REGIONDataSet.SoftwareRow>();
                if(SelectedMachine != null){
                try
                {
                    if (SelectedMachine != null)
                    {
                        using (var software_table = new Model.EoF_Configuration_Database_TEST_REGIONDataSetTableAdapters.SoftwareTableAdapter())
                        {
                            using (var associcate_table = new Model.EoF_Configuration_Database_TEST_REGIONDataSetTableAdapters.MachinesSoftwareTableAdapter())
                            {
                                // get all of our software entry ids
                                var _associations = (from entry in associcate_table.GetData()
                                                     where entry.MachineId == SelectedMachine.ID
                                                     select entry).ToList();
                                // for each of them, get the updated data from the database
                                foreach (var row in _associations)
                                {
                                    var _software = (from entry in software_table.GetData()
                                                     where entry.ID == row.SoftwareId
                                                     select entry).Single();

                                    // check for null and add to our return list
                                    if (_software != null)
                                    {
                                        _ltr.Add(_software);
                                    }
                                }
                            }
                        }
                    }
                }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                return _ltr;
            }
        }

        public ObservableCollection<Model.EoF_Configuration_Database_TEST_REGIONDataSet.SoftwareRow> AvailableSoftware
        {
            get
            {
                var _ltr = new ObservableCollection<Model.EoF_Configuration_Database_TEST_REGIONDataSet.SoftwareRow>();
                var _dontadd = new ObservableCollection<Model.EoF_Configuration_Database_TEST_REGIONDataSet.SoftwareRow>();
                try
                {
                    if (SelectedMachine != null)
                    {
                        using (var softconn = new Model.EoF_Configuration_Database_TEST_REGIONDataSetTableAdapters.SoftwareTableAdapter())
                        {
                            using (var assoconn = new Model.EoF_Configuration_Database_TEST_REGIONDataSetTableAdapters.MachinesSoftwareTableAdapter())
                            {
                                // grab all of our associations
                                var assoc = from _associ in assoconn.GetData()
                                            where SelectedMachine.ID == _associ.MachineId
                                            select _associ;
                                // wherever we have an assocation, don't put the software in the list
                                foreach (var _software in softconn.GetData())
                                {
                                    foreach (var _assoc in assoc)
                                    {
                                        if (_assoc.SoftwareId == _software.ID)
                                        {
                                            _dontadd.Add(_software);
                                        }
                                    }
                                    if (!_dontadd.Contains(_software))
                                    {
                                        _ltr.Add(_software);
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                return _ltr;
            }
        }

        public Model.EoF_Configuration_Database_TEST_REGIONDataSet.SoftwareRow SelectedRegisteredSoftware
        {
            get
            {
                return _selectedRegisteredSoftware;
            }
            set
            {
                _selectedRegisteredSoftware = value;
                NotifyOfPropertyChange(() => SelectedRegisteredSoftware);
            }
        }

        public Model.EoF_Configuration_Database_TEST_REGIONDataSet.SoftwareRow SelectedAvailableSoftware
        {
            get
            {
                return _selectedAvailableSoftware;
            }
            set
            {
                _selectedAvailableSoftware = value;
                NotifyOfPropertyChange(() => SelectedAvailableSoftware);
            }
        }

        /// <summary>
        /// Adds and entry to our registered software
        /// </summary>
        public void Add()
        {
            if (SelectedAvailableSoftware != null)
            {
                try
                {
                    using (var conn = new Model.EoF_Configuration_Database_TEST_REGIONDataSetTableAdapters.MachinesSoftwareTableAdapter())
                    {
                        conn.Insert(SelectedMachine.ID, SelectedAvailableSoftware.ID);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                NotifyOfPropertyChange(() => AvailableSoftware);
                NotifyOfPropertyChange(() => RegisteredSoftware);
            }
        }

        /// <summary>
        /// Removes entry from registered software
        /// </summary>
        public void Remove()
        {
            try
            {
                using (var conn = new Model.EoF_Configuration_Database_TEST_REGIONDataSetTableAdapters.MachinesSoftwareTableAdapter())
                {
                    if (SelectedMachine != null && SelectedRegisteredSoftware != null)
                    {
                        conn.DeleteQuery(SelectedMachine.ID, SelectedRegisteredSoftware.ID);
                    }
                }
            }
            catch (Exception ex){
                MessageBox.Show(ex.Message);
            }
            NotifyOfPropertyChange(() => RegisteredSoftware);
            NotifyOfPropertyChange(() => AvailableSoftware);
        }

        /// <summary>
        /// Generates a pdf report of all the information relevant to that machine
        /// </summary>
        public void PrintReport()
        {
            var propDic = new Dictionary<String, String>();

            propDic.Add("ComputerName", "Computer Name: ");
            propDic.Add("AssetTag", "Asset Number: ");
            propDic.Add("UpdateGroup", "Update Policy: ");
            propDic.Add("EPPositionName", "EP Title: ");
            propDic.Add("IPStatic", "Static IP Address: ");

            try
            {
                using (var assocConn = new Model.EoF_Configuration_Database_TEST_REGIONDataSetTableAdapters.MachinesSoftwareTableAdapter())
                {
                    using (var softConn = new Model.EoF_Configuration_Database_TEST_REGIONDataSetTableAdapters.SoftwareTableAdapter())
                    {
                        // grab the software relevant to the machine
                        var _software = from _soft in softConn.GetData()
                                        join _assoc in assocConn.GetData()
                                        on _soft.ID equals _assoc.SoftwareId
                                        where SelectedMachine.ID == _assoc.MachineId
                                        select _soft;
                        int i = 0;
                        foreach (var software in _software)
                        {
                            propDic.Add("Software Number " + i.ToString() + ": ", software.SoftwareName);
                            i++;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            Helpers.Reports.PDFIndividualReport.GenerateReport<Model.EoF_Configuration_Database_TEST_REGIONDataSet.MachinesRow>(SelectedMachine, propDic, SelectedMachine.ComputerName);

        }

    }



}
