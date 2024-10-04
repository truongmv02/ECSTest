using Unity.Entities;
using Unity.Mathematics;
public struct InputComponent : IComponentData
{
    public float2 Movememt;
    public float2 MousePosition;

    public bool Shoot;
}
