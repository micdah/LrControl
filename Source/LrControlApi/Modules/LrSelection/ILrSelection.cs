namespace LrControlApi.Modules.LrSelection
{
    public interface ILrSelection
    {
        /// <summary>
        ///     Clears all color labels from the selection.
        /// </summary>
        void ClearLabels();

        /// <summary>
        ///     Decreases the rating of the selection.
        /// </summary>
        void DecreaseRating();

        /// <summary>
        ///     Removes the active photo from the selection.
        /// </summary>
        void DeselectActive();

        /// <summary>
        ///     Deselects all photos except for the active photo.
        /// </summary>
        void DeselectOthers();

        /// <summary>
        ///     Extends the existing selection, selecting more photos to its beginning or end. Behaves exactly like the
        ///     Shift+Left/Right Arrow keys in Library grid.
        /// </summary>
        /// <param name="direction">either "left" or "right"</param>
        /// <param name="amount">number of photos to add to the selection in that direction</param>
        void ExtendSelection(Direction direction, int amount);

        /// <summary>
        ///     Sets the flag state of the selction to pick.
        /// </summary>
        void FlagAsPicked();

        /// <summary>
        ///     Sets the flag state of the selection to reject.
        /// </summary>
        void FlagAsRejected();

        /// <summary>
        ///     Returns the color label assigned to the active photo, one of: "red", "yellow", "green", "blue", "purple", "other",
        ///     or "none". The underlying metadata values that these names map to will depend on the current Color Label Set. The
        ///     default label set maps these names to "Red", "Yellow", "Green", "Blue", and "Purple". The return value "other"
        ///     indicates that the photo has a label that does not match any values in the current set.
        /// </summary>
        /// <returns></returns>
        ColorLabel GetColorLabel();

        /// <summary>
        ///     Returns the pick flag state of the active photo as a number (-1 = reject, 0 = none, 1 = pick).
        /// </summary>
        /// <returns></returns>
        Flag GetFlag();

        /// <summary>
        ///     Returns the rating of the selection as a number (0-5).
        /// </summary>
        /// <returns></returns>
        int GetRating();

        /// <summary>
        ///     Increases the rating of the selection.
        /// </summary>
        void IncreaseRating();

        /// <summary>
        ///     Advances the selection to the next photo in the filmstrip.
        /// </summary>
        void NextPhoto();

        /// <summary>
        ///     Advances the selection to the previous photo in the filmstrip.
        /// </summary>
        void PreviousPhoto();

        /// <summary>
        ///     Clears the flag state of the selection.
        /// </summary>
        void RemoveFlag();

        /// <summary>
        ///     Selects all photos in the filmstrip.
        /// </summary>
        void SelectAll();

        /// <summary>
        ///     Selects the first photo in the selection, or in the entire filmstrip if there is no selection. Only available in
        ///     the Library module.
        /// </summary>
        void SelectFirstPhoto();

        /// <summary>
        ///     Inverts the selection in the filmstrip.
        /// </summary>
        void SelectInverse();

        /// <summary>
        ///     Selects the last photo in the selection, or in the entire filmstrip if there is no selection. Only available in the
        ///     Library module.
        /// </summary>
        void SelectLastPhoto();

        /// <summary>
        ///     Deselects all photos in the filmstrip.
        /// </summary>
        void SelectNone();

        /// <summary>
        ///     Sets the color label of the selection, one of: "red", "yellow", "green", "blue", "purple", or "none". The
        ///     underlying metadata values that these names map to will depend on the current Color Label Set. The default label
        ///     set maps these names to "Red", "Yellow", "Green", "Blue", and "Purple".
        /// </summary>
        /// <param name="label"></param>
        void SetColorLabel(ColorLabel label);

        /// <summary>
        ///     Sets the rating of the selection.
        /// </summary>
        /// <param name="rating"></param>
        void SetRating(int rating);

        /// <summary>
        ///     Toggles the state of the Blue color label of the selection.
        /// </summary>
        void ToggleBlueLabel();

        /// <summary>
        ///     Toggles the state of the Green color label of the selection.
        /// </summary>
        void ToggleGreenLabel();

        /// <summary>
        ///     Toggles the state of the Purple color label of the selection.
        /// </summary>
        void TogglePurpleLabel();

        /// <summary>
        ///     Toggles the state of the Red color label of the selection.
        /// </summary>
        void ToggleRedLabel();

        /// <summary>
        ///     Toggles the state of the Yellow color label of the selection.
        /// </summary>
        void ToggleYellowLabel();
    }
}