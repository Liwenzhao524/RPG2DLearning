using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Blackhole_Controller : MonoBehaviour
{
    [SerializeField] GameObject hotKeyPrefab;
    [SerializeField] List<KeyCode> hotKeyList;

    [SerializeField] float growSpeed;
    [SerializeField] bool canGrow;
    [SerializeField] float maxSize;

    List<Transform> targets = new List<Transform>();

    // Update is called once per frame
    void Update()
    {
        if (canGrow)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Enemy>() != null)
        {
            CreateHotKey(collision);
        }
    }

    private void CreateHotKey(Collider2D collision)
    {
        if(hotKeyList.Count <= 0)
        {
            Debug.LogWarning("NoHotKey");
            return;
        }
        collision.GetComponent<Enemy>().FreezeTime(true);

        GameObject newHotKey = Instantiate(hotKeyPrefab, collision.transform.position + new Vector3(0, 2), Quaternion.identity);

        KeyCode chooseKey = hotKeyList[Random.Range(0, hotKeyList.Count)];
        hotKeyList.Remove(chooseKey);

        Blackhole_HotKey_Controller ctrl = newHotKey.GetComponent<Blackhole_HotKey_Controller>(); ;
        ctrl.SetUpHotKey(chooseKey, collision.transform, this);
    }

    public void AddEnemyToList(Transform enemy) => targets.Add(enemy); 

}
