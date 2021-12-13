namespace Battle.State
{
    /// <summary>
    /// 勝利
    /// </summary>
    public class Victory : BattleState
    {
        public Victory(BattleSceneEntry battleAccessor) : base(battleAccessor)
        {
        }

        public override BattleState Execute()
        {
            return null;
        }
    }
}
