using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Battle
{
    /// <summary>
    /// バトル中に表示する味方ステータス用UI
    /// </summary>
    public class FriendStatusUI : MonoBehaviour
    {
        /// <summary>
        /// 現在のHPのテキスト
        /// </summary>
        [SerializeField]
        public Text nowHPText = null;

        /// <summary>
        /// 最大のHPのテキスト
        /// </summary>
        [SerializeField]
        public Text maxHPText = null;

        /// <summary>
        /// HPゲージ
        /// </summary>
        [SerializeField]
        public BarGauge hpGauge = null;

        /// <summary>
        /// 現在のKPのテキスト
        /// </summary>
        [SerializeField]
        public Text nowKPText = null;

        /// <summary>
        /// 最大のKPのテキスト
        /// </summary>
        [SerializeField]
        public Text maxKPText = null;

        /// <summary>
        /// KPゲージ
        /// </summary>
        [SerializeField]
        public BarGauge kpGauge = null;

        /// <summary>
        /// APゲージ
        /// </summary>
        [SerializeField]
        public BarGauge apGauge = null;

        /// <summary>
        /// 顔アイコン
        /// </summary>
        [SerializeField]
        public Image icon = null;

        /// <summary>
        /// ステータスウィンドウ本体
        /// </summary>
        [SerializeField]
        public Image window = null;

        /// <summary>
        /// 指定した値で初期化します。
        /// </summary>
        public void Init(CharacterStatus status)
        {
            this.maxHPText.text = status.maxHP.ToString();
            this.nowHPText.text = status.NowHP.ToString();
            this.hpGauge.Set(status.maxHP, status.NowHP, 0.0f, BarGauge.AnimationType.None);
            this.maxKPText.text = status.maxKP.ToString();
            this.nowKPText.text = status.NowKP.ToString();
            this.kpGauge.Set(status.maxKP, status.NowKP, 0.0f, BarGauge.AnimationType.None);
            this.apGauge.Set(status.maxAP, status.NowAP, 0.0f, BarGauge.AnimationType.None);
            this.apGauge.waitTime = 0.25f;
            this.apGauge.changeTime = 0.25f;
            this.window.color = status.Color;
            this.SetIcon(status.spell);
        }

        /// <summary>
        /// 顔アイコンを設定します。
        /// </summary>
        /// <param name="characterNameSpell">キャラクター名の綴り（例：サーバルなら Serval）</param>
        public void SetIcon(string characterNameSpell)
        {
            icon.sprite = GameUtility.Load<Sprite>($"Image/Character/{characterNameSpell}/{characterNameSpell}_Icon");
        }
    }
}
