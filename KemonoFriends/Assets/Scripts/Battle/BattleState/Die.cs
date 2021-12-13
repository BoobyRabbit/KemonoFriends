using UnityEngine;

namespace Battle.State
{
    /// <summary>
    /// 敵味方問わずキャラクターが死亡した時の状態
    /// </summary>
    public class Die : BattleState
    {
        public Die(BattleSceneEntry battleAccessor) : base(battleAccessor)
        {
            foreach(var battleCharacter in this.Acr.BattleCharacters)
            {
                if(battleCharacter.status.NowHP <= 0)
                {
                    switch(battleCharacter)
                    {
                    case FriendBattleCharacter friend:
                        GameObject.Destroy(friend.gameObject);
                        break;
                    case EnemyBattleCharacter enemy:
                        GameObject.Destroy(enemy.statusUI.gameObject);
                        GameObject.Destroy(enemy.gameObject);
                        break;
                    }
                }
            }
            this.Acr.BattleCharacters.RemoveAll(c => c.status.NowHP <= 0);
        }

        public override BattleState Execute()
        {
            return new Wait(this.Acr);
        }
    }
}
