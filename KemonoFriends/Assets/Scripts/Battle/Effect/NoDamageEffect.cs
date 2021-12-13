using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Battle
{
    /// <summary>
    /// �U�����󂯂Ă��m�[�_���[�W���������ɕ\������G�t�F�N�g
    /// </summary>
    public class NoDamageEffect : MonoBehaviour
    {
        /// <summary>
        /// �G�t�F�N�g
        /// </summary>
        [SerializeField]
        public Image image = null;

        /// <summary>
        /// �w�肵���l�ŃG�t�F�N�g�𐶐����܂��B
        /// </summary>
        /// <param name="target">�U�����󂯂��L�����N�^�[</param>
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
