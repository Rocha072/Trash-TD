using UnityEngine;

[CreateAssetMenu(fileName = "New Tower", menuName = "Create Scriptable Object/Towers")]
public class TowerData : ScriptableObject
{
    public Sprite TowerSprite;

    [Header("Base Stats")]
    public float baseRange;
    public float baseDamage;
    public float baseFireRate;
    public float baseSlowFactor;
    public float baseSlowDuration;

    [Header("Tower Traits")]
    public int cost;
    public float turnSpeed;
    public enum TowerTypes {
        waterGun,
    }
    public TowerTypes towerType;

    [Header("Upgrade Paths")]
    public UpgradeDefinition[] pathA_Upgrades = new UpgradeDefinition[3]; 
    public UpgradeDefinition[] pathB_Upgrades = new UpgradeDefinition[3];

}
