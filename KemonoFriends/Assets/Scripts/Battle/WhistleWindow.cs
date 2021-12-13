using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    /// <summary>
    /// バトル中に表示するプレイヤーのホイッスルウィンドウ
    /// </summary>
    public class WhistleWindow : MonoBehaviour
    {
        /// <summary>
        /// ホイッスルの仕様上の最大値
        /// </summary>
        static private int MAX_WHISTLE_NUM = 5;

        /// <summary>
        /// ホイッスルのアイコン
        /// </summary>
        [SerializeField]
        private GameObject[] m_Whisles = new GameObject[MAX_WHISTLE_NUM];

        /// <summary>
        /// ホイッスルのアイコン枠の背景
        /// </summary>
        [SerializeField]
        private GameObject[] m_Shadows = new GameObject[MAX_WHISTLE_NUM];

        /// <summary>
        /// ホイッスルポイントの最大値
        /// MAX_WHISTLE_NUM とは違って現状の強化値での最大値となります。
        /// </summary>
        private int m_MaxPoint = 1;

        /// <summary>
        /// 現在のホイッスルポイント
        /// </summary>
        private int m_NowPoint = 1;

        /// <summary>
        /// 現在の最大ホイッスルポイントの値を設定します。
        /// ０を下回ったり、仕様上の最大値を上回ることはできません。
        /// </summary>
        public void SetMaxPoint(int value)
        {
            value = Mathf.Max(0, value);
            value = Mathf.Min(value, MAX_WHISTLE_NUM);
            m_MaxPoint = value;
            if(m_NowPoint > m_MaxPoint)
            {
                SetNowPoint(m_MaxPoint);
            }
            // 仕様上の最大値を超えるアイコンの背景は非表示にします。
            for(int i = 0; i < MAX_WHISTLE_NUM; ++i)
            {
                m_Shadows[i].SetActive(i < m_MaxPoint);
            }
        }

        /// <summary>
        /// 現在のホイッスルポイントの値を設定します。
        /// ０を下回ったり、最大値を上回ることはできません。
        /// </summary>
        public void SetNowPoint(int value)
        {
            value = Mathf.Max(0, value);
            value = Mathf.Min(value, m_MaxPoint);
            m_NowPoint = value;
            // 最大値を超えるアイコンは非表示にします。
            for(int i = 0; i < MAX_WHISTLE_NUM; ++i)
            {
                m_Whisles[i].SetActive(i < m_NowPoint);
            }
        }

        private void Start()
        {
            SetMaxPoint(m_MaxPoint);
            SetNowPoint(m_NowPoint);
        }
    }
}
