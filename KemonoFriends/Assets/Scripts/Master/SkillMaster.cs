using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 全スキルパラメータを持つマスターデータ
/// </summary>
public class SkillMaster
{
    /// <summary>
    /// Singleton な実体
    /// </summary>
    public static SkillMaster Instance { get; } = new SkillMaster();

    /// <summary>
    /// マスターデータの本体
    /// </summary>
    private Dictionary<int, SkillMasterItem> m_Master = new Dictionary<int, SkillMasterItem>();

    /// <summary>
    /// 指定したIDのアイテムのパラメータを返します。
    /// </summary>
    public SkillMasterItem this[int id] => m_Master[id];

    /// <summary>
    /// 指定した名前のキャラクターのパラメータを返します。
    /// </summary>
    public SkillMasterItem this[string name]
    {
        get
        {
            foreach(SkillMasterItem parameter in m_Master.Values)
            {
                if(parameter.name == name)
                {
                    return parameter;
                }
            }
            Debug.LogError($"\"{name}\" is not found.");
            return new SkillMasterItem();
        }
    }

    /// <summary>
    /// マスタデータを読み取り保存します。
    /// Singleton パターン実現のために private としています。
    /// </summary>
    private SkillMaster()
    {
        CSVLoader.CSV csv = CSVLoader.LoadFromResources("Master/Parameters_Skill");
        for(int row = 1; row < csv.GetRowCount(); ++row)
        {
            SkillMasterItem item = new SkillMasterItem();
            for(int column = 0; column < csv.GetColumnCount(); ++column)
            {
                string tag = csv[0, column];
                string cellValue = csv[row, column];
                switch(tag)
                {

                case "スキルID": item.id = GameUtility.StringToInt(cellValue); break;
                case "名前": item.name = cellValue; break;
                case "対象":
                    switch(cellValue)
                    {
                    case "味方１体": item.target = Battle.Target.Friend; break;
                    case "味方全員": item.target = Battle.Target.Friends; break;
                    case "敵１体": item.target = Battle.Target.Enemy; break;
                    case "敵全員": item.target = Battle.Target.Enemies; break;
                    case "全員": item.target = Battle.Target.All; break;
                    }
                    break;
                case "消費KP": item.kemonoPlasm = GameUtility.StringToInt(cellValue); break;
                case "消費AP": item.useAP = GameUtility.StringToInt(cellValue); break;
                case "説明": item.explanation = cellValue; break;
                case "備考": break;
                default: Debug.LogError($"\"{tag}\" is invalid."); break;
                }
            }
            m_Master.Add(item.id, item);
        }
    }
}
