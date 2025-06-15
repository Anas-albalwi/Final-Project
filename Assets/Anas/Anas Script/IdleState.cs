using UnityEngine;

public class IdleState : IEnemyState
{
    private Enemy enemy;
    private float idleDuration;
    private float timer;

    private float minWaitTime = 5f;
    private float maxWaitTime = 15f;

    public EnemyStateType GetStateType() => EnemyStateType.Idle;

    public void EnterState(Enemy enemy)
    {
        this.enemy = enemy;

        // Random delay before going back to patrol
        idleDuration = Random.Range(minWaitTime, maxWaitTime);
        timer = idleDuration;

        enemy.agent.isStopped = true;

        enemy.animator.SetBool("isIdle", true);
        enemy.animator.SetBool("isWalking", false);
        enemy.animator.SetBool("isChasing", false);
    }

    public void UpdateState()
    {
        if (enemy.PlayerInRange())
        {
            enemy.SwitchState(enemy.chaseState);
            return;
        }
        
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            enemy.SwitchState(enemy.patrolState);
        }
    }

    public void ExitState()
    {
        enemy.agent.isStopped = false;
    }
}
