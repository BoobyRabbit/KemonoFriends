using UnityEngine;

namespace Battle
{
    /// <summary>
    /// バトル中に使用する敵キャラクター
    /// </summary>
    public class EnemyBattleCharacter : BattleCharacter
    {
        /// <summary>
        /// ステータス用UI
        /// </summary>
        [SerializeField]
        public EnemyStatusUI statusUI = null;

        public override void AddHP(int value, BarGauge.AnimationType animationType)
        {
            this.statusUI.hpGauge.Set(this.status.maxHP, this.status.NowHP, value, animationType);
            this.status.NowHP += value;
            this.statusUI.nowHPText.text = this.status.NowHP.ToString();
        }

        public override void AddKP(int value, BarGauge.AnimationType animationType)
        {
            this.status.NowKP += value;
        }

        public override void AddAP(float value, BarGauge.AnimationType animationType)
        {
            this.statusUI.apGauge.Set(this.status.maxAP, this.status.NowAP, value, animationType);
            this.status.NowAP += value;
        }

        /// <summary>
        /// バトル用敵キャラクターを生成します。
        /// </summary>
        /// <param name="prefab">生成するキャラクターの元となるプレハブ</param>
        /// <param name="status">キャラクターのステータス</param>
        /// <param name="statusUIParent">ステータス用UIの親</param>
        /// <returns></returns>
        public static EnemyBattleCharacter Instantiate(EnemyBattleCharacter prefab, CharacterStatus status, Transform statusUIParent)
        {
            var instance = GameObject.Instantiate(prefab);
            instance.transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
            instance.status = status;
            instance.name = status.name;
            instance.group = CharacterGroup.Enemy;
            instance.statusUI.Init(status);
            instance.statusUI.transform.SetParent(statusUIParent);
            instance.statusUI.transform.localScale = Vector3.one;
            instance.UpdateTexture();
            return instance;
        }

        /// <summary>
        /// ステータス用UIと一緒にキャラクターの座標を移動します。
        /// </summary>
        public void SetPositionWithStatusUI(Vector3 position)
        {
            this.transform.position = position;
            this.statusUI.GetComponent<RectTransform>().position = RectTransformUtility.WorldToScreenPoint(Camera.main, this.transform.position);
        }
    }
}
