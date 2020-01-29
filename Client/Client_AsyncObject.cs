using System;
using System.Net.Sockets;

namespace Client
{
    class Client_AsyncObject
    {
        public byte[] Buffer;
        public Socket Working_socket;
        public readonly int BufferSize;

        public Client_AsyncObject(int bufferSize)
        {
            BufferSize = bufferSize;
            Buffer = new byte[BufferSize];
        }

        public void ClearBuffer()
        {
            Array.Clear(Buffer, 0, BufferSize);
        }
    }
}
