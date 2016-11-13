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
using System.Windows.Shapes;
using PixivApi;
using System.IO;
using System.Xml;

namespace PicaPicaPoi
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            if (File.Exists("config.xml"))
            {
                // can't login
                //LoadConfigFile();
                LoadConfig();
            }
        }

        private async void LoadConfigFile()
        {
            var reader = XmlReader.Create("config.xml");
            while (reader.Read())
            {
                if(reader.LocalName == "RefreshToken")
                {
                    var token = reader.ReadElementContentAsString();
                    if (token == "") return;

                    loginBtn.IsEnabled = usernameInput.IsEnabled = passwordInput.IsEnabled = false;
                    if (!await ApiManager.Api.LoginAsync(token))
                    {
                        hint.Visibility = Visibility.Visible;
                        loginBtn.IsEnabled = usernameInput.IsEnabled = passwordInput.IsEnabled = true;
                    } else
                    {
                        AnimationHelper.NavigatePage(loginPage, new Views.Recommend());
                    }
                }

            }
        }
        private void LoadConfig()
        {
            try
            {
                var reader = XmlReader.Create("config.xml");
                while (reader.Read())
                {
                    if (reader.LocalName == "Username")
                    {
                        usernameInput.Text = reader.ReadElementContentAsString();
                    }
                    if (reader.LocalName == "Password")
                    {
                        passwordInput.Password = reader.ReadElementContentAsString();
                    }
                }
                reader.Close();
                reader.Dispose();
                Button_Click(null, null);
            } catch { }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            loginBtn.IsEnabled = usernameInput.IsEnabled = passwordInput.IsEnabled = false;
            if (!await ApiManager.Api.LoginAsync(usernameInput.Text, passwordInput.Password))
            {
                hint.Visibility = Visibility.Visible;
                loginBtn.IsEnabled = usernameInput.IsEnabled = passwordInput.IsEnabled = true;
            } else
            {
                AnimationHelper.NavigatePage(loginPage, new Views.Recommend());
                SaveConfigFile();
            }

        }

        private void usernameInput_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                Button_Click(null, null);
            }
        }

        private void usernameInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            loginBtn.IsEnabled = usernameInput.Text.Replace(" ","") != "" && passwordInput.Password.Replace(" ", "") != "";
        }

        private void passwordInput_PasswordChanged(object sender, RoutedEventArgs e)
        {
            loginBtn.IsEnabled = usernameInput.Text.Replace(" ", "") != "" && passwordInput.Password.Replace(" ", "") != "";
        }

        private void SaveConfigFile()
        {
            var writer = XmlWriter.Create("config.xml");
            writer.WriteStartElement("Settings");
            writer.WriteElementString("RefreshToken", ApiManager.Api.RefreshToken);
            writer.WriteElementString("Username", usernameInput.Text);
            writer.WriteElementString("Password", passwordInput.Password);
            writer.WriteEndElement();
            writer.Flush();
            writer.Close();
            writer.Dispose();
        }
    }
}
