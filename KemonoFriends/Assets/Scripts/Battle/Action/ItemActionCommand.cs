using System.Collections.Generic;
using UnityEngine;

namespace Battle.ActonCommand
{
    /// <summary>
    /// アイテム用コマンド
    /// </summary>
    public abstract class ItemActionCommand : ActionCommand
    {
        /// <summary>
        /// 使用するアイテム
        /// </summary>
        protected ItemMasterItem item;

        /// <summary>
        /// 回復用エフェクトのプレハブ
        /// </summary>
        private ChangeStatusEffect recoverEffectPrefab = null;

        /// <summary>
        /// エフェクトの親
        /// </summary>
        private Transform effectParent = null;

        /// <summary>
        /// 指定した値で初期化します。
        /// </summary>
        public void Init(ItemMasterItem itemParameter, ChangeStatusEffect recoverEffectPrefab, Transform effectParent, BattleCharacter actioner, List<BattleCharacter> targets)
        {
            this.item = itemParameter;
            this.recoverEffectPrefab = recoverEffectPrefab;
            this.effectParent = effectParent;
            this.Init(actioner, targets);
        }

        /// <summary>
        /// 回復します。
        /// </summary>
        public void Recover(int num, BattleCharacter target)
        {
            if(num > 0)
            {
                this.recoverEffectPrefab.Instantiate(this.effectParent, target, num);
                target.AddHP(num, BarGauge.AnimationType.Play);
            }
        }
    }
}
