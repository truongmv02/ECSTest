using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Rendering;


[BurstCompile]
public partial struct SpawnCapsuleSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {

        state.Enabled = false;
        if (!SystemAPI.TryGetSingletonEntity<CapsulePropertiesComponent>(out Entity capsuleEntity))
        {
            return;
        }
        
        var spanwer = SystemAPI.GetComponentRW<CapsulePropertiesComponent>(capsuleEntity);


        EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);

        for (int i = 0; i < spanwer.ValueRO.numberCapsuleToSpawn; i++)
        {



            // Entity newEntity = state.EntityManager.Instantiate(spanwer.ValueRO.capsulePrefab);

            // var transforms = SystemAPI.GetComponentRW<LocalTransform>(newEntity);
            // transforms.ValueRW.Position = new float3(UnityEngine.Random.Range(-20f, 20f), 1, UnityEngine.Random.Range(0, 20f));
            var buffer = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(World.DefaultGameObjectInjectionWorld.Unmanaged);
            var entity = buffer.Instantiate(spanwer.ValueRO.capsulePrefab);

            buffer.SetComponent(entity, new LocalTransform
            {
                Scale = 1,
                Rotation = Unity.Mathematics.quaternion.identity,
                Position = new float3(UnityEngine.Random.Range(-20f, 20f), 1, UnityEngine.Random.Range(0, 20f))
            });



            // Entity newEntity = ecb.Instantiate(spanwer.ValueRO.capsulePrefab);
            // ecb.SetComponent(newEntity, new LocalTransform
            // {
            //     Scale = 1,
            //     Rotation = Unity.Mathematics.quaternion.identity,
            //     Position = new float3(UnityEngine.Random.Range(-20f, 20f), 1, UnityEngine.Random.Range(0, 20f))
            // });

        }
        // ecb.Playback(state.EntityManager);
    }
}