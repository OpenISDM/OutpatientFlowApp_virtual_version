﻿using System;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using IndoorNavigation.Views.Navigation;
using Xamarin.Essentials;
using System.Resources;
using IndoorNavigation.Resources.Helpers;
using System.Reflection;
using System.Globalization;
using Plugin.Multilingual;

namespace IndoorNavigation
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectTwoWayPopupPage : PopupPage
    {
        String _locationName;
        bool isButtonPressed = false;        
        Page page = Application.Current.MainPage;

        private const string _resourceId = "IndoorNavigation.Resources.AppResources";
        ResourceManager _resourceManager =
            new ResourceManager(_resourceId, typeof(TranslateExtension).GetTypeInfo().Assembly);
        CultureInfo currentLanguage = CrossMultilingual.Current.CurrentCultureInfo;
        public SelectTwoWayPopupPage(string BuildingName)
        {
            InitializeComponent();
            BackgroundColor = Color.FromRgba(150, 150, 150, 70);
            _locationName = BuildingName;
        }

        async private void ToNavigationBtn_Clicked(object sender, EventArgs e)
        {
            if (isButtonPressed) return;
            isButtonPressed = true;  
            
            await PopupNavigation.Instance.PopAllAsync();
            await page.Navigation.PushAsync(new NavigationHomePage(_locationName));
        }

        async private void ToOPFM_Clicked(object sender, EventArgs e)
        {
            if (isButtonPressed) return;
            isButtonPressed = true;

            await PopupNavigation.Instance.PopAsync();

            if (!Preferences.Get("NotShowAgain_ToOPFM", false))
            {
                await PopupNavigation.Instance.PushAsync(new ShiftAlertPopupPage(
                    _resourceManager.GetString("ALERT_IF_YOU_HAVE_NETWORK_STRING",currentLanguage), 
                    _resourceManager.GetString("YES_STRING",currentLanguage)
                    , _resourceManager.GetString("NO_STRING",currentLanguage), "NotShowAgain_ToOPFM"));
                MessagingCenter.Subscribe<ShiftAlertPopupPage, bool>(this, "NotShowAgain_ToOPFM",async (msgsender, msgargs) =>
                  {                     
                      if ((bool)msgargs) await page.Navigation.PushAsync(new RigisterList(_locationName));
                      else await page.Navigation.PushAsync(new NavigationHomePage(_locationName));

                      MessagingCenter.Unsubscribe<ShiftAlertPopupPage, bool>(this, "NotShowAgain_ToOPFM");
                  });              
            }
            else
            {                
                await page.Navigation.PushAsync(new RigisterList(_locationName));
            }
        }

        protected override bool OnBackButtonPressed()
        {
            return base.OnBackButtonPressed();
        }
        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAsync();
        }
    }
}