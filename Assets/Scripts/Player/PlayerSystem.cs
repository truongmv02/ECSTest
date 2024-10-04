using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
public partial struct PlayerSystem : ISystem
{
    private EntityManager entityManager;
    private Entity playerEntity;
    private Entity inputEntity;
    private PlayerComponent playerComponent;
    private InputComponent inputComponent;
    private float timeCountDown;
    private double lastShootTime;

    [BurstCompile]
    void OnCreate(ref SystemState state)
    {
        timeCountDown = 0.1f;
    }

    [BurstCompile]
    private void OnUpdate(ref SystemState state)
    {

        if (!SystemAPI.TryGetSingletonEntity<PlayerComponent>(out playerEntity))
        {
            return;
        }
        entityManager = state.EntityManager;
        inputEntity = SystemAPI.GetSingletonEntity<InputComponent>();

        playerComponent = entityManager.GetComponentData<PlayerComponent>(playerEntity);
        inputComponent = entityManager.GetComponentData<InputComponent>(inputEntity);

        Move(ref state);

        if (SystemAPI.Time.ElapsedTime >= lastShootTime + timeCountDown)
        {
            lastShootTime = SystemAPI.Time.ElapsedTime;
            Shoot(ref state);
        }

    }

    [BurstCompile]
    private void Move(ref SystemState state)
    {
        LocalTransform playerTransform = entityManager.GetComponentData<LocalTransform>(playerEntity);

        float3 moveDirection = new float3(inputComponent.Movememt.x, 0f, inputComponent.Movememt.y);
        playerTransform.Position += moveDirection * playerComponent.moveSpeed * SystemAPI.Time.DeltaTime;

        Vector3 dir = (Vector2)inputComponent.MousePosition - (Vector2)Camera.main.WorldToScreenPoint(playerTransform.Position);
        float angle = math.degrees(math.atan2(dir.y, dir.x)) - 90f;
        playerTransform.Rotation = Quaternion.AngleAxis(angle, Vector3.down);
        entityManager.SetComponentData(playerEntity, playerTransform);
    }

    [BurstCompile]
    private void Shoot(ref SystemState state)
    {
        if (inputComponent.Shoot)
        {
            for (int i = 0; i < playerComponent.numOfBulletToSpawn; i++)
            {
                EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);

                Entity bulletEntity = entityManager.Instantiate(playerComponent.bulletPrefab);

                ecb.AddComponent(bulletEntity, new BulletComponent
                {
                    speed = 10f
                });

                ecb.AddComponent(bulletEntity, new BulletLifeTimeComponent
                {
                    remainingLifeTime = 3f
                });

                LocalTransform bulletTransform = entityManager.GetComponentData<LocalTransform>(bulletEntity);
                LocalTransform playerTransform = entityManager.GetComponentData<LocalTransform>(playerEntity);

                bulletTransform.Rotation = playerTransform.Rotation;
                float randomOffset = UnityEngine.Random.Range(-playerComponent.bulletSpread, playerComponent.bulletSpread);
                bulletTransform.Position = playerTransform.Position + (playerTransform.Forward() * 1.65f) + bulletTransform.Right() * randomOffset;

                ecb.SetComponent(bulletEntity, bulletTransform);
                ecb.Playback(entityManager);
                ecb.Dispose();
            }
        }
    }
}