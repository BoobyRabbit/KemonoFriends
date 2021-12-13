using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Battle
{
    /// <summary>
    /// 攻撃を受けてもノーダメージだった時に表示するエフェクト
    /// </summary>
    public class NoDamageEffect : MonoBehaviour
    {
        /// <summary>
        /// エフェクト
        /// </summary>
        [SerializeField]
        public Image image = null;

        /// <summary>
        /// 指定した値でエフェクトを生成します。
        /// </summary>
        /// <param name="target">攻撃を受けたキャラクター</param>
        public void Instantiate(Transform parent, BattleCharacter target)
        {
            float moveTime = 0.5f;
            var effect = GameObject.Instantiate(this, parent);
            effect.transform.position = target.Bounds.center + new Vector3(0.0f, target.Bounds.extents.y, -1.0f);
            DOTween.Sequence()
                .Append(effect.transform.DORotate(new Vector3(0.0f, 0.0f, 360.0f), moveTime).SetRelative())
                .Join(effect.transform.DOMove(new Vector3(0.0f, 0.5f, 0.0f), moveTime).SetEase(Ease.OutQuart).SetRelative())
                .AppendCallback(() => GameObject.Destroy(effect.gameObject));
        }
    }
}
