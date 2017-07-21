using System;
using System.Windows;
using System.Windows.Threading;
using System.Collections.Generic;

using System.Windows.Media;

using System.Windows.Controls;
using System.Linq;
namespace IPMonitor {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {


        Timer timer1 = new Timer();
        public MainWindow() {
            InitializeComponent();
            this.Title = "IPMonitor";
            Settings.Initialize();
            CheckAndUpdateReferenceIP();
            timer1.referenceIP = timer1.currentIP;
            lblReferenceIP.Content = timer1.referenceIP;
            timer1.currentIPmsg = timer1.referenceIP.ToString();

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
            timer.Tick += IPCheckTimerInterval_Tick;
            timer.Start();

            DispatcherTimer timerPeriodicLogUpdate = new DispatcherTimer();
            timerPeriodicLogUpdate.Interval = TimeSpan.FromSeconds(Settings.LogEntryWriteInterval);
            timerPeriodicLogUpdate.Tick += timerPeriodicLogUpdate_Tick;
            timerPeriodicLogUpdate.Start();

            Closing += new System.ComponentModel.CancelEventHandler(MainWindow_Closing);


        }
        void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            if (MessageBox.Show("DO YOU WANT TO CLOSE?", "CLOSING", MessageBoxButton.YesNo) == MessageBoxResult.No) {
                e.Cancel = true;
            }
        }

        void timerPeriodicLogUpdate_Tick(object sender, EventArgs e) {
          //  Logger.logger.InfoFormat("IP={0}", timer1.currentIPmsg);
        }

        void IPCheckTimerInterval_Tick(object sender, EventArgs e) {
            string time = Utilities.GetDateTime(), msg;

            textTime.Text = time;
            CheckAndUpdateReferenceIP();

            // Note:   timer1.currentIP after checking the IP. timer1.referenceIP is the IP before  IP Check. It may be the same as currentIP
            if (timer1.currentIP.ToString() != timer1.referenceIP.ToString()) {

                timer1.currentIPmsg = timer1.currentIP.ToString();
                Utilities.KillProgram(Settings.appToKillArray); // Continue killing until reset

                UpdateStatistics();

                if (timer1.killActive == false) {
                    msg = string.Format("{0} IP Changed from {1} to {2}.", Utilities.GetDateTime(), timer1.referenceIP.ToString(), timer1.currentIP.ToString());
                    Logger.logger.InfoFormat(msg);
                    Settings.LogEntriesAdd(msg);
                    string status = Settings.appToKillList + " killed at " + time;
                    Logger.logger.InfoFormat("{0}", status);
                    labelStatus.Content = status;
                    timer1.killActive = true; //Kill only once

                }
            }

            if (timer1.killActive) {
                textKillStatus.Foreground = new SolidColorBrush(Colors.Red);
            } else {
                textKillStatus.Foreground = new SolidColorBrush(Colors.Green);

            }
            textKillStatus.Text = timer1.killActive.ToString();
        }
        private void CheckAndUpdateReferenceIP() {
            timer1.SetCurrentlIP(timer1.web.GetIPAddress());
            lblCurrentIP.Content = timer1.currentIP;

            dgIPStatistics.ItemsSource = Settings.IPCheckURLs.Select(x => new { x.Code, x.NumberUsed, x.NumberFailed, IPCheckSuccessPercent = Math.Round(x.IPCheckSuccessPercent, 2).ToString("#00.00") }).ToList();// < Tuple<string, long, long, double>>();

            //dgLog.ItemsSource = Settings.logEntries.Select(x => new { x }).ToList();
            dgLog.ItemsSource = Settings.msgArrList.ToArray().Select(x => new { x }).ToList();

         


        }
        private void UpdateStatistics() {

            IpInfoLocation ipInfo = timer1.ipInfo.GetIpInfo();
            Dictionary<string, string> statistics = timer1.ipInfo.LocationToDictionary(ipInfo);

            dataGridStatistics.ItemsSource = statistics;
           
            ;
        }
        ///--------------------------------------------------------------------------------------
        private void buttonKill_Click(object sender, RoutedEventArgs e) {
            Utilities.KillProgram(Settings.appToKillArray);
        }

        private void buttonResetIp_Click(object sender, RoutedEventArgs e) {
            lblReferenceIP.Content = timer1.ResetIpString();
            Settings.LogEntriesAdd(string.Format("Reset reference IP to :{0}", timer1.ResetIpString()));


        }

        private void buttonSetCustomIP_Click(object sender, RoutedEventArgs e) {
            timer1.SetReferenceIP(labelCustomIPValue.Content.ToString());
            lblReferenceIP.Content = timer1.referenceIP;
            Logger.logger.InfoFormat("Manual reset IP to {0}", timer1.referenceIP);
            Settings.LogEntriesAdd(string.Format("Manual reset IP to :{0}", timer1.referenceIP));
        }

        private void button1_Click(object sender, RoutedEventArgs e) {
            //     Web web = new Web();
            timer1.ipInfo.GetIpInfo();
        }

        private void buttonRunNotepad_Click(object sender, RoutedEventArgs e) {
            Utilities.RunProcess("notepad.exe");
        }
    }
}