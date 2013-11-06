using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Management;

namespace DesktopCapture
{
    /// <summary>
    /// Interaction logic for AddProgramWindow.xaml
    /// </summary>
    public partial class AddProgramWindow : Window
    {
        private List<Process> _allProcesses = new List<Process>();
        private List<string> _allProcessesStrings = new List<string>();

        private List<string> _allProcessIDs = new List<string>();

        private List<string> _trackedPrograms = new List<string>();
        private List<string> _removedPrograms = new List<string>();

        public AddProgramWindow()
        {
            ProgramsToAdd();

            InitializeComponent();

            RefreshLists();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int index = listBox1.SelectedIndex;
                string pass = _allProcessIDs.ElementAt(index);
                _allProcessesStrings.RemoveAt(index);
                _allProcessIDs.RemoveAt(index);
                _trackedPrograms.Add(pass);
                _trackedPrograms.Sort();
            }
            catch
            {
                // DO NOTHING
            }
            RefreshLists();

        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int index = listBox2.SelectedIndex;
                string pass = _trackedPrograms.ElementAt(index);
                _trackedPrograms.RemoveAt(index);
                _removedPrograms.Add(pass);
                //_allProcessesStrings.Add(pass);
                //_allProcessesStrings.Sort();
            }
            catch
            {
                // DO NOTHING
            }
            RefreshLists();
        }

        private void RefreshLists()
        {
            _trackedPrograms.Sort();
            listBox1.ItemsSource = null;
            listBox2.ItemsSource = null;
            listBox1.ItemsSource = _allProcessesStrings;
            listBox2.ItemsSource = _trackedPrograms;
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            SetPrograms();
            RemovePrograms();
            this.Close();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ReturnToMainWindow()
        {
            MainWindow newWindow = new MainWindow();
            App.Current.MainWindow = newWindow;
            this.Close();
            newWindow.WindowState = System.Windows.WindowState.Minimized;
        }

        private void ProgramsToAdd()
        {
            //This function will pull the active processes, and allow a user to add a process to the
            //active list of acceptable programs.

            string programName;
            string programID;

            var wmiQueryString = "SELECT ProcessId, ExecutablePath, CommandLine FROM Win32_Process";
            using (var searcher = new ManagementObjectSearcher(wmiQueryString))
            using (var results = searcher.Get())
            {
                var query = from process in Process.GetProcesses()
                            join mo in results.Cast<ManagementObject>()
                            on process.Id equals (int)(uint)mo["ProcessId"]
                            select new
                            {
                                Process = process,
                                Path = (string)mo["ExecutablePath"],
                                CommandLine = (string)mo["CommandLine"],
                            };
                foreach (var item in query)
                {
                    // Do what you want with the Process, Path, and CommandLine

                    if (item.Path != null)
                    {
                        _allProcesses.Add(item.Process);
                        try
                        {

                            System.Diagnostics.FileVersionInfo versionInfo = System.Diagnostics.FileVersionInfo.GetVersionInfo(item.Path);
                            programName = versionInfo.FileDescription;
                            programID = versionInfo.OriginalFilename;

                            if (!_allProcessesStrings.Contains(programName))
                            {
                                programID = programID.Replace(".exe", "");
                                _allProcessesStrings.Add(programName);
                                _allProcessIDs.Add(programID);
                                //_allRunningProcesses.Add(versionInfo);
                            }
                        }
                        catch (Exception e)
                        {
                            
                        }
                    }
                }
            }

            //Process[] processList = Process.GetProcesses();

            //foreach (Process p in processList)
            //{
            //    string pName = p.ProcessName;

                
            //    try
            //    {
            //        programName = p.MainModule.FileVersionInfo.ProductName;

            //        _allProcesses.Add(p);
            //        if (!_allProcessesStrings.Contains(programName))
            //            _allProcessesStrings.Add(programName);

                    

            //    }
            //    catch (Exception e)
            //    {

            //    }
            //}

            //_allProcessesStrings.Sort();
            _trackedPrograms = FocusedWindow.acceptablePrograms;
        }

        private void SetPrograms()
        {
            foreach (string prog in _trackedPrograms)
            {
                FocusedWindow.AddToProgramList(prog);
            }
        }

        private void RemovePrograms()
        {
            foreach (string prog in _removedPrograms)
            {
                FocusedWindow.RemoveFromProgramList(prog);
            }
        }
    }
}
