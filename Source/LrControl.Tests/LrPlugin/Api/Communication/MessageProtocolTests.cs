using System;
using System.Diagnostics.CodeAnalysis;
using LrControl.LrPlugin.Api.Communication;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace LrControl.Tests.LrPlugin.Api.Communication
{
    [SuppressMessage("ReSharper", "NotAccessedVariable")]
    [SuppressMessage("ReSharper", "RedundantAssignment")]
    public class MessageProtocolTests : TestSuite
    {
        private const string ModuleName = "TestModule";
        private const string ErrorPrefix = "E";
        
        private readonly MessageProtocol _sut;
        private readonly Mock<IPluginClient> _pluginClientMock;

        public MessageProtocolTests(ITestOutputHelper output) : base(output)
        {
            _pluginClientMock = new Mock<IPluginClient>();
            _pluginClientMock
                .Setup(m => m.IsConnected)
                .Returns(true);
            
            _sut = new MessageProtocol(_pluginClientMock.Object, "TestModule");
        }

        [Fact]
        public void Invoke_Without_Result_Shall_Return_False_If_PluginClient_Is_Not_Connected()
        {
            // Setup
            _pluginClientMock
                .Setup(m => m.IsConnected)
                .Returns(false);
            
            // Test
            var result = _sut.Invoke("Method");
            
            // Verify
            Assert.False(result);
        }

        [Fact]
        public void Invoke_Without_Result_Shall_Return_True_If_Response_Is_Ack()
        {
            // Setup
            var response = "ack";
            _pluginClientMock
                .Setup(m => m.SendMessage(It.IsAny<string>(), out response))
                .Returns(true);
            
            // Test
            var result = _sut.Invoke("Method");
            
            // Verify
            Assert.True(result);
        }

        [Fact]
        public void Invoke_Without_Result_Shall_Return_False_If_Response_It_Not_Ack()
        {
            // Setup
            var response = "not_ack";
            _pluginClientMock
                .Setup(m => m.SendMessage(It.IsAny<string>(), out response))
                .Returns(true);
            
            // Test
            var result = _sut.Invoke("Method");
            
            // Verify
            Assert.False(result);
        }

        [Fact]
        public void Shall_Return_False_If_Response_Is_Error()
        {
            // Setup
            var response = $"{ErrorPrefix}Some error occurred";
            _pluginClientMock
                .Setup(m => m.SendMessage(It.IsAny<string>(), out response))
                .Returns(true);
            
            // Test
            var result = _sut.Invoke("Method");
            
            // Verify
            Assert.False(result);
        }

        [Fact]
        public void Shall_Throw_ArgumentException_If_Method_Is_Null_Or_Empty()
        {
            // Setup
            var response = string.Empty;
            _pluginClientMock
                .Setup(m => m.SendMessage(It.IsAny<string>(), out response))
                .Returns(true);
            string out1, out2, out3;

            void AssertThrows(Action action)
            {
                Assert.Equal("method", Assert.Throws<ArgumentException>(action).ParamName);
            }
            
            // Test
            AssertThrows(() => _sut.Invoke(null));
            AssertThrows(() => _sut.Invoke(out out1, null));
            AssertThrows(() => _sut.Invoke(out out1, out out2, null));
            AssertThrows(() => _sut.Invoke(out out1, out out2, out out3, null));
            
            AssertThrows(() => _sut.Invoke(string.Empty));
            AssertThrows(() => _sut.Invoke(out out1, string.Empty));
            AssertThrows(() => _sut.Invoke(out out1, out out2, string.Empty));
            AssertThrows(() => _sut.Invoke(out out1, out out2, out out3, string.Empty));
        }
    }
}