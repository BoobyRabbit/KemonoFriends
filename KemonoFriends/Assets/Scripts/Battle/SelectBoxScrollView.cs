using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Battle
{
    /// <summary>
    /// 左右にテキストがついたセレクトボックス用
    /// </summary>
    public struct SelectBoxText
    {
        public string leftText;
        public string rightText;
    }

    /// <summary>
    /// 選択肢
    /// </summary>
    public class SelectBoxScrollView : Select
    {
        /// <summary>
        /// 選択肢用のボックス１つ分
        /// </summary>
        private GameObject m_SelectBoxPrefab = null;

        /// <summary>
        /// 選択肢のリスト
        /// </summary>
        private List<GameObject> m_SelectBoxes = new List<GameObject>();

        /// <summary>
        /// 選択肢の数
        /// </summary>
        public int SelectNum { get { return m_SelectBoxes.Count; } }

        /// <summary>
        /// カーソル移動時のSEを管理するオーディオ
        /// </summary>
        private AudioSource m_AudioSource = null;

        /// <summary>
        /// セレクトボックスの親オブジェクト
        /// </summary>
        private GameObject m_Content = null;

        /// <summary>
        /// 選択肢のリストを生成する関数
        /// </summary>
        private Action m_CreateSelectBoxes = null;

        /// <summary>
        /// 自身の MonoBehaviour の参照
        /// </summary>
        private SelectBoxScrollViewMonoBehaviour m_ThisMonoBehaviour = null;

        public SelectBoxScrollView(int selectNum, GameObject selectBoxPrefab, Action createSelectBoxes, SelectBoxScrollViewMonoBehaviour thisMonoBehaviour)
            : base(selectNum, SelectType.Vertical)
        {
            m_CreateSelectBoxes = createSelectBoxes;
            m_ThisMonoBehaviour = thisMonoBehaviour;
            m_SelectBoxPrefab = selectBoxPrefab;
            m_AudioSource = m_ThisMonoBehaviour.GetComponent<AudioSource>();
            m_AudioSource.Stop();
            m_Content = thisMonoBehaviour.transform.Find("Viewport").Find("Content").gameObject;
        }

        protected override void Enter()
        {
            m_CreateSelectBoxes();
            UpdateFrame();
        }

        protected override void Exit()
        {
            foreach(var selectBox in m_SelectBoxes)
            {
                GameObject.Destroy(selectBox);
            }
            m_SelectBoxes.Clear();
        }

        protected override void Next()
        {
            m_AudioSource.Play();
            UpdateFrame();
        }

        protected override void Previous()
        {
            m_AudioSource.Play();
            UpdateFrame();
        }

        protected override void Decision() { }

        protected override void Cancel() { }

        public override void Update() { }

        protected override ISelectMonoBehaviour GetSelectMonoBehaviour()
        {
            return m_ThisMonoBehaviour;
        }

        /// <summary>
        /// 選択肢を設定します。
        /// </summary>
        public void SetTexts(List<SelectBoxText> selectBoxTexts)
        {
            for(int i = 0; i < selectBoxTexts.Count; ++i)
            {
                SelectBoxText selectBoxText = selectBoxTexts[i];
                GameObject selectBoxTextObject = GameObject.Instantiate(m_SelectBoxPrefab);
                selectBoxTextObject.name = $"SelectBox{m_SelectBoxes.Count}";
                selectBoxTextObject.transform.Find("LeftText").GetComponent<Text>().text = selectBoxText.leftText;
                selectBoxTextObject.transform.Find("RightText").GetComponent<Text>().text = selectBoxText.rightText;
                selectBoxTextObject.transform.SetParent(m_Content.transform);
                selectBoxTextObject.transform.localScale = Vector3.one;
                RectTransform rectTransform = selectBoxTextObject.GetComponent<RectTransform>();
                rectTransform.anchorMin = new Vector2(0.5f, 1.0f);
                rectTransform.anchorMax = new Vector2(0.5f, 1.0f);
                rectTransform.anchoredPosition = new Vector3(0, -40 * (i + 1), 0);
                m_SelectBoxes.Add(selectBoxTextObject);
            }
            UpdateFrame();
        }

        /// <summary>
        /// 選択中のセレクトボックスのフレームの色を変えます
        /// </summary>
        private void UpdateFrame()
        {
            for(int i = 0; i < m_SelectBoxes.Count; ++i)
            {
                Image image = m_SelectBoxes[i].transform.Find("Frame").GetComponent<Image>();
                image.color = SelectIndex == i ? Color.yellow : Color.gray;
            }
        }

        /// <summary>
        /// 選択肢を削除します
        /// </summary>
        private void Clear()
        {
            foreach(GameObject selectBox in m_SelectBoxes)
            {
                GameObject.Destroy(selectBox);
            }
            m_SelectBoxes.Clear();
        }
    }
}
