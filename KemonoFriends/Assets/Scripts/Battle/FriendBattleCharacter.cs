using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    /// <summary>
    /// バトル中に使用する味方キャラクター
    /// </summary>
    public class FriendBattleCharacter : BattleCharacter
    {
        /// <summary>
        /// ステータス用UI
        /// </summary>
        [SerializeField]
        public FriendStatusUI statusUI = null;

        /// <summary>
        /// アクション用ボタン名
        /// </summary>
        [HideInInspector]
        public string buttonName = null;

        /// <summary>
        /// アクション用ボタンを押下した時間
        /// </summary>
        private Count buttonCount = new Count();

        /// <summary>
        /// キャラクターに対応したアクションボタンを押してからの経過時間
        /// </summary>
        public float ButtonDownTime => this.buttonCount.PassTime;

        public override void AddHP(int value, BarGauge.AnimationType animationType)
        {
            this.statusUI.hpGauge.Set(this.status.maxHP, this.status.NowHP, value, animationType);
            this.status.NowHP += value;
            this.statusUI.nowHPText.text = this.status.NowHP.ToString();
        }

        public override void AddKP(int value, BarGauge.AnimationType animationType)
        {
            this.statusUI.hpGauge.Set(this.status.maxKP, this.status.NowKP, value, animationType);
            this.status.NowKP += value;
            this.statusUI.nowKPText.text = this.status.NowKP.ToString();
        }

        public override void AddAP(float value, BarGauge.AnimationType animationType)
        {
            this.statusUI.apGauge.Set(this.status.maxAP, this.status.NowAP, value, animationType);
            this.status.NowAP += value;
        }

        /// <summary>
        /// 味方キャラクターを生成します。
        /// </summary>
        /// <param name="prefab">生成する味方キャラクターの元となるプレハブ</param>
        /// <param name="status">味方キャラクターのステータス</param>
        /// <param name="statusUIParent">ステータスUIの親</param>
        /// <param name="actionButtonName">味方のアクション用ボタンの名前</param>
        /// <returns></returns>
        public static FriendBattleCharacter Instantiate(FriendBattleCharacter prefab, CharacterStatus status, Transform statusUIParent, string actionButtonName)
        {
            var instance = GameObject.Instantiate(prefab);
            instance.status = status;
            instance.name = status.name;
            instance.group = CharacterGroup.Friend;
            instance.buttonName = actionButtonName;
            instance.statusUI.Init(status);
            instance.statusUI.transform.SetParent(statusUIParent);
            instance.statusUI.transform.SetSiblingIndex(0);
            instance.statusUI.transform.localScale = Vector3.one;
            instance.UpdateTexture();
            return instance;
        }

        /// <summary>
        /// フレンズ毎に設定されたアクション用ボタンの押下状況を更新します。
        /// </summary>
        public void UpdateButton()
        {
            this.buttonCount.Update();
            if(Input.GetButtonDown(this.buttonName))
            {
                this.buttonCount.Start();
            }
        }
    }
}
