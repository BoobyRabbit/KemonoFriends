using System;
using UnityEngine;

namespace Battle
{
    [RequireComponent(typeof(AudioSource))]
    public class SelectBoxScrollViewMonoBehaviour : SelectMonoBehaviour<SelectBoxScrollView>
    {
        /// <summary>
        /// 選択肢用のボックス１つ分
        /// </summary>
        [SerializeField]
        private GameObject m_SelectBoxPrefab = null;

        private void Awake()
        {
            gameObject.SetActive(false);
        }

        /// <summary>
        /// 初期化します。
        /// </summary>
        /// <param name="battleIcons">選択中のキャラクター</param>
        /// <param name="createSelectBoxes">選択肢のリストを生成する関数</param>
        public void Init(int selectNum, Action createSelectBoxes)
        {
            gameObject.SetActive(true);
            Body = new SelectBoxScrollView(selectNum, m_SelectBoxPrefab, createSelectBoxes, this);
        }
    }
}
