using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager call;

    private void Awake()
    {
        if (call == null)
        {
            call = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
