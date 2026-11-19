using UnityEngine;

public abstract class StageHazard : MonoBehaviour
{
    [Tooltip("the box collider for triggering the hazard")]
    [SerializeField] protected BoxCollider m_triggerbox;

    void Start()
    {
        Debug.Log("i am a stage hazard");
    }
}
