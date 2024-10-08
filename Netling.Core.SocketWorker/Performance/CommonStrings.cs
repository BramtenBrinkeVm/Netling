using System;
using System.Text;

namespace Netling.Core.SocketWorker.Performance
{
    public static class CommonStrings
    {
        public static readonly ReadOnlyMemory<byte> HeaderContentLength = Encoding.ASCII.GetBytes("\r\ncontent-length: ");
        public static readonly ReadOnlyMemory<byte> HeaderTransferEncoding = Encoding.ASCII.GetBytes( Environment.NewLine + "transfer-encoding: ");
        public static readonly ReadOnlyMemory<byte> HeaderReturn = Encoding.ASCII.GetBytes("\r\n");
        public static readonly ReadOnlyMemory<byte> HeaderEnd = Encoding.ASCII.GetBytes("\r\n\r\n");
        public static readonly ReadOnlyMemory<byte> EndOfChunkedResponse = Encoding.ASCII.GetBytes("0\r\n\r\n");
    }
}
