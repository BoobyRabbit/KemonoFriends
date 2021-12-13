using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Battle.State
{
    /// <summary>
    /// バトルの初期化をします。
    /// </summary>
    public class Init : BattleState
    {
        /// <summary>
        /// バトル用フィールドのシーン
        /// </summary>
        private Scene fieldScene;

        public Init(BattleSceneEntry battleAccessor, string battleFieldSceneName, List<CharacterStatus> friendStatuses, List<CharacterStatus> enemyStatuses) : base(battleAccessor)
        {
            var loadSceneParameters = new LoadSceneParameters(LoadSceneMode.Additive);
            this.fieldScene = SceneManager.LoadScene(battleFieldSceneName, loadSceneParameters);
            // 味方キャラクターを生成します。
            foreach(var friendStatus in friendStatuses)
            {
                var index = friendStatuses.IndexOf(friendStatus);
                var actionButtonName = $"BattleAction{index + 1}";
                var friendBattleCharacter = FriendBattleCharacter.Instantiate(this.Acr.FriendBattleCharacterPrefab, friendStatus, this.Acr.FriendStatusUIParent, actionButtonName);
                friendBattleCharacter.transform.position = this.GetInitPosition(friendStatus, friendStatuses, true);
                this.Acr.BattleCharacters.Add(friendBattleCharacter);
            }
            // 敵キャラクターを生成します。
            foreach(var enemyStatus in enemyStatuses)
            {
                var enemyBattleCharacter = EnemyBattleCharacter.Instantiate(this.Acr.EnemyBattleCharacterPrefab, enemyStatus, this.Acr.EnemyStatusUIParent.transform);
                enemyBattleCharacter.SetPositionWithStatusUI(this.GetInitPosition(enemyStatus, enemyStatuses, false));
                this.Acr.BattleCharacters.Add(enemyBattleCharacter);
            }
            // 全キャラクターの初期APを設定します。
            // 行動必要なAPの1/4～3/4のAPを設定します。素早さの高いキャラクター程高いAPが設定されます。
            var minSpeed = this.Acr.BattleCharacters.Min(b => b.status.speed);
            var maxSpeed = this.Acr.BattleCharacters.Max(b => b.status.speed);
            foreach(var battleCharacter in this.Acr.BattleCharacters)
            {
                battleCharacter.status.NowAP = (CharacterStatus.needAP / 4.0f) + (CharacterStatus.needAP / 2.0f) * (1.0f - (maxSpeed - battleCharacter.status.speed) / (maxSpeed - minSpeed));
            }
        }

        public override BattleState Execute()
        {
            if(this.fieldScene.isLoaded)
            {
                return new Wait(this.Acr);
            }
            return null;
        }

        /// <summary>
        /// 初期座標を取得します。
        /// </summary>
        private Vector3 GetInitPosition(CharacterStatus character, List<CharacterStatus> characters, bool isFriend)
        {
            var initPosX = 4.0f;
            var distance = 2.0f;
            var index = characters.IndexOf(character);
            var x = initPosX + distance * index;
            var z = index % 2 == 1 ? 1.0f : 0.0f;
            if(isFriend)
            {
                x *= -1;
            }
            return new Vector3(x, 0.0f, z);
        }
    }
}
