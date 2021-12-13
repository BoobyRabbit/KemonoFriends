using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public class SelectBattleIconMonoBehaviour : SelectMonoBehaviour<SelectBattleIcon>
    {
        /// <summary>
        /// バトルアイコンのオブジェクトのリスト
        /// </summary>
        [SerializeField]
        private List<GameObject> m_BattleIcons = new List<GameObject>();

        private void Awake()
        {
            gameObject.SetActive(false);
        }

        /// <summary>
        /// 初期化します。
        /// </summary>
        /// <param name="battleIcons">選択中のキャラクター</param>
        public void Init(Character battleCharacter)
        {
            gameObject.SetActive(true);
            Body = new SelectBattleIcon(m_BattleIcons, battleCharacter, this);
        }
    }
}
