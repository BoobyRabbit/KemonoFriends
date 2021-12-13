using System.Collections.Generic;
using UnityEngine;

namespace Battle.State
{
    /// <summary>
    /// 行動可能なキャラクターがアクションを選択します。
    /// 味方の場合はプレイヤーに操作を促し、
    /// 敵の場合はAIに自動でアクションを選択させます。
    /// </summary>
    public class SelectAction : BattleState
    {
        /// <summary>
        /// 行動選択中のキャラクター
        /// </summary>
        private BattleCharacter actioner = null;

        /// <summary>
        /// 使用するスキル
        /// </summary>
        private SkillMasterItem? skill = null;

        /// <summary>
        /// 使用するアイテム
        /// </summary>
        private ItemMasterItem? item = null;

        /// <summary>
        /// スキルやアイテムの対象
        /// </summary>
        private List<BattleCharacter> targets = null;

        public SelectAction(BattleSceneEntry battleAccessor, BattleCharacter actioner) : base(battleAccessor)
        {
            this.actioner = actioner;
            switch(this.actioner)
            {
            case FriendBattleCharacter friend:
                this.Acr.SelectBattleIcon.Init(this.actioner);
                this.SetSelectButton(this.Acr.SelectBattleIcon.Body, this.actioner);
                this.Acr.SelectBattleIcon.SetNextSelectGetter((int)BattleIconType.Attack, GetAttackSelect);
                this.Acr.SelectBattleIcon.SetNextSelectGetter((int)BattleIconType.Item, GetItemSelect);
                break;
            case EnemyBattleCharacter enemy:
                List<string> skillList = this.actioner.status.skillNameList;
                int skillIndex = UnityEngine.Random.Range(0, skillList.Count - 1);
                this.skill = SkillMaster.Instance[skillList[skillIndex]];
                this.targets = GetTarget.List(this.skill.Value.target, this.actioner, this.Acr.BattleCharacters);
                break;
            }
        }

        public override BattleState Execute()
        {
            if(this.skill.HasValue)
            {
                this.actioner.AddAP(-this.skill.Value.useAP, BarGauge.AnimationType.Play);
                return new PlaySkill(this.Acr, this.skill.Value, this.actioner, this.targets);
            }
            if(this.item.HasValue)
            {
                this.actioner.AddAP(-this.item.Value.useAP, BarGauge.AnimationType.Play);
                return new UseItem(this.Acr, this.item.Value, this.actioner, this.targets);
            }
            this.Acr.SelectBattleIcon.UpdateSelect();
            return null;
        }

        /// <summary>
        /// 選択肢用のボタンを設定します。
        /// </summary>
        private void SetSelectButton(Select select, BattleCharacter battleCharacter)
        {
            var friendBattleCharacter = battleCharacter as FriendBattleCharacter;
            if(friendBattleCharacter != null)
            {
                select.decisionButtonName = friendBattleCharacter.buttonName;
                select.cancelButtonName = "Cancel";
            }
        }

        /// <summary>
        /// スキル用の選択肢クラスを取得します。
        /// </summary>
        private Select GetAttackSelect(Select select)
        {
            this.Acr.SelectBoxScrollView.Init(this.actioner.status.skillNameList.Count, CreateAttackSelectBoxes);
            this.SetSelectButton(this.Acr.SelectBoxScrollView.Body, this.actioner);
            for(int i = 0; i < this.actioner.status.skillNameList.Count; ++i)
            {
                this.Acr.SelectBoxScrollView.SetNextSelectGetter(i, GetSkillTargetSelect);
            }
            return this.Acr.SelectBoxScrollView.Body;
        }

        /// <summary>
        /// スキル用の選択肢の表示物を生成します。
        /// </summary>
        private void CreateAttackSelectBoxes()
        {
            List<SelectBoxText> selectBoxTexts = new List<SelectBoxText>();
            for(int i = 0; i < this.actioner.status.skillNameList.Count; ++i)
            {
                string skillName = this.actioner.status.skillNameList[i];
                SkillMasterItem skillParameter = SkillMaster.Instance[skillName];
                SelectBoxText selectBoxText = new SelectBoxText();
                selectBoxText.leftText = skillParameter.name;
                selectBoxTexts.Add(selectBoxText);
                this.Acr.SelectBoxScrollView.SetNextSelectGetter(i, GetSkillTargetSelect);
            }
            this.Acr.SelectBoxScrollView.Body.SetTexts(selectBoxTexts);
        }

        /// <summary>
        /// アイテム用の選択肢クラスを取得します。
        /// </summary>
        private Select GetItemSelect(Select select)
        {
            this.Acr.SelectBoxScrollView.Init(GameManager.Instance.HaveItems.Count, CreateItemSelectBoxes);
            this.SetSelectButton(this.Acr.SelectBoxScrollView.Body, this.actioner);
            for(int i = 0; i < GameManager.Instance.HaveItems.Count; ++i)
            {
                this.Acr.SelectBoxScrollView.SetNextSelectGetter(i, GetItemTargetSelect);
            }
            return this.Acr.SelectBoxScrollView.Body;
        }

        /// <summary>
        /// アイテム用の選択肢の表示物を生成します。
        /// </summary>
        private void CreateItemSelectBoxes()
        {
            List<SelectBoxText> selectBoxTexts = new List<SelectBoxText>();
            for(int i = 0; i < GameManager.Instance.HaveItems.Count; ++i)
            {
                ItemMasterItem itemParameter = GameManager.Instance.HaveItems[i];
                SelectBoxText selectBoxText = new SelectBoxText();
                selectBoxText.leftText = itemParameter.name;
                selectBoxTexts.Add(selectBoxText);
            }
            this.Acr.SelectBoxScrollView.Body.SetTexts(selectBoxTexts);
        }

        /// <summary>
        /// スキル使用によるターゲットの選択クラスを取得
        /// </summary>
        private Select GetSkillTargetSelect(Select select)
        {
            SkillMasterItem skillParameter = SkillMaster.Instance[this.actioner.status.skillNameList[select.SelectIndex]];
            this.Acr.SelectTarget.Init(this.Acr.BattleCharacters, this.actioner, skillParameter.target, DecisionAttack);
            this.SetSelectButton(this.Acr.SelectTarget.Body, this.actioner);
            return this.Acr.SelectTarget.Body;
        }

        /// <summary>
        /// アイテム使用によるターゲットの選択クラスを取得
        /// </summary>
        private Select GetItemTargetSelect(Select select)
        {
            var item = GameManager.Instance.HaveItems[select.SelectIndex];
            this.Acr.SelectTarget.Init(this.Acr.BattleCharacters, this.actioner, item.target, DecisionItem);
            this.SetSelectButton(this.Acr.SelectTarget.Body, this.actioner);
            return this.Acr.SelectTarget.Body;
        }

        /// <summary>
        /// スキル決定時の動作
        /// </summary>
        private void DecisionAttack()
        {
            string skillName = this.actioner.status.skillNameList[this.Acr.SelectBoxScrollView.Body.SelectIndex];
            this.skill = SkillMaster.Instance[skillName];
            this.targets = this.Acr.SelectTarget.Body.Targets();
        }

        /// <summary>
        /// アイテム決定時の動作
        /// </summary>
        private void DecisionItem()
        {
            this.item = GameManager.Instance.HaveItems[this.Acr.SelectBoxScrollView.Body.SelectIndex];
            this.targets = this.Acr.SelectTarget.Body.Targets();
        }
    }
}
