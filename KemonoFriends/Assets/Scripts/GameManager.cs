using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    /// <summary>
    /// 現在のシーンのエントリ
    /// </summary>
    private SceneEntry sceneEntry = null;

    /// <summary>
    /// プレイヤーのパーティー
    /// </summary>
    public List<Character> Party { get; private set; } = new List<Character>();

    /// <summary>
    /// 所持しているアイテムのリスト
    /// </summary>
    public List<ItemMasterItem> HaveItems { get; private set; } = new List<ItemMasterItem>();

    /// <summary>
    /// メッセージ用クラス
    /// </summary>
    [SerializeField]
    public Message message;

    override protected void Init()
    {
        this.GetSceneEntry();
        this.sceneEntry?.ManualInit();
        DontDestroyOnLoad(this);
    }

    /// <summary>
    /// シーンを変更します。
    /// </summary>
    /// <param name="sceneName">シーン名</param>
    public void ChangeScene(string sceneName)
    {
        var loadMainSceneParameter = new LoadSceneParameters(LoadSceneMode.Single);
        SceneManager.LoadScene(sceneName, loadMainSceneParameter);
        SceneManager.sceneLoaded += this.SceneLoaded;
    }

    public void Update()
    {
        this.sceneEntry?.ManualUpdate();
        this.message.ManualUpdate();
    }

    /// <summary>
    /// アクティブなシーンからエントリを取得してメンバに設定します。
    /// </summary>
    public void GetSceneEntry()
    {
        var mainScene = SceneManager.GetActiveScene();
        var rootObjects = mainScene.GetRootGameObjects();
        foreach(var rootObject in rootObjects)
        {
            this.sceneEntry = rootObject.GetComponent<SceneEntry>();
            if(this.sceneEntry != null)
            {
                break;
            }
        }
        if(this.sceneEntry == null)
        {
            Debug.LogError($"SceneEntry is not found in {mainScene.name} scene.");
        }
    }

    /// <summary>
    /// シーンのロード完了後に呼び出されるコールバック
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mode"></param>
    public void SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        this.GetSceneEntry();
        SceneManager.sceneLoaded -= this.SceneLoaded;
    }
}
