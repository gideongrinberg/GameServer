using System;
using Xunit;

using GameServer;

namespace Tests
{
    public class TestServer
    {
        [Fact]
        public void DoesServerStart()
        {
            Server.Start(10, 1646);
        }


    }
}
