﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Net;
using System.Net.Sockets;
using System.Windows.Input;

namespace _20_Project_Orion_Lead
{
    public partial class MainWindow : Window
    {
        Socket main_socket;
        IPAddress Default_iPAddress = null;
        public MainWindow()
        {
            InitializeComponent();
            main_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
        }
      
        private void client_Loaded(object sender, RoutedEventArgs e)
        {
            IPHostEntry hostEntry = Dns.GetHostEntry(Dns.GetHostName());

            
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
            txtAddress.Text = Default_iPAddress.ToString();
        }

        private void Btn_start_Click_1(object sender, RoutedEventArgs e)
        {
            int port;
            if(!int.TryParse(txtPort.Text, out port))
            {
                MessageBox.Show("포트번호가 잘못 입력되거나 입력되지 않았읍니다.","경고",MessageBoxButton.OK, MessageBoxImage.Warning);
                txtPort.Focus();
                txtPort.SelectAll();
                return;
            }

            IPEndPoint severEP = new IPEndPoint(Default_iPAddress, port);
            main_socket.Bind(severEP);
            main_socket.Listen(10);
            main_socket.BeginAccept(AcceptCallBack, null);
        }

        List<Socket> connectedClients = new List<Socket>();


        void  AcceptCallBack(IAsyncResult ar)
        {
            Socket client = main_socket.EndAccept(ar);

            main_socket.BeginAccept(AcceptCallBack, null);
            AsyncObect obj = new AsyncObect(4096);
            obj.Working_socket = client;
            connectedClients.Add(client);
            TxtHistory.Text += string.Format("\n클라이언트(@{0})가 연결되;었습니다.", client.RemoteEndPoint);
            client.BeginReceive(obj.Buffer, 0, 4096, 0, DataReceived, obj);
        }

        void DataReceived(IAsyncResult ar)
        {
            AsyncObect obj = (AsyncObect)ar.AsyncState;

            int received = obj.Working_socket.EndReceive(ar);
            
            if(received <= 0)
            {
                obj.Working_socket.Close();
                return;
            }

            string text = Encoding.UTF8.GetString(obj.Buffer);

            string[] token = text.Split('\x01');
            string ip = token[0]; 
            string msg = token[1];

            TxtHistory.Text += string.Format("\n[받음]{0}: {1}", ip, msg);

            for(int i = connectedClients.Count-1; i >=0; i--)
            {
                Socket socket = connectedClients[i];
                if(socket != obj.Working_socket)
                {
                    try { socket.Send(obj.Buffer); }
                    catch {
                        try { socket.Dispose(); }
                        catch { }
                        connectedClients.RemoveAt(i);
                    }
                }
            }
            obj.ClearBuffer();
            obj.Working_socket.BeginReceive(obj.Buffer, 0, 4096, 0, DataReceived, obj);
        }

        private void Btn_Transfer_Click(object sender, RoutedEventArgs e)
        {
            if (!main_socket.IsBound)
            {
                MessageBox.Show("Server Is not Working","경고",MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string tts = txtInput.Text.Trim();
            if (string.IsNullOrEmpty(tts))
            {
                MessageBox.Show("Text NULL", "Warn", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtInput.Focus();
                return;
            }
            byte[] vs = Encoding.UTF8.GetBytes(Default_iPAddress.ToString() + '\x01' + tts);
            
            for(int i = connectedClients.Count-1; i>=0; i--)
            {
                Socket socket = connectedClients[i];
                try { socket.Send(vs); }
                catch
                {
                    try { socket.Dispose(); }
                    catch { }
                    connectedClients.RemoveAt(i);
                }
            }
            TxtHistory.Text += string.Format("\n[보냄]{0}: {1}", Default_iPAddress.ToString(), tts);
            txtInput.Clear();
        }

        private void txtInput_KeyDown(object sender, KeyEventArgs e)
        {
            if(Keyboard.IsKeyDown(Key.Enter))
            {
                Btn_Transfer.Focus();
            }
        }
    }
}
