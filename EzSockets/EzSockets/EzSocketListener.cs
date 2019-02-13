using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace EzSockets
{
    /// <summary>
    /// Listen and accept new connections.
    /// </summary>
    public class EzSocketListener
    {
        /// <summary>
        /// The class that handle socket events.
        /// </summary>
        public IEzEventsListener EventsListener { get; private set; }

        // listener to bind and accept connections on port
        TcpListener _listener;

        /// <summary>
        /// Get if this listener is currently listening.
        /// </summary>
        public bool IsListening { get; private set; }

        /// <summary>
        /// Port we are currently listening to (0 if not listening).
        /// </summary>
        public int Port { get; private set; }

        /// <summary>
        /// Create the sockets listener.
        /// </summary>
        /// <param name="eventsListener">Object to handle socket events.</param>
        public EzSocketListener(IEzEventsListener eventsListener)
        {
            EventsListener = eventsListener;
        }

        /// <summary>
        /// Close socket listener.
        /// </summary>
        ~EzSocketListener()
        {
            if (_listener != null)
            {
                _listener.Stop();
                _listener.Server.Close();
                _listener = null;
            }
        }

        /// <summary>
        /// Listen on port and accept connections.
        /// </summary>
        /// <param name="port">Port to listen to.</param>
        public void Listen(int port)
        {
            // create listener and start
            Port = port;
            _listener = new TcpListener(IPAddress.Any, port);
            _listener.Start();

            // accept connections in endless loop until set to false
            IsListening = true;
            while (IsListening)
            {
                var client = _listener.AcceptTcpClient();
                var sock = new EzSocket(client, EventsListener);
                // ^ while we don't do anything with 'sock' here, it will trigger event internally.
                // the user should use it from there.
            }

            // stop listening and delete listener
            Port = 0;
            _listener.Stop();
            _listener.Server.Close();
            _listener = null;
        }

        /// <summary>
        /// Listen on port and accept connections in background.
        /// </summary>
        /// <param name="port">Port to listen to.</param>
        public void ListenAsync(int port)
        {
            Task.Factory.StartNew(() =>
            {
                Listen(port);
            });
        }

        /// <summary>
        /// Stop listening to port.
        /// </summary>
        public void StopListening()
        {
            IsListening = false;
        }
    }
}
