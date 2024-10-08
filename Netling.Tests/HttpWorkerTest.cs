using System;
using Netling.Core.SocketWorker.Performance;
using NUnit.Framework;

namespace Netling.Tests
{
    [TestFixture]  
    public class HttpWorkerTest
    {
        [Test]
        public void ReadResponse()
        {
            var client = (IHttpWorkerClient)new FakeHttpWorkerClient("HTTP/1.1 200 OK\r\nDate: Wed, 06 Jul 2016 18:26:27 GMT\r\ncontent-length: 13\r\ncontent-type: text/plain\r\nServer: Kestrel\r\n\r\nHello, World!");
            var httpWorker = new HttpWorker(client, new Uri("http://netling.test", UriKind.Absolute));

            var (length, statusCode) = httpWorker.Send();

            Assert.That(200, Is.EqualTo(statusCode));
            Assert.That(132, Is.EqualTo(length));
        }
        
        [Test]
        public void ReadResponseSplit()
        {
            var client = (IHttpWorkerClient)new FakeHttpWorkerClient("HTTP/1.1 200 OK\r\nDate: Wed, 06 Jul 2016 18:26:27 GMT\r\ncontent-length: 13\r\ncontent-type: text/plain\r\nServer: Kestrel\r\n\r\n", "Hello, World!");
            var httpWorker = new HttpWorker(client, new Uri("http://netling.test", UriKind.Absolute));

            var (length, statusCode) = httpWorker.Send();

            Assert.That(200, Is.EqualTo(statusCode));
            Assert.That(132, Is.EqualTo(length));
        }
    }
}
