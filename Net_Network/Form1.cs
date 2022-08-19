using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Network_Data;
namespace Net_Network
{
    public partial class Form1 : Form
    {
        Socket_Server _mServer;
        public Form1()
        {
            InitializeComponent();
            _mServer = new Socket_Server();
        }

        public void AcceptIncomingOnSocket()
        {
            Socket ListenerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            IPAddress ipadr = IPAddress.Any;

            IPEndPoint ipep = new IPEndPoint(ipadr, 23000);

            ListenerSocket.Bind(ipep);

            ListenerSocket.Listen(5);

            //if (ListenerSocket == null)
            //{
            //    MessageBox.Show("About to accept incoming connection.");

            //}

            Console.WriteLine("About to accept incoming connection.");

            Socket client = ListenerSocket.Accept();
            //if (client == null)
            //{
            //    MessageBox.Show("Client connected. " + client.ToString() + " - IP End Point: " + client.RemoteEndPoint.ToString());
            //}

            Console.WriteLine("Client connected. " + client.ToString() + " - IP End Point: " + client.RemoteEndPoint.ToString());
        }
        private void button1_Click(object sender, EventArgs e)
        {
            AcceptIncomingOnSocket();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _mServer.StartListeningForIncomingConnection();
        }
    }
}
