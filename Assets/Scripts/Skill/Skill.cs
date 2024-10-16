using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���ܸ���
/// </summary>
public class Skill : MonoBehaviour
{
    [SerializeField] protected float coolDown;
    protected float coolDownTimer;
    protected Player _player;

    protected virtual void Start()
    {
        _player = PlayerManager.instance.player;
    }

    protected virtual void Update()
    {
        coolDownTimer -= Time.deltaTime;
    }

    /// <summary>
    /// �ܷ�ʹ�ü��ܣ���ȴ
    /// </summary>
    /// <returns></returns>
    public virtual bool CanUseSkill()
    {
        if(coolDownTimer < 0)
        {
            UseSkill();
            coolDownTimer = coolDown;
            return true;
        }
        return false;
    }

    public virtual void UseSkill()
    {
        // ����ִ��
    }
}
