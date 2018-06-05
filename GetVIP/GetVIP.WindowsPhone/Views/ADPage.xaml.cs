using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using JYAnalyticsUniversal;
using static GetVIP.Views.MainPage;
using VungleSDK;
using Windows.UI.Xaml;
using System;
using Windows.UI.Core;
using System.Runtime.InteropServices;
using Windows.Web.Http;
using Windows.ApplicationModel.Core;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkID=390556 上有介绍

namespace GetVIP.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class ADPage : Page
    {
        // Vungle广告
        VungleAd sdkInstance;
        bool IsCompletedView, adPlayable, CallToActionClicked;
        string appID = "591915adc427736422000c16";
        private string interst = "INTERST96578";
        public ADPage()
        {
            this.InitializeComponent();

            sdkInstance = AdFactory.GetInstance(appID, interst);
            //为广告位置加载广告
             sdkInstance.LoadAd(interst);

            //广告状态改变后事件
            sdkInstance.OnAdPlayableChanged += SdkInstance_OnAdPlayableChanged;

            sdkInstance.OnAdEnd += SdkInstance_OnAdEnd;

            sdkInstance.OnInitCompleted += SdkInstance_OnInitCompleted;
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
            JYAnalytics.TrackPageStart("AD_Page");

            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, new Uri("http://s.moreplay.cn/index.php?c=app&a=puyuetian_htmlpage:index&htmlname=info_page"));
            httpRequestMessage.Headers.Append("User-Agent", "Mozilla/5.0 (Linux;Android 6.0;zh-cn) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/52.0.2743.116 Safari/537.36 Edge/15.15063");
            //   ad.NavigateWithHttpRequestMessage(
            //   httpRequestMessage);
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            //注意：以下几种情形不会调用此方法：
            //1.首页按“后退”键
            //2.应用挂起时
            //为了保证数据完整性，此方法可灵活放置在跳转页面（离开页面）或离开应用的事件中，请确保和TrackPageStart成对使用并避免重复调用
            base.OnNavigatedFrom(e);
            JYAnalytics.TrackPageEnd("AD_Page");
        }

        //OnAdPlayableChanged 事件的事件处理程序
        private void SdkInstance_OnAdPlayableChanged(object sender, AdPlayableEventArgs e)
        {
            //广告是否可用  
            adPlayable = e.AdPlayable;
        }
        int click_times = 0;
        private async void PlayAD_Click(object sender, RoutedEventArgs e)
        {
            click_times += 1;
            if (click_times >= 10)
            {
                Frame.Navigate(typeof(WebPage));
            }
            await sdkInstance.PlayAdAsync(new AdConfig
            {
                SoundEnabled = false
            }, interst);
        }

        private void OnLevelStart(Object sender, RoutedEventArgs e)
        {
            //     sdkInstance.LoadAd(interst);
        }

        // OnAdEnd
        //   e.Id - 字符串中的 Vungle 应用 ID
        //   e.Placement - 字符串中的广告位置 ID
        //   e.IsCompletedView- 观看视频内容 80% 或更多时为 true
        //   e.CallToActionClicked - 用户点击了结束卡上的下载按钮时为 true
        //   e.WatchedDuration - 已弃用
        private void SdkInstance_OnAdEnd(object sender, AdEndEventArgs e)
        {
            IsCompletedView = e.IsCompletedView;

            CallToActionClicked = e.CallToActionClicked;
        } 

        // OnInitCompleted
        //   e.Initialized - 如果初始化成功为 true，失败为 false
        //   e.ErrorMessage - 当 e.Initialized 为 false 时的失败原因
        private void SdkInstance_OnInitCompleted(object sender, ConfigEventArgs e)
        {
            var placementsInfo = "OnInitCompleted: " + e.Initialized;
            //初始化成功
            if (e.Initialized == true)
            {
                //打印广告位置列表
                for (var i = 0; i < e.Placements.Length; i++)
                {
                    placementsInfo += "\n\tPlacement" + (i + 1) + ": " + e.Placements[i].ReferenceId;
                    if (e.Placements[i].IsAutoCached == true)
                        placementsInfo += " (Auto-Cached)";
                }
            }
            //初始化失败
            else
            {
                placementsInfo += "\n\t" + e.ErrorMessage;
            }
        }
        int n = 0;
        DispatcherTimer timer = new DispatcherTimer();
        int Switch_times = 0;
        private void Switch_Click(object sender, RoutedEventArgs e)
        {
            if (Switch_times == 0)
            {
                zfb_hb.Visibility = Visibility.Collapsed;
                zfb_fk.Visibility = Visibility.Visible;
                wx_zsm.Visibility = Visibility.Collapsed;
                Switch_times += 1;
            }
            else if (Switch_times == 1)
            {
                zfb_hb.Visibility = Visibility.Collapsed;
                zfb_fk.Visibility = Visibility.Collapsed;
                wx_zsm.Visibility = Visibility.Visible;
                Switch_times += 1;
            }
            else if (Switch_times == 2)
            {
                zfb_hb.Visibility = Visibility.Visible;
                zfb_fk.Visibility = Visibility.Collapsed;
                wx_zsm.Visibility = Visibility.Collapsed;
                Switch_times = 0;
            }
        }
      
        private void Snow_Pics_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            Pics.Visibility = Visibility.Visible;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += time_Tick;
            timer.Start();

            if (IsCompletedView == true || CallToActionClicked == true)
            {
                Frame.Navigate(typeof(WebPage));
            }
        }

        private void time_Tick(object sender, object e)
        {
            if (n == 5)
            {
                timer.Stop();
                time.Text = "请点击“观看视频”";
                Playbtn.Visibility = Visibility.Visible;
            }
            else
            {
                n += 1;
                int snow_num = 5 - n;
                time.Text = snow_num + "秒后观看广告，加载广告中...";
            }

        }
    }
}
