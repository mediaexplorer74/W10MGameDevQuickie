﻿using System;
using System.Net;

namespace GameManager
{
    public class NetClient
    {
        internal object Status;
        private NetConfiguration netConfig;

        public NetClient(NetConfiguration netConfig)
        {
            this.netConfig = netConfig;
        }

        internal void Connect(IPEndPoint serverEndPoint, object v)
        {
            //throw new NotImplementedException();
        }

        internal NetBuffer CreateBuffer()
        {
            //throw new NotImplementedException();
            return default;
        }

        internal void Disconnect(string v)
        {
            //throw new NotImplementedException();
        }

        internal void DiscoverLocalServers(int v)
        {
            //throw new NotImplementedException();
        }

        internal bool ReadMessage(NetBuffer msgBuffer, out NetMessageType msgType)
        {
            msgType = NetMessageType.StatusChanged;
            return true;//throw new NotImplementedException();
        }

        internal void SendMessage(NetBuffer msgBuffer, object reliableUnordered)
        {
            //throw new NotImplementedException();
        }
               

        internal void SetMessageTypeEnabled(NetMessageType connectionRejected, bool v)
        {
            //throw new NotImplementedException();
        }

        internal void Shutdown(string v)
        {
            //throw new NotImplementedException();
        }

        internal void Start()
        {
            //throw new NotImplementedException();
        }
    }
}