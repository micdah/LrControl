using LrControl.Configurations;
using LrControl.Functions;
using LrControl.Functions.Factories;
using LrControl.LrPlugin.Api;
using LrControl.Tests.Devices;
using Xunit;
using Xunit.Abstractions;

namespace LrControl.Tests.Functions.Factories
{
    public abstract class FunctionFactoryTestSuite<TFactory> : ProfileManagerTestSuite
        where TFactory : IFunctionFactory
    {
        protected FunctionFactoryTestSuite(ITestOutputHelper output) : base(output)
        {
        }
    }

    public abstract class FunctionFactoryTestSuite<TFactory,TArg> : FunctionFactoryTestSuite<TFactory>
        where TFactory : IFunctionFactory
    {
        protected FunctionFactoryTestSuite(ITestOutputHelper output) : base(output)
        {
        }

        protected (TFactory Factory, IFunction Function) Create(TArg arg)
        {
            var factory = CreateFactory(Settings.Object, LrApi.Object, arg);
            Assert.NotNull(factory);

            var function = factory.CreateFunction();
            Assert.NotNull(function);

            Assert.Equal(factory.DisplayName, function.DisplayName);
            Assert.Equal(factory.Key, function.Key);

            return (factory, function);
        }

        protected abstract TFactory CreateFactory(ISettings settings, ILrApi lrApi, TArg arg);
    }
    
    public abstract class FunctionFactoryTestSuite<TFactory,TArg1,TArg2> : FunctionFactoryTestSuite<TFactory>
        where TFactory : IFunctionFactory
    {
        protected FunctionFactoryTestSuite(ITestOutputHelper output) : base(output)
        {
        }

        protected (TFactory Factory, IFunction Function) Create(TArg1 arg1, TArg2 arg2)
        {
            var factory = CreateFactory(Settings.Object, LrApi.Object, arg1, arg2);
            Assert.NotNull(factory);

            var function = factory.CreateFunction();
            Assert.NotNull(function);

            Assert.Equal(factory.DisplayName, function.DisplayName);
            Assert.Equal(factory.Key, function.Key);

            return (factory, function);
        }

        protected abstract TFactory CreateFactory(ISettings settings, ILrApi lrApi, TArg1 arg1, TArg2 arg2);
    }
}