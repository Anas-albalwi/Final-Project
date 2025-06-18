using System.Collections;
using UnityEngine;

public class AttackState : IEnemyState
{
    private Enemy enemy;
    private Transform player;
    private float attackCooldown = 3f;
    private bool canAttack = true;

    public EnemyStateType GetStateType() => EnemyStateType.Attack;

    public void EnterState(Enemy enemy)
    {
        this.enemy = enemy;
        player = GameObject.FindWithTag("Player").transform;
        enemy.agent.isStopped = true;
        canAttack = true;
    }

    public void UpdateState()
    {
        // Face the player smoothly
        Vector3 direction = (player.position - enemy.transform.position).normalized;
        direction.y = 0f;

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, lookRotation, Time.deltaTime * 5f);
        }

        // Start attack only if cooldown is over
        if (canAttack)
        {
            enemy.enemyMono.StartCoroutine(AttackCooldown());
        }

        // Check if player is out of attack range
        Vector3 enemyPos = new Vector3(enemy.transform.position.x, 0, enemy.transform.position.z);
        Vector3 playerPos = new Vector3(player.position.x, 0, player.position.z);
        float distance = Vector3.Distance(enemyPos, playerPos);
        if (distance > 2.3f)
        {
            enemy.SwitchState(enemy.chaseState);
        }
    }

    public void ExitState()
    {
        enemy.agent.isStopped = false;
        enemy.animator.SetBool("isWaiting", false);
    }

    private IEnumerator AttackCooldown()
    {
        canAttack = false;

        enemy.animator.SetTrigger("Attack");
        enemy.animator.SetBool("isWaiting", false);
        Debug.Log("Normal Attack Triggered");

        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }
}
