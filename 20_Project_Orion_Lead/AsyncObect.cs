using System;
using System.Net.Sockets;

namespace _20_Project_Orion_Lead
{
    public class AsyncObect
    {
        public byte[] Buffer;
        public Socket Working_socket;
        public readonly int BufferSize;

        public AsyncObect(int bufferSize)
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
