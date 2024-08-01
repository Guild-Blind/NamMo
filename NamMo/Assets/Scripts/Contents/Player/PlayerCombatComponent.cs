using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatComponent : MonoBehaviour
{
    private PlayerController _pc;
    public void SetPlayerController(PlayerController pc)
    {
        _pc = pc;
    }
    public void GetDamaged(/*TODO*/float damage, Vector3 attackPos)
    {
        if (_pc.GetASC().IsExsistTag(Define.GameplayTag.Player_State_Invincible))
        {
            // ���������϶� ������ ������ ���
            return;
        }
        if (_pc.GetASC().IsExsistTag(Define.GameplayTag.Player_Action_Block))
        {
            // ����ϰ� �ִ� ���
            damage /= 2;
            StartCoroutine(CoHurtShortTime());
        }
        else
        {
            float force = 1;
            if (transform.position.x < attackPos.x) force = -1;
            (_pc.GetASC().GetAbility(Define.GameplayAbility.GA_Hurt) as GA_Hurt).SetKnockBackDirection(force);
            _pc.GetASC().TryActivateAbilityByTag(Define.GameplayAbility.GA_Hurt);
            
        }
        _pc.GetASC().TryActivateAbilityByTag(Define.GameplayAbility.GA_Invincible);
        StartCoroutine(CoShowAttackedEffect());
        _pc.GetPlayerStat().ApplyDamage(damage);
    }
    IEnumerator CoHurtShortTime()
    {
        _pc.GetASC().AddTag(Define.GameplayTag.Player_State_Hurt);
        yield return new WaitForSeconds(0.5f);
        _pc.GetASC().RemoveTag(Define.GameplayTag.Player_State_Hurt);
    }
    IEnumerator CoShowAttackedEffect()
    {
        _pc.GetPlayerSprite().GetComponent<SpriteRenderer>().color = new Color(1, 0.5f, 0.5f, 0.5f);
        yield return new WaitForSeconds(0.5f);
        _pc.GetPlayerSprite().GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);
    }
}
