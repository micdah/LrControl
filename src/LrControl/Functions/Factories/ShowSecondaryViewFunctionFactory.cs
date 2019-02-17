using LrControl.Configurations;
using LrControl.LrPlugin.Api;
using LrControl.LrPlugin.Api.Modules.LrApplicationView;

namespace LrControl.Functions.Factories
{
    public class ShowSecondaryViewFunctionFactory : FunctionFactory
    {
        public SecondaryView SecondaryView { get; }

        public ShowSecondaryViewFunctionFactory(ISettings settings, ILrApi api, SecondaryView secondaryView) 
            : base(settings, api)
        {
            SecondaryView = secondaryView;
            DisplayName = $"Change secondary monitor to {secondaryView.Name}";
            Key = $"ShowSecondaryView{secondaryView.Value}";
        }

        public override string DisplayName { get; }
        public override string Key { get; }

        protected override IFunction CreateFunction(ISettings settings, ILrApi api)
            => new ShowSecondaryViewFunction(settings, api, DisplayName, Key, SecondaryView);
    }
}