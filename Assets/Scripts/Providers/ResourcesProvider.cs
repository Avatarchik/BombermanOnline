using UnityEngine;

public class ResourcesProvider : MonoBehaviour
{
    public string explosionTextureName;

    public static ResourcesProvider Instance { get; private set; }
    public Sprite[] ExplosionSprites { get; private set; }

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this.gameObject);

        Register();
    }

    private void Register()
    {
        ExplosionSprites = Resources.LoadAll<Sprite>(explosionTextureName);
    }
}
