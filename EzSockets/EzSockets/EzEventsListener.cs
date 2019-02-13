using System;

namespace EzSockets
{
    /// <summary>
    /// Class to handle different events from sockets.
    /// </summary>
    public class EzEventsListener : IEzEventsListener
    {
        /// <summary>
        /// Called when data is sent.
        /// </summary>
        public Action<EzSocket, byte[]> OnDataSendHandler;

        /// <summary>
        /// Trigger the OnDataSend event.
        /// </summary>
        /// <param name="socket">Socket the event originated from.</param>
        /// <param name="data">Data sent.</param>
        public virtual void OnDataSend(EzSocket socket, byte[] data)
        {
            OnDataSendHandler?.Invoke(socket, data);
        }

        /// <summary>
        /// Called when data is read.
        /// </summary>
        public Action<EzSocket, byte[]> OnDataReadHandler;

        /// <summary>
        /// Trigger the OnDataRead event.
        /// </summary>
        /// <param name="socket">Socket the event originated from.</param>
        /// <param name="data">Data read.</param>
        public virtual void OnDataRead(EzSocket socket, byte[] data)
        {
            OnDataReadHandler?.Invoke(socket, data);
        }

        /// <summary>
        /// Called when a whole framed message is sent.
        /// </summary>
        public Action<EzSocket, byte[]> OnMessageSendHandler;

        /// <summary>
        /// Trigger the OnMessageSend event.
        /// </summary>
        /// <param name="socket">Socket the event originated from.</param>
        /// <param name="data">Data sent.</param>
        public virtual void OnMessageSend(EzSocket socket, byte[] data)
        {
            OnMessageSendHandler?.Invoke(socket, data);
        }

        /// <summary>
        /// Called when a whole framed message is read.
        /// </summary>
        public Action<EzSocket, byte[]> OnMessageReadHandler;

        /// <summary>
        /// Trigger the OnMessageRead event.
        /// </summary>
        /// <param name="socket">Socket the event originated from.</param>
        /// <param name="data">Data read.</param>
        public virtual void OnMessageRead(EzSocket socket, byte[] data)
        {
            OnMessageReadHandler?.Invoke(socket, data);
        }

        /// <summary>
        /// Called when a new connection is created.
        /// </summary>
        public Action<EzSocket> OnNewConnectionHandler;

        /// <summary>
        /// Trigger the OnNewConnection event.
        /// </summary>
        /// <param name="socket">Socket the event originated from.</param>
        public virtual void OnNewConnection(EzSocket socket)
        {
            OnNewConnectionHandler?.Invoke(socket);
        }

        /// <summary>
        /// Called when a connection is closed.
        /// </summary>
        public Action<EzSocket> OnConnectionClosedHandler;

        /// <summary>
        /// Trigger the OnConnectionClosed event.
        /// </summary>
        /// <param name="socket">Socket the event originated from.</param>
        public virtual void OnConnectionClosed(EzSocket socket)
        {
            OnConnectionClosedHandler?.Invoke(socket);
        }

        /// <summary>
        /// Called on exceptions.
        /// </summary>
        /// <returns>How to handle the exception.</returns>
        public Func<EzSocket, Exception, ExceptionHandlerResponse> OnExceptionHandler;

        /// <summary>
        /// Trigger the OnException event.
        /// </summary>
        /// <param name="socket">Socket the event originated from.</param>
        /// <param name="exception">Exception that triggered the event.</param>
        public virtual ExceptionHandlerResponse OnException(EzSocket socket, Exception exception)
        {
            if (OnExceptionHandler == null) { return ExceptionHandlerResponse.CloseSocket; }
            return OnExceptionHandler(socket, exception);
        }
    }
}