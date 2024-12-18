using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SLS.StateMachineV2;
using System;
using Cinemachine;

public class PlayerStateMachine : StateMachine
{
    #region Config

    #endregion

    #region Data
    [HideInInspector] public Animator animator;
    [HideInInspector] public Input input;
    [HideInInspector] public PlayerMovementBody body;
    [HideInInspector] public PlayerController controller;
    public Transform cameraTransform;
    public CinemachineFreeLook freeLookCamera;

    #endregion

    protected override void Initialize()
    {
        animator = GetComponent<Animator>();
        input = Input.Get();
        body = GetGlobalBehavior<PlayerMovementBody>();
        controller = GetGlobalBehavior<PlayerController>();
        whenInitializedEvent?.Invoke(this);

        // Initialize the Cinemachine FreeLook camera
        freeLookCamera = FindObjectOfType<CinemachineFreeLook>();
        if (freeLookCamera != null)
        {
            freeLookCamera.Follow = transform;
            freeLookCamera.LookAt = transform;
        }
    }

    [SerializeField]
    private AYellowpaper.SerializedCollections.SerializedDictionary<string, bool> upgrades = new()
    {
        { "GroundSlam", true },
        { "DropLaunch", true },
        { "WallJump", true },
        { "RageCharge", true }
    }; 
    public bool uGroundSlam => upgrades["GroundSlam"];
    public bool uDropLaunch => upgrades["DropLaunch"];
    public bool uWallJump => upgrades["WallJump"];
    public bool uRageCharge => upgrades["RageCharge"];
    public void SetUpgrade(string id, bool value) => upgrades[id] = value; 


    public static Action<PlayerStateMachine> whenInitializedEvent;

}
public abstract class PlayerStateBehavior : StateBehavior
{
    [HideInInspector] public new PlayerStateMachine M;
    [HideInInspector] public Input input;
    [HideInInspector] public PlayerMovementBody body;
    [HideInInspector] public PlayerController controller;

    protected override void Initialize(StateMachine machine)
    {
        M = machine as PlayerStateMachine;
        input = M.input;
        body = M.body;
        controller = M.controller;
    }


    #region States

    public State sGrounded => M.states["Grounded"];
    public State sIdleWalk => M.states["IdleWalk"];
    public State sCharge => M.states["Charge"];
    public State sAirborne => M.states["Airborne"];
    public State sJump1 => M.states["Jump1"];
    public State sJump2 => M.states["Jump2"];
    public State sFall => M.states["Fall"];
    public State sGlide => M.states["Glide"];
    public State sWallJump => M.states["WallJump"];
    public State sGroundSlam => M.states["GroundSlam"];
    public State sBounce => M.states["Bounce"];
    public State sAirCharge => M.states["AirCharge"];
    public State sAirChargeFall => M.states["AirChargeFall"];

    #endregion

}
