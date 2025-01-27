using System;
using System.Threading.Tasks;
using System.Windows;
using System.IO.Ports;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Windows.Input;

namespace ORACULO_PR
{
    public partial class MainWindow : Window
    {
        // Shared timer to manage periodic actions across the application.
        public static DispatcherTimer SharedTimer { get; private set; }

        // Interface for serial communication, allowing mock or real implementations.
        public ISerialCommunication SerialPort { get; private set; }

        public MainWindow()
        {
            InitializeComponent();

            // Initialize the shared timer with a 40-second interval.
            SharedTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(40) };
            SharedTimer.Tick += Timer_Tick;

            // Navigate to the loading page on application start.
            MainFrame.Navigate(new Loading());

            // Try to detect a connected serial port, default to simulation if not found.
            string portName = FindSerialPort();
            if (!string.IsNullOrEmpty(portName))
            {
                try
                {
                    SerialPort = new RealSerialPort(portName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao abrir a porta serial {portName}: {ex.Message}");
                    SerialPort = new MockSerialPort();
                }
            }
            else
            {
                MessageBox.Show("CH340 não encontrada. Continuando com o modo de simulação...");
                SerialPort = new MockSerialPort();
            }

            // Register an event to handle data received from the serial port.
            SerialPort.DataReceived += SerialPort_DataReceived;

            // Set the window to fullscreen mode without borders.
            WindowStyle = WindowStyle.None;
            WindowState = WindowState.Maximized;

            // Delay navigation to the start page to display the loading screen briefly.
            Task.Delay(TimeSpan.FromSeconds(3)).ContinueWith(task =>
            {
                Dispatcher.Invoke(() => NavigateWithoutLoading(new StartPage()));
            });

            // Allow keyboard input to simulate serial port events in mock mode.
            KeyDown += MainWindow_KeyDown;

            // Send an initial message through the serial port.
            SerialPort?.WriteLine("Start");
        }

        // Handles key presses to simulate serial port inputs when in mock mode.
        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (SerialPort is MockSerialPort mockSerialPort)
            {
                string simulatedInput = e.Key switch
                {
                    Key.D1 => "button1P",
                    Key.D2 => "button2P",
                    Key.D3 => "button3P",
                    Key.D4 => "button4P",
                    Key.D5 => "button5P",
                    Key.D6 => "button6P",
                    Key.S => "Start",
                    _ => null
                };

                if (!string.IsNullOrEmpty(simulatedInput))
                {
                    mockSerialPort.SimulateDataReceived(simulatedInput);
                }
            }
        }

        // Attempts to find the first available serial port.
        private string FindSerialPort()
        {
            string[] portNames = RealSerialPort.GetPortNames();
            return portNames.Length > 0 ? portNames[0] : null;
        }

        // Resets the application state if the timer ticks outside of the showroom page.
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (!(MainFrame.Content is Showroom))
            {
                SharedTimer.Stop();
                NavigateWithLoading(new StartPage());
                SerialPort?.WriteLine("Restart");
                SharedTimer.Start();
            }
        }

        // Handles data received from the serial port and navigates to corresponding pages.
        private void SerialPort_DataReceived(object sender, string data)
        {
            Dispatcher.Invoke(() =>
            {
                switch (data?.Trim())
                {
                    case "Start":
                    case "Restart":
                        NavigateWithLoading(new StartPage());
                        break;
                    case "button1P":
                        NavigateWithLoading(new Socials());
                        break;
                    case "button2P":
                        NavigateWithLoading(new Website());
                        break;
                    case "button3P":
                        NavigateWithLoading(new Robot());
                        break;
                    case "button4P":
                        NavigateWithLoading(new Timeline());
                        break;
                    case "button5P":
                        NavigateWithLoading(new Showroom());
                        break;
                    case "button6P":
                        NavigateWithLoading(new Products());
                        break;
                    case "btnAct":
                        NavigateWithLoading(new Loading());
                        break;
                }
            });
        }

        // Navigates to a specified page after showing a loading screen for 3 seconds.
        private async void NavigateWithLoading(UserControl userControl)
        {
            MainFrame.Navigate(new Loading());
            await Task.Delay(3000);
            MainFrame.Navigate(userControl);
            SharedTimer.Stop();
            SharedTimer.Start();
        }

        private async void NavigateWithoutLoading(UserControl userControl)
        {
            MainFrame.Navigate(userControl);
            SharedTimer.Stop();
            SharedTimer.Start();
        }

        // Ensures serial port resources are closed when the application is closed.
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            SerialPort?.Close();
        }
    }

    // Interface for serial communication abstraction to support mock and real implementations.
    public interface ISerialCommunication
    {
        event EventHandler<string> DataReceived;
        void WriteLine(string data);
        void Close();
    }

    // Mock serial port implementation for simulation purposes.
    public class MockSerialPort : ISerialCommunication
    {
        public event EventHandler<string> DataReceived;

        public void WriteLine(string data) => Console.WriteLine($"Mock Write: {data}");

        public void Close() { }

        // Simulates receiving data, triggering the DataReceived event.
        public void SimulateDataReceived(string data)
        {
            DataReceived?.Invoke(this, data);
        }
    }

    // Real serial port implementation using System.IO.Ports.
    public class RealSerialPort : ISerialCommunication
    {
        private readonly SerialPort _serialPort;

        public event EventHandler<string> DataReceived;

        public RealSerialPort(string portName)
        {
            _serialPort = new SerialPort(portName, 9600);
            _serialPort.DataReceived += (sender, e) =>
            {
                string data = _serialPort.ReadLine();
                DataReceived?.Invoke(this, data);
            };
            _serialPort.Open();
        }

        public void WriteLine(string data) => _serialPort.WriteLine(data);

        public void Close()
        {
            if (_serialPort.IsOpen)
            {
                _serialPort.Close();
            }
        }

        // Retrieves the list of available serial port names.
        public static string[] GetPortNames() => SerialPort.GetPortNames();
    }
}
