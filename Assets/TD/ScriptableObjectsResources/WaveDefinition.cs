using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New WaveDefinition", menuName = "Create Scriptable Object/WaveDefinition")]
public class WaveDefinition : ScriptableObject
{
    public List<EnemyGroup> enemyGroups;
    public int waveReward;
}


[System.Serializable]
public class EnemyGroup
{
    public int enemyID;
    public int count;
    public float spawnIntervalBetweenMembers;
    public float delayBeforeStartGroup;
}

