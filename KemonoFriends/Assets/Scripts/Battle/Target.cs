using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    /// <summary>
    /// 攻撃やアイテムなどの使用対象
    /// </summary>
    public enum Target
    {
        Own, // 自分自身
        Friend, // 味方１体
        Friends, // 味方全員
        Enemy, // 敵１体
        Enemies, // 敵全員
        All, // 全員
    }

    public class GetTarget
    {
        /// <summary>
        /// アクション(攻撃など)の対象キャラクターのリストを作成します。
        /// </summary>
        /// <param name="target">対象</param>
        /// <param name="actioner">アクション(攻撃など)を行うキャラクター</param>
        /// <param name="battleCharacters">バトル中の全キャラクターのリスト</param>
        static public List<BattleCharacter> List(Target target, BattleCharacter actioner, List<BattleCharacter> battleCharacters)
        {
            switch(target)
            {
            case Target.Own: return battleCharacters.FindAll(b => b == actioner);
            case Target.Friend: return Ally(actioner, battleCharacters);
            case Target.Friends: return Allies(actioner, battleCharacters);
            case Target.Enemy: return Enemy(actioner, battleCharacters);
            case Target.Enemies: return Enemies(actioner, battleCharacters);
            case Target.All: return battleCharacters;
            }
            Debug.LogError($"target = {target} is out of range.");
            return new List<BattleCharacter>();
        }

        /// <summary>
        /// 味方を１人ランダムで返します。
        /// 実装上の都合でリストが返りますが中身は味方１人です。
        /// </summary>
        static public List<BattleCharacter> Ally(BattleCharacter actioner, List<BattleCharacter> battleCharacters)
        {
            List<BattleCharacter> alliesTarget = Allies(actioner, battleCharacters);
            int targetIndex = UnityEngine.Random.Range(0, alliesTarget.Count - 1);
            List<BattleCharacter> allyTarget = new List<BattleCharacter>();
            allyTarget.Add(alliesTarget[targetIndex]);
            return allyTarget;
        }

        /// <summary>
        /// 味方全員を返します。
        /// </summary>
        static public List<BattleCharacter> Allies(BattleCharacter actioner, List<BattleCharacter> battleCharacters)
        {
            switch(actioner)
            {
            case FriendBattleCharacter friend:
                return battleCharacters.FindAll(b => b is FriendBattleCharacter);
            case EnemyBattleCharacter enemy:
                return battleCharacters.FindAll(b => b is EnemyBattleCharacter);
            }
            Debug.LogError($"actioner type({actioner.GetType()}) is invalid.");
            return new List<BattleCharacter>();
        }

        /// <summary>
        /// 敵を１人ランダムで返します。
        /// 実装上の都合でリストが返りますが中身は敵１人です。
        /// </summary>
        static public List<BattleCharacter> Enemy(BattleCharacter actioner, List<BattleCharacter> battleCharacterList)
        {
            List<BattleCharacter> enemiesTarget = Enemies(actioner, battleCharacterList);
            int targetIndex = UnityEngine.Random.Range(0, enemiesTarget.Count - 1);
            List<BattleCharacter> allyTarget = new List<BattleCharacter>();
            allyTarget.Add(enemiesTarget[targetIndex]);
            return allyTarget;

        }

        /// <summary>
        /// 敵全員を返します。
        /// </summary>
        static public List<BattleCharacter> Enemies(BattleCharacter actioner, List<BattleCharacter> battleCharacters)
        {
            switch(actioner)
            {
            case FriendBattleCharacter c:
                return battleCharacters.FindAll(b => b is EnemyBattleCharacter);
            case EnemyBattleCharacter c:
                List<BattleCharacter> enemiesTarget = new List<BattleCharacter>();
                // プレイヤーの仲間の中から最も左にいるキャラ（つまりプレイヤーを守っているキャラ）が生存しているならターゲットにプレイヤーを含む
                // そもそも仲間がいない(null)場合もプレイヤーをターゲットに含む
                List<BattleCharacter> enemies = battleCharacters.FindAll(b => b is FriendBattleCharacter);
                BattleCharacter guardian = null;
                foreach(BattleCharacter enemy in enemies)
                {
                    if(guardian == null || guardian.transform.position.x < enemy.transform.position.x)
                    {
                        guardian = enemy;
                    }
                }
                if(guardian == null || guardian.status.NowHP <= 0)
                {
                    // @todo プレイヤーをターゲットにしてください。
                }
                // 生存しているプレイヤーの仲間もターゲットに含む
                enemiesTarget.AddRange(battleCharacters.FindAll(b => b is FriendBattleCharacter && b.status.NowHP > 0));
                return enemiesTarget;
            }
            Debug.LogError($"actioner type({actioner.GetType()}) is invalid.");
            return new List<BattleCharacter>();
        }
    }
}