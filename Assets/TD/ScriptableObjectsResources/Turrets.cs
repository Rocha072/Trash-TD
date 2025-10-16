using UnityEngine;

[CreateAssetMenu(fileName = "Turrets", menuName = "Create Scriptable Objects/Turrets")]
public class Turrets : ScriptableObject
{
    public float range;
    public float turnSpeed;
    public float Damage;
    public float fireRate;
    public float slowFactor;
    public float slowDuration;
    public enum TurretTypes {
    waterGun = 1
    }
    public TurretTypes turretType;
}
