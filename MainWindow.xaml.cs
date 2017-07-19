using System;
using System.Windows;
using System.Windows.Threading;
using System.Collections.Generic;

using System.Windows.Media;

using System.Windows.Controls;
namespace IPMonitor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        Timer timer1 = new Timer();
        public MainWindow()
        {
            InitializeComponent();
            this.Title = "IPMonitor";

            SetCurrentIpAddress();
            timer1.initialIP = timer1.currentIP;
            lblInitalIP.Content = timer1.initialIP;
            timer1.currentIPmsg = timer1.initialIP.ToString();
 
            UpdateStatistics();

            //   timer1 = new Timer();
          //  Settings.SetAppsToKill();
            buttonKill.Content = "Kill " + Settings.appToKillList;
            //timer1.initialIP = timer1.web.GetIpifyIPAddress();
            //timer1.currentIP = timer1.initialIP;


            log4net.Config.XmlConfigurator.Configure();
            Logger.logger.Debug("Start");
            Logger.logger.InfoFormat("IP={0}", timer1.currentIPmsg);
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(Settings.IPCheckTimerInterval);
            timer.Tick += timer_Tick;
            timer.Start();

            DispatcherTimer timerPeriodicLogUpdate = new DispatcherTimer();
            timerPeriodicLogUpdate.Interval = TimeSpan.FromSeconds(Settings.LogEntryWritePeriod);
            timerPeriodicLogUpdate.Tick += timerPeriodicLogUpdate_Tick;
            timerPeriodicLogUpdate.Start();

            Closing += new System.ComponentModel.CancelEventHandler(MainWindow_Closing);


        }
        void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MessageBox.Show("DO YOU WANT TO CLOSE?", "CLOSING", MessageBoxButton.YesNo) == MessageBoxResult.No)
            {
                e.Cancel = true;
            }
        }

        void timerPeriodicLogUpdate_Tick(object sender, EventArgs e)
        {
            Logger.logger.InfoFormat("IP={0}", timer1.currentIPmsg);
        }

        void timer_Tick(object sender, EventArgs e)
        {
            string time = Utilities.GetDateTime();

            textTime.Text = time;
            SetCurrentIpAddress();
            if (timer1.currentIP.ToString() != timer1.initialIP.ToString())
            {
                timer1.currentIPmsg = timer1.currentIP.ToString();
                Utilities.KillProgram(Settings.appToKillArray); // Continue killing until reset

                UpdateStatistics();

                if (timer1.killActive == false)
                {
                    string status = Settings.appToKillList + " killed at " + time;
                    Logger.logger.InfoFormat("{0}", status);
                    labelStatus.Content = status;
                    timer1.killActive = true; //Kill only once

                }
            }

            if (timer1.killActive)
            {
                textKillStatus.Foreground = new SolidColorBrush(Colors.Red);
            }
            else
            {
                textKillStatus.Foreground = new SolidColorBrush(Colors.Green);

            }
            textKillStatus.Text = timer1.killActive.ToString();
        }
        private void SetCurrentIpAddress()
        {
            timer1.SetCurrentlIP(timer1.web.GetIpifyIPAddress(Web.URL_IpiFy));
            lblCurrentIP.Content = timer1.currentIP;

        }
        private void UpdateStatistics()
        {

            IpInfoLocation ipInfo = timer1.ipInfo.GetIpInfo();
            Dictionary<string, string> statistics = timer1.ipInfo.LocationToDictionary(ipInfo);

            dataGridStatistics.ItemsSource = statistics;

        }
        ///--------------------------------------------------------------------------------------
        private void buttonKill_Click(object sender, RoutedEventArgs e)
        {
            Utilities.KillProgram(Settings.appToKillArray);
        }

        private void buttonResetIp_Click(object sender, RoutedEventArgs e)
        {
            lblInitalIP.Content = timer1.ResetIpString();

        }

        private void buttonSetCustomIP_Click(object sender, RoutedEventArgs e)
        {
            timer1.SetInitalIP(labelCustomIPValue.Content.ToString());
            lblInitalIP.Content = timer1.initialIP;
            Logger.logger.InfoFormat("IP Manually reset to custom IP {0}", timer1.initialIP);
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            //     Web web = new Web();
            timer1.ipInfo.GetIpInfo();
        }

        private void buttonRunNotepad_Click(object sender, RoutedEventArgs e)
        {
            Utilities.RunProcess("notepad.exe");
        }
    }
}