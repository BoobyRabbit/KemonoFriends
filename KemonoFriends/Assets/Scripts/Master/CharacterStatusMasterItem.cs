using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatusMasterItem : IMasterItem
{
    public int ID { get; set; }

    /// <summary>
    /// 名前
    /// </summary>
    public string name = string.Empty;

    /// <summary>
    /// 名前の綴り
    /// </summary>
    public string spell = string.Empty;

    /// <summary>
    /// 最大たいりょく
    /// </summary>
    public int maxHP = 0;

    /// <summary>
    /// 最大けものぷらずむ
    /// </summary>
    public int maxKP = 0;

    /// <summary>
    /// こうげき
    /// </summary>
    public int attack = 0;

    /// <summary>
    /// まもり
    /// </summary>
    public int deffence = 0;

    /// <summary>
    /// はやさ
    /// </summary>
    public float speed = 0;

    /// <summary>
    /// 習得スキルリスト
    /// </summary>
    public List<string> skillNameList = null;

    /// <summary>
    /// イメージカラー赤
    /// </summary>
    public int red = 0;

    /// <summary>
    /// イメージカラー青
    /// </summary>
    public int blue = 0;

    /// <summary>
    /// イメージカラー緑
    /// </summary>
    public int green = 0;

    /// <summary>
    /// イメージカラー
    /// </summary>
    public Color Color { get { return new Color(red / 255.0f, green / 255.0f, blue / 255.0f); } }

    /// <summary>
    /// 特定の値で初期化します。
    /// </summary>
    public CharacterStatusMasterItem() { }

    /// <summary>
    /// 指定したマスタの値をコピーします
    /// </summary>
    public CharacterStatusMasterItem(CharacterStatusMasterItem master)
    {
        this.ID = master.ID;
        this.name = master.name;
        this.spell = master.spell;
        this.maxHP = master.maxHP;
        this.maxKP = master.maxKP;
        this.attack = master.attack;
        this.deffence = master.deffence;
        this.speed = master.speed;
        this.skillNameList = master.skillNameList;
        this.red = master.red;
        this.blue = master.blue;
        this.green = master.green;
    }
}
