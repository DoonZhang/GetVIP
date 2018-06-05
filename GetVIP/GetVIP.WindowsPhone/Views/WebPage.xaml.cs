using JYAnalyticsUniversal;
using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;
using static GetVIP.Views.MainPage;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkID=390556 上有介绍

namespace GetVIP.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class WebPage : Page
    {
        public WebPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 在此页将要在 Frame 中显示时进行调用。
        /// </summary>
        /// <param name="e">描述如何访问此页的事件数据。
        /// 此参数通常用于配置页。</param>
        /// 

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            //注意，应用从挂起恢复时不会调用此方法
            //为了保证数据完整性，此方法可灵活放置在恢复页面的事件中（如页面onload事件），请确保和TrackPageEnd成对使用并避免重复调用
            JYAnalytics.TrackPageStart("显示页");

            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, new Uri("http://s.moreplay.cn/index.php?c=app&a=puyuetian_htmlpage:index&htmlname=info_page"));
            httpRequestMessage.Headers.Append("User-Agent", "Mozilla/5.0 (Linux; U; Android 6.0; zh-cn;) AppleWebKit/537.36 (KHTML, like Gecko)Version/4.0 Chrome/37.0.0.0 Browser/7.6 Mobile Safari/537.36");
       //     adwv.NavigateWithHttpRequestMessage(
       //     httpRequestMessage);
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            //注意：以下几种情形不会调用此方法：
            //1.首页按“后退”键
            //2.应用挂起时
            //为了保证数据完整性，此方法可灵活放置在跳转页面（离开页面）或离开应用的事件中，请确保和TrackPageStart成对使用并避免重复调用
            base.OnNavigatedFrom(e);
            JYAnalytics.TrackPageEnd("显示页");
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Menu.snow = true;

            switch (Menu.name)
            {
                //留言板
                case "Comments": WebSite.Navigate(new Uri("ms-appx-web:///Assets/HTML/getvip/menu/comment/index.html")); break;
                //爱奇艺
                case "Iqiyi": WebSite.Navigate(new Uri("ms-appx-web:///Assets/HTML/getvip/menu/aiqiyi/index.html")); break;
                //迅雷
                case "Thunder": WebSite.Navigate(new Uri("ms-appx-web:///Assets/HTML/getvip/menu/thunder/index.html")); break;
                //优酷
                case "YouKu": WebSite.Navigate(new Uri("ms-appx-web:///Assets/HTML/getvip/menu/youku/index.html")); break;
                //腾讯视频
                case "QQLive": WebSite.Navigate(new Uri("ms-appx-web:///Assets/HTML/getvip/menu/qqlive/index.html")); break;
                //响巢看看
                case "KanKan": WebSite.Navigate(new Uri("ms-appx-web:///Assets/HTML/getvip/menu/kankan/index.html")); break;
                //虾米音乐
                case "XiaMi": WebSite.Navigate(new Uri("ms-appx-web:///Assets/HTML/getvip/menu/xiami/index.html")); break;
                //暴风影音
                case "BaoFeng": WebSite.Navigate(new Uri("ms-appx-web:///Assets/HTML/getvip/menu/baofeng/index.html")); break;
                //乐视视频
                case "LeShi": WebSite.Navigate(new Uri("ms-appx-web:///Assets/HTML/getvip/menu/leshi/index.html")); break;
                //搜狐视频
                case "SouHu": WebSite.Navigate(new Uri("ms-appx-web:///Assets/HTML/getvip/menu/souhu/index.html")); break;
                //QQ音乐
                case "QQMusic": WebSite.Navigate(new Uri("ms-appx-web:///Assets/HTML/getvip/menu/qqmusic/index.html")); break;
                //网易云音乐
                case "WangYiYun": WebSite.Navigate(new Uri("ms-appx-web:///Assets/HTML/getvip/menu/wangyi/index.html")); break;
                //酷狗音乐
                case "KuGou": WebSite.Navigate(new Uri("ms-appx-web:///Assets/HTML/getvip/menu/kugou/index.html")); break;
                //酷我音乐
                case "KuWo": WebSite.Navigate(new Uri("ms-appx-web:///Assets/HTML/getvip/menu/kuwo/index.html")); break;
                //百度音乐
                case "BaiduMusic": WebSite.Navigate(new Uri("ms-appx-web:///Assets/HTML/getvip/menu/baidumusic/index.html")); break;
                //QQ旋风
                case "QQXuanFeng": WebSite.Navigate(new Uri("ms-appx-web:///Assets/HTML/getvip/menu/xuanfeng/index.html")); break;
                //百度云
                case "BaiduYun": WebSite.Navigate(new Uri("ms-appx-web:///Assets/HTML/getvip/menu/baiduyun/index.html")); break;
                //PPTV
                case "PPTV": WebSite.Navigate(new Uri("ms-appx-web:///Assets/HTML/getvip/menu/pptv/index.html")); break;
                //芒果TV
                case "MangGuo": WebSite.Navigate(new Uri("ms-appx-web:///Assets/HTML/getvip/menu/mangguo/index.html")); break;
                //百度文库
                case "BaiduWenku": WebSite.Navigate(new Uri("ms-appx-web:///Assets/HTML/getvip/menu/wenku/index.html")); break;

            }
        }     

    }
}
