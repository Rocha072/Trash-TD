using UnityEngine;

[CreateAssetMenu(fileName = "New Tower", menuName = "Create Scriptable Object/Towers")]
public class TowerData : ScriptableObject
{
    public int cost;
    public float range;
    public float turnSpeed;
    public float Damage;
    public float fireRate;
    public float slowFactor;
    public float slowDuration;
    public enum TowerTypes {
        waterGun,
    }
    public TowerTypes towerType;

    public Sprite TowerSprite;
}
