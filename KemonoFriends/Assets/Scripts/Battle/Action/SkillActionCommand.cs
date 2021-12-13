using System.Collections.Generic;
using UnityEngine;

namespace Battle.ActonCommand
{
    /// <summary>
    /// スキル用コマンド
    /// </summary>
    public abstract class SkillActionCommand : ActionCommand
    {
        /// <summary>
        /// タイミング押下系の攻撃コマンドが成功するボタン押下タイミングの時間(秒)
        /// 1.0f ならボタン押下から 1.0f までに攻撃を与えれば攻撃コマンドが成功する事になる。
        /// </summary>
        public static readonly float attackActionTime = 0.1f;

        /// <summary>
        /// 防御コマンドが成功するボタン押下タイミングの時間(秒)
        /// 1.0f ならボタン押下から 1.0f までに攻撃を受ければ防御コマンドが成功する事になる。
        /// </summary>
        public static readonly float defenceActionTime = 0.1f;

        /// <summary>
        /// 使用するスキル
        /// </summary>
        protected SkillMasterItem skill;

        /// <summary>
        /// ダメージ用エフェクトのプレハブ
        /// </summary>
        private ChangeStatusEffect damageEffectPrefab = null;

        /// <summary>
        /// ノーダメージ時に表示するエフェクトのプレハブ
        /// </summary>
        private NoDamageEffect noDamageEffectPrefab = null;

        /// <summary>
        /// アクション時に表示する評価用文字のエフェクトのプレハブ
        /// </summary>
        private ActionRateEffect actionRatingEffectPrefab = null;

        /// <summary>
        /// エフェクトの親
        /// </summary>
        private Transform effectParent = null;

        /// <summary>
        /// 指定した値で初期化します。
        /// </summary>
        public void Init(SkillMasterItem skill, ChangeStatusEffect damageEffectPrefab, NoDamageEffect noDamageEffectPrefab, ActionRateEffect actionRatingEffectPrefab, Transform effectParent, BattleCharacter actioner, List<BattleCharacter> targets)
        {
            this.skill = skill;
            this.damageEffectPrefab = damageEffectPrefab;
            this.noDamageEffectPrefab = noDamageEffectPrefab;
            this.actionRatingEffectPrefab = actionRatingEffectPrefab;
            this.effectParent = effectParent;
            this.Init(actioner, targets);
        }

        /// <summary>
        /// ダメージを与えます。
        /// </summary>
        /// <param name="target">ダメージを与える対象</param>
        /// <param name="animationType">HPゲージ増減用アニメーションの種類</param>
        /// <returns>与えたダメージ</returns>
        public int Damage(BattleCharacter target, BarGauge.AnimationType animationType, ActionRate actionRate)
        {
            int damage = this.actioner.status.attack - target.status.deffence;
            var friendActioner = this.actioner as FriendBattleCharacter;
            if(friendActioner != null && friendActioner.ButtonDownTime <= attackActionTime)
            {
                damage += 1;
                this.actionRatingEffectPrefab.Instantiate(this.effectParent, this.actioner, actionRate);
            }
            // 防御アクション成功によるダメージ変動
            var friendTarget = target as FriendBattleCharacter;
            if(friendTarget != null && friendTarget.ButtonDownTime <= defenceActionTime)
            {
                damage -= 1;
                this.actionRatingEffectPrefab.Instantiate(this.effectParent, target, actionRate);
            }
            // ダメージは０を下回らないようにしておく
            damage = Mathf.Max(damage, 0);

            if(damage == 0)
            {
                this.noDamageEffectPrefab.Instantiate(this.effectParent, target);
            }
            else
            {
                this.damageEffectPrefab.Instantiate(this.effectParent, target, damage);
                target.AddHP(-damage, animationType);
            }
            return damage;
        }
    }
}
