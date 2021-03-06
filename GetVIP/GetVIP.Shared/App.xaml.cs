﻿using JYAnalyticsUniversal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using GetVIP.Views;
using VungleSDK;


#if WINDOWS_APP

using System.Threading.Tasks;
using Windows.UI.ApplicationSettings;
#endif
#if WINDOWS_PHONE_APP
using Windows.Phone.UI.Input;
#endif
// “空白应用程序”模板在 http://go.microsoft.com/fwlink/?LinkId=234227 上有介绍

namespace GetVIP
{
    /// <summary>
    /// 提供特定于应用程序的行为，以补充默认的应用程序类。
    /// </summary>
    public sealed partial class App : Application
    {    
      
#if WINDOWS_PHONE_APP
        private TransitionCollection transitions;
#endif

        /// <summary>
        /// 初始化单一实例应用程序对象。这是执行的创作代码的第一行，
        /// 已执行，逻辑上等同于 main() 或 WinMain()。
        /// </summary>
        public App()
        {
            this.InitializeComponent();
          
            this.Suspending += this.OnSuspending;
            //注册Resuming事件，九幽数据统计插件需要在应用生命周期中获取相应信息以用于统计分析
            this.Resuming += OnResuming;
#if WINDOWS_PHONE_APP
            //对windows phone8.1注册HardwareButtons.BackPressed，九幽数据统计插件将调用
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
#endif
        }
#if WINDOWS_PHONE_APP
        private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame != null && rootFrame.CanGoBack)
            {
                rootFrame.GoBack();
                e.Handled = true;
            }
        }
#endif    
        /// <summary>
        /// 在应用程序由最终用户正常启动时进行调用。
        /// 当启动应用程序以打开特定的文件或显示时使用
        /// 搜索结果等
        /// </summary>
        /// <param name="e">有关启动请求和过程的详细信息。</param>
        protected async override void OnLaunched(LaunchActivatedEventArgs e)
        {
           
            //初始化九幽数据统计插件，appkey(密钥)请登陆九幽后台获取:http://www.windows.sc,可以替换成你的appkey在demo中测试
            //为保证数据的完整和准确性，请尽量在OnLaunched中优先调用此方法
            await JYAnalytics.StartTrackAsync("6d97ca1a3adb2853dad982d20d37dc95");
            //初始化更新和公告插件，appkey请登陆九幽开发者后台获取http://www.windows.sc
            await JYUpdateSDK.JYUpdateManager.UpdateInitialize("6d97ca1a3adb2853dad982d20d37dc95", false);
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif

            Frame rootFrame = Window.Current.Content as Frame;

            // 不要在窗口已包含内容时重复应用程序初始化，
            // 只需确保窗口处于活动状态
            if (rootFrame == null)
            {
                // 创建要充当导航上下文的框架，并导航到第一页
                rootFrame = new Frame();

                // TODO: 将此值更改为适合您的应用程序的缓存大小
                rootFrame.CacheSize = 1;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // TODO: 从之前挂起的应用程序加载状态
                }

                // 将框架放在当前窗口中
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
#if WINDOWS_PHONE_APP

                // 删除用于启动的旋转门导航。
                if (rootFrame.ContentTransitions != null)
                {
                    this.transitions = new TransitionCollection();
                    foreach (var c in rootFrame.ContentTransitions)
                    {
                        this.transitions.Add(c);
                    }
                }

                rootFrame.ContentTransitions = null;
                rootFrame.Navigated += this.RootFrame_FirstNavigated;
     
#endif

                // 当导航堆栈尚未还原时，导航到第一页，
                // 并通过将所需信息作为导航参数传入来配置
                // 参数
                if (!rootFrame.Navigate(typeof(SplashPage), e.Arguments))
                {
                    throw new Exception("Failed to create initial page");
                }
            }

            // 确保当前窗口处于活动状态
            Window.Current.Activate();

         
        }

#if WINDOWS_PHONE_APP
        /// <summary>
        /// 启动应用程序后还原内容转换。
        /// </summary>
        /// <param name="sender">附加了处理程序的对象。</param>
        /// <param name="e">有关导航事件的详细信息。</param>
        private void RootFrame_FirstNavigated(object sender, NavigationEventArgs e)
        {
            var rootFrame = sender as Frame;
            rootFrame.ContentTransitions = this.transitions ?? new TransitionCollection() { new NavigationThemeTransition() };
            rootFrame.Navigated -= this.RootFrame_FirstNavigated;
        }
#endif

        /// <summary>
        /// 在将要挂起应用程序执行时调用。  在不知道应用程序
        /// 无需知道应用程序会被终止还是会恢复，
        /// 并让内存内容保持不变。
        /// </summary>
        /// <param name="sender">挂起的请求的源。</param>
        /// <param name="e">有关挂起请求的详细信息。</param>
        private async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();

            // TODO: 保存应用程序状态并停止任何后台活动
            await JYAnalytics.EndTrackAsync(); //需注意此处代码位置不可更改
            deferral.Complete();
        }

        async void OnResuming(object sender, object e)
        {

            await JYAnalytics.StartTrackAsync("6d97ca1a3adb2853dad982d20d37dc95");
        }
        protected async override void OnActivated(IActivatedEventArgs args)
        {
            base.OnActivated(args);

            await JYAnalytics.StartTrackAsync("6d97ca1a3adb2853dad982d20d37dc95");
        }

#if WINDOWS_APP
        protected override void OnWindowCreated(WindowCreatedEventArgs args)
        {
            SettingsPane.GetForCurrentView().CommandsRequested += OnCommandsRequested;
            base.OnWindowCreated(args);
        }

        private void OnCommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            args.Request.ApplicationCommands.Add(new SettingsCommand("Help", "帮助", (handler) => ShowHelpFlyout()));
          
       //     args.Request.ApplicationCommands.Add(new SettingsCommand("FeedBack", "反馈", (handler) => ShowFeedBackFlyout()));

       //     args.Request.ApplicationCommands.Add(new SettingsCommand("Assess", "好评", (handler) => ShowAssessFlyout()));

            args.Request.ApplicationCommands.Add(new SettingsCommand("Privacy", "隐私", (handler) => ShowPrivacyFlyout()));

            args.Request.ApplicationCommands.Add(new SettingsCommand("About", "关于", (handler) => ShowAboutFlyout()));

          //  args.Request.ApplicationCommands.Add(new SettingsCommand("More", "更多软件", (handler) => ShowAboutSettingFlyout()));
        }

        private void ShowHelpFlyout()
        {
            var flyout = new AppFlyouts.HelpFlyout();
            flyout.Show();
        }
        private void ShowAboutFlyout()
        {
            var flyout = new AppFlyouts.AboutFlyout();
            flyout.Show();
        }

        private void ShowPrivacyFlyout()
        {
            var flyout = new AppFlyouts.PrivacyFlyout();
            flyout.Show();
        }
#endif

    }
}