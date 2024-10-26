using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    public UI_ItemToolTip itemToolTip;
    public UI_StatToolTip statToolTip;
    public UI_CraftWindow craftWindow;

    [SerializeField] GameObject characterUI;
    [SerializeField] GameObject skillTreeUI;
    [SerializeField] GameObject craftUI;
    [SerializeField] GameObject optionUI;

    // Start is called before the first frame update
    void Start()
    {
        itemToolTip.gameObject.SetActive(false);
        statToolTip.gameObject.SetActive(false);
        craftWindow.gameObject.SetActive(false);

        SwitchToMenu(null);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
            SwitchWithKey(characterUI);

        if (Input.GetKeyDown(KeyCode.K))
            SwitchWithKey(skillTreeUI);

        if(Input.GetKeyDown(KeyCode.B))
            SwitchWithKey(craftUI);

        if(Input.GetKeyDown(KeyCode.O))
            SwitchWithKey(optionUI);
    }

    public void SwitchToMenu(GameObject menu)
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        menu?.SetActive(true);
    }

    public void SwitchWithKey(GameObject menu)
    {
        if(menu != null && menu.activeSelf)
        {
            menu.SetActive(false);
            return;
        }

        SwitchToMenu(menu);
    }
}
