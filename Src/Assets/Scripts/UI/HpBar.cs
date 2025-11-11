using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    public Slider slider;
    public Slider subSlider;
    public Transform target;
    public Vector3 offset = new Vector3(-4f, -1.5f, 0f);
    [Header("外观")]
    public Image fillImage; // Slider Fill 图像，用于颜色渐变
    public Gradient hpGradient; // 0=红,1=绿
    public float baseWidth = 80f; // 基础宽度
    public float widthPerHp = 0.4f; // 每点最大血量带来的额外宽度

	[Header("对齐/偏移")]
	public bool useWorldSpaceCanvas = false; // 勾选则直接用世界坐标放置
	public bool autoOffsetByBounds = true;   // 根据渲染/碰撞边界自动计算头顶偏移
	public float headMargin = 0.25f;         // 头顶额外上移

    private Unit unit;
    private Camera cam;
	private Renderer cachedRenderer;
	private Collider2D cachedCollider2D;

    void Awake()
    {
        cam = Camera.main;
    }

    public void Bind(Unit targetUnit)
    {
        unit = targetUnit;
        target = targetUnit.transform;
        if (slider != null)
        {
            slider.minValue = 0f;
            slider.maxValue = targetUnit.HPMax;
            slider.value = targetUnit.HP;

        }
        if (subSlider != null)
        {
            subSlider.minValue = 0f;
            subSlider.maxValue = targetUnit.HPMax;
            subSlider.value = targetUnit.HP;

        }
        AutoSizeByHpMax(targetUnit.HPMax);
        UpdateColor();
		CacheBoundsSources();
		if (autoOffsetByBounds)
		{
			ComputeAutoOffset();
		}
        targetUnit.OnDeath += HandleOwnerDeath;
    }

    void OnDestroy()
    {
        if (unit != null)
        {
            unit.OnDeath -= HandleOwnerDeath;
        }
    }

    void HandleOwnerDeath(Unit sender)
    {
        if(this.gameObject != null)
        Destroy(gameObject);
    }

    void LateUpdate()
    {
        if (unit == null || target == null)
        {
            Destroy(gameObject);
            return;
        }
        if (slider != null)
        {
            slider.value = unit.HP;
        }
        if(subSlider != null) 
        subSlider.value = Mathf.Lerp(this.subSlider.value, unit.HP, Time.deltaTime); 
        UpdateColor();
		Vector3 worldPos = GetAnchorWorldPosition() + offset;
		RectTransform rect = this.transform as RectTransform;
		if (useWorldSpaceCanvas)
		{
			rect.position = worldPos;
		}
		else
		{
			Vector3 screenPos = cam != null ? cam.WorldToScreenPoint(worldPos) : worldPos;
			rect.position = screenPos;
		}
    }

    void AutoSizeByHpMax(float hpMax)
    {
        RectTransform rect = this.transform as RectTransform;
        if (rect == null) return;
        float width = baseWidth + Mathf.Max(0f, hpMax) * widthPerHp;
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
    }

    void UpdateColor()
    {
        if (fillImage == null || hpGradient == null || unit == null || unit.HPMax <= 0) return;
        float t = Mathf.Clamp01((float)unit.HP / unit.HPMax);
        fillImage.color = hpGradient.Evaluate(t);
    }

	void CacheBoundsSources()
	{
		if (target == null) return;
		cachedRenderer = target.GetComponentInChildren<Renderer>();
		cachedCollider2D = target.GetComponentInChildren<Collider2D>();
	}

	Vector3 GetAnchorWorldPosition()
	{
		if (cachedCollider2D != null)
		{
			Bounds b = cachedCollider2D.bounds;
			return new Vector3(b.center.x, b.max.y, target.position.z);
		}
		if (cachedRenderer != null)
		{
			Bounds b = cachedRenderer.bounds;
			return new Vector3(b.center.x, b.max.y, target.position.z);
		}
		return target != null ? target.position : Vector3.zero;
	}

	void ComputeAutoOffset()
	{
		// 将 offset 调整为头顶上方且水平居中
		Vector3 anchor = GetAnchorWorldPosition();
		float topY = anchor.y;
		float baseY = target != null ? target.position.y : topY;
		float dy = Mathf.Max(0f, topY - baseY) + headMargin;
	}
}


