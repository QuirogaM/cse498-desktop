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

namespace DesktopCapture
{
    /// <summary>
    /// Interaction logic for AddProgramWindow.xaml
    /// </summary>
    public partial class AddProgramWindow : Window
    {
        private List<Process> _allProcesses = new List<Process>();
        private List<string> _allProcessesStrings = new List<string>();

        public AddProgramWindow()
        {
            ProgramsToAdd();

            InitializeComponent();

            listBox1.ItemsSource = _allProcessesStrings;
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

            Process[] processList = Process.GetProcesses();

            foreach (Process p in processList)
            {
                string pName = p.ProcessName;
                _allProcesses.Add(p);
                if(!_allProcessesStrings.Contains(pName))
                    _allProcessesStrings.Add(p.ProcessName);
            }

            _allProcessesStrings.Sort();
        }
    }
}
