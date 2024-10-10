using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 挂在animator物体上，只负责动画事件触发
/// </summary>
public class PlayerAnimTriggers : MonoBehaviour
{
    private Player player => GetComponentInParent<Player>();

    private void AnimTrigger()
    {
        player.AnimTrigger();
    }

}
