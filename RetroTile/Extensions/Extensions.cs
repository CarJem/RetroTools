using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Collections.Specialized;
using System.Data;
using System.Windows.Threading;

namespace RetroTile.Extensions
{
    public static class Extensions
    {
        public static void GetRowColIndex(this System.Windows.Controls.Grid @this, System.Windows.Point position, out int row, out int column)
        {
            column = -1;
            double total = 0;
            foreach (System.Windows.Controls.ColumnDefinition clm in @this.ColumnDefinitions)
            {
                if (position.X < total)
                {
                    break;
                }
                column++;
                total += clm.ActualWidth;
            }
            row = -1;
            total = 0;
            foreach (System.Windows.Controls.RowDefinition rowDef in @this.RowDefinitions)
            {
                if (position.Y < total)
                {
                    break;
                }
                row++;
                total += rowDef.ActualHeight;
            }
        }

        private static Action EmptyDelegate = delegate () { };


        public static void Refresh(this System.Windows.UIElement uiElement)
        {
            uiElement.Dispatcher.Invoke(DispatcherPriority.Render, EmptyDelegate);
        }
    }


}
