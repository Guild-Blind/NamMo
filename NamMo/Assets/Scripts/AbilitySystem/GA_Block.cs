using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GA_Block : GameAbility
{
    [SerializeField] private float _perfectParryingTime;
    [SerializeField] private float _blockTime;
    private bool _isPerfectParryingTiming = false;
    public Action OnBlockStart;
    public Action OnBlockEnd;

    private Coroutine _cacluateParryingTimingCoroutine = null;
    private Coroutine _blockCoroutine = null;
    protected override void ActivateAbility()
    {
        base.ActivateAbility();

        _cacluateParryingTimingCoroutine = StartCoroutine(CoChangeParryingTypeByTimeFlow());
        _blockCoroutine = StartCoroutine(CoBlock());

    }
    public override void CancelAbility()
    {
        EndAbility();
    }
    protected override void EndAbility()
    {
        base.EndAbility();

        _asc.gameObject.GetComponent<PlayerMovement>().CanMove = true;
        _asc.GetPlayerController().GetBlockArea().OnBlockAreaTriggerEntered -= HandleTriggeredObject;
        _asc.GetPlayerController().GetBlockArea().DeActiveBlockArea();

        if (_cacluateParryingTimingCoroutine != null)
        {
            StopCoroutine(_cacluateParryingTimingCoroutine);
            _cacluateParryingTimingCoroutine = null;
        }
        if(_blockCoroutine != null)
        {
            StopCoroutine(_blockCoroutine);
            _blockCoroutine = null;
        }
        _isPerfectParryingTiming = false;

        if (OnBlockEnd != null) OnBlockEnd.Invoke();
    }
    private void HandleTriggeredObject(GameObject go)
    {
        if (_isPerfectParryingTiming)
        {
            if (_asc.IsExsistTag(Define.GameplayTag.Player_State_Hurt) == false)
            {
                // TODO �и� ��ȹ�� ���� ����
                Debug.Log("Parrying");
                CancelAbility();
                Destroy(go);
                _asc.TryActivateAbilityByTag(Define.GameplayAbility.GA_Parrying);
            }
        }
        else
        {
            // TODO : ������ ���ݸ� ����
            Debug.Log("Ÿ�̹� �̽�");
        }
    }
    IEnumerator CoBlock()
    {
        _asc.gameObject.GetComponent<PlayerMovement>().CanMove = false;
        _asc.GetPlayerController().GetBlockArea().OnBlockAreaTriggerEntered += HandleTriggeredObject;
        _asc.GetPlayerController().GetBlockArea().ActiveBlockArea();
        if (OnBlockStart != null) OnBlockStart.Invoke();
        yield return new WaitForSeconds(_blockTime);
        _asc.TryCancelAbilityByTag(Define.GameplayAbility.GA_Block);
    }
    IEnumerator CoChangeParryingTypeByTimeFlow()
    {
        _isPerfectParryingTiming = true;
        yield return new WaitForSeconds(_perfectParryingTime);
        _isPerfectParryingTiming = false;
        _cacluateParryingTimingCoroutine = null;
    }
}
