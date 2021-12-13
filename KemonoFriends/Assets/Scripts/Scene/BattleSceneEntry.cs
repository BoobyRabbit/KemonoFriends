using Battle;
using Battle.State;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// バトル用シーンの共通処理
/// バトルの進行を管理します。
/// </summary>
public class BattleSceneEntry : SceneEntry
{
    /// <summary>
    /// Overlay UI用キャンバス
    /// </summary>
    public Canvas OverlayCanvas => this.overlayCanvas;
    [SerializeField] private Canvas overlayCanvas = null;

    /// <summary>
    /// World UI用キャンバス
    /// </summary>
    public Canvas WorldCanvas => this.worldCanvas;
    [SerializeField] private Canvas worldCanvas = null;

    /// <summary>
    /// 味方キャラクターのプレハブ
    /// </summary>
    public FriendBattleCharacter FriendBattleCharacterPrefab => this.friendBattleCharacterPrefab;
    [SerializeField] private FriendBattleCharacter friendBattleCharacterPrefab = null;

    /// <summary>
    /// 味方ステータスUIの親
    /// </summary>
    public Transform FriendStatusUIParent => this.friendStatusUIParent;
    [SerializeField] private Transform friendStatusUIParent = null;

    /// <summary>
    /// 敵キャラクターのプレハブ
    /// </summary>
    public EnemyBattleCharacter EnemyBattleCharacterPrefab => this.enemyBattleCharacterPrefab;
    [SerializeField] private EnemyBattleCharacter enemyBattleCharacterPrefab = null;

    /// <summary>
    /// 敵ステータスUIの親
    /// </summary>
    public Transform EnemyStatusUIParent => this.enemyStatusUIParent;
    [SerializeField] private Transform enemyStatusUIParent = null;

    /// <summary>
    /// バトルアイコンを選択するクラス
    /// </summary>
    public SelectBattleIconMonoBehaviour SelectBattleIcon => this.selectBattleIcon;
    [SerializeField] private SelectBattleIconMonoBehaviour selectBattleIcon = null;

    /// <summary>
    /// 使用するスキルやアイテムなどを選択するクラス
    /// </summary>
    public SelectBoxScrollViewMonoBehaviour SelectBoxScrollView => this.selectBoxScrollView;
    [SerializeField] private SelectBoxScrollViewMonoBehaviour selectBoxScrollView = null;

    /// <summary>
    /// 攻撃やアイテムの使用対象を選択するクラス
    /// </summary>
    public SelectTargetMonoBehaviour SelectTarget => this.selectTarget;
    [SerializeField] private SelectTargetMonoBehaviour selectTarget = null;

    /// <summary>
    /// HPがダメージを受けた時に表示するエフェクトのプレハブ
    /// </summary>
    public ChangeStatusEffect DamageEffectPrefab => this.damageEffectPrefab;
    [SerializeField] private ChangeStatusEffect damageEffectPrefab = null;

    /// <summary>
    /// HPが回復した時に表示するエフェクトのプレハブ
    /// </summary>
    public ChangeStatusEffect RecoveryEffectPrefab => this.recoveryEffectPrefab;
    [SerializeField] private ChangeStatusEffect recoveryEffectPrefab = null;

    /// <summary>
    /// ノーダメージ時に表示するエフェクトのプレハブ
    /// </summary>
    public NoDamageEffect NoDamageEffectPrefab => this.noDamageEffectPrefab;
    [SerializeField] private NoDamageEffect noDamageEffectPrefab = null;

    /// <summary>
    /// アクション時に表示する評価用文字のエフェクトのプレハブ
    /// </summary>
    public ActionRateEffect ActionRatingPrefab => this.actionRatingPrefab;
    [SerializeField] private ActionRateEffect actionRatingPrefab = null;

    /// <summary>
    /// バトルするキャラクターのリスト
    /// </summary>
    public List<BattleCharacter> BattleCharacters { get; } = new List<BattleCharacter>();

    /// <summary>
    /// バトルの状態
    /// </summary>
    public BattleState BattleState { get; private set; } = null;

    /// <summary>
    /// バトルするキャラクターの内の味方のリスト
    /// </summary>
    public List<BattleCharacter> Friends => this.BattleCharacters.FindAll(b => b is FriendBattleCharacter);

    /// <summary>
    /// バトルするキャラクターの内の敵のリスト
    /// </summary>
    public List<BattleCharacter> Enemies => this.BattleCharacters.FindAll(b => b is EnemyBattleCharacter);

    public override void ManualInit()
    {
        // 仮味方パーティー生成
        List<CharacterStatus> friendStatuses = new List<CharacterStatus>();
        friendStatuses.Add(new CharacterStatus(CharacterStatusMaster.Instance["Serval"]));
        friendStatuses.Add(new CharacterStatus(CharacterStatusMaster.Instance["Raccoon"]));
        friendStatuses.Add(new CharacterStatus(CharacterStatusMaster.Instance["Fennec"]));
        // 仮敵パーティー生成
        List<CharacterStatus> enemyStatuses = new List<CharacterStatus>();
        enemyStatuses.Add(new CharacterStatus(CharacterStatusMaster.Instance["Serval"]));
        enemyStatuses.Add(new CharacterStatus(CharacterStatusMaster.Instance["Raccoon"]));
        enemyStatuses.Add(new CharacterStatus(CharacterStatusMaster.Instance["Fennec"]));
        // 仮アイテムを追加
        GameManager.Instance.HaveItems.Add(ItemMaster.Instance[0]);
        GameManager.Instance.HaveItems.Add(ItemMaster.Instance[1]);
        GameManager.Instance.HaveItems.Add(ItemMaster.Instance[2]);

        this.BattleState = new Init(this, "SampleBattleField", friendStatuses, enemyStatuses);
    }

    public override void ManualUpdate()
    {
        foreach(FriendBattleCharacter friend in this.Friends)
        {
            friend.UpdateButton();
        }
        if(this.BattleState != null)
        {
            var nextState = this.BattleState.Execute();
            if(nextState != null)
            {
                this.BattleState = nextState;
            }
        }
    }
}
