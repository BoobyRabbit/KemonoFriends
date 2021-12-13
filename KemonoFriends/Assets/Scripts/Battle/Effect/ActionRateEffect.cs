using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Battle
{
    /// <summary>
    /// アクション時に表示する評価用文字のエフェクト
    /// Miss, Nice などの文字が表示されます。
    /// </summary>
    public class ActionRateEffect : MonoBehaviour
    {
        /// <summary>
        /// エフェクト
        /// </summary>
        [SerializeField]
        public Image image = null;

        /// <summary>
        /// アクション失敗、成功時に表示する文字
        /// </summary>
        [SerializeField]
        public Sprite[] effectList = null;

        /// <summary>
        /// エフェクトを生成します。
        /// </summary>
        /// <param name="target">対象のキャラクター</param>
        public void Instantiate(Transform parent, BattleCharacter target, ActionRate rate)
        {
            var effect = GameObject.Instantiate(this, parent);
            effect.transform.position = target.Bounds.center + new Vector3(0.0f, target.Bounds.extents.y, -2.0f);
            DOTween.Sequence()
                .Append(effect.transform.DOMove(new Vector3(0.0f, 0.5f, 0.0f), 0.5f).SetEase(Ease.OutQuart).SetRelative())
                .AppendInterval(0.5f)
                .AppendCallback(() => GameObject.Destroy(effect.gameObject));
            var sprite = this.effectList[(int)rate];
            effect.image.sprite = sprite;
            effect.image.GetComponent<RectTransform>().sizeDelta = new Vector2(sprite.texture.width, sprite.texture.height);
        }
    }
}
