using UnityEngine;

[CreateAssetMenu(fileName = "New UpgradeDefinition", menuName = "Create Scriptable Object/UpgradeDefinition")]
public class UpgradeDefinition : ScriptableObject
{
    public string upgradeName;
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
    public float range_add = 0;
    public float fireRate_multiplier = 1.0f;
    public float slowFactor_multiplier = 1.0f;

    public float slowDuration_add = 0;
    
}
