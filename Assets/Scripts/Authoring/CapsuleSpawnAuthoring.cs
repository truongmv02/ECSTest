 using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class CapsuleSpawnAuthoring : MonoBehaviour
{
    public float3 fieldDimensions;
    public int mumberCapsuleToSpawn;
    public GameObject capsulePrefab;
    public uint randomSpeed;
}

public class CapusleSpawnBaker : Baker<CapsuleSpawnAuthoring>
{
    public override void Bake(CapsuleSpawnAuthoring authoring)
    {
        Entity entity = GetEntity(TransformUsageFlags.None);

        AddComponent(entity, new CapsulePropertiesComponent
        {
            fieldDimensions = authoring.fieldDimensions,
            numberCapsuleToSpawn = authoring.mumberCapsuleToSpawn,
            capsulePrefab = GetEntity(authoring.capsulePrefab, TransformUsageFlags.Dynamic)
        });

    }
}
