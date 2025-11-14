using UnityEngine;

[CreateAssetMenu(fileName = "New Tower", menuName = "Create Scriptable Object/Towers")]
public class TowerData : ScriptableObject
{

    [Header("Base Stats")]
    public float baseRange;
    public float baseDamage;
    public float baseFireRate;
    public float baseSlowFactor = 1f;
    public float baseSlowDuration;
    public float baseStunDuration;

    [Header("Tower Traits")]
    public Sprite TowerSprite;
    public string towerName;
    [TextArea(3, 5)]
    public string towerDescription;
    public int towerID;
    public int cost;
    public float turnSpeed;
    public bool requiresTarget = true;
    public bool isCollector = false;

    [Header("Upgrade Paths")]
    public UpgradeDefinition[] pathA_Upgrades = new UpgradeDefinition[3]; 
    public UpgradeDefinition[] pathB_Upgrades = new UpgradeDefinition[3];

    [Header("Sounds")]
    public AudioClip placeSound;
    public float placeSoundVolume = 1f;
    public AudioClip attackSound;
    public float attackSoundVolume = 1f;
    public bool attackSoundLoop = false;
}
