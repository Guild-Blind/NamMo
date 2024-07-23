using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAbility : MonoBehaviour
{
    [SerializeField] protected bool _canOverlapAbility = false;
    // �ߵ��ɶ� �߰��� �±�, �ߵ��� ����ɶ� ������ �±�
    [SerializeField] protected List<Define.GameplayTag> _tagsToAdd;
    // �ߵ��� �ʿ��� �±�
    [SerializeField] protected List<Define.GameplayTag> _needTags;
    // �ߵ��� �� ������ �ȵǴ� �±�
    [SerializeField] protected List<Define.GameplayTag> _blockTags;
    protected AbilitySystemComponent _asc;
    protected int _overlapCnt = 0;

    private bool _isActivated = false;
    public bool IsActivated { get { return _isActivated; } }
    private void Start()
    {
        Init();
    }
    public void SetASC(AbilitySystemComponent asc)
    {
        _asc = asc;
    }
    public void TryActivateAbility()
    {
        if (CanActivateAbility() == false) return;
        foreach(Define.GameplayTag tag in _tagsToAdd)
        {
            _asc.AddTag(tag);
        }
        ActivateAbility();
        return;
    }
    protected virtual void Init()
    {

    }
    protected virtual void ActivateAbility()
    {
        _isActivated = true;
        _overlapCnt++;
    }
    protected virtual bool CanActivateAbility()
    {
        foreach(Define.GameplayTag tag in _needTags)
        {
            if (_asc.IsExsistTag(tag) == false) return false;
        }
        foreach(Define.GameplayTag tag in _blockTags)
        {
            if (_asc.IsExsistTag(tag)) return false;
        }
        if (_canOverlapAbility == false)
        {
            if (_isActivated) return false;
        }
        return true;
    }
    protected virtual void EndAbility()
    {
        for(int i=0;i<_overlapCnt;i++)
        {
            RemoveTags();
        }
        _isActivated = false;
        _overlapCnt = 0;
    }
    public virtual void CancelAbility()
    {
    }
    private void RemoveTags()
    {
        foreach (Define.GameplayTag tag in _tagsToAdd)
        {
            _asc.RemoveTag(tag);
        }
    }
}
