using System;
using System.Text;
using Netling.Core.SocketWorker.Extensions;
using Netling.Core.SocketWorker.Performance;
using NUnit.Framework;

namespace Netling.Tests
{
    [TestFixture]  
    public class MiscTest
    {
        private ReadOnlyMemory<byte> _request;
        private ReadOnlyMemory<byte> _response;

        [SetUp]
        protected void SetUp() 
        {
            _request = Encoding.UTF8.GetBytes("GET / HTTP/1.1\r\nAccept-Encoding: gzip, deflate, sdch\r\nHost: test.netling\r\ncontent-length: 5\r\n\r\n12345");
            _response = Encoding.UTF8.GetBytes("HTTP/1.1 200 OK\r\nDate: Wed, 06 Jul 2016 18:26:27 GMT\r\ncontent-length: 13\r\ncontent-type: text/plain\r\nServer: Kestrel\r\n\r\nHello, World!");
        }

        [Test]
        public void ByteExtensions_ConvertToInt()
        {
            Assert.That(5, Is.EqualTo(ByteExtensions.ConvertToInt(_request.Span.Slice(90, 1))));
            Assert.That(12345, Is.EqualTo(ByteExtensions.ConvertToInt(_request.Span.Slice(95, 5))));
        }

        [Test]
        public void HttpHelper_GetResponseType()
        {
            Assert.That(HttpHelper.GetResponseType(_response.Span), Is.EqualTo(ResponseType.ContentLength));
        }

        [Test]
        public void HttpHelper_GetStatusCode()
        {
            Assert.That(HttpHelper.GetStatusCode(_response.Span), Is.EqualTo(200));
        }

        [Test]
        public void HttpHelper_SeekHeader()
        {
            HttpHelper.SeekHeader(_response.Span, CommonStrings.HeaderContentLength.Span, out var index, out var length);
            Assert.That(index, Is.EqualTo(70));
            Assert.That(length, Is.EqualTo(2));
        }

        [Test]
        public void HttpHelperContentLength_GetHeaderContentLength()
        {
            Assert.That(HttpHelperContentLength.GetHeaderContentLength(_response.Span), Is.EqualTo(13));
        }

        [Test]
        public void HttpHelperContentLength_GetResponseLength()
        {
            Assert.That(HttpHelperContentLength.GetResponseLength(_response.Span), Is.EqualTo(132));
        }

        [Test]
        public void HttpHelperChunked_IsEndOfChunkedStream()
        {
            var chunkedResponse = Encoding.UTF8.GetBytes("HTTP/1.1 200 OK\r\nDate: Wed, 06 Jul 2016 18:26:27 GMT\r\ntransfer-encoding: chunked\r\nContent-Type: text/plain\r\nServer: Kestrel\r\n\r\nHello, World!0\r\n\r\n").AsSpan();
            Assert.That(HttpHelperChunked.IsEndOfChunkedStream(chunkedResponse), Is.True);
        }

        [Test]
        public void HttpHelperChunked_SeekEndOfChunkedStream()
        {
            var chunkedResponse = Encoding.UTF8.GetBytes("HTTP/1.1 200 OK\r\nDate: Wed, 06 Jul 2016 18:26:27 GMT\r\ntransfer-encoding: chunked\r\nContent-Type: text/plain\r\nServer: Kestrel\r\n\r\nHello, World!0\r\n\r\n").AsSpan();
            Assert.That(HttpHelperChunked.SeekEndOfChunkedStream(chunkedResponse), Is.EqualTo(145));
        }
    }
}
