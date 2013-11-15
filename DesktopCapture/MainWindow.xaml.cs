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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using System.Drawing;
using System.Windows.Threading;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;

namespace DesktopCapture
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private string userLogin;
        private string userPassword;
        private NotifyIcon trayIcon;
        private System.Windows.Forms.ContextMenu trayMenu;

        private bool _isUserLoggedIn = false;

        private FocusedWindowManager _focusedWindowManager = new FocusedWindowManager();

        public MainWindow()
        {
            InitializeComponent();

            FocusedWindow.SetupPrograms();

            trayIcon = new NotifyIcon();
            trayIcon.Icon = new System.Drawing.Icon("techSmith-01.ico");
            trayIcon.Visible = true;

            trayMenu = new System.Windows.Forms.ContextMenu();
            trayMenu.MenuItems.Add(0, new System.Windows.Forms.MenuItem("Show", new System.EventHandler(Show_Click)));
            trayMenu.MenuItems.Add(1, new System.Windows.Forms.MenuItem("Add/Remove Program", new System.EventHandler(Add_Click)));
            trayMenu.MenuItems.Add(2, new System.Windows.Forms.MenuItem("Logout", new System.EventHandler(Logout_Click)));
            trayMenu.MenuItems.Add(3, new System.Windows.Forms.MenuItem("Exit", new System.EventHandler(Exit_Click)));

            trayIcon.ContextMenu = trayMenu;

            UserField.Focus();

        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            LogIntoTinCan();
        }

        private void LogIntoTinCan()
        {
            userLogin = UserField.Text;
            userPassword = passwordBox1.Password;
            _isUserLoggedIn = TinCan.ConnectToTinCan(userLogin, userPassword);
            if (_isUserLoggedIn)
            {
                _focusedWindowManager.StartWatching();
                passwordBox1.Clear();
                MinimizeWindow();
            }
            else
            {
                UserField.Clear();
                passwordBox1.Clear();
            }
        }

        protected void Show_Click(Object sender, System.EventArgs e)
        {
            ReviveWindow();
        }

        protected void Add_Click(Object sender, System.EventArgs e)
        {
            AddProgramWindow newWindow = new AddProgramWindow();
            App.Current.MainWindow = newWindow;
            //MinimizeWindow();
            //this.Close();
            newWindow.Show();
        }

        protected void Logout_Click(Object sender, System.EventArgs e)
        {
            LogOut();
        }

        protected void LogOut()
        {
            _focusedWindowManager.ClearActivities();
            TinCan.LogoutTinCan();
        }

        protected void Exit_Click(Object sender, System.EventArgs e)
        {
            Close();
        }

        private void ReviveWindow()
        {
            this.Show();
            this.WindowState = System.Windows.WindowState.Normal;
        }

        private void MinimizeWindow()
        {
            this.WindowState = System.Windows.WindowState.Minimized;
            this.Hide();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            trayIcon.Visible = false;
        }

        private void UserField_OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                LogIntoTinCan();
            }
        }

        private void PasswordField_OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                LogIntoTinCan();
            }
        }
    }
}
