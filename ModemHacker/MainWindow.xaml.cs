using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;
using Microsoft.Win32;
using System.IO;
using LibModemBase;
using System.Reflection;

namespace ModemHacker
{
    public partial class MainWindow : Window
    {
        List<ModelData> ModelDatas = new List<ModelData>();
        bool isGetResult = false;
        public MainWindow()
        {
            InitializeComponent();
        }
        void FlushDll()
        {
            if (!Directory.Exists($"{AppDomain.CurrentDomain.BaseDirectory}Modems")) {
                Directory.CreateDirectory($"{AppDomain.CurrentDomain.BaseDirectory}Modems");
            }
            modelList.Items.Clear();
            ModelDatas.Clear();
            string[] dlls = Directory.GetFiles($"{AppDomain.CurrentDomain.BaseDirectory}Modems", "*.dll");
            for (int i = 0; i < dlls.Length; i++)
            {
                Assembly dll = Assembly.LoadFrom(dlls[i]);
                Type[] type = dll.GetTypes();
                if (type.Length == 1)
                {
                    if (type[0].IsSubclassOf(typeof(ModemBase)))
                        ModelDatas.Add(new ModelData(type[0]));
                }

            }

            foreach (var data in ModelDatas)
            {
                modelList.Items.Add(data);
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FlushDll();
        }
        private void modelList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((ModelData)modelList.SelectedItem).useUser)
            {
                userBox.Visibility = Visibility.Visible;
            }
            else
            {
                userBox.Visibility = Visibility.Collapsed;
            }
            isGetResult = false;
            output.Text = $"请先确认自己的光猫型号为\n{modelList.SelectedItem.ToString()}";
        }

        private void GET_Click(object sender, RoutedEventArgs e)
        {
            if (modelList.SelectedIndex > -1) {
                isGetResult = true;
                ModelData selectdata = modelList.SelectedItem as ModelData;
                string username = userBox.Text;
                string passwd = passwdBox.Text;
                string host = ipBox.Text;
                ModemBase modem;
                if (selectdata.useUser) {
                    modem = System.Activator.CreateInstance(selectdata.ClassData, host, username, passwd) as ModemBase;
                }
                else {
                    modem = System.Activator.CreateInstance(selectdata.ClassData, host, passwd) as ModemBase;
                }
                string info = modem.getModemInfo();
                output.Text = info;
            }
        }
        private void SAVE_Click(object sender, RoutedEventArgs e)
        {
            if (output.Text != "" && isGetResult) {
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.Filter = "文本文档|*.txt";
                dialog.FileName = ((ModelData)modelList.SelectedItem).name;
                bool res = (bool)dialog.ShowDialog();
                if (res)
                {
                    File.WriteAllText(dialog.FileName, output.Text);
                }
            }
        }

        private void MenuItem_Flush_Click(object sender, RoutedEventArgs e)
        {
            FlushDll();
        }
    }
}
