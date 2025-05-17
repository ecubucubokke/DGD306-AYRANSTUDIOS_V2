using UnityEngine;
using UnityEngine.InputSystem;

public class RebindMenuManager : MonoBehaviour
{
    public InputActionReference MoveRef, jumpRef, fireRef;
    public InputActionReference MoveRef2, jumpRef2, fireRef2;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void OnEnable()
    {
        MoveRef.action.Disable();
        jumpRef.action.Disable();
        fireRef.action.Disable();
        if (MoveRef2 != null) MoveRef2.action.Disable();
        if (jumpRef2 != null) jumpRef2.action.Disable();
        if (fireRef2 != null) fireRef2.action.Disable();
    }

    private void OnDisable()
    {
        MoveRef.action.Enable();
        jumpRef.action.Enable();
        fireRef.action.Enable();
        if (MoveRef2 != null) MoveRef2.action.Enable();
        if (jumpRef2 != null) jumpRef2.action.Enable();
        if (fireRef2 != null) fireRef2.action.Enable();
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
