using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Battle
{
    /// <summary>
    /// バトルアイコンの種類.
    /// バトルアイコン = プレイヤーのターンに表示される、行動の選択肢を表すアイコンの画像
    /// </summary>
    public enum BattleIconType
    {
        Attack, // 攻撃
        Item, // アイテム
        Plan, // 作戦、その他
    }

    public class SelectBattleIcon : Select
    {
        /// <summary>
        /// バトルアイコンのオブジェクトのリスト
        /// </summary>
        private List<GameObject> m_BattleIcons = null;

        /// <summary>
        /// バトルアイコン同士の角度
        /// </summary>
        private static readonly int s_AngleBetweenIcons = 35;

        /// <summary>
        /// １つ目のバトルアイコンの角度の初期値
        /// </summary>
        private static readonly int s_InitAngle = 45;

        /// <summary>
        /// １つ目のバトルアイコンの角度
        /// ２つ目以降は計算で算出されます。
        /// </summary>
        private int m_Angle = s_InitAngle;

        /// <summary>
        /// バトルアイコンの中心からの距離
        /// （アイコンは中心点からこの値の分だけ離れた場所で円状に並んでいます）
        /// </summary>
        static private float s_Distance = 150.0f;

        /// <summary>
        /// バトルアイコンの動きを管理します
        /// </summary>
        private Sequence m_BattleIconSequence = null;

        /// <summary>
        /// 選択中のキャラクター
        /// </summary>
        private Character m_BattleCharacter = null;

        /// <summary>
        /// 自身の MonoBehaviour
        /// </summary>
        private SelectBattleIconMonoBehaviour m_ThisMonoBehaviour;

        /// <summary>
        /// 指定した値で初期化します
        /// </summary>
        /// <param name="battleCharacter">バトルアイコンのリスト</param>
        /// <param name="battleIcons">選択中のキャラクター</param>
        public SelectBattleIcon(List<GameObject> battleIcons, Character battleCharacter, SelectBattleIconMonoBehaviour thisMonoBehaviour)
            : base(battleIcons.Count, SelectType.Vertical)
        {
            m_BattleIcons = battleIcons;
            m_BattleCharacter = battleCharacter;
            m_ThisMonoBehaviour = thisMonoBehaviour;
            Enter();
        }

        override protected void Enter()
        {
            for(int i = 0; i < m_BattleIcons.Count; ++i)
            {
                Image iconImage = m_BattleIcons[i].GetComponent<Image>();
                iconImage.color = i == SelectIndex ? Color.white : Color.grey;
            }
            Update();
        }

        override protected void Exit() { }

        override protected void Next()
        {
            Select(true);
        }

        override protected void Previous()
        {
            Select(false);
        }

        protected override void Decision() { }

        protected override void Cancel() { }

        override public void Update()
        {
            for(int i = 0; i < m_BattleIcons.Count; ++i)
            {
                var battleIcon = m_BattleIcons[i];
                float radian = Mathf.PI * (m_Angle + s_AngleBetweenIcons * i) / 180.0f;
                Vector3 characterPosition = RectTransformUtility.WorldToScreenPoint(Camera.main, m_BattleCharacter.transform.position + Vector3.up * m_BattleCharacter.Bounds.extents.y / 2);
                Vector3 circlePosition = new Vector3(Mathf.Sin(radian) * s_Distance, Mathf.Cos(radian) * s_Distance, battleIcon.transform.position.z);
                battleIcon.transform.position = characterPosition + circlePosition;
            }
        }

        protected override ISelectMonoBehaviour GetSelectMonoBehaviour()
        {
            return m_ThisMonoBehaviour;
        }

        /// <summary>
        /// アイコンを選択します
        /// </summary>
        private void Select(bool isNext)
        {
            const float time = 0.1f;
            int sign = isNext ? 1 : -1;
            m_BattleIconSequence.Kill(true);
            m_BattleIconSequence = DOTween.Sequence();
            m_BattleIconSequence.Join(
                DOTween.To(
                    () => m_Angle,
                    (int x) => m_Angle = x,
                    m_Angle - s_AngleBetweenIcons * sign,
                    time
                )
            );
            for(int i = 0; i < m_BattleIcons.Count; ++i)
            {
                var battleIconImage = m_BattleIcons[i].GetComponent<Image>();
                m_BattleIconSequence.Join(
                    DOTween.To(
                        () => battleIconImage.color,
                        (x) => battleIconImage.color = x,
                        i == SelectIndex ? Color.white : Color.grey,
                        time
                    )
                );
            }
        }
    }
}