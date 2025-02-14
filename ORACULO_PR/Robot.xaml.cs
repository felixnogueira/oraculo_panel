using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;


namespace ORACULO_PR
{
    /// <summary>
    /// Interação lógica para Robot.xam
    /// </summary>
    public partial class Robot : UserControl
    {

        public Robot()
        {
            // Initialize components and set initial visibility states for pages and controls
            InitializeComponent();
            SetupClickHandlers();

            // Show only the P1_Robot page, hide others
            SetPageVisibility(Visibility.Visible, P1_Robot);
            SetPageVisibility(Visibility.Collapsed, Prog_Int_Page, Montagem_Page, Gemeo_Page, Ind_Ali_Page, Ind_Auto_Page, Ind_Farm_Page, TLBtn2, BckBrdr);

            // Get the shared timer from the MainWindow
            var sharedTimer = MainWindow.SharedTimer;
        }

        // Helper method to set the visibility of multiple UI elements
        private void SetPageVisibility(Visibility visibility, params UIElement[] elements)
        {
            foreach (var element in elements)
            {
                element.Visibility = visibility;
            }
        }


        // Timer Tick event handler, navigates to StartPage and sends a "Restart" command over the serial port
        private void Timer_Tick(object sender, EventArgs e)
        {
            Frame frame = (Frame)this.Parent;

            // Stop the shared timer, navigate to the StartPage, and restart the timer
            MainWindow.SharedTimer.Stop();
            frame.NavigationService.Navigate(new StartPage());

            // Send "Restart" command to the serial port
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.SerialPort.WriteLine("Restart");

            // Restart the shared timer
            MainWindow.SharedTimer.Start();
        }

        private void TimerSet()
        {
            var mainWindow = (MainWindow)Application.Current.MainWindow;

            // Send "btnAct" command to the serial port and restart the timer
            mainWindow.SerialPort.WriteLine("btnAct");
            MainWindow.SharedTimer.Stop();
            MainWindow.SharedTimer.Start();
        }

        private Button? lastBtn = null;

        private void PlayVideo(string videoFileName, MediaElement mediaElement)
        {
            // Construct the full path to the video file
            string videoPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, videoFileName);

            // Set the source and play the video
            mediaElement.Source = new Uri(videoPath);
            mediaElement.Play();
        }

        private void Prog_Int_Btn_Click(object sender, RoutedEventArgs e)
        {
            // Show the Prog_Int_Page and hide others
            SetPageVisibility(Visibility.Visible, Prog_Int_Page);
            SetPageVisibility(Visibility.Collapsed, P1_Robot, Montagem_Page, Gemeo_Page, Ind_Ali_Page, Ind_Auto_Page, Ind_Farm_Page, PI2Overlay, PI1Overlay, TLBtn2, BckBrdr);

            // Set the timer
            TimerSet();
        }

        // Button click handler for Gemeo_Page
        private void Gemeo_Btn_Click(object sender, RoutedEventArgs e)
        {
            // Show the Gemeo_Page and hide others
            SetPageVisibility(Visibility.Visible, Gemeo_Page);
            SetPageVisibility(Visibility.Collapsed, P1_Robot, Montagem_Page, Prog_Int_Page, videoPI, GD1Overlay, GD2Overlay, GD3Overlay, TLBtn2, BckBrdr);

            // Set the timer
            TimerSet();
        }

        // Button click handler for Montagem_Page
        private void Montagem_Btn_Click(object sender, RoutedEventArgs e)
        {
            // Show the Montagem_Page and hide others
            SetPageVisibility(Visibility.Visible, Montagem_Page);
            SetPageVisibility(Visibility.Collapsed, P1_Robot, Gemeo_Page, Prog_Int_Page, Ind_Ali_Page, Ind_Auto_Page, Ind_Farm_Page, videoMont, Mont1Overlay, Mont2Overlay, Mont3Overlay, Mont4Overlay, TLBtn2, BckBrdr);

            // Set the timer
            TimerSet();
        }

        // Button click handler for Ind_Ali_Page
        private void Ind_Ali_Btn_Click(object sender, RoutedEventArgs e)
        {
            // Show the Ind_Ali_Page and hide other pages and elements
            SetPageVisibility(Visibility.Visible, Ind_Ali_Page);
            SetPageVisibility(Visibility.Collapsed, P1_Robot, Gemeo_Page, Prog_Int_Page, Ind_Auto_Page, Ind_Farm_Page, Montagem_Page, videoIAl, IAl1Overlay, IAl2Overlay, IAl3Overlay, TLBtn2, BckBrdr);

            // Set the timer
            TimerSet();
        }

        // Button click handler for Ind_Auto_Page
        private void Ind_Auto_Btn_Click(object sender, RoutedEventArgs e)
        {
            // Show the Ind_Auto_Page and hide other pages and elements
            SetPageVisibility(Visibility.Visible, Ind_Auto_Page);
            SetPageVisibility(Visibility.Collapsed, P1_Robot, Gemeo_Page, Prog_Int_Page, Ind_Ali_Page, Ind_Farm_Page, Montagem_Page, videoIAu, IAu1Overlay, IAu2Overlay, TLBtn2, BckBrdr);

            // Set the timer
            TimerSet();
        }

        // Button click handler for Ind_Farm_Page
        private void Ind_Farm_Btn_Click(object sender, RoutedEventArgs e)
        {
            // Show the Ind_Farm_Page and hide other pages and elements
            SetPageVisibility(Visibility.Visible, Ind_Farm_Page);
            SetPageVisibility(Visibility.Collapsed, P1_Robot, Gemeo_Page, Prog_Int_Page, Ind_Ali_Page, Montagem_Page, Ind_Auto_Page, videoIF, IF1Overlay, IF2Overlay, IF3Overlay, TLBtn2, BckBrdr);

            // Set the timer
            TimerSet();
        }

        // Helper method to play a video, set overlays, and update UI elements
        private async void PlayAndSetUI(string videoFile, MediaElement videoElement, Button lastButton, params UIElement[] overlays)
        {
            // Set timer and show video
            TimerSet();
            videoElement.Visibility = Visibility.Visible;

            // Hide all overlays
            foreach (var overlay in overlays)
            {
                overlay.Visibility = Visibility.Collapsed;
            }

            // Play the video
            PlayVideo(videoFile, videoElement);
            lastBtn = lastButton;

            // Wait before showing additional UI elements
            await Task.Delay(500);

            // Make UI elements visible
            TLBtn2.Visibility = Visibility.Visible;
            BckBrdr.Visibility = Visibility.Visible;
        }

        // RBbt1 Click handler
        private void RBbt1_Click(object sender, RoutedEventArgs e)
        {
            PlayAndSetUI("Resources/04BA_Programacao Interativa-01.mp4", videoPI, RBbt1, IAl1Overlay, IAl2Overlay, IAl3Overlay);
        }

        // RBbt2 Click handler
        private void RBbt2_Click(object sender, RoutedEventArgs e)
        {
            PlayAndSetUI("Resources/04BB_Programacao Interativa-02.mp4", videoPI, RBbt2, IAl1Overlay, IAl2Overlay, IAl3Overlay);
        }

        // Video GD Click handlers
        private void Vid1GD_On_Click(object sender, RoutedEventArgs e) => PlayAndSetUI("Resources/04CA_Gemeo Digital-01.mp4", videoGD, Vid1GD_On, GD1Overlay);
        private void Vid2GD_On_Click(object sender, RoutedEventArgs e) => PlayAndSetUI("Resources/04CB_Gemeo Digital-02.mp4", videoGD, Vid2GD_On, GD2Overlay);
        private void Vid3GD_On_Click(object sender, RoutedEventArgs e) => PlayAndSetUI("Resources/04CC_Gemeo Digital-03.mp4", videoGD, Vid3GD_On, GD3Overlay);

        // Back button handlers (for navigating to previous pages)
        private void GD_Back_Click(object sender, RoutedEventArgs e) => GoBackToPage(Gemeo_Page);
        private void Farm_Back_Click(object sender, RoutedEventArgs e) => GoBackToPage(Ind_Farm_Page);
        private void Mont_Back_Click(object sender, RoutedEventArgs e) => GoBackToPage(Montagem_Page);
        private void Ali_Back_Click(object sender, RoutedEventArgs e) => GoBackToPage(Ind_Ali_Page);
        private void Auto_Back_Click(object sender, RoutedEventArgs e) => GoBackToPage(Ind_Auto_Page);
        private void RBbackIMG_Click(object sender, RoutedEventArgs e) => GoBackToPage(Prog_Int_Page);

        // Helper method to go back to the previous page
        private void GoBackToPage(UIElement page)
        {
            page.Visibility = Visibility.Collapsed;
            P1_Robot.Visibility = Visibility.Visible;
            TimerSet();
        }

        // Vid1M to Vid4M Click handlers for Montagem Videos
        private void Vid1M_On_Click(object sender, RoutedEventArgs e) => PlayAndSetUI("Resources/04DA_Montagem de Eletronicos-01.mp4", videoMont, Vid1M_On, Mont1Overlay, Mont2Overlay, Mont3Overlay, Mont4Overlay);
        private void Vid2M_On_Click(object sender, RoutedEventArgs e) => PlayAndSetUI("Resources/04DB_Montagem de Eletronicos-02.mp4", videoMont, Vid2M_On, Mont1Overlay, Mont2Overlay, Mont3Overlay, Mont4Overlay);
        private void Vid3M_On_Click(object sender, RoutedEventArgs e) => PlayAndSetUI("Resources/04DC_Montagem de Eletronicos-03.mp4", videoMont, Vid3M_On, Mont1Overlay, Mont2Overlay, Mont3Overlay, Mont4Overlay);
        private void Vid4M_On_Click(object sender, RoutedEventArgs e) => PlayAndSetUI("Resources/04DD_Montagem de Eletronicos-04.mp4", videoMont, Vid4M_On, Mont1Overlay, Mont2Overlay, Mont3Overlay, Mont4Overlay);

        // Vid1F to Vid3F Click handlers for Farmaceutica Videos
        private void Vid1F_On_Click(object sender, RoutedEventArgs e) => PlayAndSetUI("Resources/04GA_Industria Farmaceutica-01.mp4", videoIF, Vid1F_On, IF1Overlay, IF2Overlay, IF3Overlay);
        private void Vid2F_On_Click(object sender, RoutedEventArgs e) => PlayAndSetUI("Resources/04GB_Industria Farmaceutica-02.mp4", videoIF, Vid2F_On, IF1Overlay, IF2Overlay, IF3Overlay);
        private void Vid3F_On_Click(object sender, RoutedEventArgs e) => PlayAndSetUI("Resources/04GC_Industria Farmaceutica-03.mp4", videoIF, Vid3F_On, IF1Overlay, IF2Overlay, IF3Overlay);

        // Vid1Au to Vid2Au Click handlers for Automotiva Videos
        private void Vid1Au_On_Click(object sender, RoutedEventArgs e) => PlayAndSetUI("Resources/04FA_Industria Automotiva-01.mp4", videoIAu, Vid1Au_On, IAu1Overlay, IAu2Overlay);
        private void Vid2Au_On_Click(object sender, RoutedEventArgs e) => PlayAndSetUI("Resources/04FB_Industria Automotiva-02.mp4", videoIAu, Vid2Au_On, IAu1Overlay, IAu2Overlay);

        // Vid1Al to Vid3Al Click handlers for Alimenticia Videos
        private void Vid1Al_On_Click(object sender, RoutedEventArgs e) => PlayAndSetUI("Resources/04EA_Industria Alimenticia-01.mp4", videoIAl, Vid1Al_On, IAl1Overlay, IAl2Overlay, IAl3Overlay);
        private void Vid2Al_On_Click(object sender, RoutedEventArgs e) => PlayAndSetUI("Resources/04EB_Industria Alimenticia-02.mp4", videoIAl, Vid2Al_On, IAl1Overlay, IAl2Overlay, IAl3Overlay);
        private void Vid3Al_On_Click(object sender, RoutedEventArgs e) => PlayAndSetUI("Resources/04EC_Industria Alimenticia-03.mp4", videoIAl, Vid3Al_On, IAl1Overlay, IAl2Overlay, IAl3Overlay);

        private void UpdateOverlay()
        {
            var overlays = new Dictionary<Button, Action>
            {
                { Vid1GD_On, () => SetOverlayVisibility(GD1Overlay, GD2Overlay, GD3Overlay) },
                { Vid2GD_On, () => SetOverlayVisibility(GD2Overlay, GD1Overlay, GD3Overlay) },
                { Vid3GD_On, () => SetOverlayVisibility(GD3Overlay, GD1Overlay, GD2Overlay) },
                { RBbt1, () => SetOverlayVisibility(PI1Overlay, PI2Overlay) },
                { RBbt2, () => SetOverlayVisibility(PI2Overlay, PI1Overlay) },
                { Vid1M_On, () => SetOverlayVisibility(Mont1Overlay, Mont2Overlay, Mont3Overlay, Mont4Overlay) },
                { Vid2M_On, () => SetOverlayVisibility(Mont2Overlay, Mont1Overlay, Mont3Overlay, Mont4Overlay) },
                { Vid3M_On, () => SetOverlayVisibility(Mont3Overlay, Mont2Overlay, Mont1Overlay, Mont4Overlay) },
                { Vid4M_On, () => SetOverlayVisibility(Mont4Overlay, Mont2Overlay, Mont3Overlay, Mont1Overlay) },
                { Vid1F_On, () => SetOverlayVisibility(IF1Overlay, IF2Overlay, IF3Overlay) },
                { Vid2F_On, () => SetOverlayVisibility(IF2Overlay, IF1Overlay, IF3Overlay) },
                { Vid3F_On, () => SetOverlayVisibility(IF3Overlay, IF2Overlay, IF1Overlay) },
                { Vid1Al_On, () => SetOverlayVisibility(IAl1Overlay, IAl2Overlay, IAl3Overlay) },
                { Vid2Al_On, () => SetOverlayVisibility(IAl2Overlay, IAl1Overlay, IAl3Overlay) },
                { Vid3Al_On, () => SetOverlayVisibility(IAl3Overlay, IAl2Overlay, IAl1Overlay) },
                { Vid1Au_On, () => SetOverlayVisibility(IAu1Overlay, IAu2Overlay) },
                { Vid2Au_On, () => SetOverlayVisibility(IAu2Overlay, IAu1Overlay) }
            };

            // Run the action if the key exists in the dictionary
            if (overlays.ContainsKey(lastBtn))
            {
                overlays[lastBtn].Invoke();
            }

            // Common visibility adjustments
            TLBtn2.Visibility = Visibility.Collapsed;
            BckBrdr.Visibility = Visibility.Collapsed;
        }

        private void SetOverlayVisibility(UIElement visibleOverlay, params UIElement[] hiddenOverlays)
        {
            visibleOverlay.Visibility = Visibility.Visible;
            foreach (var overlay in hiddenOverlays)
            {
                overlay.Visibility = Visibility.Collapsed;
            }
        }


        private void StopVideoAndUpdateVisibility(MediaElement video)
        {
            video.Stop();
            video.Visibility = Visibility.Collapsed;
            UpdateOverlay();
        }

        private void videoGD_MediaEnded(object sender, RoutedEventArgs e) => StopVideoAndUpdateVisibility(videoGD);
        private void videoPI_MediaEnded(object sender, RoutedEventArgs e) => StopVideoAndUpdateVisibility(videoPI);
        private void videoMont_MediaEnded(object sender, RoutedEventArgs e) => StopVideoAndUpdateVisibility(videoMont);
        private void videoIAl_MediaEnded(object sender, RoutedEventArgs e) => StopVideoAndUpdateVisibility(videoIAl);
        private void videoIAu_MediaEnded(object sender, RoutedEventArgs e) => StopVideoAndUpdateVisibility(videoIAu);
        private void videoIF_MediaEnded(object sender, RoutedEventArgs e) => StopVideoAndUpdateVisibility(videoIF);

        private void Overlay_Click(object sender, RoutedEventArgs e)
        {
            if (sender is UIElement overlay)
            {
                overlay.Visibility = Visibility.Collapsed;
                TimerSet();
            }
        }


        private void SetupClickHandlers()
        {
            var overlays = new Button[]  
            {
                PI1Overlay, PI2Overlay, Mont1Overlay, Mont2Overlay, Mont3Overlay, Mont4Overlay,
                IAl1Overlay, IAl2Overlay, IAl3Overlay, IAu1Overlay, IAu2Overlay, IF1Overlay, IF2Overlay, IF3Overlay
            };

            foreach (var overlay in overlays)
            {
                overlay.Click += Overlay_Click;
            }
        }




        private void TLBtn2_Click(object sender, RoutedEventArgs e)
        {
            var videoElements = new[] { videoGD, videoIAl, videoIAu, videoIF, videoMont, videoPI };

            foreach (var video in videoElements)
            {
                video.Visibility = Visibility.Collapsed;
                video.Stop();
            }

            TLBtn2.Visibility = Visibility.Collapsed;
            BckBrdr.Visibility = Visibility.Collapsed;
        }

    }
}
