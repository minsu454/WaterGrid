using UnityEngine;

/// <summary>
/// 아웃라인 보여주는 Class
/// </summary>
[ExecuteInEditMode]
[RequireComponent(typeof(SpriteRenderer))]
public class SpriteOutline : MonoBehaviour
{
    public Color color = Color.white;

    [Range(0, 100)]
    public float outlineThickness = 1f;

    public bool outlineEnabled = true;

    private SpriteRenderer spriteRenderer;
    private MaterialPropertyBlock mpb;

    void OnEnable()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        mpb = new MaterialPropertyBlock();
        UpdateOutline(true);
    }

    void OnDisable()
    {
        UpdateOutline(false);
    }

    void Update()
    {
        UpdateOutline(outlineEnabled);
    }

    /// <summary>
    /// 쉐이더에 아웃라인 관련 파라미터 설정
    /// </summary>
    void UpdateOutline(bool outline)
    {
        if (spriteRenderer == null || mpb == null) return;

        spriteRenderer.GetPropertyBlock(mpb);

        mpb.SetFloat("_OutlineEnabled", outline ? 1f : 0f);
        mpb.SetColor("_SolidOutline", color);
        mpb.SetFloat("_Thickness", outlineThickness);

        spriteRenderer.SetPropertyBlock(mpb);
    }
}
