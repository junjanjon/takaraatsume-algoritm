using System.IO;
using System.Linq;
using UnityEditor;

public static class Build
{
    /// <summary>「コレクション」の全シーン情報を取得する</summary>
    private static string[] GetAllBuildScenePaths()
    {
        return TitleStart.DefineScene.Values.ToArray();
    }
    
    [MenuItem("Collection/Build Web GL")]
    public static void WebglBuild()
    {
        // ビルド対象シーンリスト
        string[] sceneList = GetAllBuildScenePaths();
        // 実行
        BuildPipeline.BuildPlayer(
            sceneList,         //!< ビルド対象シーンリスト
            Path.Combine(Directory.GetCurrentDirectory(), "webgl"), 
                               //!< 出力先
            BuildTarget.WebGL, //!< ビルド対象プラットフォーム
            BuildOptions.None  //!< ビルドオプション
        );
    }

    public static string IosBuildDirName     = "XcodeProject";
    public static string AndroidBuildDirName = "AndroidProject";

    /// <summary>ビルド対象の全シーン
    public static void IosBuild()
    {
        // ビルド対象シーンリスト
        string[] sceneList = GetAllBuildScenePaths();
        PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.iOS, "jp.co.drecom.beef.ios.source");
        // 実行
        var buildReport = BuildPipeline.BuildPlayer(
            sceneList,                          //!< ビルド対象シーンリスト
            Path.Combine(Directory.GetCurrentDirectory(), IosBuildDirName), //!< 出力先
            BuildTarget.iOS,                    //!< ビルド対象プラットフォーム
            BuildOptions.Development            //!< ビルドオプション
        );
    }

    public static void AndroidBuild()
    {
        // ビルド対象シーンリスト
        string[] sceneList = GetAllBuildScenePaths();
        PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, "jp.test.android.beef");
        EditorUserBuildSettings.androidBuildSystem = AndroidBuildSystem.Gradle;
        // 実行
        var buildReport = BuildPipeline.BuildPlayer(
            sceneList,                          //!< ビルド対象シーンリスト
            Path.Combine(Directory.GetCurrentDirectory(), AndroidBuildDirName), //!< 出力先
            BuildTarget.Android,                    //!< ビルド対象プラットフォーム
            BuildOptions.AcceptExternalModificationsToPlayer |
            BuildOptions.Development            //!< ビルドオプション
        );
    }

}
