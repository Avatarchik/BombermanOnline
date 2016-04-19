using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public abstract class SpriteActor : MonoBehaviour
{
    public SpriteRenderer SpriteRenderer { get; protected set; }
    public Sprite Sprite
    {
        get { return SpriteRenderer.sprite; }
        protected set { SpriteRenderer.sprite = value; }
    }

    public float Width { get { return SpriteRenderer.bounds.size.x; } }
    public float Height { get { return SpriteRenderer.bounds.size.y; } }

    public float Size
    {
        get { return transform.localScale.sqrMagnitude; }
        set { transform.localScale = Vector3.one * value; }
    }

    public float EulerAngle
    {
        get { return transform.eulerAngles.z; }
        set { transform.eulerAngles = new Vector3(transform.transform.eulerAngles.x, transform.transform.eulerAngles.y, value); }
    }

    public Color Color
    {
        get { return SpriteRenderer.color; }
        set { SpriteRenderer.color = value; }
    }

    public Vector2 Pivot
    {
        get { return Sprite.pivot; }
    }

    public Vector2 SpriteTopLeftPosition { get { return new Vector2(SpriteRenderer.bounds.min.x, SpriteRenderer.bounds.max.y); } }
    public Vector2 SpriteTopPosition { get { print("nome da descraça: "+gameObject.name); return new Vector2(SpriteRenderer.bounds.center.x, SpriteRenderer.bounds.max.y); } }
    public Vector2 SpriteTopRightPosition { get { return SpriteRenderer.bounds.max; } }

    public Vector2 SpriteLeftPosition { get { return new Vector2(SpriteRenderer.bounds.min.x, SpriteRenderer.bounds.center.y); } }
    public Vector2 SpriteCenterPosition { get { return SpriteRenderer.bounds.center; } }
    public Vector2 SpriteRightPosition { get { return new Vector2(SpriteRenderer.bounds.max.x, SpriteRenderer.bounds.center.y); } }

    public Vector2 SpriteBottomLeftPosition { get { return SpriteRenderer.bounds.min; } }
    public Vector2 SpriteBottomPosition { get { return new Vector2(SpriteRenderer.bounds.center.x, SpriteRenderer.bounds.min.y); } }
    public Vector2 SpriteBottomRightPosition { get { return new Vector2(SpriteRenderer.bounds.max.x, SpriteRenderer.bounds.min.y); } }

    protected virtual void Start()
    {
        InitComponents();
        InitVariables();
    }

    protected virtual void InitComponents()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected abstract void InitVariables();
}
