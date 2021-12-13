namespace Battle.State
{
    /// <summary>
    /// バトルシーンの状態
    /// 開始演出、終了演出などバトルシーンには様々な状態があります。
    /// 各状態における初期化（イニシャライズ）、毎フレーム行う更新処理、ファイナライズを実装します。
    /// 初期化はコンストラクタで行う想定なので関数は未定義です。
    /// </summary>
    public abstract class BattleState
    {
        /// <summary>
        /// バトルシーン関連のアクセサ
        /// Acr は Accessor の略称
        /// 使用頻度が高くなる想定のため短くしました。
        /// </summary>
        protected BattleSceneEntry Acr { get; }

        /// <summary>
        /// 指定の値で初期化します。
        /// </summary>
        protected BattleState(BattleSceneEntry battleAccessor)
        {
            this.Acr = battleAccessor;
        }

        /// <summary>
        /// 毎フレーム実行される状態の更新処理。
        /// </summary>
        /// <returns>
        /// 次の状態へ移行する場合は次の状態を返します。
        /// そうでなければ null を返します。
        /// </returns>
        public abstract BattleState Execute();
    }
}
