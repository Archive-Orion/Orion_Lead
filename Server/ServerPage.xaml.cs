using System;
using System.Text;
using System.Windows;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;


namespace Server
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        delegate void Append_Txt_Delegate(Control Ctrl, string M_string);
        Append_Txt_Delegate _Txt_Delegate;
        Socket mainsocket;
        IPAddress pAddress;
        public MainWindow()
        {
            InitializeComponent();
            mainsocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            _Txt_Delegate = new Append_Txt_Delegate(AppendText);
        }

        void AppendText(Control Ctrl, string s)
        {
            if (Ctrl.InvokeRequired)
            {
                Ctrl.Invoke(_Txt_Delegate, Ctrl, s);
            }
            else
            {
                string source = Ctrl.Text;
                Ctrl.text = source + Environment.NewLine + s;
            }
        }

        void onServerLoaded(object sender, EventArgs e)
        {
            IPHostEntry IPChart = Dns.GetHostEntry(Dns.GetHostName());

            foreach(IPAddress addr in IPChart.AddressList)
            {
                if(addr.AddressFamily == AddressFamily.InterNetwork)
                {
                    pAddress = addr;
                    break;
                }
            }
            //로컬주소 사용
            if(pAddress == null)
            {
                pAddress = IPAddress.Loopback;
                pAddress.ToString(); //=> 어디에다가?

            }
        }


    }
}
