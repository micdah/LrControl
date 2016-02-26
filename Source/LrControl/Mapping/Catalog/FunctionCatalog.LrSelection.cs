using System.Collections.Generic;
using System.Collections.ObjectModel;
using micdah.LrControl.Mapping.Functions;
using micdah.LrControlApi;
using micdah.LrControlApi.Modules.LrSelection;

namespace micdah.LrControl.Mapping.Catalog
{
    public partial class FunctionCatalog
    {
        private static FunctionCatalogGroup CreateSelectionGroup(LrApi api)
        {
            var functions = new List<FunctionFactory>();
            functions.AddRange(new []
            {
                new MethodFunctionFactory(api, "Clear labels", "ClearLabels", a => a.LrSelection.ClearLabels()),
                new MethodFunctionFactory(api, "Decrease rating", "DecreaseRating", a => a.LrSelection.DecreaseRating()),
                new MethodFunctionFactory(api, "Deselect active", "DeselectActive", a => a.LrSelection.DeselectActive()),
                new MethodFunctionFactory(api, "Deselect others", "DeselectOthers", a => a.LrSelection.DeselectOthers()),
            });
            functions.AddRange(CreateExtendSelection(api));
            functions.AddRange(new []
            {
                new MethodFunctionFactory(api, "Flag as picked", "FlagAsPicked", a => a.LrSelection.FlagAsPick()),
                new MethodFunctionFactory(api, "Flag as rejected", "FlagAsReject", a => a.LrSelection.FlagAsReject()),
                new MethodFunctionFactory(api, "Toggle Flag as picked", "ToggleFlagAsPicked", a =>
                {
                    Flag flag;
                    if (!a.LrSelection.GetFlag(out flag)) return;

                    if (flag != Flag.Pick)
                        a.LrSelection.FlagAsPick();
                    else
                        a.LrSelection.RemoveFlag();
                }), 
                new MethodFunctionFactory(api, "Toggle Flag as rejected", "ToggleFlagAsRejected", a =>
                {
                    Flag flag;
                    if (!a.LrSelection.GetFlag(out flag)) return;

                    if (flag != Flag.Reject)
                        a.LrSelection.FlagAsReject();
                    else
                        a.LrSelection.RemoveFlag();
                }), 
                new MethodFunctionFactory(api, "Increase rating", "IncreaseRating", a => a.LrSelection.IncreaseRating()),
                new MethodFunctionFactory(api, "Next photo", "NextPhoto", a => a.LrSelection.NextPhoto()),
                new MethodFunctionFactory(api, "Previous photo", "PreviousPhoto", a => a.LrSelection.PreviousPhoto()),
                new MethodFunctionFactory(api, "Remove flag", "RemoveFlag", a => a.LrSelection.RemoveFlag()),
                new MethodFunctionFactory(api, "Select all", "SelectAll", a => a.LrSelection.SelectAll()),
                new MethodFunctionFactory(api, "Select first", "SelectFirstPhoto", a => a.LrSelection.SelectFirstPhoto()),
                new MethodFunctionFactory(api, "Select inverse", "SelectInverse", a => a.LrSelection.SelectInverse()),
                new MethodFunctionFactory(api, "Select last", "SelectLastPhoto", a => a.LrSelection.SelectLastPhoto()),
                new MethodFunctionFactory(api, "Select none", "SelectNone", a => a.LrSelection.SelectNone()),
            });
            functions.AddRange(CreateSetColorLabel(api));
            functions.AddRange(new []
            {
                new MethodFunctionFactory(api, "Toggle blue label", "ToggleBlueLabel", a => a.LrSelection.ToggleBlueLabel()),
                new MethodFunctionFactory(api, "Toggle green label", "ToggleGreenLabel", a => a.LrSelection.ToggleGreenLabel()),
                new MethodFunctionFactory(api, "Toggle purple label", "TogglePurpleLabel", a => a.LrSelection.TogglePurpleLabel()),
                new MethodFunctionFactory(api, "Toggle red label", "ToggleRedLabel", a => a.LrSelection.ToggleRedLabel()),
                new MethodFunctionFactory(api, "Toggle yellow label", "ToggleYellowLabel", a => a.LrSelection.ToggleYellowLabel()),
            });

            return new FunctionCatalogGroup
            {
                DisplayName = "Selection",
                Key = "LrSelection",
                Functions = new ObservableCollection<FunctionFactory>(functions)
            };
        }

        private static IEnumerable<MethodFunctionFactory> CreateExtendSelection(LrApi api)
        {
            foreach (var direction in Direction.AllEnums)
            {
                for (var i = 1; i < 5; i++)
                {
                    var amount = i;
                    yield return
                        new MethodFunctionFactory(api, $"Extend selection {direction.Name} by {amount}",
                            $"ExtendSelection{direction.Value}By{amount}",
                            a => a.LrSelection.ExtendSelection(direction, amount));
                }
            }
        }

        private static IEnumerable<MethodFunctionFactory> CreateSetColorLabel(LrApi api)
        {
            foreach (var label in ColorLabel.AllEnums)
            {
                yield return
                    new MethodFunctionFactory(api, $"Set color label {label.Name}", $"SetColorLabel{label.Value}",
                        a => a.LrSelection.SetColorLabel(label));
            }
        }

        private static IEnumerable<MethodFunctionFactory> CreateSetRating(LrApi api)
        {
            for (var i = 0; i <= 5; i++)
            {
                var rating = i;
                yield return new MethodFunctionFactory(api, $"Set rating to {rating}", $"SetRatingTo{rating}",
                    a => a.LrSelection.SetRating(rating));
            }
        } 
    }
}