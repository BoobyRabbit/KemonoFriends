using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Battle
{
    /// <summary>
    /// �A�N�V�������ɕ\������]���p�����̃G�t�F�N�g
    /// Miss, Nice �Ȃǂ̕������\������܂��B
    /// </summary>
    public class ActionRateEffect : MonoBehaviour
    {
        /// <summary>
        /// �G�t�F�N�g
        /// </summary>
        [SerializeField]
        public Image image = null;

        /// <summary>
        /// �A�N�V�������s�A�������ɕ\�����镶��
        /// </summary>
        [SerializeField]
        public Sprite[] effectList = null;

        /// <summary>
        /// �G�t�F�N�g�𐶐����܂��B
        /// </summary>
        /// <param name="target">�Ώۂ̃L�����N�^�[</param>
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
