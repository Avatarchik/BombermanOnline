public class WeekBlock : AnimatedActor
{
    public float resistence = 1;

    public override void Damage(float damage)
    {
        resistence -= damage;
        if (resistence <= 0f)
            DestroyIt();
    }

    public override bool CanDamageIt()
    {
        return resistence > 0;
    }

    public override void DestroyIt()
    {
        BoundingBox.enabled = false;
        StartAnimation();        
    }

    public override void PutInContainer()
    {
    }
}
