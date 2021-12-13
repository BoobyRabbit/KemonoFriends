using UnityEngine;

/// <summary>
/// シーン移行時に最初に動作する MonoBehaviour
/// 各シーンに一つだけ配置してください。
/// 
/// </summary>
public class SceneEntry : MonoBehaviour, IManualInit, IManualUpdate
{
    /// <summary>
    /// Unityエディタから直接各シーンを起動した時のみ実行する初期化
    /// </summary>
    public virtual void ManualInit() { }

    /// <summary>
    /// シーン中に毎フレーム実行する処理
    /// </summary>
    public virtual void ManualUpdate() { }
}
