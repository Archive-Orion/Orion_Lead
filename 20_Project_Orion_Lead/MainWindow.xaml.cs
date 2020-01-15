using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace _20_Project_Orion_Lead
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string source;
        delegate void Append_txt_Dele(Control control, string s);
        Socket main_socket;
        Append_txt_Dele _Txt_Dele;
        public MainWindow()
        {
            InitializeComponent();
            main_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            _Txt_Dele = new Append_txt_Dele(AppendTxt);
        }
        void AppendTxt(Control control, string s)
        {
            if(control.Dispatcher.CheckAccess())
            {
                control.Dispatcher.Invoke(_Txt_Dele, control, s);
            }
            else
            {
                control.SetValue(ContentControl.ContentProperty, source);
                string v = source + Environment.NewLine + s;
            }
        }
        private void client_Loaded(object sender, RoutedEventArgs e)
        {
            IPHostEntry hostEntry = Dns.GetHostEntry(Dns.GetHostName());

            IPAddress Default_iPAddress = null;
            foreach(IPAddress addr in hostEntry.AddressList)
            {
                if(addr.AddressFamily == AddressFamily.InterNetwork)
                {
                    Default_iPAddress = addr;
                    break;
                }
            }

            if(Default_iPAddress == null)
            {
                Default_iPAddress = IPAddress.Loopback;
            }

            ////
        }

    }
}
