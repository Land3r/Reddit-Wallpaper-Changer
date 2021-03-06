﻿using System;
using System.Windows.Forms;
using System.Net;
namespace Reddit_Wallpaper_Changer
{
    public partial class Update : Form
    {
        private string latestVersion;
        private RWC RWC;
        public Update(string latestVersion, RWC RWC)
        {
            InitializeComponent();
            this.latestVersion = latestVersion;
            textBox1.Text = latestVersion.Replace("\n", System.Environment.NewLine);
            this.RWC = RWC;
        }
   
        //======================================================================
        // Begin updating RWC
        //======================================================================
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            Logging.LogMessageToFile("Updating Reddit Wallpaper Changer.");
            btnUpdate.Enabled = false;
            btnUpdate.BackgroundImage = Properties.Resources.update_disabled;
            progressBar.Visible = true;

            try
            {
                WebClient wc = Proxy.setProxy();        
                wc.DownloadProgressChanged += (s, a) =>
                {
                    progressBar.Value = a.ProgressPercentage;
                };
                wc.DownloadFileCompleted += (s, a) =>
                {
                    progressBar.Visible = false;
                // any other code to process the file
                    try
                    {
                        //Update Settings
                        Properties.Settings.Default.UpgradeRequired = true;
                        Properties.Settings.Default.Save();

                        //run the program again and close this one
                        System.Diagnostics.Process.Start(System.Reflection.Assembly.GetExecutingAssembly().Location.Replace(".old", ""));


                        //close this one
                        System.Environment.Exit(0);
                        //Application.Exit();

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error Updating: " + ex.Message, "RWC", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Logging.LogMessageToFile("Error Updating: " + ex.Message);
                    }
                };
                System.IO.File.Move(System.Reflection.Assembly.GetExecutingAssembly().Location, System.Reflection.Assembly.GetExecutingAssembly().Location + ".old");
                wc.DownloadFileAsync(new Uri("https://github.com/Rawns/Reddit-Wallpaper-Changer/releases/download/release/Reddit.Wallpaper.Changer.exe"),
                    @"" + System.Reflection.Assembly.GetExecutingAssembly().Location);

                wc.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Updating: " + ex.Message, "RWC", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Logging.LogMessageToFile("Error Updating: " + ex.Message);
            }
        }

        //======================================================================
        // Code to run on form load
        //======================================================================
        private void Update_Load(object sender, EventArgs e)
        {
            this.BringToFront();
            this.TopMost = true;
        }

        //======================================================================
        // Code to run on form close
        //======================================================================
        private void Update_FormClosing(object sender, FormClosingEventArgs e)
        {
            RWC.changeWallpaperTimerEnabled();
        }
    }
}
