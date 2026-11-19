using UnityEngine;

[CreateAssetMenu(fileName = "pickupPistol", menuName = "Pistol Shrimp Pickup")]
public class PickupPistol : ScriptableObject
{
    // these are just placeholders and will have actual ones later
    public float A;
    public float B;

    private void Awake()
    {
        if (GameManager.call.GameStarted == true)
        {
            Debug.Log("i am active");
        }
        else
        {
            Debug.Log("i am inactive");
        }
    }

    void Shoot()
    {
        return;
    }
}
