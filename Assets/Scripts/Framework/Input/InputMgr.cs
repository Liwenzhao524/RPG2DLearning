using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputMgr : BaseManager<InputMgr>
{
    Dictionary<E_EventType, InputInfo> _inputDic = new();

    //当前遍历时取出的输入信息
    InputInfo _nowInputInfo;

    //是否开启了输入系统检测
    bool _isStart;
    //用于在改建时获取输入信息的委托 只有当update中获取到信息的时候 再通过委托传递给外部
    UnityAction<InputInfo> _getInputInfoCallBack;
    //是否开始检测输入信息
    bool _isBeginCheckInput = false;

    private InputMgr()
    {
        MonoMgr.instance.AddUpdateListener(InputUpdate);
    }

    /// <summary>
    /// 开启或者关闭我们的输入管理模块的检测
    /// </summary>
    /// <param name="isStart"></param>
    public void StartOrCloseInputMgr(bool isStart)
    {
        _isStart = isStart;
    }

    /// <summary>
    /// 提供给外部改建或初始化的方法(键盘)
    /// </summary>
    /// <param name="key"></param>
    /// <param name="inputType"></param>
    public void ChangeKeyboardInfo(E_EventType eventType, KeyCode key, InputInfo.E_InputType inputType)
    {
        //初始化
        if(!_inputDic.ContainsKey(eventType))
        {
            _inputDic.Add(eventType, new InputInfo(inputType, key));
        }
        else//改建
        {
            //如果之前是鼠标 我们必须要修改它的按键类型
            _inputDic[eventType].keyOrMouse = InputInfo.E_KeyOrMouse.Key;
            _inputDic[eventType].key = key;
            _inputDic[eventType].inputType = inputType;
        }
    }

    /// <summary>
    /// 提供给外部改建或初始化的方法(鼠标)
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="mouseID"></param>
    /// <param name="inputType"></param>
    public void ChangeMouseInfo(E_EventType eventType, int mouseID, InputInfo.E_InputType inputType)
    {
        //初始化
        if (!_inputDic.ContainsKey(eventType))
        {
            _inputDic.Add(eventType, new InputInfo(inputType, mouseID));
        }
        else//改建
        {
            //如果之前是鼠标 我们必须要修改它的按键类型
            _inputDic[eventType].keyOrMouse = InputInfo.E_KeyOrMouse.Mouse;
            _inputDic[eventType].mouseID = mouseID;
            _inputDic[eventType].inputType = inputType;
        }
    }

    /// <summary>
    /// 移除指定行为的输入监听
    /// </summary>
    /// <param name="eventType"></param>
    public void RemoveInputInfo(E_EventType eventType)
    {
        if (_inputDic.ContainsKey(eventType))
            _inputDic.Remove(eventType);
    }
    
    /// <summary>
    /// 获取下一次的输入信息
    /// </summary>
    /// <param name="callBack"></param>
    public void GetInputInfo(UnityAction<InputInfo> callBack)
    {
        _getInputInfoCallBack = callBack;
        MonoMgr.instance.StartCoroutine(BeginCheckInput());
    }

    private IEnumerator BeginCheckInput()
    {
        //等一帧
        yield return 0;
        //一帧后才会被置成true
        _isBeginCheckInput = true;
    }

    private void InputUpdate()
    {
        //当委托不为空时 证明想要获取到输入的信息 传递给外部
        if(_isBeginCheckInput)
        {
            //当一个键按下时 然后遍历所有按键信息 得到是谁被按下了
            if (Input.anyKeyDown)
            {
                InputInfo inputInfo = null;
                //我们需要去遍历监听所有键位的按下 来得到对应输入的信息
                //键盘
                Array keyCodes = Enum.GetValues(typeof(KeyCode));
                foreach (KeyCode inputKey in keyCodes)
                {
                    //判断到底是谁被按下了 那么就可以得到对应的输入的键盘信息
                    if (Input.GetKeyDown(inputKey))
                    {
                        inputInfo = new InputInfo(InputInfo.E_InputType.Down, inputKey);
                        break;
                    }
                }
                //鼠标
                for (int i = 0; i < 3; i++)
                {
                    if (Input.GetMouseButtonDown(i))
                    {
                        inputInfo = new InputInfo(InputInfo.E_InputType.Down, i);
                        break;
                    }
                }
                //把获取到的信息传递给外部
                _getInputInfoCallBack.Invoke(inputInfo);
                _getInputInfoCallBack = null;
                //检测一次后就停止检测了
                _isBeginCheckInput = false;
            }
        }
       


        //如果外部没有开启检测功能 就不要检测
        if (!_isStart)
            return;

        foreach (E_EventType eventType in _inputDic.Keys)
        {
            _nowInputInfo = _inputDic[eventType];
            //如果是键盘输入
            if(_nowInputInfo.keyOrMouse == InputInfo.E_KeyOrMouse.Key)
            {
                //是抬起还是按下还是长按
                switch (_nowInputInfo.inputType)
                {
                    case InputInfo.E_InputType.Down:
                        if (Input.GetKeyDown(_nowInputInfo.key))
                            EventCenter.Instance.EventTrigger(eventType);
                        break;
                    case InputInfo.E_InputType.Up:
                        if (Input.GetKeyUp(_nowInputInfo.key))
                            EventCenter.Instance.EventTrigger(eventType);
                        break;
                    case InputInfo.E_InputType.Always:
                        if (Input.GetKey(_nowInputInfo.key))
                            EventCenter.Instance.EventTrigger(eventType);
                        break;
                    default:
                        break;
                }
            }
            //如果是鼠标输入
            else
            {
                switch (_nowInputInfo.inputType)
                {
                    case InputInfo.E_InputType.Down:
                        if (Input.GetMouseButtonDown(_nowInputInfo.mouseID))
                            EventCenter.Instance.EventTrigger(eventType);
                        break;
                    case InputInfo.E_InputType.Up:
                        if (Input.GetMouseButtonUp(_nowInputInfo.mouseID))
                            EventCenter.Instance.EventTrigger(eventType);
                        break;
                    case InputInfo.E_InputType.Always:
                        if (Input.GetMouseButton(_nowInputInfo.mouseID))
                            EventCenter.Instance.EventTrigger(eventType);
                        break;
                    default:
                        break;
                }
            }
        }

        EventCenter.Instance.EventTrigger(E_EventType.E_Input_Horizontal, Input.GetAxis("Horizontal"));
        EventCenter.Instance.EventTrigger(E_EventType.E_Input_Vertical, Input.GetAxis("Vertical"));
    }

}
