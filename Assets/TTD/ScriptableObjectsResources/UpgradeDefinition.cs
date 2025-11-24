using UnityEngine;

[CreateAssetMenu(fileName = "New UpgradeDefinition", menuName = "Create Scriptable Object/UpgradeDefinition")]
public class UpgradeDefinition : ScriptableObject
{
    public string upgradeName;
    [TextArea(3,5)]
    public string upgradeDescription;
    public int upgradeCost;

    [Header("Mudanças de Stats")]
    public UpgradeStats statModifiers;

    //Colocar outras coisas que podem mudar
}


[System.Serializable]
public class UpgradeStats
{
    //Outros estados que podem ser incrementados
    public float damage_add = 0;
    public float damage_multiplier = 1.0f;
    public float range_add = 0;
    public float fireRate_multiplier = 1.0f;
    public float slowFactor_multiplier = 1.0f;
    public float slowDuration_add = 0;

    public float StunDuration_add = 0;
    public float GenerateRate_add = 0;
    public float GenerateRate_multiply = 1.0f;
    
}
