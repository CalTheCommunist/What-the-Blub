using UnityEngine;

public class GameManager : MonoBehaviour
{
    // named 'call' for convenience, because you are calling it
    public static GameManager call;

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
    }
}
