using Unity.Entities;

public struct PlayerComponent : IComponentData
{
    public float moveSpeed;
    public Entity bulletPrefab;
    public int numOfBulletToSpawn;
    public float bulletSpread;

}