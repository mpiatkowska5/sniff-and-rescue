using UnityEngine;
using UnityEngine.InputSystem;

public class JumpBlock : MonoBehaviour
{
    InputSystem_Actions input;

    void Start()
    {
        Debug.Log("should disable shit by now");
        input = new InputSystem_Actions();
        
        input.Disable();
    }
}
