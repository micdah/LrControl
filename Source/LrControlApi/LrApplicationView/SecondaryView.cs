using System;
using LrControlApi.Common;

namespace LrControlApi.LrApplicationView
{
    public class SecondaryView : ClassEnum<string, SecondaryView>
    {
        public static readonly SecondaryView Loupe       = new SecondaryView("Loupe", "loupe");
        public static readonly SecondaryView LiveLoupe   = new SecondaryView("Live Loupe", "live_loupe");
        public static readonly SecondaryView LockedLoupe = new SecondaryView("Locked Loupe", "locked_loupe");
        public static readonly SecondaryView Grid        = new SecondaryView("Grid", "grid");
        public static readonly SecondaryView Compare     = new SecondaryView("Compare", "compare");
        public static readonly SecondaryView Survey      = new SecondaryView("Survey", "survey");
        public static readonly SecondaryView Slideshow   = new SecondaryView("Slidesohw", "slideshow");

        private SecondaryView(string name, string value) : base(name, value)
        {
        }
    }
}