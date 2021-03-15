// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

using LogManager;

using GameServer;

namespace GameServer
{
    /// <summary>
    /// A TCP server.
    /// </summary>
    public class Server
    {
        public static int maxPlayers { get; private set; }
        public static int port { get; private set; }
        public static Dictionary<int, Client> clients = new Dictionary<int, Client>();

        private static TcpListener tcpListener;

        /// <summary>
        /// Creates and runs a <seealso cref="Server"/>.
        /// </summary>
        /// <param name="_maxPlayers">The maximum number of clients that can connect to the server.</param>
        /// <param name="_port">The port to run on.</param>
        public static void Start(int _maxPlayers, int _port)
        {
            maxPlayers = _maxPlayers;
            port = _port;

            LogMan.Initialize();
            LogMan.Info($"Copyright (c) {DateTime.Now.Year} Gideon Grinberg. Subject to the terms of the Mozilla PublicLicense, v. 2.0.");
            LogMan.Info($"Starting server.");

            try
            {
                tcpListener = new TcpListener(IPAddress.Any, port);

                tcpListener.Start();
                tcpListener.BeginAcceptTcpClient(new AsyncCallback(TCPConnectCallback), null);

                LogMan.Info($"Listening for connections on port {port}. Max players is currently set to {_maxPlayers}");
            }
            catch (Exception ex)
            {
                LogMan.Info($"ERROR: Failed to start TCP server. Exception: \n {ex}");
            }

            InitializeServerData();
            LogMan.Info("Finished initializing server!");
        }

        /// <summary>
        /// Connects a new client to the server.
        /// </summary>
        private static void TCPConnectCallback(IAsyncResult _result)
        {
            TcpClient _client = tcpListener.EndAcceptTcpClient(_result);
            tcpListener.BeginAcceptTcpClient(new AsyncCallback(TCPConnectCallback), null);

            LogMan.Info($"Incoming connection from {_client.Client.RemoteEndPoint}");
            for (int i = 1; i <= maxPlayers; i++)
            {
                if (clients[i].tcp.socket == null)
                {
                    clients[i].tcp.Connect(_client);
                    LogMan.Info($"Connected client {clients[i].tcp.socket.Client.RemoteEndPoint}. Client ID: {i}");
                    return;
                }
            }

            LogMan.Info($"Failed to connect client. Server is full. (Players: {maxPlayers}/{maxPlayers} connections used).");
        }

        /// <summary>
        /// Adds client "slots" to <see cref="clients"/>.
        /// </summary>
        private static void InitializeServerData()
        {
            for (int i = 1; i <= maxPlayers; i++)
            {
                clients.Add(i, new Client(i));
            }
        }
    }
}
