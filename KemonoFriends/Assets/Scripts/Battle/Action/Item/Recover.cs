namespace Battle.ActonCommand
{
    /// <summary>
    /// 回復アイテムを使用する
    /// </summary>
    public class Recover : ItemActionCommand
    {
        public override void Execute()
        {
            foreach(var target in targets)
            {
                this.Recover(item.recoverHP, target);
            }
        }
    }
}
