using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Player : Character
{
    [SerializeField]
    private float m_MaxMoveSpeed = 8f;

    /// <summary>
    /// 最大移動速度
    /// </summary>
    public float MaxMoveSpeed { get { return m_MaxMoveSpeed; } }

    [SerializeField]
    private float m_JumpSpeed = 12f;

    /// <summary>
    /// ジャンプ、落下スピード
    /// </summary>
    public float JumpSpeed { get { return m_JumpSpeed; } }

    /// <summary>
    /// 重力係数
    /// </summary>
    static public float Gravity = 100f;
    /// <summary>
    /// ジャンプ時間
    /// </summary>
    [SerializeField]
    private float m_JumpTime = 0.15f;

    /// <summary>
    /// プレイヤーコントローラー
    /// </summary>
    private CharacterController m_Controller = null;

    /// <summary>
    /// ジャンプ時間用のカウンタ
    /// </summary>
    private Count m_JumpCount = new Count();

    /// <summary>
    /// 初期化します
    /// </summary>
    private void Start()
    {
        m_Controller = this.GetComponent<CharacterController>();
        m_JumpCount = new Count(m_JumpTime);
    }

    /// <summary>
    /// 一定時間ごとに呼び出される処理
    /// </summary>
    private void Update()
    {
        m_JumpCount.Update();
        Vector3 currentMoveSpeed = m_Controller.velocity;
        bool isGrounded = m_Controller.isGrounded || IsContact(Vector3.down);
        Jump(ref currentMoveSpeed, isGrounded);
        Move(ref currentMoveSpeed, isGrounded);
        m_Controller.Move(currentMoveSpeed * Time.deltaTime);
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
            m_JumpCount.Start();
        }
        if(Input.GetButtonUp("Jump"))
        {
            m_JumpCount.IsStop = true;
        }
        bool isJumping = !m_JumpCount.IsStop && m_JumpCount.RestTime > 0.0f;
        if(isJumping)
        {
            currentMoveSpeed.y = JumpSpeed;
        }
        else
        {
            currentMoveSpeed.y -= Gravity * Time.deltaTime;
            if(currentMoveSpeed.y < -JumpSpeed)
            {
                currentMoveSpeed.y = -JumpSpeed;
            }
        }
    }

    /// <summary>
    /// 移動します
    /// </summary>
    private void Move(ref Vector3 currentMoveSpeed, bool isGrounded)
    {
        float x = GetAxisRaw("Horizontal") * MaxMoveSpeed;
        float z = GetAxisRaw("Vertical") * MaxMoveSpeed;
        Vector3 axisRowSpeed = new Vector3(x, 0, z);
        axisRowSpeed = this.transform.TransformDirection(axisRowSpeed);
        currentMoveSpeed.x = axisRowSpeed.x;
        currentMoveSpeed.z = -axisRowSpeed.y;
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
