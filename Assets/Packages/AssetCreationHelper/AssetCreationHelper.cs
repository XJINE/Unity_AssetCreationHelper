#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;
using System.Reflection;
using System.Text.RegularExpressions;
using Object = UnityEngine.Object;

public static class AssetCreationHelper
{
    #region Field

    private static readonly EditorWindow ProjectWindow;
    private static readonly MethodInfo   FrameObjectInProjectWindow;

    #endregion Field

    #region Constructor

    static AssetCreationHelper()
    {
        var unityEditor       = Assembly.Load("UnityEditor");
        var projectBrowser    = unityEditor.GetType("UnityEditor.ProjectBrowser");
        var projectWindowUtil = unityEditor.GetType("UnityEditor.ProjectWindowUtil");
        
        ProjectWindow              = EditorWindow.GetWindow(projectBrowser);
        FrameObjectInProjectWindow = projectWindowUtil.GetMethod("FrameObjectInProjectWindow",
                                                                 BindingFlags.NonPublic |
                                                                 BindingFlags.Instance  |
                                                                 BindingFlags.Static);
    }

    #endregion Constructor

    #region Method

    public static string CreateAssetInCurrentDirectory(Object asset, string nameWithExtension, bool overwrite = false)
    {
        return CreateAssetInCurrentDirectory<Object>(asset, nameWithExtension, overwrite);
    }

    public static string CreateAssetInCurrentDirectory(string asset, string nameWithExtension, bool overwrite = false)
    {
        return CreateAssetInCurrentDirectory<string>(asset, nameWithExtension, overwrite);
    }

    private static string CreateAssetInCurrentDirectory<T>(object asset, string nameWithExtension, bool overwrite)
    {
        var path = GetCurrentAssetDirectoryPathRelative() + "/" + nameWithExtension;

        if (!overwrite)
        {
            path = CorrectAssetNameToAvoidOverwrite(path);
        }

        if (typeof(T) == typeof(Object))
        {
            AssetDatabase.CreateAsset((Object)asset, path);
        }
        else
        {
            System.IO.File.WriteAllText(path, (string)asset);
        }

        AssetDatabase.Refresh();

        StartToRenameAsset(path);

        return path;
    }

    public static string GetCurrentAssetDirectoryPathAbsolute()
    {
        var path = Application.dataPath;

        foreach (var asset in Selection.GetFiltered(typeof(Object), SelectionMode.Assets))
        {
            var tempPath = AssetDatabase.GetAssetPath(asset);

            if (System.IO.File.Exists(path))
            {
                path += "/" + System.IO.Path.GetDirectoryName(tempPath);
                break;
            }
        }

        return path;
    }

    public static string GetCurrentAssetDirectoryPathRelative()
    {
        // WARNING:
        // "Application.dataPath" returns absolute path
        // and the following code makes a relative path.
        // If any assets are not selected in UnityEditor like a just after started,
        // this function returns absolute path.
        // "AssetDatabase.CreateAsset()" will be failed if the path is absolute path.

        var path = "Assets";

        foreach (var asset in Selection.GetFiltered(typeof(Object), SelectionMode.Assets))
        {
            path = AssetDatabase.GetAssetPath(asset);

            if (System.IO.File.Exists(path))
            {
                path = System.IO.Path.GetDirectoryName(path);
                break;
            }
        }

        return path;
    }

    public static string CorrectAssetNameToAvoidOverwrite(string assetPath)
    {
        // NOTE:
        // This assetPath has extension.

        var suffixNum = "";
        var extension = System.IO.Path.GetExtension(assetPath);
            assetPath = assetPath.Remove(assetPath.Length - extension.Length, extension.Length);

        var regex = new Regex(@"(?<assetPath>) (?<suffixNum>\d+)");
        var match = regex.Match(assetPath);

        if (match.Success)
        {
            assetPath = match.Groups["assetPath"].Value;
            suffixNum = match.Groups["suffixNum"].Value;
        }

        while (true)
        {
            var tempPath = assetPath + (string.IsNullOrEmpty(suffixNum) ? "" : " " + suffixNum) + extension;

            if (!System.IO.File.Exists(tempPath))
            {
                return tempPath;
            }

            suffixNum = string.IsNullOrEmpty(suffixNum) ? "1" : (int.Parse(suffixNum) + 1).ToString();
        }
    }

    public static void StartToRenameAsset(string assetPath)
    {
        StartToRenameAsset(AssetDatabase.LoadAssetAtPath<Object>(assetPath));
    }

    public static void StartToRenameAsset(Object asset)
    {
        // NOTE:
        // PingObject function is quite simple & work completely to rename.
        // However the animation is not so good.
        // EditorGUIUtility.PingObject(asset);

        // NOTE:
        // FocusAsset function is important to rename asset in following case.
        // - "Project/Assets/~Any Directory" is focused.
        // - Any other UnityEditor window is focused.

        Selection.activeObject = asset;
        EditorUtility.FocusProjectWindow();
        FocusAsset(asset);
        ProjectWindow.SendEvent(Event.KeyboardEvent(KeyCode.F2.ToString()));
    }

    public static void FocusAsset(Object asset)
    {
        // NOTE:
        // https://github.com/Unity-Technologies/UnityCsReference/blob/master/Editor/Mono/ProjectWindow/ProjectWindowUtil.cs

        FrameObjectInProjectWindow.Invoke(ProjectWindow, new object[] { asset.GetInstanceID() });
    }

    #endregion Method
}

#endif // UNITY_EDITOR