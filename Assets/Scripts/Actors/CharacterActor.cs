using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
//[RequireComponent(typeof(AudioSource))]
public abstract class CharacterActor : Actor2D, IRespawnable, IHealthful, ITouchable {

    public float speed = 0.5f;

    protected Vector2 input;
    protected Vector3 currentSpeed;

    public float Life { get; protected set; }
    public Vector2 LookingDirection {get; protected set;} 
    public Vector2 NormalizedSpeed { get { return currentSpeed.normalized; }}
    public Vector3 RespawnPosition { get; protected set;}
    public bool IsMoving { get { return currentSpeed.sqrMagnitude > 0f; } }
    public bool IsGrounded { get; protected set; }

    public Animator animController{get; protected set;}
    public AudioSource AudioSource {get; protected set;}

    protected override void InitComponents()
    {
        base.InitComponents();
        animController = GetComponent<Animator>();
        //AudioSource = GetComponent<AudioSource>();
    }



    // Update is called once per frame
    protected virtual void Update () {
        UpdatePhysics();
        UpdateInput();
        UpdateMovement();
        UpdateAnimation();
    }
    

    protected abstract void UpdateInput();
    protected abstract void UpdateMovement();
    protected abstract void UpdateAnimation();

    protected virtual void UpdatePhysics()
    {
    }

    public override void DestroyIt()
    {
        throw new NotImplementedException();
    }

    public override void Damage(float damage)
    {
        print("Bomberman tomando damage");
    }

    public override bool CanDamageIt()
    {
        return false;
    }

    public virtual void Respawn(){
        transform.position = RespawnPosition;
    }
    public virtual void SetRespawnPosition(Vector3 position){
        RespawnPosition = position;
    }
    public virtual Vector3 GetRespawnPosition(){
        return RespawnPosition;
    }

    public virtual void AddLife(float ammount){
        Life += ammount;
    }
    public virtual void RemoveLife(float ammount){
        Life -= ammount;   
    }
    public virtual float GetLife(){
        return Life;
    }

    public void OnTouch()
    {
        throw new NotImplementedException();
    }

    public RaycastHit2D BottomCollisionRayHit(LayerMask layers)
    {
        return Physics2D.Raycast(BoundingBox.bounds.min, Vector3.right, BoundingBox.bounds.size.x, layers);
    }

    public RaycastHit2D TopCollisionRayHit(LayerMask layers)
    {
        return Physics2D.Raycast(BoundingBox.bounds.max, Vector3.left, BoundingBox.bounds.size.x, layers);
    }

    public RaycastHit2D LeftCollisionRayHit(LayerMask layers)
    {
        return Physics2D.Raycast(BoundingBox.bounds.min, Vector3.up, BoundingBox.bounds.size.y, layers);
    }

    public RaycastHit2D RightCollisionRayHit(LayerMask layers)
    {
        return Physics2D.Raycast(BoundingBox.bounds.max, Vector3.down, BoundingBox.bounds.size.y, layers);
    }

    public bool IsBottomCollision(LayerMask layers)
    {
        return BottomCollisionRayHit(layers).transform != null;
    }

    public bool IsTopCollision(LayerMask layers)
    {
        return TopCollisionRayHit(layers).transform != null;
    }

    public bool IsLeftCollision(LayerMask layers)
    {
        return LeftCollisionRayHit(layers).transform != null;
    }

    public bool IsRightCollision(LayerMask layers)
    {
        return RightCollisionRayHit(layers).transform != null;
    }
}
