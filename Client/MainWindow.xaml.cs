using System;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Net.Sockets;

namespace Client
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {

        Socket client_Socket;
        public MainWindow()
        {
            InitializeComponent();
            client_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            IPHostEntry iPHostEntry = Dns.GetHostEntry(Dns.GetHostName());

            IPAddress Client_address = null;
            foreach(IPAddress iPAddress in iPHostEntry.AddressList)
            {
                if(iPAddress.AddressFamily == AddressFamily.InterNetwork)
                {
                    Client_address = iPAddress;
                    break;
                }
            }

            if(Client_address == null)
            {
                Client_address = IPAddress.Loopback;
            }

            Txt_IPadRess.Text = Client_address.ToString();
        }

        private void Connect_Btn_Click(object sender, RoutedEventArgs e)
        {
            if (client_Socket.Connected)
            {
                MessageBox.Show("이미 연결되어 있습니다", "경고", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int Client_Port;
            if(!int.TryParse(TxtPort.Text, out Client_Port))
            {
                MessageBox.Show("포트번호를 다시 입력해 주십시오", "경고", MessageBoxButton.OK, MessageBoxImage.Warning);
                TxtPort.Focus();
                TxtPort.SelectAll();
                return;
            }

            try
            {
                client_Socket.Connect(Txt_IPadRess.Text, Client_Port);
            }
            catch
            {
                MessageBox.Show("연결에 실패하였습니다", "에러", MessageBoxButton.OK, MessageBoxImage.Error);
                Txt_history.Text += string.Format("\n서버와 연결실패");
                return;
            }

            Txt_history.Text += string.Format("\n서버와 연결되었습니다");
            Client_AsyncObject obj = new Client_AsyncObject(4096);
            obj.Working_socket = client_Socket;
            client_Socket.BeginReceive(obj.Buffer, 0, obj.BufferSize ,0, DataReceived, obj);
        }

        void DataReceived(IAsyncResult ar)
        {

        }


        private void close_btn_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Close();
        }

        private void minimize_btn_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Application.Current.MainWindow.DragMove();
        }

        
    }
}
