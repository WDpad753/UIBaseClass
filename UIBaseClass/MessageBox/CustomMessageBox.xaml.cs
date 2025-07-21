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
using System.Windows.Shapes;
using UIBaseClass.Controls.StyleCommands;

namespace UIBaseClass.MessageBox
{
    /// <summary>
    /// Interaction logic for CustomMessageBox.xaml
    /// </summary>
    public partial class CustomMessageBox : Window
    {
        private Point startPoint;
        private double initialHorizontalOffset;
        private double initialVerticalOffset;
        private bool draggingPopup = false;
        private Point initialMousePosition;
        private Point startDragPoint;
        static CustomMessageBox? messageBox;
        static CustomDialogResult result = CustomDialogResult.No;

        public CustomMessageBox()
        {
            InitializeComponent();
            //messageBox = new CustomMessageBox();
        }

        public enum CustomDialogResult
        {
            None,
            Yes,
            No,
            Cancel,
        }

        public enum CustomDialogTitle
        {
            Error,
            Info,
            Warning,
            Confirm
        }

        public enum CustomDialogButton
        {
            Ok,
            No,
            Yes,
            Cancel,
            Confirm
        }

        public CustomDialogResult Show(string message, CustomDialogTitle title, params CustomDialogButton[] buttons)
        {
            messageBox = new CustomMessageBox();
            messageBox.txtMessage.Text = message;
            messageBox.txtTitle.Text = messageBox.GetTitle(title);

            // Clear any existing buttons (if reused)
            messageBox.ButtonsPanel.Children.Clear();

            // Adding buttons based on the params list:
            foreach (var button in buttons)
            {
                string buttonText = GetButton(button); // Get the display text for the button using your method
                AddButton(buttonText, button);         // Add the button to the custom message box
            }

            switch (title)
            {
                case CustomDialogTitle.Error:
                    messageBox.iconMsg.Kind = MaterialDesignThemes.Wpf.PackIconKind.Error;
                    messageBox.iconMsg.Foreground = Brushes.DarkRed;
                    break;
                case CustomDialogTitle.Info:
                    messageBox.iconMsg.Kind = MaterialDesignThemes.Wpf.PackIconKind.InfoCircle;
                    messageBox.iconMsg.Foreground = Brushes.DarkBlue;
                    break;
                case CustomDialogTitle.Confirm:
                    messageBox.iconMsg.Kind = MaterialDesignThemes.Wpf.PackIconKind.QuestionMark;
                    messageBox.iconMsg.Foreground = Brushes.DarkRed;
                    break;
                case CustomDialogTitle.Warning:
                    messageBox.iconMsg.Kind = MaterialDesignThemes.Wpf.PackIconKind.Warning;
                    messageBox.iconMsg.Foreground = Brushes.DarkRed;
                    break;
            }

            messageBox.ShowDialog(); // Use ShowDialog to show the window

            return result;
        }

        public string GetTitle(CustomDialogTitle value)
        {
            return Enum.GetName(typeof(CustomDialogTitle), value);
        }

        public static string GetButton(CustomDialogButton value)
        {
            return Enum.GetName(typeof(CustomDialogButton), value);
        }

        private static void AddButton(string buttonText, CustomDialogButton dialogButton)
        {
            // Create the button
            var button = new RoundButton
            {
                Content = buttonText,
                Width = 100,
                Height = 30,
                Background = dialogButton switch
                {
                    CustomDialogButton.Yes => Brushes.DarkGreen,
                    CustomDialogButton.No => Brushes.DarkRed,
                    CustomDialogButton.Ok => Brushes.Blue,
                    CustomDialogButton.Cancel => Brushes.Gray,
                    _ => Brushes.LightGray
                },
                Foreground = Brushes.White,
                Margin = new Thickness(5),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                FocusVisualStyle=null,
                //CornerRadius=new CornerRadius(50)
            };

            // Create a Style to Remove Hover Effects
            Style noHoverStyle = new Style(typeof(RoundButton));
            noHoverStyle.Setters.Add(new Setter(RoundButton.BackgroundProperty, Brushes.LightGray));
            noHoverStyle.Setters.Add(new Setter(RoundButton.BorderBrushProperty, Brushes.Black));
            noHoverStyle.Setters.Add(new Setter(RoundButton.BorderThicknessProperty, new Thickness(1)));
            noHoverStyle.Setters.Add(new Setter(RoundButton.CornerRadiusProperty, new CornerRadius(10)));

            // Remove MouseOver Trigger
            ControlTemplate template = new ControlTemplate(typeof(RoundButton));
            FrameworkElementFactory border = new FrameworkElementFactory(typeof(Border));
            border.SetValue(Border.BackgroundProperty, new TemplateBindingExtension(RoundButton.BackgroundProperty));
            border.SetValue(Border.BorderBrushProperty, new TemplateBindingExtension(RoundButton.BorderBrushProperty));
            border.SetValue(Border.BorderThicknessProperty, new TemplateBindingExtension(RoundButton.BorderThicknessProperty));
            border.SetValue(Border.CornerRadiusProperty, new TemplateBindingExtension(RoundButton.CornerRadiusProperty));
            FrameworkElementFactory contentPresenter = new FrameworkElementFactory(typeof(ContentPresenter));
            contentPresenter.SetValue(ContentPresenter.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            contentPresenter.SetValue(ContentPresenter.VerticalAlignmentProperty, VerticalAlignment.Center);
            border.AppendChild(contentPresenter);
            template.VisualTree = border;

            noHoverStyle.Setters.Add(new Setter(RoundButton.TemplateProperty, template));

            // Apply Style to the Button
            button.Style = noHoverStyle;

            // Set the click event to set the result and close the dialog
            button.Click += (sender, e) =>
            {
                result = dialogButton switch
                {
                    CustomDialogButton.Yes => CustomDialogResult.Yes,
                    CustomDialogButton.No => CustomDialogResult.No,
                    CustomDialogButton.Ok => CustomDialogResult.None,
                    CustomDialogButton.Cancel => CustomDialogResult.Cancel,
                    _ => CustomDialogResult.None
                };

                messageBox?.Close();
            };

            // Add the button to the ButtonsPanel
            messageBox?.ButtonsPanel.Children.Add(button);
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (draggingPopup)
            {
                Point currentMousePosition = e.GetPosition(this); // Get the mouse position relative to the UserControl
                double offsetX = currentMousePosition.X - initialMousePosition.X;
                double offsetY = currentMousePosition.Y - initialMousePosition.Y;

                // Only move the window if the mouse has moved by a certain threshold
                if (Math.Abs(offsetX) > 1 || Math.Abs(offsetY) > 1)
                {
                    this.Left += offsetX;
                    this.Top += offsetY;

                    initialMousePosition = currentMousePosition; // Update initial position
                }
            }
        }

        private void Window_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released)
            {
                draggingPopup = false;
                Mouse.Capture(null);
            }
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            draggingPopup = true;
            this.DragMove();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
