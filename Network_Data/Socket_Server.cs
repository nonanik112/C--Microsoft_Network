using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Network_Data
{
    public class Socket_Server
    {
        IPAddress mIp;
        int mPort;
        TcpListener mTCPListener;

        List<TcpClient> mClient;
        public bool KeepRunning { get; set; }

        public Socket_Server()
        {
            mClient = new List<TcpClient>();
        }
        public async void StartListeningForIncomingConnection(IPAddress ipaddr = null, int port = 23000)
        {
            if (ipaddr == null)
            {
                ipaddr = IPAddress.Any;
            }
            if (port <= 0)
            {
                port = 23000;
            }

            mIp = ipaddr;
            mPort = port;

            System.Diagnostics.Debug.WriteLine(string.Format("IP Address: {0} - Port {1}", mIp.ToString(), mPort));

            mTCPListener = new TcpListener(mIp, mPort);
            try
            {
                mTCPListener.Start();

                KeepRunning = true;
                while (true)
                {

                    var returnedByAccept = await mTCPListener.AcceptTcpClientAsync();

                    mClient.Add(returnedByAccept);

                    Debug.WriteLine("Client connected successfully - count: {0} - client:  ", mClient.Count, returnedByAccept.Client.RemoteEndPoint);

                    TakeCareOfTCPClient(returnedByAccept);
                }


            }
            catch (Exception excp)
            {

                System.Diagnostics.Debug.WriteLine(excp.ToString());
            }


        }

        private async void TakeCareOfTCPClient(TcpClient paramClient)
        {
            NetworkStream stream = null;
            StreamReader reader = null;

            try
            {
                stream = paramClient.GetStream();
                reader = new StreamReader(stream);

                char[] buff = new char[64];
                while (KeepRunning)
                {
                    Debug.WriteLine("*** Ready to read");

                    int nRet = await reader.ReadAsync(buff, 0, buff.Length);

                    System.Diagnostics.Debug.WriteLine("Returned: " + nRet);

                    if (nRet == 0)
                    {
                        RemoveClient(paramClient);
                        System.Diagnostics.Debug.WriteLine("Socket disconnect");
                        break;
                    }
                    string receivdeText = new string(buff);

                    System.Diagnostics.Debug.WriteLine("Socket disconnect");

                    Array.Clear(buff, 0, buff.Length);

                }
            }
            catch (Exception excp)
            {
                RemoveClient(paramClient);
                System.Diagnostics.Debug.WriteLine(excp.ToString());
            }

        }

        private void RemoveClient(TcpClient paramClient)
        {
            if (mClient.Contains(paramClient))
            {
                RemoveClient(paramClient);
                mClient.Remove(paramClient);
                Debug.WriteLine(String.Format("Client removed, count: {0} ", mClient.Count));
            }
        }

        public async void SendToAll(string letMessage)
        {
            if (string.IsNullOrEmpty(letMessage))
            {
                return;
            }
            try
            {
                byte[] buffMessage = Encoding.ASCII.GetBytes(letMessage);
                foreach (TcpClient i in mClient)
                {
                    i.GetStream().WriteAsync(buffMessage, 0, buffMessage.Length);
                }
            }
            catch (Exception excp)
            {

                Debug.WriteLine(excp.ToString());
            }
        }
    }
}
