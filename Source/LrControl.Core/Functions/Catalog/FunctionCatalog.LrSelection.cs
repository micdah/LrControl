using System.Collections.Generic;
using System.Collections.ObjectModel;
using LrControl.Api;
using LrControl.Api.Modules.LrSelection;
using LrControl.Core.Configurations;
using LrControl.Core.Functions.Factories;

namespace LrControl.Core.Functions.Catalog
{
    internal partial class FunctionCatalog
    {
        private static IFunctionCatalogGroup CreateSelectionGroup(ISettings settings, LrApi api)
        {
            var functions = new List<IFunctionFactory>();
            functions.AddRange(new []
            {
                new MethodFunctionFactory(settings, api, "Clear labels", "ClearLabels", a => a.LrSelection.ClearLabels()),
                new MethodFunctionFactory(settings, api, "Decrease rating", "DecreaseRating", a => a.LrSelection.DecreaseRating()),
                new MethodFunctionFactory(settings, api, "Deselect active", "DeselectActive", a => a.LrSelection.DeselectActive()),
                new MethodFunctionFactory(settings, api, "Deselect others", "DeselectOthers", a => a.LrSelection.DeselectOthers()),
            });
            functions.AddRange(CreateExtendSelection(settings, api));
            functions.AddRange(new []
            {
                new MethodFunctionFactory(settings, api, "Flag as picked", "FlagAsPicked", a => a.LrSelection.FlagAsPick()),
                new MethodFunctionFactory(settings, api, "Flag as rejected", "FlagAsReject", a => a.LrSelection.FlagAsReject()),
                new MethodFunctionFactory(settings, api, "Toggle Flag as picked", "ToggleFlagAsPicked", a =>
                {
                    if (!a.LrSelection.GetFlag(out var flag)) return;

                    if (flag != Flag.Pick)
                        a.LrSelection.FlagAsPick();
                    else
                        a.LrSelection.RemoveFlag();
                }), 
                new MethodFunctionFactory(settings, api, "Toggle Flag as rejected", "ToggleFlagAsRejected", a =>
                {
                    if (!a.LrSelection.GetFlag(out var flag)) return;

                    if (flag != Flag.Reject)
                        a.LrSelection.FlagAsReject();
                    else
                        a.LrSelection.RemoveFlag();
                }), 
                new MethodFunctionFactory(settings, api, "Increase rating", "IncreaseRating", a => a.LrSelection.IncreaseRating()),
                new MethodFunctionFactory(settings, api, "Next photo", "NextPhoto", a => a.LrSelection.NextPhoto()),
                new MethodFunctionFactory(settings, api, "Previous photo", "PreviousPhoto", a => a.LrSelection.PreviousPhoto()),
                new MethodFunctionFactory(settings, api, "Remove flag", "RemoveFlag", a => a.LrSelection.RemoveFlag()),
                new MethodFunctionFactory(settings, api, "Select all", "SelectAll", a => a.LrSelection.SelectAll()),
                new MethodFunctionFactory(settings, api, "Select first", "SelectFirstPhoto", a => a.LrSelection.SelectFirstPhoto()),
                new MethodFunctionFactory(settings, api, "Select inverse", "SelectInverse", a => a.LrSelection.SelectInverse()),
                new MethodFunctionFactory(settings, api, "Select last", "SelectLastPhoto", a => a.LrSelection.SelectLastPhoto()),
                new MethodFunctionFactory(settings, api, "Select none", "SelectNone", a => a.LrSelection.SelectNone()),
            });
            functions.AddRange(CreateSetColorLabel(settings, api));
            functions.AddRange(CreateSetRating(settings, api));
            functions.AddRange(new []
            {
                new MethodFunctionFactory(settings, api, "Toggle blue label", "ToggleBlueLabel", a => a.LrSelection.ToggleBlueLabel()),
                new MethodFunctionFactory(settings, api, "Toggle green label", "ToggleGreenLabel", a => a.LrSelection.ToggleGreenLabel()),
                new MethodFunctionFactory(settings, api, "Toggle purple label", "TogglePurpleLabel", a => a.LrSelection.TogglePurpleLabel()),
                new MethodFunctionFactory(settings, api, "Toggle red label", "ToggleRedLabel", a => a.LrSelection.ToggleRedLabel()),
                new MethodFunctionFactory(settings, api, "Toggle yellow label", "ToggleYellowLabel", a => a.LrSelection.ToggleYellowLabel()),
            });

            return new FunctionCatalogGroup
            {
                DisplayName = "Selection",
                Key = "LrSelection",
                Functions = new ObservableCollection<IFunctionFactory>(functions)
            };
        }

        private static IEnumerable<MethodFunctionFactory> CreateExtendSelection(ISettings settings, LrApi api)
        {
            foreach (var direction in Direction.AllEnums)
            {
                for (var i = 1; i < 5; i++)
                {
                    var amount = i;
                    yield return
                        new MethodFunctionFactory(settings, api, $"Extend selection {direction.Name} by {amount}",
                            $"ExtendSelection{direction.Value}By{amount}",
                            a => a.LrSelection.ExtendSelection(direction, amount));
                }
            }
        }

        private static IEnumerable<MethodFunctionFactory> CreateSetColorLabel(ISettings settings, LrApi api)
        {
            foreach (var label in ColorLabel.AllEnums)
            {
                yield return
                    new MethodFunctionFactory(settings, api, $"Set color label {label.Name}", $"SetColorLabel{label.Value}",
                        a => a.LrSelection.SetColorLabel(label));
            }
        }

        private static IEnumerable<MethodFunctionFactory> CreateSetRating(ISettings settings, LrApi api)
        {
            for (var i = 0; i <= 5; i++)
            {
                var rating = i;
                yield return new MethodFunctionFactory(settings, api, $"Set rating to {rating}", $"SetRatingTo{rating}",
                    a => a.LrSelection.SetRating(rating));
            }
        } 
    }
}