using LrControl.LrPlugin.Api.Common;

namespace LrControl.LrPlugin.Api.Modules.LrApplicationView
{
    public class PrimaryView : ClassEnum<string, PrimaryView>
    {
        public static readonly PrimaryView Loupe                        = new PrimaryView("loupe", "Loupe");
        public static readonly PrimaryView Grid                         =new PrimaryView("grid", "Grid");
        public static readonly PrimaryView Compare                      = new PrimaryView("compare", "Compare");
        public static readonly PrimaryView Survey                       = new PrimaryView("survey", "Survey");
        public static readonly PrimaryView People                       = new PrimaryView("people", "People");
        public static readonly PrimaryView DevelopLoupe                 = new PrimaryView("develop_loupe", "Develop Loupe");
        public static readonly PrimaryView DevelopBeforeAfterHorizontal = new PrimaryView("develop_before_after_horiz", "Develop Before/After Horizontal");
        public static readonly PrimaryView DevelopBeforeAfterVertical   = new PrimaryView("develop_before_after_vert", "Develop Before/After Vertical");
        public static readonly PrimaryView DevelopBefore                = new PrimaryView("develop_before", "Develop Before");

        
        private PrimaryView(string value, string name) : base(value, name)
        {
        }
    }
}