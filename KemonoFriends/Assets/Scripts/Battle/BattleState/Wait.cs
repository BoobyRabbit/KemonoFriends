using UnityEngine;

namespace Battle.State
{
    /// <summary>
    /// 戦闘中に待機している状態
    /// 他の様々な状態への遷移条件を満たしているかの確認もしています。
    /// </summary>
    public class Wait : BattleState
    {
        public Wait(BattleSceneEntry battleAccessor) : base(battleAccessor)
        {
        }

        public override BattleState Execute()
        {
            // 敗北判定
            if(!this.Acr.Friends.Exists(c => c.status.NowHP > 0))
            {
                return new Lose(this.Acr);
            }
            // 勝利判定
            if(!this.Acr.Enemies.Exists(c => c.status.NowHP > 0))
            {
                return new Victory(this.Acr);
            }
            // 死亡キャラがいるか判定
            if(this.Acr.BattleCharacters.Exists(b => b.status.NowHP <= 0))
            {
                return new Die(this.Acr);
            }
            // 行動可能なキャラクターの有無を確認
            var actioner = this.Actioner(false);
            if(actioner == null)
            {
                // 行動可能なキャラクターがいなければ全キャラクターのAPを増やす
                foreach(var battleCharacter in this.Acr.BattleCharacters)
                {
                    battleCharacter.AddAP(battleCharacter.status.speed * Time.deltaTime, BarGauge.AnimationType.None);
                }
            }
            else
            {
                // 行動可能なキャラクターがいれば選択状態に移る
                return new SelectAction(this.Acr, actioner);
            }
            return null;
        }

        /// <summary>
        /// アクション可能な最低限のAPを持ちつつ、APが最も高いキャラクターを返します。
        /// なければ null を返します。
        /// </summary>
        /// <param name="onNullCheck">null チェックするかどうか</param>
        public BattleCharacter Actioner(bool onNullCheck)
        {
            BattleCharacter actioner = null;
            foreach(var battleCharacter in this.Acr.BattleCharacters)
            {
                if(battleCharacter.status.CanAction && (actioner == null || battleCharacter.status.NowAP > actioner.status.NowAP))
                {
                    actioner = battleCharacter;
                }
            }
            if(onNullCheck && actioner == null)
            {
                Debug.LogError("actioner is null");
            }
            return actioner;
        }
    }
}
