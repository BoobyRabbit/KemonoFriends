using DG.Tweening;
using UnityEngine;

namespace Battle.ActonCommand
{
    public class Scratch : SkillActionCommand
    {
        public override void Execute()
        {
            BattleCharacter target = targets[0];
            Vector3 targetPosition = target.transform.position;
            Vector3 ownPosition = actioner.transform.position;
            this.sequence.AppendInterval(0.25f);
            // 相手に近づく
            Vector3 endPoint = (targetPosition - ownPosition) / 2; // 相手との中間距離まで移動
            endPoint.y = ownPosition.y; // 地面を歩くようY座標は固定
            this.sequence.Append(actioner.transform.DOMove(endPoint, endPoint.magnitude * 0.1f).SetRelative());
            // 相手に向かってジャンプ
            float returnPositionSign = Mathf.Sign(ownPosition.x - targetPosition.x);
            endPoint = targetPosition;
            endPoint.y += target.Bounds.extents.y; // 敵の上部を踏む
            this.sequence.Append(actioner.transform.DOJump(endPoint, 1.5f, 1, 0.5f));
            // 相手にダメージを与える
            this.sequence.AppendCallback(() => this.Damage(target, BarGauge.AnimationType.Play, ActionRate.Nice));
            // 相手に攻撃した反動で少し後退しながら着地
            endPoint = new Vector3(targetPosition.x + 1.5f * returnPositionSign, ownPosition.y, 0);
            this.sequence.Append(actioner.transform.DOJump(endPoint, 1, 1, 0.5f));
            // 自分が元いた場所に戻る
            this.sequence.Append(actioner.transform.DOMove(ownPosition, (endPoint - ownPosition).magnitude * 0.1f));
        }
    }

}
