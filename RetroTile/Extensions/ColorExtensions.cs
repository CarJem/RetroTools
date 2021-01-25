using SDColor = System.Drawing.Color;
using SWMColor = System.Windows.Media.Color;
using System.Windows.Controls;
using System.Windows.Media;

namespace RetroTile.Extensions
{
    public static class ColorExtensions
    {
        public static SDColor ContrastColor(SDColor color)
        {
            int d = 0;

            // Counting the perceptive luminance - human eye favors green color... 
            double luminance = (0.299 * color.R + 0.587 * color.G + 0.114 * color.B) / 255;

            if (luminance > 0.5)
                d = 0; // bright colors - black font
            else
                d = 255; // dark colors - white font

            return SDColor.FromArgb(d, d, d);
        }


        public static SWMColor ToSWMColor(this SDColor color) => SWMColor.FromArgb(color.A, color.R, color.G, color.B);
        public static SDColor ToSDColor(this SWMColor color) => SDColor.FromArgb(color.A, color.R, color.G, color.B);

        private static SDColor GetResource(Control Source, string ResourceName)
        {
            try
            {
                var c = (SolidColorBrush)Source.FindResource(ResourceName);
                if (c != null) return SDColor.FromArgb(c.Color.A, c.Color.R, c.Color.G, c.Color.B);
                else return SDColor.Empty;
            }
            catch
            {
                return SDColor.Empty;
            }
        }

        public static SolidColorBrush GetSCBResource(Control Source, string ResourceName)
        {
            try
            {
                var c = (SolidColorBrush)Source.FindResource(ResourceName);
                if (c != null) return c;
                else return new SolidColorBrush();
            }
            catch
            {
                return new SolidColorBrush();
            }
        }
    }
}
