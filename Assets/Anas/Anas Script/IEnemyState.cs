using UnityEngine;

public interface IEnemyState
{
    EnemyStateType GetStateType();
    void EnterState(Enemy enemy);
    void UpdateState();
    void ExitState();
}
