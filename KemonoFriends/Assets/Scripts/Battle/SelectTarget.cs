using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Battle
{
    /// <summary>
    /// バトル時に攻撃やアイテムのターゲット選択をするクラス
    /// </summary>
    public class SelectTarget : Select
    {
        /// <summary>
        /// 選択中のターゲットを示す三角印
        /// </summary>
        private static List<GameObject> m_Cursors = new List<GameObject>();

        /// <summary>
        /// カーソル
        /// </summary>
        private GameObject m_CursorPrefab = null;

        /// <summary>
        /// 名前ウィンドウ
        /// </summary>
        private GameObject m_NameWindow = null;

        /// <summary>
        /// 名前ウィンドウの RectTransform
        /// </summary>
        private static RectTransform m_NameWindowRect = null;

        /// <summary>
        /// 名前ウィンドウの名前
        /// </summary>
        private static Text m_Name = null;

        /// <summary>
        /// 選択する順番に並び替えたキャラクターのリスト
        /// </summary>
        private List<BattleCharacter> m_Order = new List<BattleCharacter>();

        /// <summary>
        /// 決定した時の動作
        /// </summary>
        private Action m_Desision;
        /// <summary>
        /// 自身の MonoBehaviour
        /// </summary>
        private SelectTargetMonoBehaviour m_ThisMonoBehaviour = null;

        /// <summary>
        /// 指定した引数で初期化します。
        /// </summary>
        public SelectTarget(List<BattleCharacter> characters, BattleCharacter actioner, Target target, Action decision,
            GameObject cursorPrefab, GameObject nameWindowObject, SelectTargetMonoBehaviour thisMonoBehaviour)
            : base(GetSelectNum(characters, target), SelectType.Horizontal)
        {
            m_ThisMonoBehaviour = thisMonoBehaviour;
            m_CursorPrefab = cursorPrefab;
            m_NameWindow = nameWindowObject;
            m_Desision = decision;
            m_NameWindowRect = m_NameWindow.GetComponent<RectTransform>();
            m_Name = m_NameWindow.transform.Find("Name").GetComponent<Text>();
            m_Order.AddRange(SelectableCharacters(characters, actioner, target));
            m_Order.OrderBy(battleCharacter => battleCharacter.transform.position.x);
        }

        protected override void Enter()
        {
            for(int i = 0; i < m_Order.Count; ++i)
            {
                GameObject cursor = GameObject.Instantiate(m_CursorPrefab, m_ThisMonoBehaviour.transform);
                cursor.name = "Target:" + m_Order[i].name;
                Bounds bounds = m_Order[i].Bounds;
                RectTransform rectTransform = cursor.GetComponent<RectTransform>();
                Vector3 position = m_Order[i].transform.position + Vector3.up * m_Order[i].Bounds.size.y; // キャラの少し上に配置
                rectTransform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, position);
                m_Cursors.Add(cursor);
            }
            UpdateCursorActive();
            SetWindow();
        }

        protected override void Exit()
        {
            foreach(var cursor in m_Cursors)
            {
                GameObject.Destroy(cursor);
            }
            m_Cursors.Clear();
        }

        protected override void Next()
        {
            UpdateCursorActive();
            SetWindow();
        }

        protected override void Previous()
        {
            UpdateCursorActive();
            SetWindow();
        }

        protected override void Decision()
        {
            m_Desision();
            ResetAlllPreviousSelect();
        }

        protected override void Cancel() { }

        public override void Update() { }

        protected override ISelectMonoBehaviour GetSelectMonoBehaviour()
        {
            return m_ThisMonoBehaviour;
        }

        /// <summary>
        /// 選択肢に加わるキャラクターのリスト
        /// </summary>
        static private List<BattleCharacter> SelectableCharacters(List<BattleCharacter> characters, BattleCharacter actioner, Target target)
        {
            switch(target)
            {
            case Target.Own:
                return new List<BattleCharacter>() { actioner };
            case Target.Friend:
            case Target.Friends:
                return characters.FindAll(c => c is FriendBattleCharacter);
            case Target.Enemy:
            case Target.Enemies:
                return characters.FindAll(c => c is EnemyBattleCharacter);
            case Target.All:
                return characters;
            }
            Debug.LogError($"Target({target}) is out of range.");
            return new List<BattleCharacter>();
        }

        /// <summary>
        /// 選択肢の数
        /// </summary>
        static private int GetSelectNum(List<BattleCharacter> characters, Target target)
        {
            switch(target)
            {
            case Target.Own:
            case Target.Friends:
            case Target.Enemies:
            case Target.All: return 1;
            case Target.Friend: return characters.Count(c => c is FriendBattleCharacter); ;
            case Target.Enemy: return characters.Count(c => c is EnemyBattleCharacter);
            }
            Debug.LogError($"Target({target}) is out of range.");
            return 0;
        }

        private bool IsActive(int index)
        {
            return Num == 1 || index == SelectIndex;
        }

        /// <summary>
        /// 対象を返します。
        /// </summary>
        public List<BattleCharacter> Targets()
        {
            List<BattleCharacter> targets = new List<BattleCharacter>();
            for(int i = 0; i < m_Order.Count; ++i)
            {
                if(m_Cursors[i].activeSelf)
                {
                    targets.Add(m_Order[i]);
                }
            }
            return targets;
        }

        private void SetWindow()
        {
            BattleCharacter target = m_Order[SelectIndex];
            m_Name.text = target.status.name;
            const float NAME_WINDOW_WITDH_MARGIN = 60;
            m_NameWindowRect.sizeDelta = new Vector2(m_Name.preferredWidth + NAME_WINDOW_WITDH_MARGIN, m_NameWindowRect.sizeDelta.y);
            const float POSITION_X = 250.0f;
            const float POSITION_Y = 215.0f;
            switch(target)
            {
            case FriendBattleCharacter c:
                m_NameWindowRect.position = new Vector3(POSITION_X, Screen.height - POSITION_Y, 0);
                return;
            case EnemyBattleCharacter c:
                m_NameWindowRect.position = new Vector3(Screen.width - POSITION_X, Screen.height - POSITION_Y, 0);
                return;
            }
            Debug.LogError($"BattleMember({target.status.name}) is out of range.");
        }

        private void UpdateCursorActive()
        {
            for(int i = 0; i < m_Cursors.Count; ++i)
            {
                m_Cursors[i].SetActive(IsActive(i));
            }
        }
    }
}
