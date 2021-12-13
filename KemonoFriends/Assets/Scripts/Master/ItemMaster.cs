using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMaster
{
    /// <summary>
    /// Singleton な実体
    /// </summary>
    public static ItemMaster Instance { get; } = new ItemMaster();

    private List<ItemMasterItem> master = new List<ItemMasterItem>();

    /// <summary>
    /// 指定したIDのアイテムのパラメータを返します。
    /// </summary>
    public ItemMasterItem this[int id] => master.Find(i => i.ID == id);

    /// <summary>
    /// マスタデータを読み取り保存します。
    /// Singleton パターン実現のために private としています。
    /// </summary>
    private ItemMaster()
    {
        CSVLoader.CSV csv = CSVLoader.LoadFromResources("Master/Parameters_Item");
        for(int row = 1; row < csv.GetRowCount(); ++row)
        {
            ItemMasterItem item = new ItemMasterItem();
            for(int column = 0; column < csv.GetColumnCount(); ++column)
            {
                string tag = csv[0, column];
                string cellValue = csv[row, column];
                switch(tag)
                {

                case "アイテムID": item.ID = GameUtility.StringToInt(cellValue); break;
                case "名前": item.name = cellValue; break;
                case "名前の綴り": item.spell = cellValue; break;
                case "種類":
                    switch(cellValue)
                    {
                    case "消費": item.type = ItemType.Consumption; break;
                    case "重要": item.type = ItemType.Important; break;
                    default:
                        Debug.LogError($"\"{cellValue}\" is invalid.");
                        break;
                    }
                    break;
                case "回復体力": item.recoverHP = GameUtility.StringToInt(cellValue); break;
                case "回復輝き": item.recoverKagayaki = GameUtility.StringToInt(cellValue); break;
                case "付与する状態": item.giveState = cellValue; break;
                case "対象":
                    switch(cellValue)
                    {
                    case "味方１人": item.target = Battle.Target.Friend; break;
                    case "味方全員": item.target = Battle.Target.Friends; break;
                    case "敵１人": item.target = Battle.Target.Enemy; break;
                    case "敵全員": item.target = Battle.Target.Enemies; break;
                    case "全員": item.target = Battle.Target.All; break;
                    }
                    break;
                case "消費AP": item.useAP = GameUtility.StringToFloat(cellValue); break;
                case "特殊効果": item.isSpecial = GameUtility.StringToBool(cellValue); break;
                case "説明": item.explanation = cellValue; break;
                case "備考": break;
                default: Debug.LogError($"\"{tag}\" is invalid."); break;
                }
            }
            this.master.Add(item);
        }
    }
}
