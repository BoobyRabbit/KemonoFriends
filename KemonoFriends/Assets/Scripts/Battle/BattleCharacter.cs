namespace Battle
{
    /// <summary>
    /// バトル中に使用するキャラクター
    /// </summary>
    public abstract class BattleCharacter : Character
    {
        /// <summary>
        /// ステータス
        /// </summary>
        public CharacterStatus status = null;

        /// <summary>
        /// キャラクターの名前(ファイル名用)
        /// </summary>
        public override string Spell => this.status?.spell;

        /// <summary>
        /// キャラクターの所属先
        /// </summary>
        public CharacterGroup group = CharacterGroup.Enemy;

        /// <summary>
        /// HPを増減させます（Addと名付けていますがマイナスの値で減少します）
        /// </summary>
        /// <param name="value">増減値</param>
        /// <param name="animationType">ゲージ増減におけるアニメーションの種類</param>
        abstract public void AddHP(int value, BarGauge.AnimationType animationType);

        /// <summary>
        /// HPを増減させます（Addと名付けていますがマイナスの値で減少します）
        /// </summary>
        /// <param name="value">増減値</param>
        /// <param name="animationType">ゲージ増減におけるアニメーションの種類</param>
        abstract public void AddKP(int value, BarGauge.AnimationType animationType);

        /// <summary>
        /// HPを増減させます（Addと名付けていますがマイナスの値で減少します）
        /// </summary>
        /// <param name="value">増減値</param>
        /// <param name="animationType">ゲージ増減におけるアニメーションの種類</param>
        abstract public void AddAP(float value, BarGauge.AnimationType animationType);
    }

}
