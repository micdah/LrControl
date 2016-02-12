using LrControlApi.Common;

namespace LrControlApi.LrApplicationView
{
    public class PrimaryView : ClassEnum<string, PrimaryView>
    {
        public static readonly PrimaryView Loupe                        = new PrimaryView("Loupe", "loupe");
        public static readonly PrimaryView Grid                         =new PrimaryView("Grid", "grid");
        public static readonly PrimaryView Compare                      = new PrimaryView("Compare", "compare");
        public static readonly PrimaryView Survey                       = new PrimaryView("Survey", "survey");
        public static readonly PrimaryView People                       = new PrimaryView("People", "people");
        public static readonly PrimaryView DevelopLoupe                 = new PrimaryView("Develop Loupe", "develop_loupe");
        public static readonly PrimaryView DevelopBeforeAfterHorizontal = new PrimaryView("Develop Before/After Horizontal", "develop_before_after_horiz");
        public static readonly PrimaryView DevelopBeforeAfterVertical   = new PrimaryView("Develop Before/After Vertical", "develop_before_after_vert");
        public static readonly PrimaryView DevelopBefore                = new PrimaryView("Develop Before", "develop_before");

        private PrimaryView(string name, string value) : base(name, value)
        {
        }
    }
}