using UnityEngine;

public class ramMechanic : MonoBehaviour
{
    [SerializeField] private BoxCollider _ramcollider;
    private PlayerMovement _playerMovement;
    private PlayerMovement _OtherPLayerMovement;
    Vector3 pushDir;
    private bool canRam;


    private float CDbase = 4f;
    private float Cooldown;

    void Start()
    {
        Cooldown = CDbase;
        canRam = false;
        _playerMovement = GetComponent<PlayerMovement>();
        
        _ramcollider.enabled = false;
    }

   
    void Update()
    {
        if(_playerMovement.isRamming == true)
        {
            _ramcollider.enabled = true;
        }
        else
        {
            _ramcollider.enabled = false;
        }
        //cooldown functionality
        if (canRam == false)
        {
            Cooldown = Cooldown - Time.deltaTime;
            if(Cooldown <= 0)
            {
                canRam =true;
                Cooldown = CDbase;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (canRam == true && _playerMovement.BeingKnockedBack == false)
        {
            if (other.gameObject.CompareTag("Player") && other != this)
            {
                if (other.TryGetComponent<PlayerMovement>(out PlayerMovement otherMovement))
                {
                    _OtherPLayerMovement = other.GetComponent<PlayerMovement>();
                    pushDir = this.transform.forward;
                    _OtherPLayerMovement.KnockBack(10, pushDir);
                    otherMovement.BeingKnockedBack = true;
                    
                }
                else
                {
                    return;
                }
            }
        }
    }


}
