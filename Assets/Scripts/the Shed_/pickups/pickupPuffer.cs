using UnityEngine;

[CreateAssetMenu(fileName = "pickupPuffer", menuName = "Pufferfish Pickup")]
public class PickupPuffer : ScriptableObject
{
    // once again these are just placeholders
    public float A;
    public float B;

    private void Awake()
    {
        if(GameManager.call.GameStarted == true)
        {
            Debug.Log("puffer A");
        }
        else
        {
            Debug.Log("puffer B");
        }
    }

    void Blow()
    {
        return;
    }
}
