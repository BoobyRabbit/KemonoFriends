using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アイテムの種類
/// </summary>
public enum ItemType
{
    Consumption, // 消費
    Important, // 重要
}

public struct ItemMasterItem : IMasterItem
{
    /// <summary>
    /// アイテムID
    /// </summary>
    public int ID { get; set; }

    /// <summary>
    /// 名前
    /// </summary>
    public string name;

    /// <summary>
    /// 名前の綴り
    /// </summary>
    public string spell;

    /// <summary>
    /// 種類
    /// </summary>
    public ItemType type;

    /// <summary>
    /// 回復するＨＰ
    /// </summary>
    public int recoverHP;

    /// <summary>
    /// 回復する輝き
    /// </summary>
    public int recoverKagayaki;

    /// <summary>
    /// 付与する状態
    /// </summary>
    public string giveState;

    /// <summary>
    /// 対象
    /// </summary>
    public Battle.Target target;

    /// <summary>
    /// 消費アクションポイント
    /// </summary>
    public float useAP;

    /// <summary>
    /// 特殊効果
    /// </summary>
    public bool isSpecial;

    /// <summary>
    /// 説明
    /// </summary>
    public string explanation;
}
