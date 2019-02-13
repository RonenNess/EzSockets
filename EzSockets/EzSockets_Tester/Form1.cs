using System;
using EzSockets;
using System.Windows.Forms;
using System.Collections.Generic;

namespace EzSockets_Tester
{
    public partial class Form1 : Form
    {
        // server listener
        EzSocketListener _server;

        // last socket we received from
        EzSocket _targetSocket;

        // list of connected clients from client side.
        List<EzSocket> _connectionsToServer = new List<EzSocket>();

        // init form
        public Form1()
        {
            InitializeComponent();
            _server = new EzSocketListener(new EzEventsListener()
            {
                OnNewConnectionHandler = (EzSocket socket) => {
                    WriteSocketOutputServerSide(socket, "Connected!");
                    PickTargetSocket(socket);
                    socket.StartReadingMessages();
                },
                OnConnectionClosedHandler = (EzSocket socket) => {
                    WriteSocketOutputServerSide(socket, "Closed!");
                    if (_targetSocket == socket) { PickTargetSocket(null); }
                },
                OnMessageReadHandler = (EzSocket socket, byte[] data) => {
                    WriteSocketOutputServerSide(socket, "Read: '" + System.Text.Encoding.UTF8.GetString(data) + "'.");
                    PickTargetSocket(socket);
                },
                OnMessageSendHandler = (EzSocket socket, byte[] data) => {
                    WriteSocketOutputServerSide(socket, "Sent " + data.Length + " bytes.");
                },
                OnExceptionHandler = (EzSocket socket, Exception ex) => {
                    WriteSocketOutputServerSide(socket, "ERROR: '" + ex.ToString() + "'.");
                    return ExceptionHandlerResponse.CloseSocket;
                }
            });
        }

        // add client connection to clients list.
        private void AddClientConnection(EzSocket socket)
        {
            ClientSocketsList.Invoke(new MethodInvoker(
                delegate
                {
                    ClientSocketsList.Items.Add("Socket " + socket.SocketId);
                })
            );
        }

        // remove client connection from clients list.
        private void RemoveClientConnection(EzSocket socket)
        {
            ClientSocketsList.Invoke(new MethodInvoker(
                delegate
                {
                    ClientSocketsList.Items.Remove("Socket " + socket.SocketId);
                })
            );
        }

        // write output for a socket - on server side
        private void WriteSocketOutputServerSide(EzSocket socket, string msg)
        {
            serverOutTextBox.Invoke(new MethodInvoker(
                delegate
                {
                    serverOutTextBox.Text += "[Socket " + socket.SocketId + "] " + msg + Environment.NewLine;
                })
            );
        }

        // write output for a socket - on client side
        private void WriteSocketOutputClientSide(EzSocket socket, string msg)
        {
            ClientOutputBox.Invoke(new MethodInvoker(
                delegate
                {
                    ClientOutputBox.Text += "[Socket " + socket.SocketId + "] " + msg + Environment.NewLine;
                })
            );
        }

        // write output for a socket - on client side
        private void WriteSocketOutputClientSide(string msg)
        {
            ClientOutputBox.Invoke(new MethodInvoker(
                delegate
                {
                    ClientOutputBox.Text += msg + Environment.NewLine;
                })
            );
        }

        // select target socket
        private void PickTargetSocket(EzSocket socket)
        {
            _targetSocket = socket;
            showActiveSocket.Invoke(new MethodInvoker(
                delegate
                {
                    if (_targetSocket != null)
                    {
                        showActiveSocket.Text = "Send from socket: " + socket.SocketId;
                    }
                    else
                    {
                        showActiveSocket.Text = "No active socket to send from.";
                    }
                })
            );
        }

        // port input change - make sure valid port
        private void portInput_TextChanged(object sender, EventArgs e)
        {
            int val;
            if (System.Int32.TryParse(portInput.Text, out val))
            {
                if (val <= 0)
                {
                    portInput.Text = "1";
                }
                else if (val > 99999)
                {
                    portInput.Text = "99999";
                }
            }
            else
            {
                portInput.Text = "8080";
            }
        }

        // start / stop button
        private void startServerBtn_Click(object sender, EventArgs e)
        {
            // if listening, stop server
            if (_server.IsListening)
            {
                _server.StopListening();
                startServerBtn.Text = "Start Server";
                portInput.ReadOnly = false;
                listeningLabel.Visible = false;
            }
            // if not listening, start server
            else
            {
                _server.ListenAsync(System.Int32.Parse(portInput.Text));
                startServerBtn.Text = "Stop Server";
                portInput.ReadOnly = true;
                listeningLabel.Visible = true;
            }
        }

        // send message to socket
        private void SendMsgBtn_Click(object sender, EventArgs e)
        {
            if (_targetSocket != null && _targetSocket.Connected)
            {
                _targetSocket.SendMessage(ServerSendMsgText.Text);
            }
        }

        // create new client connection
        private void NewConnectionBtn_Click(object sender, EventArgs e)
        {
            var socket = new EzSocket(null, _server.Port, new EzEventsListener()
            {
                OnConnectionClosedHandler = (EzSocket sock) =>
                {
                    _connectionsToServer.Remove(sock);
                    RemoveClientConnection(sock);
                },
                OnMessageReadHandler = (EzSocket sock, byte[] buff) =>
                {
                    WriteSocketOutputClientSide(sock, "Read: '" + System.Text.Encoding.UTF8.GetString(buff) + "'.");
                },
                OnMessageSendHandler = (EzSocket sock, byte[] data) => {
                    WriteSocketOutputClientSide(sock, "Sent " + data.Length + " bytes.");
                },
            });
            if (socket.Connected)
            {
                socket.StartReadingMessages();
                _connectionsToServer.Add(socket);
                AddClientConnection(socket);
            }
        }

        // get currently selected client-side socket.
        EzSocket SelectedClientSocket
        {
            get
            {
                if (ClientSocketsList.Text.Length == 0) { return null; }
                var id = ClientSocketsList.Text.Split(' ')[1];
                int idInt;
                if (Int32.TryParse(id, out idInt))
                {
                    EzSocket socket = _connectionsToServer.Find((x) => {
                        return x.SocketId == idInt;
                    });
                    return socket;
                }
                return null;
            }
        }

        // close client socket
        private void CloseSocketBtn_Click(object sender, EventArgs e)
        {
            var socket = SelectedClientSocket;
            if (socket != null) { socket.Close(); }
        }

        // send message from client
        private void ClientSendMsgBtn_Click(object sender, EventArgs e)
        {
            var socket = SelectedClientSocket;
            if (socket != null)
            {
                if (ClientSendMsgText.Text.Length > 0)
                {
                    socket.SendMessage(ClientSendMsgText.Text);
                }
            }
            else
            {
                WriteSocketOutputClientSide("[PLEASE SELECT SOCKET TO SEND FROM]");
            }
        }
    }
}
