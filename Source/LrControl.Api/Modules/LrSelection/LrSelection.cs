using LrControl.Api.Common;
using LrControl.Api.Communication;

namespace LrControl.Api.Modules.LrSelection
{
    internal class LrSelection : ModuleBase<LrSelection>, ILrSelection
    {
        public LrSelection(MessageProtocol<LrSelection> messageProtocol) : base(messageProtocol)
        {
        }

        public bool ClearLabels()
        {
            return Invoke(nameof(ClearLabels));
        }

        public bool DecreaseRating()
        {
            return Invoke(nameof(DecreaseRating));
        }

        public bool DeselectActive()
        {
            return Invoke(nameof(DeselectActive));
        }

        public bool DeselectOthers()
        {
            return Invoke(nameof(DeselectOthers));
        }

        public bool ExtendSelection(Direction direction, int amount)
        {
            return Invoke(nameof(ExtendSelection), direction, amount);
        }

        public bool FlagAsPick()
        {
            return Invoke(nameof(FlagAsPick));
        }

        public bool FlagAsReject()
        {
            return Invoke(nameof(FlagAsReject));
        }

        public bool GetColorLabel(out ColorLabel colorLabel)
        {
            if (!Invoke(out string result, nameof(GetColorLabel)))
                return False(out colorLabel);

            colorLabel = ColorLabel.GetEnumForValue(result);
            return colorLabel != null;
        }

        public bool GetFlag(out Flag flag)
        {
            if (!Invoke(out int result, nameof(GetFlag)))
                return False(out flag);

            flag = Flag.GetEnumForValue(result);
            return flag != null;
        }

        public bool GetRating(out int rating)
        {
            return Invoke(out rating, nameof(GetRating));
        }

        public bool IncreaseRating()
        {
            return Invoke(nameof(IncreaseRating));
        }

        public bool NextPhoto()
        {
            return Invoke(nameof(NextPhoto));
        }

        public bool PreviousPhoto()
        {
            return Invoke(nameof(PreviousPhoto));
        }

        public bool RemoveFlag()
        {
            return Invoke(nameof(RemoveFlag));
        }

        public bool SelectAll()
        {
            return Invoke(nameof(SelectAll));
        }

        public bool SelectFirstPhoto()
        {
            return Invoke(nameof(SelectFirstPhoto));
        }

        public bool SelectInverse()
        {
            return Invoke(nameof(SelectInverse));
        }

        public bool SelectLastPhoto()
        {
            return Invoke(nameof(SelectLastPhoto));
        }

        public bool SelectNone()
        {
            return Invoke(nameof(SelectNone));
        }

        public bool SetColorLabel(ColorLabel label)
        {
            return Invoke(nameof(SetColorLabel), label);
        }

        public bool SetRating(int rating)
        {
            return Invoke(nameof(SetRating), rating);
        }

        public bool ToggleBlueLabel()
        {
            return Invoke(nameof(ToggleBlueLabel));
        }

        public bool ToggleGreenLabel()
        {
            return Invoke(nameof(ToggleGreenLabel));
        }

        public bool TogglePurpleLabel()
        {
            return Invoke(nameof(TogglePurpleLabel));
        }

        public bool ToggleRedLabel()
        {
            return Invoke(nameof(ToggleRedLabel));
        }

        public bool ToggleYellowLabel()
        {
            return Invoke(nameof(ToggleYellowLabel));
        }
    }
}