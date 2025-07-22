using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace LibVLCSharp
{
    public class OnLoad
    {
#if (UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN)
        const string UnityPlugin = "VLCUnityPlugin";
#elif (UNITY_STANDALONE_LINUX || UNITY_EDITOR_LINUX)
        const string UnityPlugin = "libVLCUnityPlugin.so";
#elif (UNITY_ANDROID || UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX )
        const string UnityPlugin = "libVLCUnityPlugin";
#elif (UNITY_STANDALONE_LINUX || UNITY_EDITOR_LINUX)
        const string UnityPlugin = "libVLCUnityPlugin.so";
#elif UNITY_IOS
        const string UnityPlugin = "@rpath/VLCUnityPlugin.framework/VLCUnityPlugin";
#else
    // unsupported
#endif

        [DllImport(UnityPlugin, CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "libvlc_unity_set_color_space")]
        public static extern void SetColorSpace(UnityColorSpace colorSpace);

        [DllImport(UnityPlugin, CallingConvention = CallingConvention.Cdecl)]
        static extern IntPtr GetRenderEventFunc();

        public enum UnityColorSpace
        {
            Gamma = 0,
            Linear = 1,
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void OnBeforeSceneLoadRuntimeMethod()
        {
            //  Debug.Log("UnityEngine.QualitySettings.activeColorSpace: " + PlayerColorSpace);
            SetColorSpace(PlayerColorSpace);
#if UNITY_ANDROID || UNITY_IOS
            GL.IssuePluginEvent(GetRenderEventFunc(), 1);
#endif
        }

        static UnityColorSpace PlayerColorSpace =>
            QualitySettings.activeColorSpace == 0 ? UnityColorSpace.Gamma : UnityColorSpace.Linear;
    }
}