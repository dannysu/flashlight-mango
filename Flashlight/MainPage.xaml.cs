using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone;
using System.IO.IsolatedStorage;
using System.ComponentModel;
using Microsoft.Phone.Shell;
using Microsoft.Devices;

namespace Flashlight
{
    public partial class MainPage : PhoneApplicationPage
    {
        bool _isBlackBackground;
        PhotoCamera camera;

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            _isBlackBackground = true;
        }

        protected override void OnNavigatingFrom(System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            camera.AutoFocusCompleted -= new EventHandler<CameraOperationCompletedEventArgs>(camera_AutoFocusCompleted);
            camera.CancelFocus();
            camera.Dispose();
            camera = null;
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            camera = new Microsoft.Devices.PhotoCamera(CameraType.Primary);
            camera.Initialized += new EventHandler<CameraOperationCompletedEventArgs>(camera_Initialized);
            camera.AutoFocusCompleted += new EventHandler<CameraOperationCompletedEventArgs>(camera_AutoFocusCompleted);
            previewVideo.SetSource(camera);
            
            // Prevent screen lock
            PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
        }

        void camera_AutoFocusCompleted(object sender, CameraOperationCompletedEventArgs e)
        {
            ((PhotoCamera)sender).Focus();
        }

        void camera_Initialized(object sender, CameraOperationCompletedEventArgs e)
        {
            ((PhotoCamera)sender).FlashMode = FlashMode.On;
            ((PhotoCamera)sender).Focus();
        }

        private void LayoutRoot_Tap(object sender, GestureEventArgs e)
        {
            Grid grid = (Grid)sender;

            if (_isBlackBackground)
            {
                grid.Background = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
            }
            else
            {
                grid.Background = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
            }
            _isBlackBackground = !_isBlackBackground;
        }
    }
}