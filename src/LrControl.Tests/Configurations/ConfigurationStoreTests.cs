using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LrControl.Configurations;
using LrControl.Configurations.Models;
using LrControl.Devices;
using RtMidi.Core.Enums;
using Serilog;
using Xunit;
using Xunit.Abstractions;

namespace LrControl.Tests.Configurations
{
    public class ConfigurationStoreTests : TestSuite
    {
        private readonly IConfigurationStore _configurationStore;

        public ConfigurationStoreTests(ITestOutputHelper output) : base(output)
        {
            _configurationStore = new ConfigurationStore();
        }

        [Fact]
        public async Task Should_Load_From_Stream()
        {
            const string json = @"{
	""profile"": {
		""modules"": {
			""develop"": [
				{
					""controller"": ""Channel1-Nrpn-82"",
					""function"": ""test""
				}
			]
		},
		""panels"": {
			""adjustPanel"": [
				{
					""controller"": ""Channel1-Nrpn-82"",
					""function"": ""test""
				}
			]
		}
	}
}";
            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                var model = await _configurationStore.LoadAsync(ms);
                Log.Debug("{@Model}", model);

                Assert.NotNull(model);

                var profile = model.Profile;
                Assert.NotNull(profile);

                var modules = profile.Modules;
                Assert.NotNull(modules);

                Assert.True(modules.TryGetValue("develop", out var developFunctions));
                Assert.NotNull(developFunctions);

                var developFunction = developFunctions.SingleOrDefault(x =>
                    x.Controller.Channel == Channel.Channel1 &&
                    x.Controller.MessageType == MessageType.Nrpn &&
                    x.Controller.Parameter == 82);
                Assert.NotNull(developFunction);
                Assert.Equal("test", developFunction.Function);

                var panels = profile.Panels;
                Assert.NotNull(panels);
                
                Assert.True(panels.TryGetValue("adjustPanel", out var adjustPanelFunctions));
                Assert.NotNull(adjustPanelFunctions);

                var adjustPanelFunction = adjustPanelFunctions.SingleOrDefault(x =>
	                x.Controller.Channel == Channel.Channel1 &&
	                x.Controller.MessageType == MessageType.Nrpn &&
	                x.Controller.Parameter == 82);
                Assert.NotNull(adjustPanelFunction);
                Assert.Equal("test", adjustPanelFunction.Function);
            }
        }

        [Fact]
        public async Task Should_Write_To_Stream()
        {
	        var writeModel = new ConfigurationModel
	        {
		        Profile = new ProfileModel
		        {
			        Modules = new Dictionary<string, List<ControllerFunctionModel>>
			        {
				        {
					        "develop", new List<ControllerFunctionModel>
					        {
						        new ControllerFunctionModel
						        {
							        Controller = new ControllerIdModel
							        {
								        Channel = Channel.Channel1,
								        MessageType = MessageType.Nrpn,
								        Parameter = 82
							        },
							        Function = "test"
						        }
					        }
				        }
			        },
			        Panels = new Dictionary<string, List<ControllerFunctionModel>>
			        {
				        {
					        "adjustPanel", new List<ControllerFunctionModel>
					        {
						        new ControllerFunctionModel
						        {
							        Controller = new ControllerIdModel
							        {
								        Channel = Channel.Channel1,
								        MessageType = MessageType.Nrpn,
								        Parameter = 82
							        },
							        Function = "test"
						        }
					        }
				        }
			        }
		        }
	        };

	        using (var ms = await _configurationStore.SaveAsync(writeModel))
	        {
		        var readModel = await _configurationStore.LoadAsync(ms);
		        Log.Debug("{@Model}", readModel);
		        Assert.NotNull(readModel);

		        var profile = readModel.Profile;
		        Assert.NotNull(profile);

		        var modules = profile.Modules;
		        Assert.NotNull(modules);

		        Assert.True(modules.TryGetValue("develop", out var developFunctions));
		        Assert.NotNull(developFunctions);

		        var developFunction = developFunctions.SingleOrDefault(x =>
			        x.Controller.Channel == Channel.Channel1 &&
			        x.Controller.MessageType == MessageType.Nrpn &&
			        x.Controller.Parameter == 82);
		        Assert.NotNull(developFunction);
		        Assert.Equal("test", developFunction.Function);

		        var panels = profile.Panels;
		        Assert.NotNull(panels);
                
		        Assert.True(panels.TryGetValue("adjustPanel", out var adjustPanelFunctions));
		        Assert.NotNull(adjustPanelFunctions);

		        var adjustPanelFunction = adjustPanelFunctions.SingleOrDefault(x =>
			        x.Controller.Channel == Channel.Channel1 &&
			        x.Controller.MessageType == MessageType.Nrpn &&
			        x.Controller.Parameter == 82);
		        Assert.NotNull(adjustPanelFunction);
		        Assert.Equal("test", adjustPanelFunction.Function);
	        }
        }
    }
}