using UnityEngine;

public interface IEnemyDeathListener
{
    void OnEnemyDeath(Tower killer, Enemy killedEnemy);
}


// public interface IBuffAura
// {
//     void ApplyBuff(Tower target);
// }