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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UIBaseClass.Controls.Spinner
{
    /// <summary>
    /// Interaction logic for SpinnerProgressBar.xaml
    /// </summary>
    public partial class SpinnerProgressBar : UserControl
    {
        public SpinnerProgressBar()
        {
            InitializeComponent();
            StartMarqueeAnimation();
        }

        public void StartMarqueeAnimation()
        {
            // Create a double animation to rotate the Path (marquee effect)
            var rotationAnimation = new DoubleAnimation
            {
                To = 360, // Rotate a full circle (360 degrees)
                Duration = TimeSpan.FromSeconds(1), // Adjust the duration as needed
                RepeatBehavior = RepeatBehavior.Forever // Keep repeating the animation
            };

            progressPath.RenderTransform = new RotateTransform(); // Add a RotateTransform
            progressPath.RenderTransformOrigin = new Point(1, 1); // Set the rotation origin to the center

            // Begin the rotation animation
            progressPath.RenderTransform.BeginAnimation(RotateTransform.AngleProperty, rotationAnimation);
        }
    }
}
