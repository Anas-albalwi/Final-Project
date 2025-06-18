using UnityEngine;
using System.Collections;

public class PatrolState : IEnemyState
{
    private Enemy enemy;
    private Vector3 targetPoint;

    public EnemyStateType GetStateType() => EnemyStateType.Patrol;

    public void EnterState(Enemy enemy)
    {
        this.enemy = enemy;
        enemy.agent.speed = enemy.patrolSpeed;

        if (enemy.useWaypointPatrol && enemy.patrolWaypoints.Count > 0)
        {
            int nextIndex;
            do
            {
                nextIndex = Random.Range(0, enemy.patrolWaypoints.Count);
            } while (nextIndex == enemy.currentWaypointIndex);

            enemy.currentWaypointIndex = nextIndex;
            targetPoint = enemy.patrolWaypoints[enemy.currentWaypointIndex].position;
        }
        else
        {
            targetPoint = enemy.GetRandomPatrolPoint();
        }

        enemy.agent.SetDestination(targetPoint);

        enemy.animator.SetBool("Walk", true);
        enemy.animator.SetBool("isIdle", false);
        enemy.animator.SetBool("isChasing", false);
        enemy.animator.SetBool("isCrouchChasing", false);
        
    }

    public void UpdateState()
    {
        if (!enemy.agent.pathPending && enemy.agent.remainingDistance < 0.5f)
        {
            if (enemy.useWaypointPatrol && enemy.patrolWaypoints.Count > 0)
            {
                enemy.agent.isStopped = true;
                enemy.enemyMono.StartCoroutine(RotateThenIdle(enemy));
            }
            else
            {
                enemy.SwitchState(enemy.idleState);
            }
        }

        if (enemy.PlayerInRange())
        {
            enemy.SwitchState(enemy.chaseState);
            return;
        }
    }

    public void ExitState() { }

    private IEnumerator RotateThenIdle(Enemy enemy)
    {
        Transform currentWaypoint = enemy.patrolWaypoints[enemy.currentWaypointIndex];
        Vector3 targetDir = currentWaypoint.forward;
        targetDir.y = 0f;

        Quaternion targetRot = Quaternion.LookRotation(targetDir);

        while (Quaternion.Angle(enemy.transform.rotation, targetRot) > 0.5f)
        {
            enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, targetRot, Time.deltaTime * 5f);
            yield return null;
        }

        enemy.agent.isStopped = false;
        enemy.SwitchState(enemy.idleState);
    }

}
