using UnityEngine;

[CreateAssetMenu(fileName = "New EnemyData", menuName = "Create Scriptable Object/EnemyData")]
public class EnemyData : ScriptableObject
{
    public int dropAmount;
    public int damage;
    public int ID;
    public float MaxHealth;
    public float MaxSpeed;
    public float MinDistanceToPassNode;
}
