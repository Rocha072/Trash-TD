using UnityEngine;

[CreateAssetMenu(fileName = "Towers", menuName = "Create Scriptable Objects/Towers")]
public class TowerData : ScriptableObject
{
    public float range;
    public float turnSpeed;
    public float Damage;
    public float fireRate;
    public float slowFactor;
    public float slowDuration;
    public enum TowerTypes {
        waterGun = 1
    }
    public TowerTypes towerType;
}
