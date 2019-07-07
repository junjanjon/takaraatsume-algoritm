using System.IO;
using System.Linq;
using UnityEditor;

public class Build
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
}
