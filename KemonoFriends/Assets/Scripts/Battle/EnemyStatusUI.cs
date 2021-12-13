using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Battle
{
    public class EnemyStatusUI : MonoBehaviour
    {
        /// <summary>
        /// HPのゲージ
        /// </summary>
        [SerializeField]
        public BarGauge hpGauge = null;

        /// <summary>
        /// APのゲージ
        /// </summary>
        [SerializeField]
        public BarGauge apGauge = null;

        /// <summary>
        /// HPテキスト
        /// </summary>
        [SerializeField]
        public Text nowHPText = null;

        /// <summary>
        /// 指定した値で初期化します。
        /// </summary>
        public void Init(CharacterStatus status)
        {
            this.nowHPText.text = status.NowHP.ToString();
            this.hpGauge.Set(status.maxHP, status.NowHP, 0.0f, BarGauge.AnimationType.None);
            this.apGauge.Set(status.maxAP, status.NowAP, 0.0f, BarGauge.AnimationType.None);
            this.apGauge.waitTime = 0.25f;
            this.apGauge.changeTime = 0.25f;
        }
    }
}
