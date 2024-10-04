// using Unity.Burst;
// using Unity.Entities;
// using Unity.Physics;
// using Unity.Transforms;
// using Unity.Mathematics;
// using System.Diagnostics;

// [BurstCompile]
// public partial struct FreezeRotationSystem : ISystem
// {
//     [BurstCompile]
//     public void OnCreate(ref SystemState state)
//     {
//         // Đảm bảo hệ thống chạy sau các hệ thống vật lý
//         state.RequireForUpdate<PhysicsMass>();
//     }

//     [BurstCompile]
//     public void OnDestroy(ref SystemState state)
//     {
//         // Không cần thực hiện gì khi hệ thống bị hủy
//     }

//     [BurstCompile]
//     public void OnUpdate(ref SystemState state)
//     {
//         // Thời gian delta của frame hiện tại
//         float deltaTime = SystemAPI.Time.DeltaTime;
//         Debug.WriteLine(123);
//         // Truy vấn các entity có chứa PhysicsMass và PhysicsVelocity
//         foreach (var (mass, velocity) in SystemAPI.Query<RefRW<PhysicsMass>, RefRO<PhysicsVelocity>>())
//         {
//             // Khóa xoay quanh trục X và Z để ngăn capsule bị đổ
//             mass.ValueRW.InverseInertia.x = 0f;
//             mass.ValueRW.InverseInertia.z = 0f;
//         }
//     }
// }