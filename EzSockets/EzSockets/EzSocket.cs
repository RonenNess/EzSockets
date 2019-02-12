﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lords.Common.Network
{
    /// <summary>
    /// A simple socket wrapper.
    /// </summary>
    public class EzSocket
    {
        #region Members

        /// <summary>
        /// Events handler.
        /// </summary>
        private IEzEventsListener _eventsListener;

        /// <summary>
        /// The socket object we wrap.
        /// </summary>
        public TcpClient Client { get; private set; }

        /// <summary>
        /// Socket session id.
        /// </summary>
        public long SessionId { get; private set; }

        // next session id
        private static long _nextSessionId = 0;

        /// <summary>
        /// The network stream associated with this socket.
        /// </summary>
        public NetworkStream Stream { get; protected set; }

        /// <summary>
        /// Max size, in bytes, for framed messages.
        /// </summary>
        public static int MaxMessageSize = 1024 * 10;

        /// <summary>
        /// Optional data you can attach to this socket.
        /// </summary>
        public object UserData;

        // are we currently in a reading messages loop?
        bool _readingLoop;

        // did we call the closed socket event?
        bool _wasClosedEventCalled;

        /// <summary>
        /// Default IP to use when creating socket with dest ip == null.
        /// </summary>
        public static string DefaultDestIp = IPAddress.Loopback.ToString();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Handle exceptions.
        /// </summary>
        private void HandleException(Exception ex)
        {
            var ret = _eventsListener.OnException(this, ex);
            switch (ret)
            {
                case ExceptionHandlerResponse.CloseSocket:
                    Close();
                    break;

                case ExceptionHandlerResponse.Rethrow:
                    throw ex;

                case ExceptionHandlerResponse.Silence:
                    break;
            }
        }

        /// <summary>
        /// Connect to given IP and port.
        /// </summary>
        /// <param name="ip">IP to connect to or null to use localhost.</param>
        /// <param name="port">Port to connect to.</param>
        /// <param name="eventsListener">Object to handle socket events.</param>
        public EzSocket(string ip, int port, IEzEventsListener eventsListener)
        {
            // store events listener
            _eventsListener = eventsListener;

            try
            {
                // get session id
                SessionId = Interlocked.Increment(ref _nextSessionId);

                // create socket and connect
                Client = new TcpClient();
                Client.Connect(ip ?? DefaultDestIp, port);
                Stream = Client.GetStream();
            }
            catch (Exception e)
            {
                HandleException(e);
            }

            // invoke event
            _eventsListener.OnNewConnection(this);
        }

        /// <summary>
        /// Create the socket from existing connected socket.
        /// </summary>
        /// <param name="socket">TCP client to wrap.</param>
        /// <param name="eventsListener">Object to handle socket events.</param>
        public EzSocket(TcpClient socket, IEzEventsListener eventsListener)
        {
            // store events listener
            _eventsListener = eventsListener;

            try
            {
                // init socket
                SessionId = Interlocked.Increment(ref _nextSessionId);
                Client = socket;
                Stream = Client.GetStream();
            }
            catch (Exception e)
            {
                HandleException(e);
            }

            // invoke event
            _eventsListener.OnNewConnection(this);
        }

        /// <summary>
        /// Socket destructor.
        /// </summary>
        ~EzSocket()
        {
            Close();
        }

        /// <summary>
        /// Close the connection.
        /// </summary>
        public virtual void Close()
        {
            // close stream and socket
            try
            {
                if (Client.Connected)
                {
                    Stream.Close();
                    Client.Close();
                }
            }
            catch (Exception e)
            {
                HandleException(e);
            }

            // invoke event
            if (!_wasClosedEventCalled)
            {
                _eventsListener.OnConnectionClosed(this);
                _wasClosedEventCalled = true;
            }
        }

        #endregion

        #region Send Methods

        /// <summary>
        /// Send data to remote device.
        /// </summary>
        public virtual void Send(string data)
        {
            byte[] byteData = Encoding.UTF8.GetBytes(data);
            Send(byteData);
        }

        /// <summary>
        /// Send data to remote device.
        /// </summary>
        public virtual void Send(byte[] byteData)
        {
            try
            {
                Stream.Write(byteData, 0, byteData.Length);
            }
            catch (Exception e)
            {
                HandleException(e);
                return;
            }

            _eventsListener.OnDataSend(this, byteData);
        }

        /// <summary>
        /// Send data to remote device.
        /// </summary>
        public virtual async Task SendAsync(string data)
        {
            byte[] byteData = Encoding.UTF8.GetBytes(data);
            await SendAsync(byteData);
        }

        /// <summary>
        /// Send data to remote device.
        /// </summary>
        public virtual async Task SendAsync(byte[] byteData)
        {
            try
            {
                await Stream.WriteAsync(byteData, 0, byteData.Length);
            }
            catch (Exception e)
            {
                HandleException(e);
                return;
            }

            _eventsListener.OnDataSend(this, byteData);
        }

        #endregion

        #region Read Methods

        /// <summary>
        /// Read data from remote device.
        /// </summary>
        /// <param name="data">Data received, as string.</param>
        public virtual int Read(out string data)
        {
            byte[] buffer;
            Read(out buffer);
            data = Encoding.UTF8.GetString(buffer);
            return data.Length;
        }

        /// <summary>
        /// Read data from remote device.
        /// </summary>
        /// <param name="byteData">Data received, as bytes array.</param>
        public virtual int Read(out byte[] byteData)
        {
            try
            {
                var toRead = (int)Stream.Length;
                byteData = new byte[toRead];
                Stream.Read(byteData, 0, toRead);
            }
            catch (Exception e)
            {
                HandleException(e);
                byteData = null;
                return 0;
            }

            _eventsListener.OnDataRead(this, byteData);
            return byteData.Length;
        }

        /// <summary>
        /// Read data from remote device.
        /// </summary>
        public virtual async Task<string> ReadAsyncStr()
        {
            byte[] buff = await ReadAsync();
            return Encoding.UTF8.GetString(buff);
        }

        /// <summary>
        /// Read data from remote device.
        /// </summary>
        public virtual async Task<byte[]> ReadAsync()
        {
            byte[] byteData;
            try
            {
                var toRead = (int)Stream.Length;
                byteData = new byte[toRead];
                await Stream.ReadAsync(byteData, 0, toRead);
            }
            catch (Exception e)
            {
                HandleException(e);
                return null;
            }

            _eventsListener.OnDataRead(this, byteData);
            return byteData;
        }

        #endregion

        #region Send Framed Messages

        /// <summary>
        /// Send a single message with size, to be read as whole from the other side.
        /// </summary>
        public virtual void SendMessage(byte[] data)
        {
            byte[] msgBuffer;
            try
            {
                // to encode message size
                msgBuffer = new byte[4 + data.Length];

                // could optionally call BitConverter.GetBytes(data.length);
                msgBuffer[0] = (byte)data.Length;
                msgBuffer[1] = (byte)(data.Length >> 8);
                msgBuffer[2] = (byte)(data.Length >> 16);
                msgBuffer[3] = (byte)(data.Length >> 24);
                _eventsListener.OnDataSend(this, data);

                // merge and send size + data
                Buffer.BlockCopy(data, 0, msgBuffer, 4, data.Length);
                Send(msgBuffer);
            }
            catch (Exception e)
            {
                HandleException(e);
                return;
            }

            // invoke events
            _eventsListener.OnMessageSend(this, msgBuffer);
            _eventsListener.OnDataSend(this, data);
        }

        /// <summary>
        /// Send a single message with size, to be read as whole from the other side.
        /// </summary>
        public virtual void SendMessage(string msg)
        {
            SendMessage(Encoding.UTF8.GetBytes(msg));
        }

        /// <summary>
        /// Send a single message with size, to be read as whole from the other side.
        /// </summary>
        public virtual async Task SendMessageAsync(byte[] data)
        {
            byte[] msgBuffer;
            try
            {
                // to encode message size
                msgBuffer = new byte[4 + data.Length];

                // could optionally call BitConverter.GetBytes(data.length);
                msgBuffer[0] = (byte)data.Length;
                msgBuffer[1] = (byte)(data.Length >> 8);
                msgBuffer[2] = (byte)(data.Length >> 16);
                msgBuffer[3] = (byte)(data.Length >> 24);

                // merge and send size + data
                Buffer.BlockCopy(data, 0, msgBuffer, 4, data.Length);
                await SendAsync(msgBuffer);
            }
            catch (Exception e)
            {
                HandleException(e);
                return;
            }

            // invoke events
            _eventsListener.OnMessageSend(this, msgBuffer);
            _eventsListener.OnMessageSend(this, data);
        }

        /// <summary>
        /// Send a single message with size, to be read as whole from the other side.
        /// </summary>
        public virtual async Task SendMessageAsync(string msg)
        {
            await SendMessageAsync(Encoding.UTF8.GetBytes(msg));
        }

        #endregion

        #region Read Framed Messages

        /// <summary>
        /// Read until a given buffer is full.
        /// </summary>
        public virtual byte[] ReadExactly(int size)
        {
            byte[] buff;
            try
            {
                // create buffer
                buff = new byte[size];

                // do first reading attempt
                int totalread, currentread;
                currentread = totalread = Stream.Read(buff, 0, buff.Length);

                // read rest of the bytes
                while (totalread < buff.Length && currentread > 0)
                {
                    currentread = Stream.Read(buff, totalread, buff.Length - totalread);
                    totalread += currentread;
                }
            }
            catch (Exception e)
            {
                HandleException(e);
                return null;
            }

            // invoke event and return
            _eventsListener.OnDataRead(this, buff);
            return buff;
        }

        /// <summary>
        /// Read until a given buffer is full.
        /// </summary>
        public virtual async Task<byte[]> ReadExactlyAsync(int size)
        {
            byte[] buff;
            try
            {
                // create buffer
                buff = new byte[size];

                // do first reading attempt
                int totalread, currentread;
                currentread = totalread = await Stream.ReadAsync(buff, 0, buff.Length);

                // read rest of the bytes
                while (totalread < buff.Length && currentread > 0)
                {
                    currentread = await Stream.ReadAsync(buff, totalread, buff.Length - totalread);
                    totalread += currentread;
                }
            }
            catch (Exception e)
            {
                HandleException(e);
                return null;
            }

            // invoke event and return
            _eventsListener.OnDataRead(this, buff);
            return buff;
        }

        /// <summary>
        /// Convert bytes array to int.
        /// </summary>
        int BuffToInt(byte[] arr)
        {
            // convert the size we read into int
            // could optionally call BitConverter.ToInt32(sizeinfo, 0);
            int ret = 0;
            ret |= arr[0];
            ret |= (((int)arr[1]) << 8);
            ret |= (((int)arr[2]) << 16);
            ret |= (((int)arr[3]) << 24);
            return ret;
        }

        /// <summary>
        /// Read a single integer from stream.
        /// </summary>
        public int ReadInt()
        {
            return BuffToInt(ReadExactly(4));
        }

        /// <summary>
        /// Read a single integer from stream.
        /// </summary>
        public virtual async Task<int> ReadIntAsync()
        {
            var buff = await ReadExactlyAsync(4);
            return BuffToInt(buff);
        }

        /// <summary>
        /// read a single message as a whole.
        /// Must be message sent via SendMessage().
        /// </summary>
        public virtual byte[] ReadMessage()
        {
            byte[] data;
            try
            {
                // read message size
                var messagesize = ReadInt();

                // sanity check
                if (messagesize > 1024 * 10)
                {
                    throw new Exception("Message buffer too big!");
                }

                // read message and return
                data = ReadExactly(messagesize);
            }
            catch (Exception e)
            {
                HandleException(e);
                return null;
            }

            // invoke events and return
            _eventsListener.OnDataRead(this, data);
            _eventsListener.OnMessageRead(this, data);
            return data;
        }

        /// <summary>
        /// read a single message as a whole and convert to string.
        /// Must be message sent via SendMessage().
        /// </summary>
        public virtual string ReadMessageStr()
        {
            byte[] buff = ReadMessage();
            return Encoding.Unicode.GetString(buff, 0, buff.Length);
        }

        /// <summary>
        /// read a single message as a whole.
        /// Must be message sent via SendMessage().
        /// </summary>
        public virtual async Task<byte[]> ReadMessageAsync()
        {
            byte[] data;
            try
            {
                // read message size
                var messagesize = await ReadIntAsync();

                // sanity check
                if (messagesize > MaxMessageSize)
                {
                    throw new Exception("Message buffer too big!");
                }

                // read the message and return
                data = await ReadExactlyAsync(messagesize);
            }
            catch (Exception e)
            {
                HandleException(e);
                return null;
            }

            // invoke events and return
            _eventsListener.OnDataRead(this, data);
            _eventsListener.OnMessageRead(this, data);
            return data;
        }

        /// <summary>
        /// read a single message as a whole and convert to string.
        /// Must be message sent via SendMessage().
        /// </summary>
        public virtual async Task<string> ReadMessageStrAsync()
        {
            // convert to string and return
            byte[] buff = await ReadMessageAsync();
            return Encoding.Unicode.GetString(buff, 0, buff.Length);
        }

        /// <summary>
        /// Read messages in endless async loop, triggering events for every new message.
        /// </summary>
        public async void StartReadingMessages()
        {
            _readingLoop = true;
            while (_readingLoop)
            {
                try
                {
                    var ret = await ReadMessageAsync();
                }
                catch (Exception e)
                {
                    HandleException(e);
                    _readingLoop = false;
                }
            }
        }

        /// <summary>
        /// Stop reading messages after 'StartReadingMessages' was called.
        /// </summary>
        public void StopReadingMessages()
        {
            _readingLoop = false;
        }

        #endregion
    }
}