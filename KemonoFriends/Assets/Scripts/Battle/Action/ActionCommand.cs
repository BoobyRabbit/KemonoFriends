using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle.ActonCommand
{
    /// <summary>
    /// スキルやアイテムの実行内容を Command パターンとして扱います。
    /// https://www.techscore.com/tech/DesignPattern/Command.html/
    /// </summary>
    public abstract class ActionCommand
    {
        /// <summary>
        /// このアクションのシークエンス
        /// </summary>
        protected Sequence sequence = null;

        /// <summary>
        /// このアクションを行うキャラクター
        /// </summary>
        protected BattleCharacter actioner = null;

        /// <summary>
        /// このアクションの（攻撃などの）対象
        /// </summary>
        protected List<BattleCharacter> targets = null;

        /// <summary>
        /// 指定した値で初期化します。
        /// </summary>
        protected void Init(BattleCharacter actioner, List<BattleCharacter> targets)
        {
            this.sequence = DOTween.Sequence();
            this.actioner = actioner;
            this.targets = targets;
        }

        /// <summary>
        /// アクションの実行内容
        /// </summary>
        public abstract void Execute();

        /// <summary>
        /// 終了時の共通処理
        /// </summary>
        public void Finalize(ref List<Sequence> sequences)
        {
            sequences.Add(sequence);
        }
    }
}
