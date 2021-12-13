using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Battle
{
    /// <summary>
    /// HP、KPの回復、ダメージで発生するエフェクト
    /// </summary>
    public class ChangeStatusEffect : MonoBehaviour
    {
        /// <summary>
        /// エフェクト
        /// </summary>
        [SerializeField]
        public Image image = null;

        /// <summary>
        /// 回復、ダメージの量のテキスト
        /// </summary>
        [SerializeField]
        public Text num = null;

        /// <summary>
        /// 指定した値でエフェクトを生成します。
        /// </summary>
        /// <param name="target">回復、ダメージの影響を受けたキャラクター</param>
        /// <param name="value">回復、ダメージの量</param>
        public void Instantiate(Transform parent, BattleCharacter target, int value)
        {
            float moveTime = 0.2f;
            var effect = GameObject.Instantiate(this, parent);
            effect.num.text = value.ToString();
            effect.transform.localScale = Vector3.one;
            effect.transform.position = target.Bounds.center + new Vector3(0.0f, target.Bounds.extents.y, -1.0f);
            DOTween.Sequence()
                .AppendInterval(1.0f)
                .Append(effect.transform.DOMoveY(1.0f, moveTime).SetEase(Ease.OutQuart).SetRelative())
                .Join(effect.transform.DOScale(0.0f, moveTime))
                .AppendCallback(() => GameObject.Destroy(effect.gameObject));
        }
    }
}
