using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 選択方法
/// </summary>
public enum SelectType
{
    Horizontal, // 横方向に選択
    Vertical, // 縦方向に選択
}

/// <summary>
/// 選択肢の処理
/// </summary>
abstract public class Select
{
    /// <summary>
    /// 次の選択肢のリスト
    /// </summary>
    private List<Func<Select, Select>> m_NextSelectGetters = new List<Func<Select, Select>>();

    /// <summary>
    /// Enter 済みの次の選択肢
    /// </summary>
    private Select m_NextSelect = null;

    /// <summary>
    /// 次の選択肢
    /// なければ null
    /// </summary>
    public Select NextSelect
    {
        get
        {
            return m_NextSelect == null ? null : m_NextSelect;
        }
    }

    /// <summary>
    /// 前の選択肢
    /// なければ null
    /// </summary>
    private Select m_PreviousSelect = null;

    /// <summary>
    /// 選択肢の数
    /// </summary>
    public int Num { get; private set; } = 0;

    /// <summary>
    /// この選択肢で選択中の項目の番号
    /// ０から始まります
    /// </summary>
    public int SelectIndex { get; private set; }

    /// <summary>
    /// 選択方法
    /// </summary>
    private SelectType m_Type;

    /// <summary>
    /// 最初の選択肢の時に Enter を実行済みかどうか
    /// 最初の選択肢の場合は構造上 Enter が行われないため、
    /// このフラグを利用して Enter を行っています。
    /// </summary>
    private bool m_HasEnterdIfFirstSelect = false;

    /// <summary>
    /// 選択肢を決定するボタンの名前
    /// </summary>
    public string decisionButtonName = "Decision";

    /// <summary>
    /// 選択肢を前に戻すボタンの名前
    /// </summary>
    public string cancelButtonName = "Cancel";

    /// <summary>
    /// 選択肢カーソルを上に移動させるボタンの名前
    /// </summary>
    public string upButtonName = "Up";

    /// <summary>
    /// 選択肢カーソルを下に移動させるボタンの名前
    /// </summary>
    public string downButtonName = "Down";

    /// <summary>
    /// 選択肢カーソルを左に移動させるボタンの名前
    /// </summary>
    public string leftButtonName = "Left";

    /// <summary>
    /// 選択肢カーソルを右に移動させるボタンの名前
    /// </summary>
    public string rightButtonName = "Right";

    /// <summary>
    /// 指定した値で初期化します。
    /// </summary>
    /// <param name="selectNum">選択肢の数</param>
    /// <param name="type">選択方法</param>
    /// <param name="thisObject">このクラスを管理する SelectMonoBehaviour</param>
    protected Select(int selectNum, SelectType type)
    {
        Num = selectNum;
        SelectIndex = 0;
        m_Type = type;
        for(int i = 0; i < Num; ++i)
        {
            m_NextSelectGetters.Add(null);
        }
    }

    /// <summary>
    /// 指定した値で初期化します。
    /// 選択肢の数は後で設定します。
    /// </summary>
    protected Select(SelectType type)
    {
        m_Type = type;
    }

    /// <summary>
    /// この選択肢の表示を開始した時に実行する処理
    /// 表示内容が動的に変わる場合はこちらで表示物を生成してください。
    /// </summary>
    protected abstract void Enter();

    /// <summary>
    /// この選択肢の表示を終了した時に実行する処理
    /// 表示内容が動的に変わる場合はこちらで表示物を削除してください。
    /// </summary>
    protected abstract void Exit();

    /// <summary>
    /// 次の項目を選択する時の処理
    /// </summary>
    protected abstract void Next();

    /// <summary>
    /// 前の項目を選択する時の処理
    /// </summary>
    protected abstract void Previous();

    /// <summary>
    /// 選択中の項目に決定した時の処理
    /// </summary>
    protected abstract void Decision();

    /// <summary>
    /// 選択肢をキャンセルした時の処理
    /// </summary>
    protected abstract void Cancel();

    /// <summary>
    /// 毎フレーム実行する処理
    /// </summary>
    public abstract void Update();

    /// <summary>
    /// 自身の SelectMonoBehaviour を取得します。
    /// </summary>
    /// <returns></returns>
    protected abstract ISelectMonoBehaviour GetSelectMonoBehaviour();

    /// <summary>
    /// 指定したインデックスを選んだ時の次の選択肢を取得する方法を設定します。
    /// </summary>
    public void SetNextSelectGetter(int index, Func<Select, Select> nextSelectGetter)
    {
        m_NextSelectGetters[index] = nextSelectGetter;
    }

    private void MyEnter()
    {
        GetSelectMonoBehaviour().Own.gameObject.SetActive(true);
        Enter();
    }

    private void MyExit()
    {
        Exit();
        GetSelectMonoBehaviour().Own.gameObject.SetActive(false);
    }

    /// <summary>
    /// 前の選択肢を全て初期化します（再帰）
    /// 選択肢を決定した際に呼び出すと全ての選択肢を初期化できます。
    /// </summary>
    protected void ResetAlllPreviousSelect()
    {
        Exit();
        m_NextSelect = null;
        GetSelectMonoBehaviour().Own.gameObject.SetActive(false);
        GetSelectMonoBehaviour().Uninitialize();
        if(m_PreviousSelect != null)
        {
            m_PreviousSelect.ResetAlllPreviousSelect();
            m_PreviousSelect = null;
        }
    }

    /// <summary>
    /// 毎フレーム実行する処理の共通部分
    /// </summary>
    virtual public void UpdateCommon()
    {
        // 選択肢がなければ何もしない
        if(Num == 0)
        {
            return;
        }

        if(m_PreviousSelect == null)
        {
            if(!m_HasEnterdIfFirstSelect)
            {
                m_HasEnterdIfFirstSelect = true;
                MyEnter();
            }
        }
        if(m_NextSelect != null)
        {
            m_NextSelect.UpdateCommon();
        }
        else
        {
            if(GetButtonDownNext())
            {
                if(SelectIndex < Num - 1)
                {
                    ++SelectIndex;
                    Next();
                }
            }
            if(GetButtonDownPrevious())
            {
                if(SelectIndex > 0)
                {
                    --SelectIndex;
                    Previous();
                }
            }
            if(Input.GetButtonDown(this.decisionButtonName))
            {
                var nextSelectGetter = m_NextSelectGetters[SelectIndex];
                if(nextSelectGetter == null)
                {
                    Decision();
                }
                else
                {
                    MyExit();
                    m_NextSelect = nextSelectGetter(this);
                    m_NextSelect.m_PreviousSelect = this;
                    m_NextSelect.MyEnter();
                }
            }
            if(Input.GetButtonDown(this.cancelButtonName))
            {
                if(m_PreviousSelect == null)
                {
                    Cancel();
                }
                else
                {
                    MyExit();
                    m_PreviousSelect.m_NextSelect = null;
                    m_PreviousSelect.MyEnter();
                    m_PreviousSelect = null;
                }
            }
        }
    }

    private bool GetButtonDownNext()
    {
        switch(m_Type)
        {
        case SelectType.Horizontal: return Input.GetButtonDown(this.rightButtonName);
        case SelectType.Vertical: return Input.GetButtonDown(this.downButtonName);
        }
        Debug.LogError($"SelectType({m_Type}) is out of range.");
        return false;
    }

    private bool GetButtonDownPrevious()
    {
        switch(m_Type)
        {
        case SelectType.Horizontal: return Input.GetButtonDown(this.leftButtonName);
        case SelectType.Vertical: return Input.GetButtonDown(this.upButtonName);
        }
        Debug.LogError($"SelectType({m_Type}) is out of range.");
        return false;
    }
}
