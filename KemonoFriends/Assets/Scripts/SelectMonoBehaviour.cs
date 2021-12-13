using System;
using UnityEngine;

/// <summary>
/// Select で使用するための実装など含んだインターフェース
/// </summary>
public interface ISelectMonoBehaviour
{
    /// <summary>
    /// 自身の MonoBehaviour
    /// </summary>
    MonoBehaviour Own { get; }

    /// <summary>
    /// 初期化済みかどうか
    /// </summary>
    bool IsInitialized { get; }

    /// <summary>
    /// 初期化解除
    /// </summary>
    void Uninitialize();
}

/// <summary>
/// 選択肢クラスを持つ MonoBehaviour
/// </summary>
/// <typeparam name="OwnSelect"></typeparam>
public class SelectMonoBehaviour<OwnSelect> : MonoBehaviour, ISelectMonoBehaviour where OwnSelect : Select
{
    public MonoBehaviour Own { get { return this; } }

    public bool IsInitialized { get { return Body != null; } }

    /// <summary>
    /// 選択肢
    /// </summary>
    public OwnSelect Body { get; protected set; } = null;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 指定したインデックスを選んだ時の次の選択肢を設定します。
    /// </summary>
    /// <param name="index">指定されるインデックス</param>
    /// <param name="selectInit">次の選択肢の取得処理(引数は現在の選択肢)</param>
    public void SetNextSelectGetter(int index, Func<Select, Select> nextSelectGetter)
    {
        Body.SetNextSelectGetter(index, nextSelectGetter);
    }

    /// <summary>
    /// 毎フレーム行う処理
    /// 次の選択肢が有効なら次の選択肢も更新を行います（再帰）
    /// </summary>
    public void UpdateSelect()
    {
        Body.Update();
        Body.UpdateCommon();
    }

    public void Uninitialize()
    {
        Body = null;
    }
}