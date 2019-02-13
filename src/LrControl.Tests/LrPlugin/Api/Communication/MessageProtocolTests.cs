using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using LrControl.LrPlugin.Api.Communication;
using LrControl.LrPlugin.Api.Modules.LrDevelopController;
using LrControl.Tests.Mocks;
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
        private const char RecordSeparator = '\u001E';
        private const char ErrorPrefix = 'E';
        private const char StringPrefix = 'S';
        private const char NumberPrefix = 'N';
        private const char BooleanPrefix = 'B';
        private const char NullPrefix = 'L';
        
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
        
        private void SetupSendMessage(string response = "ack", bool returns = true)
        {
            _pluginClientMock
                .Setup(m => m.SendMessage(It.IsAny<string>(), out response))
                .Returns(returns);
        }

        private void VerifySendMessage(string message)
        {
            string response = string.Empty;
            _pluginClientMock
                .Verify(m => m.SendMessage(It.Is<string>(x => x == message), out response), Times.Once());
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
            SetupSendMessage();
            
            // Test
            var result = _sut.Invoke("Method");
            
            // Verify
            Assert.True(result);
        }

        [Fact]
        public void Invoke_Without_Result_Shall_Return_False_If_Response_It_Not_Ack()
        {
            // Setup
            SetupSendMessage("not_ack");
            
            // Test
            var result = _sut.Invoke("Method");
            
            // Verify
            Assert.False(result);
        }

        [Fact]
        public void Shall_Return_False_If_Response_Is_Error()
        {
            // Setup
            SetupSendMessage($"{ErrorPrefix}Some error occurred");
            
            // Test
            var result = _sut.Invoke("Method");
            
            // Verify
            Assert.False(result);
        }

        [Fact]
        public void Shall_Throw_ArgumentException_If_Method_Is_Null_Or_Empty()
        {
            // Setup
            SetupSendMessage();
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
        
        [Fact]
        public void Should_Throw_ArgumentException_On_Unsupported_Argument_Type()
        {
            // Setup
            SetupSendMessage();
            
            // Test
            var ex = Assert.Throws<ArgumentException>(() =>
                _sut.Invoke("Method", Guid.NewGuid()));

            // Verify
            Assert.Equal("arg", ex.ParamName);
        }

        [Fact]
        public void Should_Ensure_Method_Name_Starts_With_Lowercase() 
        {
            // Setup
            SetupSendMessage();

            // Test
            Assert.True(_sut.Invoke("Method"));

            // Verify
            VerifySendMessage($"{ModuleName}.method");
        }

        [Fact]
        public void Should_Add_Null_Argument()
        {
            // Setup
            SetupSendMessage();
            
            // Test
            Assert.True(_sut.Invoke("Method", (object)null));

            // Verify
            VerifySendMessage($"{ModuleName}.method {NullPrefix}");
        }

        [Fact]
        public void Should_Add_String_Argument()
        {
            // Setup
            SetupSendMessage();
            var arg = "string";

            // Test
            Assert.True(_sut.Invoke("Method", arg));

            // Verify
            VerifySendMessage($"{ModuleName}.method {StringPrefix}{arg}");
        }
        
        [Fact]
        public void Should_Add_Int_Argument()
        {
            // Setup
            SetupSendMessage();
            var arg = 42;

            // Test
            Assert.True(_sut.Invoke("Method", arg));

            // Verify
            VerifySendMessage($"{ModuleName}.method {NumberPrefix}{arg}");
        }
        
        [Fact]
        public void Should_Add_Double_Argument()
        {
            // Setup
            SetupSendMessage();
            var arg = 42.42d;

            // Test
            Assert.True(_sut.Invoke("Method", arg));

            // Verify
            VerifySendMessage($"{ModuleName}.method {NumberPrefix}{arg.ToString(CultureInfo.InvariantCulture)}");
        }
        
        [Fact]
        public void Should_Add_Boolean_Argument()
        {
            // Setup
            SetupSendMessage();

            // Test
            Assert.True(_sut.Invoke("Method", true));

            // Verify
            VerifySendMessage($"{ModuleName}.method {BooleanPrefix}1");
        }

        [Fact]
        public void Should_Add_Parameter_Argument()
        {
            // Setup
            SetupSendMessage();
            var param = new Parameter<string>("TestParameter", "Test Parameter");

            // Test
            Assert.True(_sut.Invoke("Method", param));

            // Verify
            VerifySendMessage($"{ModuleName}.method {StringPrefix}{param.Name}");
        }

        [Fact]
        public void Should_Add_String_Enumeration()
        {
            // Setup
            SetupSendMessage();
            var @enum = new TestEnumeration<string>("value", "StringEnum");

            // Test
            Assert.True(_sut.Invoke("Method", @enum));

            // Verify
            VerifySendMessage($"{ModuleName}.method {StringPrefix}{@enum.Value}");
        }

        [Fact]
        public void Should_Add_Int_Enumeration()
        {
            // Setup
            SetupSendMessage();
            var @enum = new TestEnumeration<int>(42, "IntEnum");

            // Test
            Assert.True(_sut.Invoke("Method", @enum));

            // Verify
            VerifySendMessage($"{ModuleName}.method {NumberPrefix}{@enum.Value}");
        }

        [Fact]
        public void Should_Add_Double_Enumeration()
        {
            // Setup
            SetupSendMessage();
            var @enum = new TestEnumeration<double>(42.42d, "DoubleEnum");

            // Test
            Assert.True(_sut.Invoke("Method", @enum));

            // Verify
            VerifySendMessage(
                $"{ModuleName}.method {NumberPrefix}{@enum.Value.ToString(CultureInfo.InvariantCulture)}");
        }

        [Fact]
        public void Should_Add_Boolean_Enumeration()
        {
            // Setup
            SetupSendMessage();
            var @enum = new TestEnumeration<bool>(true, "BoolEnum");

            // Test
            Assert.True(_sut.Invoke("Method", @enum));

            // Verify
            VerifySendMessage($"{ModuleName}.method {BooleanPrefix}{(@enum.Value ? 1 : 0)}");
        }

        [Fact]
        public void Should_Add_Multiple_Arguments()
        {
            // Setup
            SetupSendMessage();
            var arg1 = "hello";
            var arg2 = "world";
            var arg3 = 42;

            // Test
            Assert.True(_sut.Invoke("Method", arg1, arg2, arg3));

            // Verify
            VerifySendMessage(
                $"{ModuleName}.method {StringPrefix}{arg1}" +
                $"{RecordSeparator}{StringPrefix}{arg2}" +
                $"{RecordSeparator}{NumberPrefix}{arg3}");
        }

        [Fact]
        public void Should_Return_String_Type()
        {
            // Setup
            var ret = "Hello World";
            SetupSendMessage($"{StringPrefix}{ret}");

            // Test
            Assert.True(_sut.Invoke(out string response, "Method"));

            // Verify
            Assert.Equal(ret, response);
        }
        
        [Fact]
        public void Should_Return_Int_Type()
        {
            // Setup
            var ret = 42;
            SetupSendMessage($"{NumberPrefix}{ret}");

            // Test
            Assert.True(_sut.Invoke(out int response, "Method"));

            // Verify
            Assert.Equal(ret, response);
        }
        
        [Fact]
        public void Should_Return_Double_Type()
        {
            // Setup
            var ret = 42.42d;
            SetupSendMessage($"{NumberPrefix}{ret.ToString(CultureInfo.InvariantCulture)}");

            // Test
            Assert.True(_sut.Invoke(out double response, "Method"));

            // Verify
            Assert.Equal(ret, response);
        }
        
        [Fact]
        public void Should_Return_Boolean_Type()
        {
            // Setup
            var ret = true;
            SetupSendMessage($"{BooleanPrefix}{1}");

            // Test
            Assert.True(_sut.Invoke(out bool response, "Method"));

            // Verify
            Assert.Equal(ret, response);
        }

        [Fact]
        public void Should_Return_Default_Value_For_Unsupported_Response_Type()
        {
            // Setup
            var ret = Guid.NewGuid();
            SetupSendMessage($"G{ret}");

            // Test
            Assert.False(_sut.Invoke(out Guid response, "Method"));

            // Verify
            Assert.Equal(default(Guid), response);
        }

        [Fact]
        public void Should_Support_Two_Returns()
        {
            // Setup
            var ret1 = "Hello";
            var ret2 = 42;
            SetupSendMessage($"{StringPrefix}{ret1}{RecordSeparator}" +
                             $"{NumberPrefix}{ret2}");

            // Test
            Assert.True(_sut.Invoke(out string response1, out int response2, "Method"));

            // Verify
            Assert.Equal(ret1, response1);
            Assert.Equal(ret2, response2);
        }
        
        [Fact]
        public void Should_Support_Three_Returns()
        {
            // Setup
            var ret1 = "Hello";
            var ret2 = 42;
            var ret3 = "World";
            SetupSendMessage($"{StringPrefix}{ret1}{RecordSeparator}" +
                             $"{NumberPrefix}{ret2}{RecordSeparator}" +
                             $"{StringPrefix}{ret3}");

            // Test
            Assert.True(_sut.Invoke(out string response1, out int response2, out string response3, "Method"));

            // Verify
            Assert.Equal(ret1, response1);
            Assert.Equal(ret2, response2);
            Assert.Equal(ret3, response3);
        }
    }
}