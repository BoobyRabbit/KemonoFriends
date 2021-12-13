using Battle.ActonCommand;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

namespace Battle.State
{
    /// <summary>
    /// スキルを実行する状態
    /// </summary>
    public class PlaySkill : BattleState
    {
        /// <summary>
        /// 実行中のアクションのリスト
        /// </summary>
        private List<Sequence> sequences = new List<Sequence>();

        /// <summary>
        /// 指定した値で初期化します。
        /// </summary>
        public PlaySkill(BattleSceneEntry battleAccessor, SkillMasterItem skill, BattleCharacter actioner, List<BattleCharacter> targets) : base(battleAccessor)
        {
            SkillActionCommand command = GetSkillCommand(skill);
            if(command != null)
            {
                command.Init(skill, this.Acr.DamageEffectPrefab, this.Acr.NoDamageEffectPrefab, this.Acr.ActionRatingPrefab, this.Acr.WorldCanvas.transform, actioner, targets);
                command.Execute();
                command.Finalize(ref this.sequences);
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

        private SkillActionCommand GetSkillCommand(SkillMasterItem skillParameter)
        {
            switch(skillParameter.name)
            {
            case "テスト": return new DummySkill();
            case "ひっかく": return new Scratch();
            case "つよくひっかく": return new Scratch();
            }
            Debug.LogError($"{skillParameter.name} is not found.");
            return null;
        }
    }
}