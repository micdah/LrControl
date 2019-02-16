using LrControl.Functions;
using LrControl.LrPlugin.Api.Modules.LrSelection;
using LrControl.Tests.Devices;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace LrControl.Tests.Functions
{
    public class ToggleFlagFunctionTests : ProfileManagerTestSuite
    {
        public ToggleFlagFunctionTests(ITestOutputHelper output) : base(output)
        {
        }

        private static readonly Flag[] Flags = new[]
        {
            Flag.None,
            Flag.Pick,
            Flag.Reject
        };

        private ToggleFlagFunction Create(Flag flag) => new ToggleFlagFunction(Settings.Object, LrApi.Object,
            "Test Function", "TestFunction", flag);

        [Theory]
        [InlineData(0)]
        [InlineData(2)]
        public void Should_Add_Flag_Pick_If_Not_Present(int currentFlagIndex)
        {
            // Setup
            var function = Create(Flag.Pick);
            ProfileManager.AssignFunction(DefaultModule, Id1, function);

            var currentFlag = Flags[currentFlagIndex];
            LrSelection.Setup(m => m.GetFlag(out currentFlag)).Returns(true);
            LrSelection.Setup(_ => _.FlagAsPick()).Returns(true).Verifiable();

            // Test
            ControllerInput(Id1, Range1.Maximum);

            // Verify
            LrSelection.Verify();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public void Should_Add_Flag_Reject_If_Not_Present(int currentFlagIndex)
        {
            // Setup
            var function = Create(Flag.Reject);
            ProfileManager.AssignFunction(DefaultModule, Id1, function);

            var currentFlag = Flags[currentFlagIndex];
            LrSelection.Setup(_ => _.GetFlag(out currentFlag)).Returns(true);
            LrSelection.Setup(_ => _.FlagAsReject()).Returns(true).Verifiable();

            // Test
            ControllerInput(Id1, Range1.Maximum);

            // Verify
            LrSelection.Verify();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void Should_Remove_Flag_If_Already_Present(int flagIndex)
        {
            // Setup
            var flag = Flags[flagIndex];
            var function = Create(flag);
            ProfileManager.AssignFunction(DefaultModule, Id1, function);

            LrSelection.Setup(_ => _.GetFlag(out flag)).Returns(true);
            LrSelection.Setup(_ => _.RemoveFlag()).Returns(true).Verifiable();

            // Test
            ControllerInput(Id1, Range1.Maximum);

            // Verify
            LrSelection.Verify();
        }

        [Fact]
        public void Should_Only_Apply_When_Controller_Is_At_Maximum_Value()
        {
            // Setup
            var function = Create(Flag.Pick);
            ProfileManager.AssignFunction(DefaultModule, Id1, function);

            // Test
            ControllerInput(Id1, Range1.Maximum - 0.01d);

            // Verify
            LrApi.Verify(_ => _.LrSelection, Times.Never());
        }
    }
}