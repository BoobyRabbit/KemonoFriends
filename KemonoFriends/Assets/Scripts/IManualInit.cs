/// <summary>
/// Unity の機能を使わずに手動で Awake, Start と同様に初期化するための機能を提供します。
/// </summary>
public interface IManualInit
{
    /// <summary>
    /// 初期化します。
    /// </summary>
    void ManualInit();
}
