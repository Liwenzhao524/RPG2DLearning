using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Blackhole_Controller : Skill_Controller
{
    [SerializeField] GameObject hotKeyPrefab;
    [SerializeField] List<KeyCode> hotKeyList;

    float _blackholeTimer;
    float _maxSize;
    float _growSpeed;
    float _shrinkSpeed;
    
    bool _canShrink;
    readonly bool _canGrow = true;
    
    bool _canCreateHotKey = true;
    List<Transform> _targets = new();
    List<GameObject> _createdHotKey = new();

    bool _canCloneAttack;
    int _cloneAttackCount = 8;
    float _cloneAttackCoolDown = 0.4f;
    float _cloneAttackTimer;

    [HideInInspector] public bool playerCanExitSkill;
    bool _playerCanTransparent = true;

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="maxSize">最大大小</param>
    /// <param name="growSpeed">变大速度</param>
    /// <param name="shrinkSpeed">缩小速度</param>
    /// <param name="cloneAttackCount">攻击次数</param>
    /// <param name="cloneAttackCoolDown">攻击间隔</param>
    /// <param name="blackholeDuration">时间窗口</param>
    public void SetUpBlackhole(float maxSize, float growSpeed, float shrinkSpeed, int cloneAttackCount, float cloneAttackCoolDown, float blackholeDuration)
    {
        _maxSize = maxSize;
        _growSpeed = growSpeed;
        _shrinkSpeed = shrinkSpeed;
        _cloneAttackCount = cloneAttackCount;
        _cloneAttackCoolDown = cloneAttackCoolDown;
        _blackholeTimer = blackholeDuration;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        _cloneAttackTimer -= Time.deltaTime;
        _blackholeTimer -= Time.deltaTime;

        if (_blackholeTimer < 0)
        {
            _blackholeTimer = Mathf.Infinity;
            if (_targets.Count > 0)
                CloneAttackStart();
            else
                EndBlackholeSkill();
        }

        if (Input.GetKeyDown(KeyCode.R) && _targets.Count > 0)
            CloneAttackStart();

        CloneAttackLogic();

        if (_canGrow && !_canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(_maxSize, _maxSize), _growSpeed * Time.deltaTime);

        }
        if (_canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1, -1), _shrinkSpeed * Time.deltaTime);
            if (transform.localScale.x < 0)
            {
                Destroy(gameObject);
            }
        }
    }

    /// <summary>
    /// 对List里的敌人发动攻击
    /// </summary>
    private void CloneAttackStart()
    {
        DestroyHotKey();
        _canCloneAttack = true;
        _canCreateHotKey = false;

        if(_playerCanTransparent)
        {
            _playerCanTransparent = false; 
            PlayerManager.instance.player.fx.MakeTransparent(true);
        }
    }

    private void CloneAttackLogic()
    {
        if (_cloneAttackTimer < 0 && _canCloneAttack && _cloneAttackCount > 0)
        {
            _cloneAttackTimer = _cloneAttackCoolDown;

            int randomIndex = Random.Range(0, _targets.Count);
            float xOffset = Random.Range(0, 100) > 50 ? 1 : -1;

            SkillManager.instance.clone.CreateClone(_targets[randomIndex], new Vector2(xOffset, 0));

            _cloneAttackCount--;
            if (_cloneAttackCount <= 0)
            {
                _canCloneAttack = false;
                Invoke(nameof(EndBlackholeSkill), 0.5f); 
            }
        }
    }

    private void EndBlackholeSkill()
    {
        _canShrink = true;
        playerCanExitSkill = true;
        PlayerManager.instance.player.fx.MakeTransparent(false);
        DestroyHotKey();
    }

   protected override void OnTriggerEnter2D(Collider2D collision) 
    {
        if(collision.GetComponent<Enemy>() != null)
        {
            collision.GetComponent<Enemy>().FreezeTime(true);
            CreateHotKey(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            collision.GetComponent<Enemy>().FreezeTime(false);
        }
    }

    #region HotKey
    private void DestroyHotKey()
    {
        if (_createdHotKey.Count <= 0) return;

        for(int i = 0; i < _createdHotKey.Count; i++)
        {
            Destroy(_createdHotKey[i]);
        }

        _createdHotKey.Clear();
    }

    /// <summary>
    /// 根据热键名单在敌人头顶创建热键GO
    /// </summary>
    /// <param name="collision"></param>
    private void CreateHotKey(Collider2D collision)
    {
        if(hotKeyList.Count <= 0)
        {
            Debug.LogWarning("NoHotKey");
            return;
        }

        if(!_canCreateHotKey) return;

        collision.GetComponent<Enemy>().FreezeTime(true);

        GameObject newHotKey = Instantiate(hotKeyPrefab, collision.transform.position + new Vector3(0, 2), Quaternion.identity);
        _createdHotKey.Add(newHotKey);

        // 随机选择一个键 生成对应热键
        KeyCode chooseKey = hotKeyList[Random.Range(0, hotKeyList.Count)];
        hotKeyList.Remove(chooseKey);

        Blackhole_HotKey_Controller ctrl = newHotKey.GetComponent<Blackhole_HotKey_Controller>(); ;
        ctrl.SetUpHotKey(chooseKey, collision.transform, this);
    }

    #endregion

    public void AddEnemyToList(Transform enemy) => _targets.Add(enemy); 

}
