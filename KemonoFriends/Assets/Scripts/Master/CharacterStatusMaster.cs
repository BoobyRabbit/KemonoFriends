using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// キャラクターのステータスを管理するマスタ
/// </summary>
public class CharacterStatusMaster
{
    /// <summary>
    /// Singleton な実体
    /// </summary>
    public static CharacterStatusMaster Instance { get; } = new CharacterStatusMaster();


    private List<CharacterStatusMasterItem> master = new List<CharacterStatusMasterItem>();

    /// <summary>
    /// 指定したIDの項目を取得します。
    /// </summary>
    public CharacterStatusMasterItem this[int id] => master.Find(item => item.ID == id);

    /// <summary>
    /// 指定したIDの項目を取得します。
    /// </summary>
    public CharacterStatusMasterItem this[string spell] => master.Find(item => item.spell == spell);

    /// <summary>
    /// マスタデータを読み取り保存します。
    /// Singleton パターン実現のために private としています。
    /// </summary>
    private CharacterStatusMaster()
    {
        CSVLoader.CSV csv = CSVLoader.LoadFromResources("Master/Parameters_Character");
        for(int row = 1; row < csv.GetRowCount(); ++row)
        {
            CharacterStatusMasterItem item = new CharacterStatusMasterItem();
            for(int column = 0; column < csv.GetColumnCount(); ++column)
            {
                string tag = csv[0, column];
                string cellValue = csv[row, column];
                switch(tag)
                {
                case "キャラクターID": item.ID = GameUtility.StringToInt(cellValue); break;
                case "名前": item.name = cellValue; break;
                case "ファイル名": item.spell = cellValue; break;
                case "たいりょく": item.maxHP = GameUtility.StringToInt(cellValue); break;
                case "こうげき": item.attack = GameUtility.StringToInt(cellValue); break;
                case "まもり": item.deffence = GameUtility.StringToInt(cellValue); break;
                case "はやさ": item.speed = GameUtility.StringToFloat(cellValue); break;
                case "KP": item.maxKP = GameUtility.StringToInt(cellValue); break;
                case "習得スキルリスト":
                    string[] skillArray = cellValue.Split(',');
                    List<string> skillList = new List<string>();
                    foreach(string skillName in skillArray)
                    {
                        skillList.Add(skillName);
                    }
                    item.skillNameList = skillList;
                    break;
                case "赤": item.red = GameUtility.StringToInt(cellValue); break;
                case "緑": item.green = GameUtility.StringToInt(cellValue); break;
                case "青": item.blue = GameUtility.StringToInt(cellValue); break;
                default: Debug.LogError($"\"{tag}\" is invalid."); break;
                }
            }
            master.Add(item);
        }
    }
}
