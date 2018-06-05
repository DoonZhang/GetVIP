using System;
using System.Collections.Generic;
using System.Text;
using Windows.ApplicationModel.Resources;

namespace GetVIP
{
    class Constants
    {
       
            /// <summary>
            /// Appkey 请在九幽开发者后台中获取。
            /// </summary>
            internal const string Appkey = @"6d97ca1a3adb2853dad982d20d37dc95";

            internal const string SecretId = @"5bb71c1ea98c64277a4ec81d60696e93";

            public static string FastLoginFailedMessage
            {
                get
                {
                    return ResourceLoader.GetForCurrentView().GetString("FastLoginFailedMessage");
                }
            }

            public static string FastLoginSuccessMessage
            {
                get
                {
                    return ResourceLoader.GetForCurrentView().GetString("FastLoginSuccessMessage");
                }
            }

            public static string LoginFailedMessage
            {
                get
                {
                    return ResourceLoader.GetForCurrentView().GetString("LoginFailedMessage");
                }
            }

            public static string NewFeedbackReplyCountMessage
            {
                get
                {
                    return ResourceLoader.GetForCurrentView().GetString("NewFeedbackReplyCountMessage");
                }
            }

            public static string PleaseLoginFirstMessage
            {
                get
                {
                    return ResourceLoader.GetForCurrentView().GetString("PleaseLoginFirstMessage");
                }
            }
        }
    

}
