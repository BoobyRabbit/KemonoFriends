using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 時間を計測します
/// </summary>
public class Count
{
    /// <summary>
    /// 計測停止中かどうか
    /// </summary>
    public bool IsStop { get; set; }

    /// <summary>
    /// 経過時間（秒）
    /// </summary>
    private float? passTime;

    /// <summary>
    /// 目標時間（秒）
    /// </summary>
    private float? targetTime;

    /// <summary>
    /// 経過時間（秒）
    /// 未計測時は 0.0f 取得します。
    /// </summary>
    public float PassTime
    {
        get
        {
            return this.passTime.HasValue ? this.passTime.Value : 0.0f;
        }
    }

    /// <summary>
    /// 残り時間（秒）
    /// 目標時間が未設定なら 0.0f 取得します。
    /// </summary>
    public float RestTime
    {
        get
        {
            if(this.targetTime.HasValue)
            {
                if(this.passTime.HasValue)
                {
                    return Mathf.Max(this.targetTime.Value - this.passTime.Value, 0.0f);
                }
                return this.targetTime.Value;
            }
            return 0.0f;
        }
    }

    /// <summary>
    /// 計測中かどうか
    /// </summary>
    public bool IsPlay => this.passTime.HasValue;

    /// <summary>
    /// 目標時間が経過済みどうか
    /// </summary>
    public bool IsEnd => this.RestTime == 0.0f;

    /// <summary>
    /// 線形補間の係数
    /// 計測開始時は 0.0f で目標時間で 1.0f を取得します。
    /// 目標時間が未設定なら 0.0f を取得します。
    /// </summary>
    public float LerpF
    {
        get
        {
            if(this.targetTime.HasValue)
            {
                return Mathf.Clamp(PassTime, 0.0f, 1.0f) / this.targetTime.Value;
            }
            return 0.0f;
        }
    }

    /// <summary>
    /// 固定の値で初期化します
    /// </summary>
    public Count()
    {
        this.Reset();
    }

    /// <summary>
    /// 指定した値で初期化します
    /// </summary>
    /// <param name="time">目標の時間</param>
    public Count(float time)
    {
        this.passTime = null;
        this.targetTime = time;
    }

    /// <summary>
    /// 計測を開始します
    /// </summary>
    public void Start()
    {
        this.passTime = 0;
    }

    /// <summary>
    /// 未計測の状態に戻します。
    /// </summary>
    public void Reset()
    {
        this.passTime = null;
    }

    /// <summary>
    /// 毎フレーム行う処理
    /// </summary>
    public void Update()
    {
        if(!this.IsStop && this.passTime.HasValue)
        {
            passTime += Time.deltaTime;
        }
    }
}
