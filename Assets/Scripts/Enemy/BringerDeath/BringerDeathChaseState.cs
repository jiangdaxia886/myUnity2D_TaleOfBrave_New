using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class BringerDeathChaseState : BaseState
{
    private bool isAttack;
    private float attackRateCounter = 0;
    private Attack attack;

    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.lostTimeCounter = currentEnemy.lostTime;
        currentEnemy.anim.SetBool("chase", true);
        attack = enemy.GetComponent<Attack>();
        //�����ٶ�
        currentEnemy.currentSpeed = currentEnemy.chaseSpeed;

        currentEnemy.anim.SetBool("run", true);
    }
    public override void LogicUpdate()
    {
        //��׷����ʱ<0ʱ������׷�����л�״̬
        //�ӵ����к����٣�����currentEnemy.attacker == nullʱҲҪ�л���Ѳ��״̬
        if (currentEnemy.lostTimeCounter <= 0 || currentEnemy.attacker == null)
        {
            currentEnemy.SwitchState(NPCState.Patrol);
            return;
        }
        //���泯ǽ������ǽʱ��ת��
        if (!currentEnemy.physicsCheck.isGround || (currentEnemy.physicsCheck.touchLeftWall && currentEnemy.faceDir.x < 0 || currentEnemy.physicsCheck.touchRightWall && currentEnemy.faceDir.x > 0))
        {
            //Debug.Log("111111"+ !currentEnemy.physicsCheck.isGround);
            currentEnemy.transform.localScale = new Vector3(currentEnemy.faceDir.x, 1, 1);
        }

        float distince = (currentEnemy.attacker.position - currentEnemy.transform.position).magnitude;


        //�жϾ���
        //��������Զ�̹�������Զ����׷��
        if (distince > attack.remoteDistanceMax)
        {
            //����������Χ��ȡ������״̬
            isAttack = false;
        }
        else if (distince >= attack.remoteDistanceMin && distince <= attack.remoteDistanceMax)
        {
            //Զ�̹���
            GameObject magicAttack = currentEnemy.MagicAttack();
        }
        //���﹥����Χ��ͣ��
        else if (distince < attack.remoteDistanceMin)
        {
            //��ս
            isAttack = true;
            //�������״̬������Ա����ˣ������������״̬���򵽴﹥����Χֹͣ
            if (!currentEnemy.isHurt)
                currentEnemy.rb.velocity = Vector2.zero;
            attackRateCounter -= Time.deltaTime;
            if (attackRateCounter <= 0)
            {
                //���ù���ʱ��
                attackRateCounter = attack.attackRate;
                currentEnemy.anim.SetTrigger("attack");
            }
        }

        //ģ��ת��
        currentEnemy.transform.localScale = new Vector3(currentEnemy.faceDir.x, 1, 1);
        // �ƶ�����
        currentEnemy.faceDir = new Vector3(currentEnemy.attacker.position.x - currentEnemy.transform.position.x ,0 ,0);


    }


    public override void PhysicsUpdate()
    {
        if (!currentEnemy.isHurt && !currentEnemy.isDead && !isAttack)
        {
            currentEnemy.rb.velocity = new Vector2(currentEnemy.currentSpeed * currentEnemy.faceDir.x * Time.deltaTime, currentEnemy.rb.velocity.y);
        }
    }

    public override void OnExit()
    {
        currentEnemy.anim.SetBool("run", false);

    }
}
