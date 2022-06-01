using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBasicMovement : MonoBehaviour
{
    private PlayerMovement_Controls playerControls;
    private InputAction groundMovement;
  
    void Start()
    {
        
    }
    private void Awake()
    {
        playerControls = new PlayerMovement_Controls();
    }
    private void OnEnable()
    {      
        groundMovement = playerControls.Player.GroundMovement;
        groundMovement.Enable();
        playerControls.Player.Jump.performed += playerJump;
        playerControls.Player.Jump.Enable();       
    }
    private void OnDisable()
    {
        groundMovement.Disable();
    }
    private void FixedUpdate()
    {
        Debug.Log("MovementValues" + groundMovement.ReadValue<float>());
    }
    void Update()
    {
        
    }
    private void playerJump(InputAction.CallbackContext obj)
    {
        Debug.Log("I jumped!");
    }


}
