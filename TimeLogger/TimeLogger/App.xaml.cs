﻿using TimeLogger.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TimeLogger
{
    public partial class App : Application
    {
        //TODO: Replace with *.azurewebsites.net url after deploying backend to Azure
        //To debug on Android emulators run the web backend against .NET Core not IIS
        //If using other emulators besides stock Google images you may need to adjust the IP address

        // for Mobile Device
        //public static string AzureBackendUrl =
        //    DeviceInfo.Platform == DevicePlatform.Android ? "http://192.168.10.187:5000" : "http://localhost:5000";

        //For Emulator
        public static string AzureBackendUrl =
            DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2:5000" : "http://localhost:5000";

        public static bool UseMockDataStore = false;

        public App()
        {
            InitializeComponent();

            if (UseMockDataStore)
            {
                DependencyService.Register<MockDataStore>();
            }
            else
            {
                DependencyService.Register<AzureDataStore>();
            }

            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}