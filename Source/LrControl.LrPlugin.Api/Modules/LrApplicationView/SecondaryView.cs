using LrControl.LrPlugin.Api.Common;

namespace LrControl.LrPlugin.Api.Modules.LrApplicationView
{
    public class SecondaryView : ClassEnum<string, SecondaryView>
    {
        public static readonly SecondaryView Loupe       = new SecondaryView("loupe", "Loupe");
        public static readonly SecondaryView LiveLoupe   = new SecondaryView("live_loupe", "Live Loupe");
        public static readonly SecondaryView LockedLoupe = new SecondaryView("locked_loupe", "Locked Loupe");
        public static readonly SecondaryView Grid        = new SecondaryView("grid", "Grid");
        public static readonly SecondaryView Compare     = new SecondaryView("compare", "Compare");
        public static readonly SecondaryView Survey      = new SecondaryView("survey", "Survey");
        public static readonly SecondaryView Slideshow   = new SecondaryView("slideshow", "Slidesohw");

        
        private SecondaryView(string value, string name) : base(value, name)
        {
        }
    }
}