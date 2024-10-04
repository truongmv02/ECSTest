using Unity.Entities;
using Unity.Mathematics;

public struct CapsulePropertiesComponent : IComponentData
{
    public float3 fieldDimensions;
    public int numberCapsuleToSpawn;
    public Entity capsulePrefab;
}