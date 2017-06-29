using System.Windows;
using System.Windows.Media;

namespace LrControl.Gui.Utils
{
    public static class DependencyObjectExtensions
    {
        public static T FindParent<T>(this DependencyObject child) where T : class
        {
            var parentObject = VisualTreeHelper.GetParent(child);
            if (parentObject == null) return null;

            var parent = parentObject as T;
            if (parent != null)
            {
                return parent;
            }
            else
            {
                return FindParent<T>(parentObject);
            }
        }
    }
}