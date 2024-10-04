using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using UnityEngine;


[BurstCompile]
public partial struct BulletSystem : ISystem
{

    [BurstCompile]
    private void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(Allocator.TempJob); // Sử dụng ECB để thực thi lệnh hủy entity sau job

        // Tạo một job để cập nhật vị trí và thời gian sống của đạn
        var bulletJob = new BulletJob
        {
            deltaTime = SystemAPI.Time.DeltaTime,
            ecb = ecb.AsParallelWriter()
        };

        // Lên lịch job
        JobHandle jobHandle = bulletJob.ScheduleParallel(state.Dependency);

        // Đảm bảo job hoàn tất trước khi thực thi ECB
        jobHandle.Complete();

        // Playback ECB để thực thi lệnh hủy các entity đã hết thời gian sống
        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }

}

[BurstCompile]
public partial struct BulletJob : IJobEntity
{
    public float deltaTime;
    public EntityCommandBuffer.ParallelWriter ecb;

    public void Execute(Entity entity, [EntityIndexInQuery] int entityIndex, ref LocalTransform bulletTransform, ref BulletComponent bulletComponent, ref BulletLifeTimeComponent bulletLifeTimeComponent)
    {
        // Cập nhật vị trí viên đạn dựa trên hướng và tốc độ
        bulletTransform.Position += bulletComponent.speed * deltaTime * bulletTransform.Forward();

        // Giảm thời gian sống còn lại của viên đạn
        bulletLifeTimeComponent.remainingLifeTime -= deltaTime;

        // Nếu thời gian sống của viên đạn đã hết, tiêu diệt nó
        if (bulletLifeTimeComponent.remainingLifeTime <= 0f)
        {
            ecb.DestroyEntity(entityIndex, entity);
        }
    }
}
