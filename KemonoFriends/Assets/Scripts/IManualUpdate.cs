using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Unity の機能を使わずに手動で Update を呼び出すための機能を提供します。
/// </summary>
public interface IManualUpdate
{
    /// <summary>
    /// 更新します。
    /// </summary>
    void ManualUpdate();
}
