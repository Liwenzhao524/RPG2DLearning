using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputMgr : BaseManager<InputMgr>
{
    Dictionary<E_EventType, InputInfo> _inputDic = new();

    //��ǰ����ʱȡ����������Ϣ
    InputInfo _nowInputInfo;

    //�Ƿ���������ϵͳ���
    bool _isStart;
    //�����ڸĽ�ʱ��ȡ������Ϣ��ί�� ֻ�е�update�л�ȡ����Ϣ��ʱ�� ��ͨ��ί�д��ݸ��ⲿ
    UnityAction<InputInfo> _getInputInfoCallBack;
    //�Ƿ�ʼ���������Ϣ
    bool _isBeginCheckInput = false;

    private InputMgr()
    {
        MonoMgr.instance.AddUpdateListener(InputUpdate);
    }

    /// <summary>
    /// �������߹ر����ǵ��������ģ��ļ��
    /// </summary>
    /// <param name="isStart"></param>
    public void StartOrCloseInputMgr(bool isStart)
    {
        _isStart = isStart;
    }

    /// <summary>
    /// �ṩ���ⲿ�Ľ����ʼ���ķ���(����)
    /// </summary>
    /// <param name="key"></param>
    /// <param name="inputType"></param>
    public void ChangeKeyboardInfo(E_EventType eventType, KeyCode key, InputInfo.E_InputType inputType)
    {
        //��ʼ��
        if(!_inputDic.ContainsKey(eventType))
        {
            _inputDic.Add(eventType, new InputInfo(inputType, key));
        }
        else//�Ľ�
        {
            //���֮ǰ����� ���Ǳ���Ҫ�޸����İ�������
            _inputDic[eventType].keyOrMouse = InputInfo.E_KeyOrMouse.Key;
            _inputDic[eventType].key = key;
            _inputDic[eventType].inputType = inputType;
        }
    }

    /// <summary>
    /// �ṩ���ⲿ�Ľ����ʼ���ķ���(���)
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="mouseID"></param>
    /// <param name="inputType"></param>
    public void ChangeMouseInfo(E_EventType eventType, int mouseID, InputInfo.E_InputType inputType)
    {
        //��ʼ��
        if (!_inputDic.ContainsKey(eventType))
        {
            _inputDic.Add(eventType, new InputInfo(inputType, mouseID));
        }
        else//�Ľ�
        {
            //���֮ǰ����� ���Ǳ���Ҫ�޸����İ�������
            _inputDic[eventType].keyOrMouse = InputInfo.E_KeyOrMouse.Mouse;
            _inputDic[eventType].mouseID = mouseID;
            _inputDic[eventType].inputType = inputType;
        }
    }

    /// <summary>
    /// �Ƴ�ָ����Ϊ���������
    /// </summary>
    /// <param name="eventType"></param>
    public void RemoveInputInfo(E_EventType eventType)
    {
        if (_inputDic.ContainsKey(eventType))
            _inputDic.Remove(eventType);
    }
    
    /// <summary>
    /// ��ȡ��һ�ε�������Ϣ
    /// </summary>
    /// <param name="callBack"></param>
    public void GetInputInfo(UnityAction<InputInfo> callBack)
    {
        _getInputInfoCallBack = callBack;
        MonoMgr.instance.StartCoroutine(BeginCheckInput());
    }

    private IEnumerator BeginCheckInput()
    {
        //��һ֡
        yield return 0;
        //һ֡��Żᱻ�ó�true
        _isBeginCheckInput = true;
    }

    private void InputUpdate()
    {
        //��ί�в�Ϊ��ʱ ֤����Ҫ��ȡ���������Ϣ ���ݸ��ⲿ
        if(_isBeginCheckInput)
        {
            //��һ��������ʱ Ȼ��������а�����Ϣ �õ���˭��������
            if (Input.anyKeyDown)
            {
                InputInfo inputInfo = null;
                //������Ҫȥ�����������м�λ�İ��� ���õ���Ӧ�������Ϣ
                //����
                Array keyCodes = Enum.GetValues(typeof(KeyCode));
                foreach (KeyCode inputKey in keyCodes)
                {
                    //�жϵ�����˭�������� ��ô�Ϳ��Եõ���Ӧ������ļ�����Ϣ
                    if (Input.GetKeyDown(inputKey))
                    {
                        inputInfo = new InputInfo(InputInfo.E_InputType.Down, inputKey);
                        break;
                    }
                }
                //���
                for (int i = 0; i < 3; i++)
                {
                    if (Input.GetMouseButtonDown(i))
                    {
                        inputInfo = new InputInfo(InputInfo.E_InputType.Down, i);
                        break;
                    }
                }
                //�ѻ�ȡ������Ϣ���ݸ��ⲿ
                _getInputInfoCallBack.Invoke(inputInfo);
                _getInputInfoCallBack = null;
                //���һ�κ��ֹͣ�����
                _isBeginCheckInput = false;
            }
        }
       


        //����ⲿû�п�����⹦�� �Ͳ�Ҫ���
        if (!_isStart)
            return;

        foreach (E_EventType eventType in _inputDic.Keys)
        {
            _nowInputInfo = _inputDic[eventType];
            //����Ǽ�������
            if(_nowInputInfo.keyOrMouse == InputInfo.E_KeyOrMouse.Key)
            {
                //��̧���ǰ��»��ǳ���
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
            //������������
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
