using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// シーン上に配置するキャラクター
/// </summary>
[RequireComponent(typeof(MeshRenderer))]
public class Character : MonoBehaviour
{
    /// <summary>
    /// 壁や地面との設置判定用に飛ばすレイの長さ
    /// </summary>
    private static float s_tolerance = 0.3f;

    [SerializeField]
    private string spell = null;

    /// <summary>
    /// キャラクターの名前(ファイル名用)
    /// </summary>
    public virtual string Spell
    {
        get => this.spell;
        set
        {
            this.spell = value;
            this.UpdateTexture();
        }
    }

    /// <summary>
    /// キャラクターを内包する Bounds を返します。
    /// </summary>
    public Bounds Bounds => this.GetComponent<Renderer>().bounds;

    protected void Awake()
    {
        if(!string.IsNullOrEmpty(this.Spell))
        {
            this.UpdateTexture();
        }
    }

    protected void OnValidate()
    {
        if(!string.IsNullOrEmpty(this.Spell))
        {
            this.UpdateTexture();
        }
    }

    /// <summary>
    /// 初期化します。
    /// </summary>
    protected void UpdateTexture()
    {
        Material material = this.GetComponent<Renderer>().material;
        material.shader = Shader.Find("PlaneCharacter");
        material.SetTexture("_MainTex", GameUtility.Load<Texture>($"Image/Character/{this.Spell}/{this.Spell}"));
    }

    /// <summary>
    /// 地形に接しているかどうか
    /// </summary>
    /// <param name="direction">接している方向</param>
    protected bool IsContact(Vector3 direction)
    {
        // レイは若干オブジェクトにめり込ませた位置から発射しないと正しく判定できない時がある
        // また transform.position はプレイヤーの中心です。
        Vector3 scale = this.transform.localScale;
        Vector3 offset = new Vector3(direction.x * scale.x, direction.y * scale.y, direction.z * scale.z);
        Vector3 origin = this.transform.position + offset / 2 - direction * 0.1f;
        Ray ray = new Ray(origin, direction);
        // 地形にのみ衝突するようにレイヤを指定する
        int layer = LayerMask.NameToLayer("Ground");
        return Physics.Raycast(ray, s_tolerance, 1 << layer);
    }
}
