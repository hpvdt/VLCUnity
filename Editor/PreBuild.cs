#if UNITY_2018_1_OR_NEWER
#define UNITY_SUPPORTS_BUILD_REPORT
#endif
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEngine.Rendering;
using UnityEditor.Build;
#if UNITY_SUPPORTS_BUILD_REPORT
using UnityEditor.Build.Reporting;
#endif

namespace Videolabs.VLCUnity.Editor
{
    public class PreProcessBuild :
#if UNITY_SUPPORTS_BUILD_REPORT
        IPreprocessBuildWithReport
#else
        IPreprocessBuild
#endif
    {
        public int callbackOrder
        {
            get { return 0; }
        }

        const string AndroidVulkanErrorMessage = "The Vulkan graphics API is not supported by the VLC Unity plugin." +
                                                 "\n\nPlease go to Player Settings > Android > Auto Graphics API and remove Vulkan from the list." +
                                                 "\nOnly OpenGL ES 2.0 and 3.0 are currently supported on Android.";

        const string WindowsD3D12ErrorMessage =
            "The Direct3D12 graphics API is not supported by the VLC Unity plugin." +
            "\n\nPlease go to Player Settings > Windows or UWP > Auto Graphics API and remove Direct3D12 from the list." +
            "\nOnly Direct3D11 is currently supported on Windows and UWP targets.";

        const string LinuxVulkanErrorMessage =
            "The Vulkan graphics API is not supported by the VLC Unity plugin on Linux.\n\nPlease go to Player Settings > Player > Other Settings and disable 'Auto Graphics API for Linux'. Then ensure Vulkan is not in the list of Graphics APIs.\nOnly OpenGLCore is currently supported on Linux.";

#if UNITY_SUPPORTS_BUILD_REPORT
        public void OnPreprocessBuild(BuildReport report)
        {
            OnPreprocessBuild(report.summary.platform, report.summary.outputPath);
        }
#endif

        public void OnPreprocessBuild(BuildTarget target, string path)
        {
            if (target == BuildTarget.Android)
            {
                if (PlayerSettings.GetGraphicsAPIs(target).Contains(GraphicsDeviceType.Vulkan))
                {
                    throw new BuildFailedException(AndroidVulkanErrorMessage);
                }
            }
            else if (target == BuildTarget.StandaloneWindows64 || target == BuildTarget.WSAPlayer)
            {
                if (PlayerSettings.GetGraphicsAPIs(target).Contains(GraphicsDeviceType.Direct3D12))
                {
                    throw new BuildFailedException(WindowsD3D12ErrorMessage);
                }
            }
            else if (target == BuildTarget.StandaloneLinux64)
            {
                var graphicsAPIs = PlayerSettings.GetGraphicsAPIs(target).ToList();
                if (graphicsAPIs.Contains(GraphicsDeviceType.Vulkan))
                {
                    graphicsAPIs.Remove(GraphicsDeviceType.Vulkan);
                    PlayerSettings.SetGraphicsAPIs(target, graphicsAPIs.ToArray());
                }
            }
        }
    }
}