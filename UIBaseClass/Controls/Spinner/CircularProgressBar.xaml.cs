using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace UIBaseClass.Controls.Spinner
{
    /// <summary>
    /// Interaction logic for CircularProgressBar.xaml
    /// </summary>
    public partial class CircularProgressBar : UserControl
    {
        private double progress = 0;
        private DispatcherTimer timer = new DispatcherTimer();

        public CircularProgressBar()
        {
            InitializeComponent();

            // Set up a DispatcherTimer to update the progress bar
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // Simulate progress
            progress += 1;
            if (progress > 100)
            {
                progress = 0;
            }

            // Update the circular progress bar
            double angle = 360 * (progress / 100);
            progressRect.Width = angle;

            // Update the progress text
            progressText.Text = $"{progress}%";
        }
    }
}
