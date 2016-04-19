
public abstract class AnimatedActor : Actor2D, ITimeCountable
{
    public float duration = 1;
    public int animationFrameBegin = 0;
    public int animationFrameCount = 5;

    protected int currentFrameTime;

    public float AnimationRate { get { return duration / animationFrameCount; } }

    protected virtual void StartAnimation()
    {
        Invoke("NextAnimationFrame", AnimationRate);
    }

    protected virtual void StopAnimation()
    {
        Destroy(this.gameObject);
    }

    protected virtual void NextAnimationFrame()
    {
        currentFrameTime = (currentFrameTime + 1) % animationFrameCount;
        Sprite = ResourcesProvider.Instance.ExplosionSprites[animationFrameBegin + currentFrameTime];
        UpdateTime();
    }


    public virtual void UpdateTime()
    {
        if (currentFrameTime > 0)
        {
            Invoke("NextAnimationFrame", AnimationRate);
        }
        else
        {
            TimeOutAction();
        }
    }

    public virtual void TimeOutAction()
    {
        StopAnimation();
    }
}
