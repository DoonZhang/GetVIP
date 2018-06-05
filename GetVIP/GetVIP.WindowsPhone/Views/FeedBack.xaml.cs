using JyUserFeedback;
using JyUserFeedback.view;
using JyUserInfo;
using JyUserInfo.Model;
using JYAnalyticsUniversal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.ApplicationModel.Activation;
using Windows.Storage.Pickers;
using Windows.UI.Popups;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Text;



// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkID=390556 上有介绍

namespace GetVIP.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class FeedBack : IContinueFileOpen
    {
        private readonly JyUserFeedbackSDKManager _jyUserFeedbackSdkManager = new JyUserFeedbackSDKManager();

        private UserInfo _userInfo;

        public FeedBack()
        {
            this.InitializeComponent();
            //九幽的第三方登陆
            NavigationCacheMode = NavigationCacheMode.Required;

            JyUserInfoManager.LogincompletedEvent += JyUserInfoManager_LoginCompleted;

            JyFeedbackControl.FeedbackImageRequested += delegate
            {
                var fileOpenPicker = new FileOpenPicker();
                fileOpenPicker.FileTypeFilter.Add(".png");
                fileOpenPicker.FileTypeFilter.Add(".jpg");

                fileOpenPicker.PickSingleFileAndContinue();
            };
        }


        public void ContinueFileOpen(FileOpenPickerContinuationEventArgs args)
        {
            var file = args.Files.ElementAtOrDefault(0);
            if (file != null)
            {
                _jyUserFeedbackSdkManager.UploadPicture(Constants.Appkey, Constants.SecretId, file);
            }
        }

        /// <summary>
        /// 在此页将要在 Frame 中显示时进行调用。
        /// </summary>
        /// <param name="e">描述如何访问此页的事件数据。
        /// 此参数通常用于配置页。</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            //注意，应用从挂起恢复时不会调用此方法
            //为了保证数据完整性，此方法可灵活放置在恢复页面的事件中（如页面onload事件），请确保和TrackPageEnd成对使用并避免重复调用
            JYAnalytics.TrackPageStart("FeedBack_Page");
        }
        protected override void OnNavigatedFrom(Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            //注意：以下几种情形不会调用此方法：
            //1.首页按“后退”键
            //2.应用挂起时
            //为了保证数据完整性，此方法可灵活放置在跳转页面（离开页面）或离开应用的事件中，请确保和TrackPageStart成对使用并避免重复调用
            base.OnNavigatedFrom(e);
            JYAnalytics.TrackPageEnd("FeedBack_Page");
        }


        private void JyUserInfoManager_LoginCompleted(object sender, UserInfo userInfo)
        {
            if (userInfo.isLoginSuccess)
            {
                _userInfo = userInfo;
            }
            else
            {
                // 不需要在 else 逻辑中弹出对话框显示登录失败，登录失败时，登录控件会有弹窗提示。
            }
        }
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            JyUserInfoManager.ShowLogin(RootGrid, Constants.Appkey, LoginTemplate.Template1);
        }

        private async void FastLoginButton_Click(object sender, RoutedEventArgs e)
        {
            var userInfo = await JyUserInfoManager.QuickLogin(Constants.Appkey);
            if (userInfo.isLoginSuccess)
            {
                _userInfo = userInfo;
                await new MessageDialog(Constants.FastLoginSuccessMessage).ShowAsync();
            }
            else
            {
                await new MessageDialog(Constants.FastLoginFailedMessage).ShowAsync();
            }
        }

        private async void OpenFeedbackWindowButton_Click(object sender, RoutedEventArgs e)
        {
            if (_userInfo == null)
            {
                await new MessageDialog(Constants.PleaseLoginFirstMessage).ShowAsync();
            }
            else
            {
                _jyUserFeedbackSdkManager.ShowFeedBack(RootGrid, false, Constants.Appkey, _userInfo.U_Key);
            }
        }

        private async void GetDeveloperReplyCountButton_Click(object sender, RoutedEventArgs e)
        {
            if (_userInfo == null)
            {
                await new MessageDialog(Constants.PleaseLoginFirstMessage).ShowAsync();
            }
            else
            {
                var newFeedBackRemindCount = await _jyUserFeedbackSdkManager.GetNewFeedBackRemindCount(Constants.Appkey, _userInfo.U_Key);
                await new MessageDialog(string.Format(Constants.NewFeedbackReplyCountMessage, newFeedBackRemindCount)).ShowAsync();
            }
        }
        private void Return_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        
        


            

     
    }
}
