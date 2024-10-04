using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;

public partial class InputSystem : SystemBase
{
    private ControlsECS controls;
    protected override void OnCreate()
    {
        if (!SystemAPI.TryGetSingleton(out InputComponent input))
        {
            EntityManager.CreateEntity(typeof(InputComponent));
        }
        controls = new ControlsECS();
        controls.Enable();
    }
    protected override void OnUpdate()
    {
        Vector2 moveVector = controls.Player.Movement.ReadValue<Vector2>();
        Vector2 mousePosition = controls.Player.MousePosition.ReadValue<Vector2>();
        bool shoot = controls.Player.Shoot.IsPressed();

        SystemAPI.SetSingleton(new InputComponent
        {
            MousePosition = mousePosition,
            Movememt = moveVector,
            Shoot = shoot
        });
    }
}
