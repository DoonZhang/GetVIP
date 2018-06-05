using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
using JYAnalyticsUniversal;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkID=390556 上有介绍

namespace GetVIP.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Setting : Page
    {
        public Setting()
        {
            this.InitializeComponent();
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
            JYAnalytics.TrackPageStart("设置");
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            //注意：以下几种情形不会调用此方法：
            //1.首页按“后退”键
            //2.应用挂起时
            //为了保证数据完整性，此方法可灵活放置在跳转页面（离开页面）或离开应用的事件中，请确保和TrackPageStart成对使用并避免重复调用
            base.OnNavigatedFrom(e);
            JYAnalytics.TrackPageEnd("设置");
        }

        private void Help_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(HelpPage));
        }

        private void FeedBack_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(FeedBack));
        }

        

        private void About_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(AboutPage));
        }

        private async void Privacy_Tapped(object sender, TappedRoutedEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri("https://appstudio.windows.com/home/appprivacyterms"));
        }

        private async void Assess_Tapped(object sender, TappedRoutedEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri("zune:reviewapp?appid=appd4528b23-a539-4710-b4c3-a1e4bbd44141"));
        }
      

        private async void More_Tapped(object sender, TappedRoutedEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri("zune:search?publisher=MEP Studio"));
        }

        private  void Push_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(PushSetPage));
           
        }
    }
}
