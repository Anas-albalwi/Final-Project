using UnityEngine;
using UnityEngine.AI;

public class ChaseState : IEnemyState
{
    private Enemy chef;
    private Transform player;

    private float giveUpTimer = 0f;
    private float giveUpDelay;

    public EnemyStateType GetStateType() => EnemyStateType.Chase;

    public void EnterState(Enemy enemy)
    {
        chef = enemy;
        chef.agent.speed = chef.chaseSpeed;
        player = chef.playerTransform;
        giveUpTimer = 0f;

        giveUpDelay = Random.Range(chef.minGiveUpTime, chef.maxGiveUpTime);
    }

    public void UpdateState()
    {
        if (player == null) return;

        // Move toward nearest ground point under/near player
        Vector3 playerXZ = new Vector3(player.position.x, chef.transform.position.y, player.position.z);
        NavMeshHit hit;

        if (NavMesh.SamplePosition(playerXZ, out hit, 3f, NavMesh.AllAreas))
        {
            chef.agent.SetDestination(hit.position);
        }
        else
        {
            // fallback if sampling fails
            chef.agent.SetDestination(playerXZ);
        }

        // Check if player is still visible
        if (chef.PlayerInRange())
        {
            giveUpTimer = 0f;
        }
        else if (chef.canGiveUp)
        {
            giveUpTimer += Time.deltaTime;
            if (giveUpTimer >= giveUpDelay)
            {
                chef.SwitchState(chef.idleState);
                return;
            }
        }

        // Switch to attack if close enough
        float distance = Vector3.Distance(chef.transform.position, player.position);
        if (distance < chef.attackTriggerDistance)
        {
            chef.SwitchState(chef.attackState);
        }

        
        chef.animator.SetBool("Walk", true);
        chef.animator.SetBool("isCrouchChasing", false);
        chef.animator.SetBool("isIdle", false);
        chef.animator.SetBool("isWalking", false);
        
    }

    public void ExitState()
    {
        giveUpTimer = 0f;
    }
}
