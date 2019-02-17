using LrControl.Configurations;
using LrControl.LrPlugin.Api;
using LrControl.LrPlugin.Api.Modules.LrApplicationView;

namespace LrControl.Functions.Factories
{
    public class ShowViewFunctionFactory : FunctionFactory
    {
        public PrimaryView PrimaryView { get; }

        public ShowViewFunctionFactory(ISettings settings, ILrApi api, PrimaryView primaryView) : base(settings, api)
        {
            PrimaryView = primaryView;
            DisplayName = $"Change view to {primaryView.Name}";
            Key = $"ShowView{primaryView.Value}";
        }

        public override string DisplayName { get; }
        public override string Key { get; }

        protected override IFunction CreateFunction(ISettings settings, ILrApi api)
            => new ShowViewFunction(settings, api, DisplayName, Key, PrimaryView);
    }
}