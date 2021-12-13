using Battle.ActonCommand;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

namespace Battle.State
{
    public class UseItem : BattleState
    {
        /// <summary>
        /// 実行中のアクションのリスト
        /// </summary>
        private List<Sequence> sequences = new List<Sequence>();

        /// <summary>
        /// 指定した値で初期化します。
        /// </summary>
        public UseItem(BattleSceneEntry battleAccessor, ItemMasterItem item, BattleCharacter actioner, List<BattleCharacter> targets) : base(battleAccessor)
        {
            ItemActionCommand command = GetItemCommand(item);
            if(command != null)
            {
                command.Init(item, this.Acr.RecoveryEffectPrefab, this.Acr.WorldCanvas.transform, actioner, targets);
                command.Execute();
                command.Finalize(ref this.sequences);
                actioner.status.NowAP -= item.useAP;
            }
        }

        public override BattleState Execute()
        {
            this.sequences.RemoveAll(i => !i.IsActive());
            if(this.sequences.Count == 0)
            {
                return new Wait(this.Acr);
            }
            return null;
        }

        private ItemActionCommand GetItemCommand(ItemMasterItem itemParameter)
        {
            switch(itemParameter.name)
            {
            case "ジャパリまんじゅう": return new Recover();
            case "とくせいジャパリまんじゅう": return new Recover();
            case "こうきゅうジャパリまんじゅう": return new Recover();
            }
            Debug.LogError($"{itemParameter.name} is not found.");
            return null;
        }
    }
}
