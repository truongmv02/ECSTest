using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class PlayerAuthoring : MonoBehaviour
{
    public float moveSpeed = 5f;
    public GameObject bulletPrefab;
    public int numOfbulletToSpawn = 50;

    [Range(0, 10f)]
    public float bulletSpread = 5f;


}
public class PlayerBaker : Baker<PlayerAuthoring>
{
    public override void Bake(PlayerAuthoring authoring)
    {
        Entity playerEntity = GetEntity(TransformUsageFlags.Dynamic);
        AddComponent(playerEntity, new PlayerComponent
        {
            moveSpeed = authoring.moveSpeed,
            numOfBulletToSpawn = authoring.numOfbulletToSpawn,
            bulletSpread = authoring.bulletSpread,
            bulletPrefab = GetEntity(authoring.bulletPrefab, TransformUsageFlags.None)
        });
    }
}