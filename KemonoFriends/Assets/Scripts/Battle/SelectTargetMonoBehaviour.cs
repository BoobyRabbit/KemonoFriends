using System;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public class SelectTargetMonoBehaviour : SelectMonoBehaviour<SelectTarget>
    {
        /// <summary>
        /// カーソル
        /// </summary>
        [SerializeField]
        private GameObject m_CursorPrefab = null;

        /// <summary>
        /// 名前ウィンドウ
        /// </summary>
        [SerializeField]
        private GameObject m_NameWindowObject = null;

        private void Awake()
        {
            gameObject.SetActive(false);
        }

        public void Init(List<BattleCharacter> characters, BattleCharacter actioner, Target target, Action desision)
        {
            gameObject.SetActive(true);
            Body = new SelectTarget(characters, actioner, target, desision, m_CursorPrefab, m_NameWindowObject, this);
        }
    }
}
