using UnityEngine;

public class ArenaController : MonoBehaviour
{
    public static ArenaController Instance { get; private set; }

    public Transform playersContainer;
    public Transform bombsContainer;
    public Transform explosionsContainer;
    public Transform powerUpsContainer;
    public Transform solidBlocksContainer;
    public Transform weekBlocksContainer;

    public GOPoolSystem bombPoolSystem;
    public GOPoolSystem explosionPoolSystem;

    public const float HORIZONTAL_SIZE = 0.16f;
    public const float VERTICAL_SIZE = 0.16f;

    private void Awake()
    {
        Instance = this;
    }

    public void InstanciateBomb(Vector2 position, BoxCollider2D playerCollider)
    {
        GameObject instance = bombPoolSystem.Get(position, Vector2.one, Quaternion.identity);
        instance.GetComponent<Bomb>().PlayerCollider = playerCollider;
    }

    public void InstanciateExplosion(Vector2 position)
    {
        explosionPoolSystem.Get(position + new Vector2(0.08f, -0.08f), Vector2.one, Quaternion.identity);
    }

    public static Rect GetTileRect(int x, int y)
    {
        Vector2 p = GetTilePosition(x, y);
        return new Rect(p.x, p.y, p.x + HORIZONTAL_SIZE, p.y + VERTICAL_SIZE);
    }

    public static Vector2 GetTilePosition(int x, int y)
    {
        return new Vector2(x * HORIZONTAL_SIZE, y * VERTICAL_SIZE);
    }

    public static Vector2 GetTilePosition(Point point)
    {
        return GetTilePosition(point.x, point.y);
    }

    public static Point GetTilePoint(Vector2 position)
    {
        return new Point((int)(position.x / HORIZONTAL_SIZE), (int)(position.y / VERTICAL_SIZE));
    }

    
}
