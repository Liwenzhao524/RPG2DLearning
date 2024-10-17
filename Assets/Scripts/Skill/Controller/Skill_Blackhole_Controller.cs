using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Blackhole_Controller : MonoBehaviour
{
    [SerializeField] GameObject hotKeyPrefab;
    [SerializeField] List<KeyCode> hotKeyList;

    float blackholeTimer;
    float maxSize;
    float growSpeed;
    float shrinkSpeed;
    
    bool canShrink;
    bool canGrow = true;
    
    bool canCreateHotKey = true;
    List<Transform> targets = new List<Transform>();
    List<GameObject> createdHotKey = new List<GameObject>();

    bool canCloneAttack;
    int cloneAttackCount = 8;
    float cloneAttackCoolDown = 0.4f;
    float cloneAttackTimer;

    public bool playerCanExitSkill;
    bool playerCanTransparent = true;

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="_maxSize">最大大小</param>
    /// <param name="_growSpeed">变大速度</param>
    /// <param name="_shrinkSpeed">缩小速度</param>
    /// <param name="_cloneAttackCount">攻击次数</param>
    /// <param name="_cloneAttackCoolDown">攻击间隔</param>
    /// <param name="_blackholeDuration">时间窗口</param>
    public void SetUpBlackhole(float _maxSize, float _growSpeed, float _shrinkSpeed, int _cloneAttackCount, float _cloneAttackCoolDown, float _blackholeDuration)
    {
        maxSize = _maxSize;
        growSpeed = _growSpeed;
        shrinkSpeed = _shrinkSpeed;
        cloneAttackCount = _cloneAttackCount;
        cloneAttackCoolDown = _cloneAttackCoolDown;
        blackholeTimer = _blackholeDuration;
    }

    // Update is called once per frame
    void Update()
    {
        cloneAttackTimer -= Time.deltaTime;
        blackholeTimer -= Time.deltaTime;

        if (blackholeTimer < 0)
        {
            blackholeTimer = Mathf.Infinity;
            if (targets.Count > 0)
                CloneAttackStart();
            else
                EndBlackholeSkill();
        }

        if (Input.GetKeyDown(KeyCode.R) && targets.Count > 0)
            CloneAttackStart();

        CloneAttackLogic();

        if (canGrow && !canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);

        }
        if (canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1, -1), shrinkSpeed * Time.deltaTime);
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
        canCloneAttack = true;
        canCreateHotKey = false;

        if(playerCanTransparent)
        {
            playerCanTransparent = false; 
            PlayerManager.instance.player.fx.MakeTransparent(true);
        }
    }

    private void CloneAttackLogic()
    {
        if (cloneAttackTimer < 0 && canCloneAttack && cloneAttackCount > 0)
        {
            cloneAttackTimer = cloneAttackCoolDown;

            int randomIndex = Random.Range(0, targets.Count);
            float xOffset = Random.Range(0, 100) > 50 ? 1 : -1;

            SkillManager.instance.clone.CreateClone(targets[randomIndex], new Vector2(xOffset, 0));

            cloneAttackCount--;
            if (cloneAttackCount <= 0)
            {
                canCloneAttack = false;
                Invoke("EndBlackholeSkill", 0.5f); 
            }
        }
    }

    private void EndBlackholeSkill()
    {
        canShrink = true;
        playerCanExitSkill = true;
        PlayerManager.instance.player.fx.MakeTransparent(false);
        DestroyHotKey();
    }

    private void OnTriggerEnter2D(Collider2D collision) 
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
        if (createdHotKey.Count <= 0) return;

        for(int i = 0; i < createdHotKey.Count; i++)
        {
            Destroy(createdHotKey[i]);
        }

        createdHotKey.Clear();
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

        if(!canCreateHotKey) return;

        collision.GetComponent<Enemy>().FreezeTime(true);

        GameObject newHotKey = Instantiate(hotKeyPrefab, collision.transform.position + new Vector3(0, 2), Quaternion.identity);
        createdHotKey.Add(newHotKey);

        // 随机选择一个键 生成对应热键
        KeyCode chooseKey = hotKeyList[Random.Range(0, hotKeyList.Count)];
        hotKeyList.Remove(chooseKey);

        Blackhole_HotKey_Controller ctrl = newHotKey.GetComponent<Blackhole_HotKey_Controller>(); ;
        ctrl.SetUpHotKey(chooseKey, collision.transform, this);
    }

    #endregion

    public void AddEnemyToList(Transform enemy) => targets.Add(enemy); 

}
