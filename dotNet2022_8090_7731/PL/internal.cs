using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace PL.Drones
{
    internal class InternalClass
    {
        public static void TextBox_OnPreviewTextInputt(object sender, TextCompositionEventArgs e)
        {
            var textBox = (TextBox)sender;
            if (e.Handled = new Regex("[^0-9]+").IsMatch(e.Text))
            {
                textBox.Background = Brushes.Gray;
                ToolTip toolTip = new ToolTip();
                AddToolTip(textBox, " Input has to contain only digits ");
            }
            else
            {
                textBox.Background = Brushes.White;
            }
        }
        public static  void AddToolTip(TextBox textBox, string str)
            {
            ToolTip toolTip = new ToolTip();
            toolTip.Content = str;
            toolTip.IsOpen = true;
            textBox.ToolTip = toolTip;

            // Turn off ToolTip
            DispatcherTimer timer = new DispatcherTimer { Interval = new TimeSpan(0, 0, 1), IsEnabled = true };
            timer.Tick += new EventHandler(delegate (object timerSender, EventArgs timerArgs)
            {
                toolTip.IsOpen = false;
                timer = null;
            });
        }
    }

}