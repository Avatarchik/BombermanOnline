using UnityEngine;

public class PlayerActor : CharacterActor, IPauseable
{
    public LayerMask layer;

    protected override void UpdateAnimation()
    {
        animController.SetFloat("hSpeed", input.x);
        animController.SetFloat("vSpeed", input.y);
        animController.SetFloat("hDirection", LookingDirection.x);
        animController.SetFloat("vDirection", LookingDirection.y);
    }

    protected override void PlaceTiled()
    {
        Point p = ArenaController.GetTilePoint(BoxCenterPosition);
        
        transform.position = new Vector2(p.x * ArenaController.HORIZONTAL_SIZE + ArenaController.HORIZONTAL_SIZE / 2f,
                                         p.y * ArenaController.VERTICAL_SIZE - ArenaController.VERTICAL_SIZE);
    }

    protected override void UpdateInput()
    {
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        //andar somente em uma direção
        float x = Mathf.Abs(input.x);
        float y = Mathf.Abs(input.y);
        if(x + y > 1f){
            input.y = 0f;
        }

        if (input.sqrMagnitude > 0)
            LookingDirection = input;

        if (Input.GetButtonUp("Jump"))
        {
            Vector2 pos = ArenaController.GetTilePosition(ArenaPoint);
            ArenaController.Instance.InstanciateBomb(pos, BoundingBox);            
        }

        if (Input.GetButtonUp("Fire2"))
        {
            Vector2 pos = ArenaController.GetTilePosition(ArenaPoint);
            ArenaController.Instance.InstanciateExplosion(pos);
        }
    }

    protected override void UpdateMovement()
    {
        currentSpeed = input * speed;  
    }

    protected override void UpdatePhysics()
    {
        Vector2 lastPosition = transform.position;
        transform.position += currentSpeed * Time.deltaTime;

        if (currentSpeed.x > 0f && IsRightCollision(layer) ||
            currentSpeed.x < 0f && IsLeftCollision(layer) ||
            currentSpeed.y > 0f && IsTopCollision(layer) ||
            currentSpeed.y < 0f && IsBottomCollision(layer))
        {
            transform.position = lastPosition;
        }

    }

    public void Pause(){}
    public void Resume(){}

    public override void PutInContainer()
    {
        transform.parent = ArenaController.Instance.playersContainer;
    }

    //void OnDrawGizmos()
    //{
    //    BoundingBox = GetComponent<BoxCollider2D>();

    //    if (IsBottomCollision(layer))
    //    {
    //        Debug.DrawLine(BoundingBox.bounds.min, BoundingBox.bounds.min + Vector3.right * BoundingBox.bounds.size.x, Color.red);
    //    }
    //    else
    //        Debug.DrawLine(BoundingBox.bounds.min, BoundingBox.bounds.min + Vector3.right * BoundingBox.bounds.size.x, Color.black);

    //    if(IsTopCollision(layer))
    //        Debug.DrawLine(BoundingBox.bounds.max, BoundingBox.bounds.max + Vector3.left * BoundingBox.bounds.size.x, Color.red);
    //    else
    //        Debug.DrawLine(BoundingBox.bounds.max, BoundingBox.bounds.max + Vector3.left * BoundingBox.bounds.size.x, Color.black);


    //    if(IsLeftCollision(layer))
    //    Debug.DrawLine(BoundingBox.bounds.min, BoundingBox.bounds.min + Vector3.up * BoundingBox.bounds.size.y, Color.red);
    //    else
    //        Debug.DrawLine(BoundingBox.bounds.min, BoundingBox.bounds.min + Vector3.up * BoundingBox.bounds.size.y, Color.black);


    //    if (IsRightCollision(layer))
    //        Debug.DrawLine(BoundingBox.bounds.max, BoundingBox.bounds.max + Vector3.down * BoundingBox.bounds.size.y, Color.red);
    //    else
    //        Debug.DrawLine(BoundingBox.bounds.max, BoundingBox.bounds.max + Vector3.down * BoundingBox.bounds.size.y, Color.black);

    //}
}
