using JYAnalyticsUniversal;
using JyUserFeedback;
using JyUserFeedback.view;
using JyUserInfo;
using JyUserInfo.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Data.Xml.Dom;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.UI.Input;
using Windows.Storage.Pickers;
using Windows.UI.Notifications;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;
using Windows.Web.Http;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace GetVIP.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        //定义广告声明
       // AdControl ADControl;
            
        //声明升级提醒控件
        JYUpdateSDK.View.ShowUpdateInfoControl showUpdateInofControl;

        private readonly JyUserFeedbackSDKManager _jyUserFeedbackSdkManager = new JyUserFeedbackSDKManager();

        private UserInfo _userInfo;

        public MainPage()
        {
            //在需要检查更新的页面的构造函数里实例化更新控件
            showUpdateInofControl = new JYUpdateSDK.View.ShowUpdateInfoControl();

           
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

        //修改WebView默认User-Agent代理
      
        private void JyUserInfoManager_LoginCompleted(object sender, UserInfo userInfo)
        {
            if (userInfo.isLoginSuccess)
            {
                _userInfo = userInfo;
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
            JYAnalytics.TrackPageStart("Main_Page");

            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, new Uri("http://s.moreplay.cn"));
            httpRequestMessage.Headers.Append("User-Agent", "Mozilla/5.0 (Linux; U; Android 6.0; zh-cn;) AppleWebKit/537.36 (KHTML, like Gecko)Version/4.0 Chrome/37.0.0.0 Browser/7.6 Mobile Safari/537.36");
            ad.NavigateWithHttpRequestMessage(
            httpRequestMessage);
            
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            //注意：以下几种情形不会调用此方法：
            //1.首页按“后退”键
            //2.应用挂起时
            //为了保证数据完整性，此方法可灵活放置在跳转页面（离开页面）或离开应用的事件中，请确保和TrackPageStart成对使用并避免重复调用
            base.OnNavigatedFrom(e);
            JYAnalytics.TrackPageEnd("Main_Page");
        }
        private void Setting_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Setting));
        }

        public async void CheckTips()
        {
            try {

                string Uri = "http://s.moreplay.cn/Apps/getvip/Control.json";
                Uri uri = new Uri(Uri);
                HttpClient client = new HttpClient();
                string ControlJson = await client.GetStringAsync(uri);
                JObject JSON = JObject.Parse(ControlJson);
                string Tip = (string)JSON["Tips"];

                switch (Tip)
                  {
                    case "0":
                        Tips.Visibility = Visibility.Collapsed; break;
                    case "1":
                        Tips.Visibility = Visibility.Visible;
                        TipNum.Text = "+1"; break;
                  }

               }
            catch
              {
                Tips.Visibility = Visibility.Collapsed;
            }
        }
        private Ellipse Tips;
        private TextBlock TipNum,Info,Num;
        private StackPanel InfoControl;
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {         
                     
            //在页面的加载事件中添加控件
            if (!this.Root_Grid.Children.Contains(showUpdateInofControl))
            {
                //无需做其他处理，当加载此页面时如有更新版本信息将自动弹出更新消息框，如果没有更新内容则不会展示
                //在这个页面的根控件（grid_root:需自定义控件名称，参考本页面 xaml中的控件起名）添加showUpdateInofControl
                //样式和模板及内容可在九幽后台管理
                this.Root_Grid.Children.Add(showUpdateInofControl);
            }

            if (Menu.snow == true)
            {               
                Menu.snow = false;
            }

        }    
        public static class Menu
        {
            //开屏开关
            public static int n = 0;
            //定义视频名称选项
            public static string name;
            //获取广告返回主页面显示广告
            public static bool snow = false;

        }      

        //视频类
        private void Iqiyi_Click(object sender, RoutedEventArgs e)
        {
            Menu.name = "Iqiyi";
            Frame.Navigate(typeof(ADPage));
            //点击爱奇艺按钮的统计
            JYAnalytics.TrackEvent("爱奇艺");
        }

        private void YouKu_Click(object sender, RoutedEventArgs e)
        {
            Menu.name = "YouKu";
            Frame.Navigate(typeof(ADPage));
            //点击优酷按钮的统计
            JYAnalytics.TrackEvent("优酷");

        }
        private void QQLive_Click(object sender, RoutedEventArgs e)
        {

            Menu.name = "QQLive";
            Frame.Navigate(typeof(ADPage));
            //点击腾讯视频按钮的统计
            JYAnalytics.TrackEvent("腾讯视频");

        }
        private void KanKan_Click(object sender, RoutedEventArgs e)
        {
            Menu.name = "KanKan";
            Frame.Navigate(typeof(ADPage));
            //点击响巢看看按钮的统计
            JYAnalytics.TrackEvent("响巢看看");

        }
        private void BaoFeng_Click(object sender, RoutedEventArgs e)
        {
            Menu.name = "BaoFeng";
            Frame.Navigate(typeof(ADPage));
            //点击暴风影音按钮的统计
            JYAnalytics.TrackEvent("暴风影音");
        }
        private void LeShi_Click(object sender, RoutedEventArgs e)
        {
            Menu.name = "LeShi";

            Frame.Navigate(typeof(ADPage));
            //点击乐视视频按钮的统计
            JYAnalytics.TrackEvent("乐视");
        }

        private void SouHu_Click(object sender, RoutedEventArgs e)
        {
            Menu.name = "SouHu";
            Frame.Navigate(typeof(ADPage));
            JYAnalytics.TrackEvent("搜狐视频");
        }
        private void PPTV_Click(object sender, RoutedEventArgs e)
        {
            Menu.name = "PPTV";
            Frame.Navigate(typeof(ADPage));
            //点击PPTV聚力按钮的统计
            JYAnalytics.TrackEvent("PPTV聚力");
        }

        private void MangGuo_Click(object sender, RoutedEventArgs e)
        {
            Menu.name = "MangGuo";
            Frame.Navigate(typeof(ADPage));
            //点击芒果TV按钮的统计
            JYAnalytics.TrackEvent("芒果TV");
        }
        //音乐类
        private void XiaMi_Click(object sender, RoutedEventArgs e)
        {
            Menu.name = "XiaMi";
            Frame.Navigate(typeof(ADPage));
            //点击虾米音乐按钮的统计
            JYAnalytics.TrackEvent("虾米音乐");

        }
        private void QQMusic_Click(object sender, RoutedEventArgs e)
        {
            Menu.name = "QQMusic";
            Frame.Navigate(typeof(ADPage));
            //点击QQ音乐按钮的统计
            JYAnalytics.TrackEvent("QQ音乐");
        }

        private void WangYiYun_Click(object sender, RoutedEventArgs e)
        {
            Menu.name = "WangYiYun";
            Frame.Navigate(typeof(ADPage));
            //点击网易云音乐按钮的统计
            JYAnalytics.TrackEvent("网易云音乐");
        }

        private void KuGou_Click(object sender, RoutedEventArgs e)
        {
            Menu.name = "KuGou";
            Frame.Navigate(typeof(ADPage));
            //点击酷狗音乐按钮的统计
            JYAnalytics.TrackEvent("酷狗音乐");
        }

        private void KuWo_Click(object sender, RoutedEventArgs e)
        {
            Menu.name = "KuWo";
            Frame.Navigate(typeof(ADPage));
            //点击酷我按钮的统计
            JYAnalytics.TrackEvent("酷我音乐");
        }

        private void BaiduMusic_Click(object sender, RoutedEventArgs e)
        {
            Menu.name = "BaiduMusic";
            Frame.Navigate(typeof(ADPage));
            //点击百度音乐按钮的统计
            JYAnalytics.TrackEvent("百度音乐");
        }

        //下载类
        private void Thunder_Click(object sender, RoutedEventArgs e)
        {

            Menu.name = "Thunder";
            Frame.Navigate(typeof(ADPage));
            //点击迅雷按钮的统计
            JYAnalytics.TrackEvent("迅雷");

        }

        private void QQXuanFeng_Click(object sender, RoutedEventArgs e)
        {
            Menu.name = "QQXuanFeng";
            Frame.Navigate(typeof(ADPage));
            //点击QQ旋风按钮的统计
            JYAnalytics.TrackEvent("QQ旋风");
        }

        //其他类
        private void BaiduYun_Click(object sender, RoutedEventArgs e)
        {
            Menu.name = "BaiduYun";
            Frame.Navigate(typeof(ADPage));
            //点击百度云按钮的统计
            JYAnalytics.TrackEvent("百度云");
        }

        private void BaiduWenku_Click(object sender, RoutedEventArgs e)
        {
            Menu.name = "BaiduWenku";
            Frame.Navigate(typeof(ADPage));
            //点击百度云按钮的统计
            JYAnalytics.TrackEvent("百度文库");
        }

        private void EveryPiano_Click(object sender, RoutedEventArgs e)
        {
            Menu.name = "EveryPiano";
            Frame.Navigate(typeof(ADPage));
            //点击人人钢琴按钮的统计
            JYAnalytics.TrackEvent("人人钢琴");

        }
        //功能

        private void Notice_Click(object sender, RoutedEventArgs e)
        {
            JYUpdateSDK.JYBulletinManager.ShowBulletinInfo(this.Frame);
            //查看公告后就删除红点标记
           
        }

        private void Comments_Click(object sender, RoutedEventArgs e)
        {
            Menu.name = "Comments";
            Frame.Navigate(typeof(WebPage));
            //点击留言板按钮的统计
            JYAnalytics.TrackEvent("Comments");
        }


        private async void FeedBack_Click(object sender, RoutedEventArgs e)
        {
            //直接快速登陆并打开反馈窗口
            var userInfo = await JyUserInfoManager.QuickLogin(Constants.Appkey);
            if (userInfo.isLoginSuccess)
            {
                _userInfo = userInfo;
                _jyUserFeedbackSdkManager.ShowFeedBack(Root_Grid, false, Constants.Appkey, _userInfo.U_Key);
            }
        }
        private async void LeShare_Click(object sender, RoutedEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri("https://www.microsoft.com/store/apps/9pnvkq808v57"));
        }
        private async void MorePlayVideo_Click(object sender, RoutedEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri("https://www.microsoft.com/store/apps/9nblggh42kp6"));
        }

        private async void Hexagon_Click(object sender, RoutedEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri("https://www.microsoft.com/store/apps/9nblggh52rkb"));
        }

        private async void ClearSquare_Click(object sender, RoutedEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri("https://www.microsoft.com/store/apps/9nq452490z8d"));
        }
       

        private async void Assess_Click(object sender, RoutedEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri("https://www.microsoft.com/store/apps/9nblggh557hk"));
        }

        private void TipNum_Loaded(object sender, RoutedEventArgs e)
        {
            TipNum = (TextBlock)sender;
            //判断是否有公告以显示红点标记        
        }

        private void Tips_Loaded(object sender, RoutedEventArgs e)
        {
            Tips = (Ellipse)sender;
        
        }

        private void Image_Tapped(object sender, TappedRoutedEventArgs e)
        {
            InfoControl.Visibility = Visibility.Collapsed;            
        }

        private async void Info_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Info = (TextBlock)sender;
                string Uri = "http://s.moreplay.cn/Apps/getvip/Control.json";
                Uri uri = new Uri(Uri);
                HttpClient client = new HttpClient();
                string ControlJson = await client.GetStringAsync(uri);
                JObject JSON = JObject.Parse(ControlJson);
                string info = (string)JSON["Info"];
                Info.Text = info;
            }
            catch
            {
                TipNum.Text = "暂无公告！";
            }

        }

        private async void Shift_Click(object sender, RoutedEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri("https://www.microsoft.com/store/apps/9pc1r6m0ssq3"));
        }
      
        private void Num_Loaded(object sender, RoutedEventArgs e)
        {
            Num = (TextBlock)sender;
            //判断是否有开发者回复以显示数量
        }

        private void InfoControl_Loaded(object sender, RoutedEventArgs e)
        {
            InfoControl = (StackPanel)sender;
        }   
       
    }
}
