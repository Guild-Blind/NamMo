using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private AbilitySystemComponent _asc;
    [SerializeField] private List<Define.GameplayAbility> _abilities;
    public Action<float> OnMoveInputChanged;
    public Action OnAttackInputPerformed;
    private void Awake()
    {
        if (_asc == null) _asc = GetComponent<AbilitySystemComponent>();
        foreach(var ability in _abilities)
        {
            _asc.GiveAbility(ability);
        }
    }
    // �̵�
    public void HandleMoveInput(InputAction.CallbackContext context)
    {
        float value = context.ReadValue<float>();
        if (context.started)
        {
        }
        else if (context.performed)
        {
            OnMoveInputChanged.Invoke(value);
        }
        else if (context.canceled)
        {
            OnMoveInputChanged.Invoke(value);
        }
    }
    // ����
    public void HandleJumpInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
        }
        else if (context.performed)
        {
            _asc.TryActivateAbilityByTag(Define.GameplayAbility.GA_Jump);
        }
        else if (context.canceled)
        {
            _asc.TryCancelAbilityByTag(Define.GameplayAbility.GA_Jump);
        }
    }
    // ����
    public void HandleAttackInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _asc.TryActivateAbilityByTag(Define.GameplayAbility.GA_Attack);
        }
    }
    // �ĵ�Ž��
    public void HandleWaveInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("HandleWaveInput");
        }
    }
    // �и�
    public void HandleParryingInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("HandleParryingInput");
        }
    }
    // �뽬
    public void HandleDashInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("HandleDashInput");
            _asc.TryActivateAbilityByTag(Define.GameplayAbility.GA_Dash);
        }
    }
    // ��ȣ�ۿ�
    public void HandleInteractionInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("HandleInteractionInput");
        }
    }
    // ������1
    public void HandleUseItem1Input(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("HandleUseItem1Input");
        }
    }
    // ������2
    public void HandleUseItem2Input(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("HandleUseItem2Input");
        }
    }
    
}
