using UnityEngine;

public class ramMechanic : MonoBehaviour
{
    [SerializeField] private BoxCollider _ramcollider;
    private PlayerMovement _playerMovement;
    private PlayerMovement _OtherPLayerMovement;
    Vector3 pushDir;


    private float CDbase = 3f;

    void Start()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        
        _ramcollider.enabled = false;
    }

   
    void Update()
    {
        if (_playerMovement.isRamming == true)
        {
            _ramcollider.enabled = true;
        }
        else if (_playerMovement.isRamming == false)
        {
            _ramcollider.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_playerMovement.isRamming == true)
        {
            if (other.gameObject.CompareTag("Player") && other != this)
            {
                if (other.TryGetComponent<PlayerMovement>(out PlayerMovement otherMovement))
                {
                    _OtherPLayerMovement = other.GetComponent<PlayerMovement>();
                    pushDir = this.transform.forward;
                    otherMovement.KnockBack(10, pushDir);
                }
                else
                {
                    return;
                }
            }
        }
    }


}
