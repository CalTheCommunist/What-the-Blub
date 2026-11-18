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
    public GameObject pickup_PufferRef;

    // player references
    [SerializeField] private GameObject player_Player1Ref;

    // commented out because they are not being used right now
//    private ObjectPool<GameObject> pistolPool;
//    private ObjectPool<GameObject> pufferPool;
//    private readonly int poolCap = 8;

    private void Awake()
    {
        if (call == null)
        {
            // set instance to self if self is only instance
            call = this;
        }
        else
        {
            // if more than one instance then destroy other instances
            Destroy(gameObject);
        }
    }
}
