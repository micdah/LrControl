using LrControl.Configurations;
using LrControl.Functions;
using LrControl.Functions.Factories;
using LrControl.LrPlugin.Api;
using LrControl.LrPlugin.Api.Modules.LrSelection;
using Xunit;
using Xunit.Abstractions;

namespace LrControl.Tests.Functions.Factories
{
    public class ToggleFlagFunctionFactoryTests : FunctionFactoryTestSuite<ToggleFlagFunctionFactory, Flag>
    {
        public ToggleFlagFunctionFactoryTests(ITestOutputHelper output) : base(output)
        {
        }

        private static readonly Flag[] Flags = {Flag.Pick, Flag.Reject};

        protected override ToggleFlagFunctionFactory CreateFactory(ISettings settings, ILrApi lrApi, Flag arg)
            => new ToggleFlagFunctionFactory(settings, lrApi, arg);

        [Theory]
        [InlineData(0), InlineData(1)]
        public void Should_Create_ToggleFlagFunction(int flagIndex)
        {
            var flag = Flags[flagIndex];
            var (_, function) = Create<ToggleFlagFunction>(flag);
            Assert.Equal(flag, function.Flag);
        }
    }
}