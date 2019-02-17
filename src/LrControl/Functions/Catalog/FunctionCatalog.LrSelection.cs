using System.Collections.Generic;
using LrControl.Configurations;
using LrControl.Functions.Factories;
using LrControl.LrPlugin.Api;
using LrControl.LrPlugin.Api.Modules.LrSelection;

namespace LrControl.Functions.Catalog
{
    public partial class FunctionCatalog
    {
        private static IFunctionCatalogGroup CreateSelectionGroup(ISettings settings, ILrApi api)
        {
            IEnumerable<IFunctionFactory> CreateFactories()
            {
                yield return new MethodFunctionFactory(settings, api, "Clear labels", "ClearLabels", a => a.LrSelection.ClearLabels());
                yield return new MethodFunctionFactory(settings, api, "Decrease rating", "DecreaseRating", a => a.LrSelection.DecreaseRating());
                yield return new MethodFunctionFactory(settings, api, "Deselect active", "DeselectActive", a => a.LrSelection.DeselectActive());
                yield return new MethodFunctionFactory(settings, api, "Deselect others", "DeselectOthers", a => a.LrSelection.DeselectOthers());
                
                // Extend Selection
                foreach (var direction in Direction.GetAll())
                {
                    for (var i = 1; i < 5; i++)
                    {
                        var amount = i;
                        yield return new MethodFunctionFactory(settings, api,
                            $"Extend selection {direction.Name} by {amount}",
                            $"ExtendSelection{direction.Value}By{amount}",
                            a => a.LrSelection.ExtendSelection(direction, amount));
                    }
                }
                
                yield return new MethodFunctionFactory(settings, api, "Flag as picked", "FlagAsPicked", a => a.LrSelection.FlagAsPick());
                yield return new MethodFunctionFactory(settings, api, "Flag as rejected", "FlagAsReject", a => a.LrSelection.FlagAsReject());
                yield return new ToggleFlagFunctionFactory(settings, api, Flag.Pick); 
                yield return new ToggleFlagFunctionFactory(settings, api, Flag.Reject);
                yield return new MethodFunctionFactory(settings, api, "Increase rating", "IncreaseRating", a => a.LrSelection.IncreaseRating());
                yield return new MethodFunctionFactory(settings, api, "Next photo", "NextPhoto", a => a.LrSelection.NextPhoto());
                yield return new MethodFunctionFactory(settings, api, "Previous photo", "PreviousPhoto", a => a.LrSelection.PreviousPhoto());
                yield return new MethodFunctionFactory(settings, api, "Remove flag", "RemoveFlag", a => a.LrSelection.RemoveFlag());
                yield return new MethodFunctionFactory(settings, api, "Select all", "SelectAll", a => a.LrSelection.SelectAll());
                yield return new MethodFunctionFactory(settings, api, "Select first", "SelectFirstPhoto", a => a.LrSelection.SelectFirstPhoto());
                yield return new MethodFunctionFactory(settings, api, "Select inverse", "SelectInverse", a => a.LrSelection.SelectInverse());
                yield return new MethodFunctionFactory(settings, api, "Select last", "SelectLastPhoto", a => a.LrSelection.SelectLastPhoto());
                yield return new MethodFunctionFactory(settings, api, "Select none", "SelectNone", a => a.LrSelection.SelectNone());
                
                // Set Color Label
                foreach (var label in ColorLabel.GetAll())
                {
                    yield return new MethodFunctionFactory(settings, api,
                        $"Set color label {label.Name}",
                        $"SetColorLabel{label.Value}",
                        a => a.LrSelection.SetColorLabel(label));
                }
                
                // Set Rating
                for (var i = 0; i <= 5; i++)
                {
                    var rating = i;
                    yield return new MethodFunctionFactory(settings, api,
                        $"Set rating to {rating}",
                        $"SetRatingTo{rating}",
                        a => a.LrSelection.SetRating(rating));
                }
                
                yield return new MethodFunctionFactory(settings, api, "Toggle blue label", "ToggleBlueLabel", a => a.LrSelection.ToggleBlueLabel());
                yield return new MethodFunctionFactory(settings, api, "Toggle green label", "ToggleGreenLabel", a => a.LrSelection.ToggleGreenLabel());
                yield return new MethodFunctionFactory(settings, api, "Toggle purple label", "TogglePurpleLabel", a => a.LrSelection.TogglePurpleLabel());
                yield return new MethodFunctionFactory(settings, api, "Toggle red label", "ToggleRedLabel", a => a.LrSelection.ToggleRedLabel());
                yield return new MethodFunctionFactory(settings, api, "Toggle yellow label", "ToggleYellowLabel", a => a.LrSelection.ToggleYellowLabel());
            }

            return new FunctionCatalogGroup
            {
                DisplayName = "Selection",
                Key = "LrSelection",
                Functions = new List<IFunctionFactory>(CreateFactories())
            };
        }
    }
}