using UnityEngine;

public class Bomb : Actor2D, ITimeCountable, IPoolable
{
    public int time = 3;
    public int strength = 3;

    private float currentTime = 0f;

    public BoxCollider2D PlayerCollider { get; set; }

    protected override void InitVariables()
    {
    }

    protected override void PlaceTiled()
    {
    }

    public override void PutInContainer()
    {
        transform.parent = ArenaController.Instance.bombsContainer;
    }

    protected virtual void Explode()
    {
        ArenaController.Instance.InstanciateExplosion(transform.position);
    }

    private void Update()
    {
        if (PlayerCollider != null)
        {
            CheckPlayerCollision();
        }

        UpdateTime();
    }

    private void CheckPlayerCollision()
    {
        if (!BoundingBox.bounds.Intersects(PlayerCollider.bounds))
        {
            PlayerCollider = null;
            gameObject.layer = LayerMask.NameToLayer("Solid");
        }
    }

    #region IDamageable methods
    public override void DestroyIt()
    {
        Destroy(gameObject);
    }

    public override void Damage(float damage)
    {
        Explode();
    }

    public override bool CanDamageIt()
    {
        return gameObject.activeInHierarchy;
    }
    #endregion

    #region ITimeCountable methods
    public void SetTime(float time)
    {
        this.currentTime = time;
    }

    public float GetTime()
    {
        return currentTime;
    }

    public void UpdateTime()
    {
        currentTime -= Time.deltaTime;
        if(currentTime <= 0f)
        {            
            TimeOutAction();
        }
    }

    public void TimeOutAction()
    {
        Explode();
        ArenaController.Instance.bombPoolSystem.SendBack(gameObject);
    }
    #endregion

    #region IPoolable methods
    public void Ativate()
    {
        SetTime(time);
        PutInContainer();
    }

    public void Deactivate()
    {
        PlayerCollider = null;
        gameObject.layer = LayerMask.NameToLayer("Default");
    }
    #endregion
}
