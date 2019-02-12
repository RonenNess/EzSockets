using System;
using System.Collections.Generic;
using System.Text;

namespace EzSockets
{
    /// <summary>
    /// How to handle exceptions when raised.
    /// </summary>
    public enum ExceptionHandlerResponse
    {
        /// <summary>
        /// Silence the exception.
        /// </summary>
        Silence,

        /// <summary>
        /// Rethrow exception.
        /// </summary>
        Rethrow,

        /// <summary>
        /// Close socket without throwing exception.
        /// </summary>
        CloseSocket,
    }

    /// <summary>
    /// Class to handle different events from sockets.
    /// </summary>
    public interface IEzEventsListener
    {
        /// <summary>
        /// Called when data is sent.
        /// </summary>
        void OnDataSend(EzSocket socket, byte[] data);

        /// <summary>
        /// Called when data is read.
        /// </summary>
        void OnDataRead(EzSocket socket, byte[] data);

        /// <summary>
        /// Called when a whole framed message is sent.
        /// </summary>
        void OnMessageSend(EzSocket socket, byte[] data);

        /// <summary>
        /// Called when a whole framed message is read.
        /// </summary>
        void OnMessageRead(EzSocket socket, byte[] data);

        /// <summary>
        /// Called when a new connection is created.
        /// </summary>
        void OnNewConnection(EzSocket socket);

        /// <summary>
        /// Called when a connection is closed.
        /// </summary>
        void OnConnectionClosed(EzSocket socket);

        /// <summary>
        /// Called on exceptions.
        /// </summary>
        /// <returns>How to handle the exception.</returns>
        ExceptionHandlerResponse OnException(EzSocket socket, System.Exception exception);
    }
}