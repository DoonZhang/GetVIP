using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Data.Xml.Dom;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Text;
using JYAnalyticsUniversal;


// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkID=390556 上有介绍

namespace GetVIP.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class PushSetPage : Page
    {

        public PushSetPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;


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
            JYAnalytics.TrackPageStart("PushSet_Page");
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            //注意：以下几种情形不会调用此方法：
            //1.首页按“后退”键
            //2.应用挂起时
            //为了保证数据完整性，此方法可灵活放置在跳转页面（离开页面）或离开应用的事件中，请确保和TrackPageStart成对使用并避免重复调用
            base.OnNavigatedFrom(e);
            JYAnalytics.TrackPageStart("PushSet_Page");
        }

        string on_off = string.Empty;
        private void ToastOpen_Checked(object sender, RoutedEventArgs e)
        {
            on_off = "开";
            
        }
       
        private async void Ture_Click(object sender, RoutedEventArgs e)
        {
            //隐藏确认按钮，避免多次点击
            Ture_Button.Visibility = Visibility.Collapsed;

            int Hours = 0 ;

            //点击设置推送按钮的统计
            JYAnalytics.TrackEvent("Push");

            if (on_off == "开")
            {
                try
                {                  
                       for (int i = 1; i <= 7 ; i++)

                    {                       
                        Hours = Hours + 24;                       
                        XmlDocument xdoc = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText01);
                        var txtnodes = xdoc.GetElementsByTagName("text");
                        txtnodes[0].InnerText = "快来看看账号有没有更新吧！";
                        ScheduledToastNotification toast3 = new ScheduledToastNotification(xdoc, DateTimeOffset.Now.AddHours(Hours));
                        ToastNotificationManager.CreateToastNotifier().AddToSchedule(toast3);
                        
                    }
                }
                catch (Exception ex)
                {
                    Windows.UI.Popups.MessageDialog messageDialog = new Windows.UI.Popups.MessageDialog("设置成功！"); 
                    await messageDialog.ShowAsync();
                }
            }
        }
    }
}
