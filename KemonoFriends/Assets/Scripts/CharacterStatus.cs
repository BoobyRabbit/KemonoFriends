using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatus : CharacterStatusMasterItem
{
    /// <summary>
    /// 現在のたいりょく
    /// </summary>
    public int NowHP
    {
        get { return this.nowHP; }
        set { this.nowHP = Mathf.Clamp(value, 0, this.maxHP); }
    }
    private int nowHP = 0;

    /// <summary>
    /// 現在のけものぷらずむ
    /// </summary>
    public int NowKP
    {
        get { return this.nowKP; }
        set { this.nowKP = Mathf.Clamp(value, 0, this.maxKP); }
    }
    private int nowKP = 0;

    /// <summary>
    /// 最大アクションポイント
    /// </summary>
    public static float needAP = 100.0f;

    /// <summary>
    /// 最大アクションポイント
    /// </summary>
    public float maxAP = 100.0f;

    /// <summary>
    /// 現在のアクションポイント
    /// </summary>
    public float NowAP
    {
        get { return this.nowAP; }
        set { this.nowAP = Mathf.Clamp(value, 0, maxAP); }
    }
    private float nowAP = 0;

    private bool CanActionImpl(float ap) => ap >= needAP;

    /// <summary>
    /// アクション可能かどうか
    /// </summary>
    public bool CanAction => this.CanActionImpl(this.NowAP);

    /// <summary>
    /// 何ターン後にアクション可能かを返します。
    /// このターンにアクション可能なら０になります。
    /// 素早さが０以下のため無限に不可能なら－１になります。
    /// </summary>
    public int CanActionTurn
    {
        get
        {
            // 実際のアクションポイントが変動しないよう仮の値を用意しておく
            float tempActionPoint = this.NowAP;
            // 素早さが０でもすでに何らかの理由でアクションポイントがたまっている
            // 状況を想定して１度だけ可能かどうか調べておく
            if(this.CanActionImpl(tempActionPoint))
            {
                return 0;
            }
            // 素早さが０以下のため無限にアクション不可能な場合
            if(this.speed <= 0)
            {
                return -1;
            }
            // アクション可能になるまでアクションポイントを追加
            int turn = 1;
            do
            {
                tempActionPoint += this.speed;
                turn++;
            }
            while(!this.CanActionImpl(tempActionPoint));
            return turn;
        }
    }

    public CharacterStatus(CharacterStatusMasterItem item) : base(item)
    {
        this.NowHP = item.maxHP;
        this.NowKP = item.maxKP;
    }
}
