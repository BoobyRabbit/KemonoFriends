using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ゲーム起動時に実行される初期化
/// </summary>
public class GameInit
{
    private const string InitSceneName = "InitScene";

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void RuntimeInitializeApplication()
    {
        if(!SceneManager.GetSceneByName(InitSceneName).IsValid())
        {
            SceneManager.LoadScene(InitSceneName, LoadSceneMode.Additive);
        }
    }
}