using LrControl.Api.Common;

namespace LrControl.Api.Modules.LrDevelopController
{
    public class PostCropVignetteStyle : ClassEnum<int,PostCropVignetteStyle>
    {
        public static readonly PostCropVignetteStyle HighlightPriority = new PostCropVignetteStyle(1,"Highlight Priority");
        public static readonly PostCropVignetteStyle ColorPriority     = new PostCropVignetteStyle(2,"Color Priority");
        public static readonly PostCropVignetteStyle PaintOverlay      = new PostCropVignetteStyle(3,"Paint Overlay");

        private PostCropVignetteStyle(int value, string name) : base(value, name)
        {
        }
    }
}