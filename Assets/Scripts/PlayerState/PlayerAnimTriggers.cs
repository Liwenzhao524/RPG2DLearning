using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����animator�����ϣ�ֻ���𶯻��¼�����
/// </summary>
public class PlayerAnimTriggers : MonoBehaviour
{
    private Player player => GetComponentInParent<Player>();

    private void AnimTrigger()
    {
        player.AnimTrigger();
    }

}
