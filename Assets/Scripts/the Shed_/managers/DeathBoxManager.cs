using UnityEngine;

public class DeathBoxManager : MonoBehaviour
{
    private BoxCollider m_killerbox;

    private void Awake()
    {
        m_killerbox = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.call.playerRefList.Remove(other.gameObject.GetComponent<PlayerMovement>());
            other.gameObject.SetActive(false);
        }
    }
}
