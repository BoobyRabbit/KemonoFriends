using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Player : Character
{
    /// <summary>
    /// 最大移動速度
    /// </summary>
    [SerializeField]
    private float maxMoveSpeed = 8f;

    /// <summary>
    /// ジャンプ力
    /// </summary>
    [SerializeField]
    private float jumpPower = 15f;

    /// <summary>
    /// 重力
    /// </summary>
    [SerializeField]
    private float gravity = 20f;

    /// <summary>
    /// プレイヤーコントローラー
    /// </summary>
    private CharacterController controller = null;

    /// <summary>
    /// 初期化します
    /// </summary>
    private void Start()
    {
        controller = this.GetComponent<CharacterController>();
    }

    /// <summary>
    /// 一定時間ごとに呼び出される処理
    /// </summary>
    private void Update()
    {
        Vector3 currentMoveSpeed = controller.velocity;
        bool isGrounded = controller.isGrounded || IsContact(Vector3.down);
        Jump(ref currentMoveSpeed, isGrounded);
        Move(ref currentMoveSpeed, isGrounded);
        controller.Move(currentMoveSpeed * Time.deltaTime);
    }

    /// <summary>
    /// 他のオブジェクトと重なった場合の処理
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        switch(other.tag)
        {
        case "EnemyHitArea":
            // @todo 戦闘へ移行する処理を書いてください
            break;
        }
    }

    /// <summary>
    /// ジャンプする
    /// </summary>
    private void Jump(ref Vector3 currentMoveSpeed, bool isGrounded)
    {
        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            currentMoveSpeed.y = jumpPower;
        }
        currentMoveSpeed.y -= this.gravity * Time.deltaTime;
    }

    /// <summary>
    /// 移動します
    /// </summary>
    private void Move(ref Vector3 currentMoveSpeed, bool isGrounded)
    {
        float x = GetAxisRaw("Horizontal");
        float z = GetAxisRaw("Vertical");
        Vector3 axisRowSpeed = new Vector3(x, 0, z).normalized * maxMoveSpeed;
        currentMoveSpeed.x = axisRowSpeed.x;
        currentMoveSpeed.z = axisRowSpeed.z;
    }

    /// <summary>
    /// スティックの入力を取得(-1 or 0 or 1)
    /// </summary>
    private float GetAxisRaw(string axisName)
    {
        float axis = Input.GetAxis(axisName);
        if(axis < 0)
        {
            return -1f;
        }
        if(axis > 0)
        {
            return 1f;
        }
        return 0;
    }
}
