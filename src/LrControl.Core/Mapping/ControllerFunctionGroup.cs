using System.Collections.Generic;
using System.Linq;
using LrControl.LrPlugin.Api;
using LrControl.LrPlugin.Api.Modules.LrDevelopController;
using Serilog;

namespace LrControl.Core.Mapping
{
    public class ControllerFunctionGroup
    {
        private static readonly ILogger Log = Serilog.Log.ForContext<ControllerFunctionGroup>();
        private static readonly List<ControllerFunctionGroup> AllFunctionGroups = new List<ControllerFunctionGroup>();
        private readonly ILrApi _api;
        private readonly List<ControllerFunction> _controllerFunctions;

        internal ControllerFunctionGroup(ILrApi api, Panel panel = null)
        {
            _api = api;
            IsGlobal = panel == null;
            Panel = panel;

            _controllerFunctions = new List<ControllerFunction>();

            AllFunctionGroups.Add(this);
        }

        public string DisplayName => IsGlobal ? "Global" : $"{Panel.Name}";
        public IEnumerable<ControllerFunction> ControllerFunctions => _controllerFunctions;

        public string Key { get; internal set; }

        public bool IsGlobal { get; private set; }

        public Panel Panel { get; private set; }

        public bool Enabled { get; private set; }

        internal static ControllerFunctionGroup GetFunctionGroupFor(Panel panel)
        {
            // ReSharper disable once PossibleUnintendedReferenceComparison
            return AllFunctionGroups.FirstOrDefault(group => group.Panel == panel);
        }

        internal void Enable()
        {
            if (!IsGlobal)
            {
                // Disable other non-global groups
                foreach (var group in AllFunctionGroups.Where(g => !g.IsGlobal && g != this))
                {
                    group.Disable();
                }

                // Switch to panel/module
                if (Panel != null)
                {
                    _api.LrDevelopController.RevealPanel(Panel);
                }
            }

            // Enable group
            foreach (var controllerFunction in ControllerFunctions)
            {
                controllerFunction.Enable(!IsGlobal);
            }

            Enabled = true;
            Log.Debug("Enabled FunctionGroup for {Name}", Panel?.Name);
        }

        internal void Disable()
        {
            if (!Enabled) return;

            foreach (var controllerFunction in ControllerFunctions)
            {
                controllerFunction.Disable();
            }

            Enabled = false;
            Log.Debug("Disabled FunctionGroup for {Name}", Panel?.Name);
        }

        internal void AddControllerFunction(ControllerFunction controllerFunction)
        {
            _controllerFunctions.Add(controllerFunction);
        }

        internal void ClearControllerFunctions()
        {
            foreach (var controllerFunction in _controllerFunctions)
            {
                controllerFunction.Dispose();
            }

            _controllerFunctions.Clear();
        }
    }
}