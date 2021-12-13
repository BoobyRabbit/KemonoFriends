using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Battle
{
    /// <summary>
    /// 棒状のゲージを扱うクラス
    /// </summary>
    public class BarGauge : MonoBehaviour
    {
        /// <summary>
        /// 現在値のゲージ
        /// </summary>
        [SerializeField]
        private ImageFilled front = null;

        /// <summary>
        /// 増減時の差分を表すゲージ
        /// </summary>
        [SerializeField]
        private ImageFilled difference = null;

        /// <summary>
        /// 増加時の差分の色
        /// </summary>
        [SerializeField]
        public Color incleaseColor = Color.white;

        /// <summary>
        /// 減少時のゲージの色
        /// </summary>
        [SerializeField]
        public Color decreaseColor = Color.white;

        /// <summary>
        /// 増加・減少からアニメーション開始までの待機時間(秒)
        /// </summary>
        public float waitTime = 0.5f;

        /// <summary>
        /// 最大値から最小値（または逆）に変化する時にかかるアニメーション時間（秒）
        /// </summary>
        public float changeTime = 2.0f;

        /// <summary>
        /// 増減用アニメーションの種類
        /// </summary>
        public enum AnimationType
        {
            /// <summary>
            /// アニメーションなし
            /// 増減ゲージなし
            /// </summary>
            None,

            /// <summary>
            /// アニメ―ションなし
            /// 増減ゲージあり
            /// 連続攻撃における最後以外の攻撃に使用する想定
            /// </summary>
            Ready,

            /// <summary>
            /// アニメーションあり
            /// 増減ゲージあり
            /// 連続攻撃における最後の攻撃に使用する想定
            /// </summary>
            Play,
        }

        /// <summary>
        /// 値を増減させます。
        /// 最小値は0固定です。
        /// </summary>
        /// <param name="max">最大値</param>
        /// <param name="now">現在値</param>
        /// <param name="addValue">変更値</param>
        /// <param name="animationType">増減用アニメーションの種類</param>
        public void Set(float max, float now, float addValue, AnimationType animationType)
        {
            var from = Mathf.InverseLerp(0.0f, max, now);
            var to = Mathf.InverseLerp(0.0f, max, now + addValue);
            switch(animationType)
            {
            case AnimationType.None:
                this.front.FillAmount = to;
                this.difference.FillAmount = 0.0f;
                // @todo アニメーション中だった場合の処理が必要です。
                break;
            case AnimationType.Ready:
            case AnimationType.Play:
                ImageFilled first = null, second = null;
                Image differenceImage = this.difference.GetComponent<Image>();
                if(from > to)
                {
                    this.front.FillAmount = to;
                    this.difference.FillAmount = from;
                    differenceImage.color = decreaseColor;
                    first = this.front;
                    second = this.difference;
                }
                else if(from < to)
                {
                    this.front.FillAmount = from;
                    this.difference.FillAmount = to;
                    differenceImage.color = incleaseColor;
                    first = this.difference;
                    second = this.front;
                }
                else
                {
                    return;
                }
                // アニメーション開始
                if(animationType == AnimationType.Play)
                {
                    var animationTime = changeTime * Mathf.Abs(first.FillAmount - second.FillAmount);
                    DOTween.Sequence()
                        .AppendInterval(waitTime)
                        .Append(DOTween.To(() => second.FillAmount, x => { second.FillAmount = x; }, to, animationTime));
                }
                break;
            }
        }
    }
}
