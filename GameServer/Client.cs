using System;
using System.Net;
using System.Net.Sockets;

using LogManager;

namespace GameServer
{
    /// <summary>
    /// A TCP client for <see cref="Server"/>
    /// </summary>
    public class Client
    {
        public static int dataBufferSize = 4096; 

        public int id; // Client's ID.
        public TCP tcp;

        /// <summary>
        /// Initializes a new TCP client.
        /// </summary>
        /// <param name="_clientId">Client ID to use.</param>
        public Client(int _clientId)
        {
            id = _clientId;
            tcp = new TCP(id);
        }

        /// <summary>
        /// TCP utility class for <seealso cref="Client"/>.
        /// </summary>
        public class TCP
        {
            /// <summary>
            /// TCP socket to use
            /// </summary>
            public TcpClient socket;
            private readonly int id;

            private NetworkStream stream;
            private byte[] receiveBuffer;

            /// <summary>
            /// Initialize a new TCP class with the specified client ID.
            /// </summary>
            /// <param name="_id">Client ID to use.</param>
            public TCP(int _id)
            {
                id = _id;
            }

            /// <summary>
            /// Connect the TCP client to a specific socket and read data.
            /// </summary>
            /// <param name="_socket">Socket to use.</param>
            public void Connect(TcpClient _socket)
            {
                socket = _socket;

                socket.ReceiveBufferSize = dataBufferSize;
                socket.SendBufferSize = dataBufferSize;

                stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
            }

            /// <summary>
            /// Read TCP data from stream.
            /// </summary>
            private void ReceiveCallback(IAsyncResult _result)
            {
                try
                {
                    int _byteLength = stream.EndRead(_result);

                    if(_byteLength <= 0)
                    {
                        // TODO: disconnect client.
                        LogMan.Info("Received 0 bytes of data from client.");
                    }

                    byte[] _data = new byte[_byteLength];
                    Array.Copy(receiveBuffer, _data, dataBufferSize);

                    // TODO: handle data

                    stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
                }
                catch (Exception ex)
                {
                    LogMan.Info($"\u001b[31m Error receiving TCP data: \u001b[0m {ex}");
                }
            }
        }
    }
}
