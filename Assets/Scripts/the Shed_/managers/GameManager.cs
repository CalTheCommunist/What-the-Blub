using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.InputSystem;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

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
    public List<PlayerMovement> playerRefList = new List<PlayerMovement>{};

    // game control variables
    public bool GameStarted;

    // object pools for pickups
    // maybe move pooling to its own manager?
    private ObjectPool<GameObject> pistolPool;
    private ObjectPool<GameObject> pufferPool;
    private readonly int poolPickupCap = 8;

    private void Awake()
    {
        if (call == null)
        {
            // set instance to self if self is only instance
            call = this;
        }
        else if (call != null)
        {
            // if more than one instance then destroy other instances
            call = this;
        }
        GameStarted = false;
    }

    private void DeviceChange(InputDevice inputDevice, InputDeviceChange inputChange)
    {
        CountGamepads();
    }

    private void OnEnable()
    {
        InputSystem.onDeviceChange += DeviceChange;
    }

    private int CountGamepads()
    {
        // this directly queries the system for how many gamepads are connected
        int GamepadCount = InputSystem.devices.OfType<Gamepad>().Count();
        // return gamepad amount as integer
        return GamepadCount;
    }
    public void LoadMainGame()
    {
        SceneManager.LoadScene("MainScene");
    }
}
