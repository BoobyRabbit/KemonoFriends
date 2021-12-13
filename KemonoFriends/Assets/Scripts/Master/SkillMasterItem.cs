/// <summary>
/// スキルパラメータ
/// １つのスキルが持つ情報を扱う構造体
/// </summary>
public struct SkillMasterItem
{
    /// <summary>
    /// スキルID
    /// </summary>
    public int id;

    /// <summary>
    /// 名前
    /// </summary>
    public string name;

    /// <summary>
    /// 対象
    /// </summary>
    public Battle.Target target;

    /// <summary>
    /// 説明
    /// </summary>
    public string explanation;

    /// <summary>
    /// 消費けものプラズム
    /// </summary>
    public int kemonoPlasm;

    /// <summary>
    /// 消費アクションポイント
    /// </summary>
    public int useAP;
}