using UnityEngine;
using UnityEngine.Pool;

public class GameManager : MonoBehaviour
{
    // very important to the game
    [Tooltip("calls GameManager to use one of its functions")]
    public static GameManager call;
    [Tooltip("GameManager reference to Pistol Shrimp GameObject")]
    public GameObject pickup_PistolRef;
    [Tooltip("GameManager reference to Pufferfish GameObject")]
    public GameObject pickup_Puffer;

    private ObjectPool<GameObject> pistolPool;
    private readonly int poolCap = 8;

    private void Awake()
    {
        if (call == null)
        {
            // set instance to self
            call = this;
        }
        else
        {
            // if more than one instance then destroy other instances
            Destroy(gameObject);
        }

        pistolPool = new(createFunc: MakeObject, actionOnGet: Grab,
                        actionOnRelease: Flush, actionOnDestroy: RemoveObject,
                        collectionCheck: true, defaultCapacity: poolCap, maxSize: poolCap);
    }

    private void Update()
    {
        if (pistolPool.CountActive > poolCap)
        {
            return;
        }
    }

    private GameObject MakeObject()
    {
        GameObject object_to_make = Instantiate(pickup_PistolRef);
        object_to_make.SetActive(false);
        return object_to_make;
    }

    private void Grab(GameObject pooled_pistol)
    {
        pooled_pistol.SetActive(true);
    }

    private void Flush(GameObject pooled_pistol)
    {
        pooled_pistol.SetActive(false);
    }

    private void RemoveObject(GameObject object_to_remove)
    {
        Destroy(object_to_remove);
    }
}
