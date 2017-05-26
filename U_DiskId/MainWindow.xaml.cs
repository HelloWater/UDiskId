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
using System.Management;
using System.Windows.Forms;
using System.Diagnostics;

namespace U_DiskId
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string m_sAvalableUDisk = "";
        public MainWindow()
        {
            InitializeComponent();

            double dScreenWid = System.Windows.SystemParameters.PrimaryScreenWidth;
            double dScreenHei = System.Windows.SystemParameters.PrimaryScreenHeight;

            this.Left = dScreenWid - 180;
            this.Top = dScreenHei - 150;

            m_sAvalableUDisk = CheckUDisk();

            this.Topmost = true;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
        private void OpenUDisk(object sender, MouseButtonEventArgs e)
        {
            if (m_sAvalableUDisk.Length > 0)
            {
//                 FolderBrowserDialog m_Dialog = new FolderBrowserDialog();
//                 m_Dialog.SelectedPath = m_sAvalableUDisk;
//                 m_Dialog.ShowDialog();

                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.InitialDirectory = m_sAvalableUDisk;
                System.Windows.Forms.DialogResult result = openFileDialog.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    string sFile = openFileDialog.FileName;
                    System.Diagnostics.Process.Start(sFile);
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Please Insert the U disk");
            }
        }

        private string CheckUDisk()
        {
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("Select * From Win32_LogicalDisk");
                string sDisc = ""; //盘符
                double dCurSize = 0.0;

                foreach (ManagementObject mo in searcher.Get())
                {
                    string disc = mo["Name"].ToString().Trim();
                    string size = mo["Size"].ToString().Trim();
                    string DriveType = mo["DriveType"].ToString().Trim();

                    if (string.Compare(DriveType, "2") == 0) //0:UnKnown , 1:No Root Directory , 2:Removable Disk , 3:Local Disk , 4:Network Drive , 5:Compact Disc , 6: RAM Disk;
                    {
                        double dSize = Convert.ToDouble(size);
                        if (dSize > dCurSize)
                        {
                            sDisc = disc;
                            dCurSize = dSize;
                        }
                    }
                }
                U_Name.Text = sDisc;
                return sDisc;
            }
            catch
            {
                return "";
            }
        }
    }
}
