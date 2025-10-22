using UnityEngine;

[CreateAssetMenu(fileName = "New EnemyData", menuName = "Create EnemyData")]
public class EnemyData : ScriptableObject
{
    public int dropAmount;
    public int ID;
    public float MaxHealth;
    public float MaxSpeed;
    public float MinDistanceToPassNode;
}
