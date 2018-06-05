using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using VungleSDK;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace GetVIP.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class SplashPage : Page
    {
    
        public SplashPage()
        {
            this.InitializeComponent();
        }

        
        ApplicationDataContainer settings = ApplicationData.Current.LocalSettings;
        //获取本地应用设置容器
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, new Uri("http://s.moreplay.cn/index.php?c=app&a=puyuetian_htmlpage:index&htmlname=info_page"));
            httpRequestMessage.Headers.Append("User-Agent", "Mozilla/5.0 (Linux;Android 5.1; zh-cn;) AppleWebKit/537.36 (KHTML, like Gecko)Version/4.0 Chrome/37.0.0.0 Browser/7.6 Mobile Safari/537.36");
            ad.NavigateWithHttpRequestMessage(
            httpRequestMessage);          
        }

        int n = 0;     
        DispatcherTimer timer = new DispatcherTimer();
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            
            //判断settings容器里面有没有"First"这个键
            if (!settings.Values.ContainsKey("First"))
            { //应用首次启动，必定不会含"First"这个键，让应用导航到GuidePage这个页面，GuidePage这个页面就是对应用的介绍啦
                Frame.Navigate(typeof(GuidePage));
                //在settings容器里面写入"First"这个键值对，应用再次启动时，就不会在导航到介绍页面了。
                settings.Values["First"] = "yes";
            }
            else
            {              
                timer.Interval = new TimeSpan(0, 0, 1);
                timer.Tick += time_Tick;
                timer.Start();
                
            }
        }
        private void time_Tick(object sender, object e)
        {
            if (n == 3)
            {
                timer.Stop();
                Frame.Navigate(typeof(MainPage));
            }
           else{
                n += 1;
                int snow_num = 4 - n;
                time.Text = snow_num + "秒进入";
            }

        }

    }
}
