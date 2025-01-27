using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ORACULO_PR
{
    /// <summary>
    /// UserControl for timeline interaction and navigation.
    /// </summary>
    public partial class Timeline : UserControl
    {
        private bool isInteracting = false;
        private Point startPoint;
        private Point touchStartPoint;
        private double totalXOffset = 0.0;
        private TranslateTransform translation = new TranslateTransform();

        public Timeline()
        {
            InitializeComponent();
            HideElements(); // Initialize by hiding page elements
            InitializeEventHandlers(); // Set up interaction handlers
            Touch.FrameReported += Touch_FrameReported; // Monitor global touch events
            var sharedTimer = MainWindow.SharedTimer;
        }

        private void HideElements()
        {
            // Hide specific page elements
            var elementsToHide = new UIElement[]
            {
                C2012, C2013, C2014, C2015, C2016, C2017, C2018, C2019, C2020, C2021, C2022, C2023,
                B2012, B2013, B2014, B2015, B2016, B2017, B2018, B2019, B2020, B2021, B2022, B2023,
                BtnScroll, TLBtn, TLBtn2
            };

            foreach (var element in elementsToHide)
            {
                element.Visibility = Visibility.Collapsed;
            }
        }

        private void InitializeEventHandlers()
        {
            // Bind mouse and touch events to the scroll button
            BtnScroll.TouchDown += BtnScroll_TouchDown;
            BtnScroll.TouchMove += BtnScroll_TouchMove;
            BtnScroll.TouchUp += BtnScroll_TouchUp;
            BtnScroll.MouseDown += BtnScroll_MouseDown;
            BtnScroll.MouseMove += BtnScroll_MouseMove;
            BtnScroll.MouseUp += BtnScroll_MouseUp;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // Restart the application by navigating to the start page
            Frame frame = (Frame)this.Parent;
            MainWindow.SharedTimer.Stop();
            frame.NavigationService.Navigate(new StartPage());

            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.SerialPort.WriteLine("Restart");
            MainWindow.SharedTimer.Start();
        }

        private void TimerSet()
        {
            // Reset and restart the shared timer
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.SerialPort.WriteLine("btnAct");
            MainWindow.SharedTimer.Stop();
            MainWindow.SharedTimer.Start();
        }

        private void Touch_FrameReported(object sender, TouchFrameEventArgs e)
        {
            TimerSet(); // Reset timer on any touch interaction
        }

        private void ScreenTouched_Click(object sender, RoutedEventArgs e)
        {
            // Show specific timeline elements on screen interaction
            var elementsToShow = new UIElement[]
            {
                BtnScroll, B2012, B2013, B2014, B2015, B2016, B2017, B2018, B2019, B2020, B2021, B2022, B2023
            };

            foreach (var element in elementsToShow)
            {
                element.Visibility = Visibility.Visible;
            }

            TimerSet();
        }

        private void ToggleControlVisibility(UIElement[] elements, Visibility visibility)
        {
            // Toggle visibility for a given list of elements
            foreach (var element in elements)
            {
                element.Visibility = visibility;
            }
        }

        private void Control_TouchDown(object sender, TouchEventArgs e)
        {
            // Handle visibility toggle for associated timeline image
            var button = sender as Button;
            var image = button?.Tag as UIElement;
            if (image != null)
            {
                image.Visibility = Visibility.Visible;
                TLBtn.Visibility = Visibility.Visible;
            }
        }

        private void Control_Click(object sender, RoutedEventArgs e)
        {
            // Same as Control_TouchDown but triggered by mouse click
            var button = sender as Button;
            var image = button?.Tag as UIElement;
            if (image != null)
            {
                image.Visibility = Visibility.Visible;
                TLBtn.Visibility = Visibility.Visible;
            }
        }

        private void TLBtn_Click(object sender, RoutedEventArgs e)
        {
            // Hide page elements and reset timer
            var elementsToHide = new UIElement[]
            {
                C2012, C2013, C2014, C2015, C2016, C2017, C2018, C2019, C2020, C2021, C2022, C2023,
                TLBtn
            };

            foreach (var element in elementsToHide)
            {
                element.Visibility = Visibility.Collapsed;
            }
            TimerSet();
        }

        private void TLBtn2_Click(object sender, RoutedEventArgs e)
        {
            // Collapse additional page elements and reset visibility
            var elementsToCollapse = new UIElement[]
            {
                TLBtn2, C2012, C2013, C2014, C2015, C2016, C2017, C2018, C2019, C2020, C2021, C2022, C2023
            };

            foreach (var element in elementsToCollapse)
            {
                element.Visibility = Visibility.Collapsed;
            }
            TL.Visibility = Visibility.Visible;
            TimerSet();
        }

        private void BtnScroll_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Start dragging interaction for scrolling
            isInteracting = true;
            startPoint = e.GetPosition(BtnScroll);
            e.Handled = true;
            BtnScroll.CaptureMouse();
            BtnStack.CaptureMouse();
        }

        private void BtnScroll_MouseMove(object sender, MouseEventArgs e)
        {
            // Handle horizontal scrolling while dragging
            if (isInteracting)
            {
                Point currentPoint = e.GetPosition(BtnScroll);
                double xOffset = startPoint.X - currentPoint.X;
                totalXOffset += xOffset;

                double maxOffset = Math.Max(TL.ActualWidth, BtnStack.ActualWidth) - BtnScroll.ActualWidth;
                totalXOffset = Math.Max(0, Math.Min(totalXOffset, maxOffset));

                TL.Margin = new Thickness(-totalXOffset, 0, 0, 0);
                BtnStack.Margin = new Thickness(-totalXOffset, 0, 0, 0);
                startPoint = currentPoint;

                e.Handled = true;
            }
        }

        private void BtnScroll_MouseUp(object sender, MouseButtonEventArgs e)
        {
            // End dragging interaction for scrolling
            isInteracting = false;
            e.Handled = true;
            BtnScroll.ReleaseMouseCapture();
            BtnStack.ReleaseMouseCapture();
        }

        private void BtnScroll_TouchDown(object? sender, TouchEventArgs e)
        {
            // Start touch interaction for scrolling
            (sender as UIElement)?.CaptureTouch(e.TouchDevice);
            isInteracting = true;
            touchStartPoint = e.GetTouchPoint(BtnScroll).Position;
            e.Handled = true;
        }

        private void BtnScroll_TouchMove(object? sender, TouchEventArgs e)
        {
            // Handle horizontal scrolling during touch interaction
            if (isInteracting)
            {
                Point currentTouchPoint = e.GetTouchPoint(BtnScroll).Position;
                double xOffset = touchStartPoint.X - currentTouchPoint.X;
                totalXOffset += xOffset;

                double maxOffset = Math.Max(TL.ActualWidth, BtnStack.ActualWidth) - BtnScroll.ActualWidth;
                totalXOffset = Math.Max(0, Math.Min(totalXOffset, maxOffset));

                TL.Margin = new Thickness(-totalXOffset, 0, 0, 0);
                BtnStack.Margin = new Thickness(-totalXOffset, 0, 0, 0);

                touchStartPoint = currentTouchPoint;
                e.Handled = false;
            }
        }

        private void BtnScroll_TouchUp(object? sender, TouchEventArgs e)
        {
            // End touch interaction for scrolling
            (sender as UIElement)?.ReleaseTouchCapture(e.TouchDevice);
            isInteracting = false;
            e.Handled = false;
        }

        private void BtnScroll_PreviewTouchDown(object sender, TouchEventArgs e)
        {
            // Synchronize mouse and touch events
            Mouse.Synchronize();
        }
    }
}
