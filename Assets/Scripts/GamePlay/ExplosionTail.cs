using System;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionTail : AnimatedActor, IPoolable
{
	public int strength = 1;
    public int range = 3;
    public BoxCollider2D horizontalBoxCollider;

    private const int centerAnimationBegin = 5;
    private const int horizontalAnimationBegin = 15;
    private const int horizontalEndAnimationBegin = 20;

    private Dictionary<Direction, List<SpriteRenderer>> spriteRenderers;
    private Dictionary<Direction, float> distance;

    public LayerMask contactLayer;

    protected override void InitVariables()
    {
        spriteRenderers = new Dictionary<Direction, List<SpriteRenderer>>();
        distance = new Dictionary<Direction, float>();

        foreach (Direction dir in Enum.GetValues(typeof(Direction)))
        {
            spriteRenderers.Add(dir, new List<SpriteRenderer>());
            distance.Add(dir, 0f);
        }
    }

    private void SpreadExplosionTails()
    {
        InstanciateBlastTail("top_", Direction.UP, SpriteTopPosition, Vector2.up, ArenaController.VERTICAL_SIZE, 90f);
        InstanciateBlastTail("right_", Direction.RIGHT, SpriteRightPosition, Vector2.right, ArenaController.HORIZONTAL_SIZE, 0f);
        InstanciateBlastTail("bottom_", Direction.DOWN, SpriteBottomPosition, Vector2.down, ArenaController.VERTICAL_SIZE, -90f);
        InstanciateBlastTail("left_", Direction.LEFT, SpriteLeftPosition, Vector2.left, ArenaController.HORIZONTAL_SIZE, 180f);

        StartAnimation();
    }

    private void InstanciateBlastTail(string name, Direction dirKey, Vector2 startPosition, Vector3 direction, float gridSize, float angle)
    {
        RaycastHit2D ray2D = Physics2D.Raycast(startPosition, direction, range * gridSize - 0.1f, contactLayer);
        int grids = 0;        
        if(ray2D.transform != null)
        {
        	grids = Mathf.RoundToInt(ray2D.distance / gridSize);
        	//Check damageable collision game object
        	IDamageable damageable = ray2D.transform.GetComponent<IDamageable>();
        	if(damageable != null && damageable.CanDamageIt())
        	{
        		damageable.Damage(strength);
            }
        }
        else
        {
        	grids = range;
        }

        distance[dirKey] = grids * gridSize - 0.1f;

        //print(direction + " -> grids " + grids + ", distance: " + distances[distanceIndex]);
        int spriteRendererCount = 0;
        for (int i = 0; i < grids; i++)
        {
            GameObject go = new GameObject(name + i);
            spriteRenderers[dirKey].Add(go.AddComponent<SpriteRenderer>());
            spriteRendererCount = spriteRenderers[dirKey].Count - 1;
            spriteRenderers[dirKey][spriteRendererCount].sortingLayerID = SpriteRenderer.sortingLayerID;
            go.transform.parent = transform;
            go.transform.position = transform.position + direction * (i + 1) * gridSize;
            spriteRenderers[dirKey][spriteRendererCount].sprite = 
                ResourcesProvider.Instance.ExplosionSprites[horizontalAnimationBegin];
            go.transform.localScale = Vector2.one;
            go.transform.localRotation = Quaternion.Euler(Vector3.forward * angle);
        }

        //troca o último Sprite para ponta da explosão caso exista
        if (grids > 0)
        {
            spriteRenderers[dirKey][spriteRendererCount].sprite = 
                ResourcesProvider.Instance.ExplosionSprites[horizontalEndAnimationBegin];
        }
    }
    
    private void Update()
    {
        CheckBoxCollision();
    }

    private void ResizeBoxCollider()
    {
        BoundingBox.enabled = distance[Direction.UP] > 0f || distance[Direction.DOWN] > 0f;
        if (BoundingBox.enabled)
        {
            BoundingBox.size = new Vector2(ArenaController.HORIZONTAL_SIZE,
                distance[Direction.UP] + ArenaController.VERTICAL_SIZE + distance[Direction.DOWN]);
            BoundingBox.offset = new Vector2(0f, (distance[Direction.UP] - distance[Direction.DOWN]) / 2f);
        }
        else
        {
            BoundingBox.size = new Vector2(ArenaController.HORIZONTAL_SIZE, ArenaController.VERTICAL_SIZE);
            BoundingBox.offset = Vector2.zero;
        }

        horizontalBoxCollider.enabled = distance[Direction.LEFT] > 0f || distance[Direction.RIGHT] > 0f;
        if (horizontalBoxCollider.enabled)
        {
            horizontalBoxCollider.size = new Vector2(distance[Direction.LEFT] + ArenaController.HORIZONTAL_SIZE + distance[Direction.RIGHT],
                ArenaController.VERTICAL_SIZE);
            horizontalBoxCollider.offset = new Vector2((-distance[Direction.LEFT] + distance[Direction.RIGHT]) / 2f, 0f);
        }
        else
        {
            horizontalBoxCollider.size = new Vector2(ArenaController.HORIZONTAL_SIZE, ArenaController.VERTICAL_SIZE);
            horizontalBoxCollider.offset = Vector2.zero;
        }

        //caso os dois box colliders estejam desabilitados, habilita o primeiro
        if(!(BoundingBox.enabled && horizontalBoxCollider.enabled))
        {
            BoundingBox.enabled = true;
        }
    }

    private void CheckBoxCollision()
    {
        BoxCollider2D box = GetComponent<BoxCollider2D>();

        RaycastHit2D[] hits = Physics2D.BoxCastAll(box.bounds.center, box.size, 0f, Vector2.zero, 0f, contactLayer);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.transform != null)
            {
                IDamageable damageable = hit.transform.GetComponent<IDamageable>();
                if (damageable != null && damageable.CanDamageIt())
                {
                    damageable.Damage(strength);
                }
            }
        }
    }

    protected override void NextAnimationFrame()
    {
        currentFrameTime = (currentFrameTime + 1) % animationFrameCount;
        Sprite = ResourcesProvider.Instance.ExplosionSprites[centerAnimationBegin + currentFrameTime];

        foreach (Direction dir in Enum.GetValues(typeof(Direction)))
        {
            for (int i = 0; i < spriteRenderers[dir].Count - 1; i++)
            {
                spriteRenderers[dir][i].sprite = ResourcesProvider.Instance.ExplosionSprites[horizontalAnimationBegin + currentFrameTime];
            }
            //ponta da explosão
            if (spriteRenderers[dir].Count > 0)
            {
                spriteRenderers[dir][spriteRenderers[dir].Count - 1].sprite = ResourcesProvider.Instance.ExplosionSprites[horizontalEndAnimationBegin + currentFrameTime];
            }
        }

        UpdateTime();
    }

    protected override void StopAnimation()
    {
        ArenaController.Instance.explosionPoolSystem.SendBack(gameObject);
    }

    public override void PutInContainer()
    {
        transform.parent = ArenaController.Instance.explosionsContainer;
    }

    #region IPoolable methods
    public void Ativate()
    {
        PutInContainer();
        currentFrameTime = 0; 
        SpreadExplosionTails();
        ResizeBoxCollider();
    }

    public void Deactivate()
    {
        foreach (Direction dir in Enum.GetValues(typeof(Direction)))
        {
            spriteRenderers[dir].Clear();
            distance[dir] = 0f;
        }

        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
    #endregion

    #region IDamageable methods
    public override void DestroyIt()
    {
        Destroy(gameObject);
    }

    public override void Damage(float damage)
    {
        throw new UnityException("Cannot damage explosion yet.");
    }

    public override bool CanDamageIt()
    {
        return false;
    }
    #endregion


#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        if (Application.isPlaying)
            return;

        InitComponents();

        Gizmos.color = Color.red;
        Gizmos.DrawLine(SpriteTopPosition, SpriteTopPosition + Vector2.up * range * ArenaController.VERTICAL_SIZE);
        Gizmos.DrawLine(SpriteRightPosition, SpriteRightPosition + Vector2.right * range * ArenaController.VERTICAL_SIZE);
        Gizmos.DrawLine(SpriteLeftPosition, SpriteLeftPosition + Vector2.left * range * ArenaController.VERTICAL_SIZE);
        Gizmos.DrawLine(SpriteBottomPosition, SpriteBottomPosition + Vector2.down * range * ArenaController.VERTICAL_SIZE);
    }
#endif
}
