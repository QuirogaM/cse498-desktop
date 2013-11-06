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
        private List<string> _trackedPrograms = new List<string>();

        public AddProgramWindow()
        {
            ProgramsToAdd();

            InitializeComponent();

            listBox1.ItemsSource = _allProcessesStrings;
            listBox2.ItemsSource = _trackedPrograms;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            //TODO
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            //TODO
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            //TODO
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

                            if (!_allProcessesStrings.Contains(programName))
                                _allProcessesStrings.Add(programName);
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

            _allProcessesStrings.Sort();
            _trackedPrograms = FocusedWindow.acceptablePrograms;
        }

        private void SetPrograms()
        {
            //TODO
        }
    }
}
