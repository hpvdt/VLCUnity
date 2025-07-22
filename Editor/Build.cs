using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using System.IO;

// standalone build step for copying necessary additional libvlc build files
public class CopyLibVLCFiles : IPostprocessBuildWithReport
{
    const string lua = "lua";
    const string hrtfs = "hrtfs";
    const string locale = "locale";
    const string pluginsDat = "plugins.dat";
    const string VLCUnity = "VLCUnity";
    const string x64 = "x86_64";
    const string Plugins = "Plugins";
    const string plugins = "plugins";
    const string Data = "_Data";
    const string Windows = "Windows";
    const string Linux = "Linux";
    public int callbackOrder => 0;
    
    public void OnPostprocessBuild(BuildReport report)
    {
        string platformFolder;
        var platform = report.summary.platform;

        if (platform == BuildTarget.StandaloneWindows64)
            platformFolder = Windows;
        else if (platform == BuildTarget.StandaloneLinux64)
            platformFolder = Linux;
        else
            return;

        var buildOutput = Path.GetDirectoryName(report.summary.outputPath);
        var libvlcBuildOutput = Path.Combine(buildOutput, $"{Application.productName}{Data}", Plugins, x64);
        var sourceLibvlcLocation = Path.Combine(Path.GetFullPath(Application.dataPath), VLCUnity, Plugins, platformFolder, x64);
        var sourcePluginsLibvlcLocation = Path.Combine(sourceLibvlcLocation, plugins);

        CopyFolder(Path.Combine(sourceLibvlcLocation, lua), Path.Combine(libvlcBuildOutput, lua));
        CopyFolder(Path.Combine(sourceLibvlcLocation, hrtfs), Path.Combine(libvlcBuildOutput, hrtfs));
        CopyFolder(Path.Combine(sourceLibvlcLocation, locale), Path.Combine(libvlcBuildOutput, locale));

        CopyFile(Path.Combine(sourcePluginsLibvlcLocation, pluginsDat), Path.Combine(libvlcBuildOutput, pluginsDat));
    }

    void CopyFolder(string sourceFolder, string destFolder)
    {
        if(!Directory.Exists(sourceFolder))
            return;
        if (!Directory.Exists( destFolder ))
            Directory.CreateDirectory( destFolder );
        string[] files = Directory.GetFiles( sourceFolder );
        foreach (string file in files)
        {
            string name = Path.GetFileName( file );
            string dest = Path.Combine( destFolder, name );
            CopyFile( file, dest );
        }
        string[] folders = Directory.GetDirectories( sourceFolder );
        foreach (string folder in folders)
        {
            string name = Path.GetFileName( folder );
            string dest = Path.Combine( destFolder, name );
            CopyFolder( folder, dest );
        }
    }

    void CopyFile(string sourceFile, string destFile, bool overwrite = true)
    {
        if(File.Exists(sourceFile))
        {
            File.Copy(sourceFile, destFile, overwrite);
        }
    }
}
