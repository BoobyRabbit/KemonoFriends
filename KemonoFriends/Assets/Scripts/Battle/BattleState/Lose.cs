namespace Battle.State
{
    /// <summary>
    /// 敗北
    /// </summary>
    public class Lose : BattleState
    {
        public Lose(BattleSceneEntry battleAccessor) : base(battleAccessor)
        {
        }

        public override BattleState Execute()
        {
            return null;
        }
    }
}
