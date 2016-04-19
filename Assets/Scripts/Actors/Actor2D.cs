using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public abstract class Actor2D : SpriteActor, IDestructable, IDamageable
{
    public BoxCollider2D BoundingBox { get; protected set; }
    public Point ArenaPoint { get { return ArenaController.GetTilePoint(BoxCenterPosition); } }

    public Vector2 BoxTopPosition { get { return new Vector2(BoundingBox.bounds.center.x, BoundingBox.bounds.max.y); } }
    public Vector2 BoxTopLeftPosition { get { return new Vector2(BoundingBox.bounds.min.x, BoundingBox.bounds.max.y); } }
    public Vector2 BoxTopRightPosition { get { return BoundingBox.bounds.max; } }

    public Vector2 BoxLeftPosition { get { return new Vector2(BoundingBox.bounds.min.x, BoundingBox.bounds.center.y); } }
    public Vector2 BoxCenterPosition { get { return BoundingBox.bounds.center; } }
    public Vector2 BoxRightPosition { get { return new Vector2(BoundingBox.bounds.max.x, BoundingBox.bounds.center.y); } }

    public Vector2 BoxBottomLeftPosition { get { return BoundingBox.bounds.min; } }
    public Vector2 BoxBottomPosition { get { return new Vector2(BoundingBox.bounds.center.x, BoundingBox.bounds.min.y); } }
    public Vector2 BoxBottomRightPosition { get { return new Vector2(BoundingBox.bounds.max.x, BoundingBox.bounds.min.y); } }

    protected override void InitComponents()
    {
        base.InitComponents();
        BoundingBox = GetComponent<BoxCollider2D>();
    }

    protected override void InitVariables()
    {
        PutInContainer();
    }

    protected virtual void PlaceTiled()
    {
        Point p = ArenaController.GetTilePoint(BoxCenterPosition);
        transform.position = new Vector2(p.x * ArenaController.HORIZONTAL_SIZE,
                                         p.y * ArenaController.VERTICAL_SIZE);
    }

    public abstract void PutInContainer();
    public abstract void DestroyIt();
    public abstract void Damage(float damage);
    public abstract bool CanDamageIt();
}
