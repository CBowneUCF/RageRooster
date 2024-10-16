using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Button = UnityEngine.InputSystem.InputAction;

public class Input : Staticon<Input>
{
	public PlayerActions asset;



	public override void Awake()
	{
		asset = new();
		asset.Enable();
	}

	public static Vector2 Movement => Get().asset.Gameplay.Movement.ReadValue<Vector2>();
	public static Vector2 Camera => Get().asset.Gameplay.Camera.ReadValue<Vector2>();
	public static Button Jump => Get().asset.Gameplay.Jump;
	public static Button Attack => Get().asset.Gameplay.Attack;
	public static Button Block => Get().asset.Gameplay.Block;
	public static Button Grab => Get().asset.Gameplay.Grab;
	public static Button ShootMode => Get().asset.Gameplay.ShootMode;
	public static Button Shoot => Get().asset.Gameplay.Shoot;
	public static Button Charge => Get().asset.Gameplay.Charge;
	public static Button Sonic => Get().asset.Gameplay.Sonic;


}