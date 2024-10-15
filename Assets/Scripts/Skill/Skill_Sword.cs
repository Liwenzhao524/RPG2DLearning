using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Skill_Sword : Skill
{
    [Header("Skill Info")]
    [SerializeField] private GameObject swordPrefab;
    [SerializeField] private Vector2 launchDir;
    [SerializeField] private float swordGravity;

    private Vector2 finalDir;

    [SerializeField] private int numOfDots;
    [SerializeField] private float dotsBetweenDis;

    [SerializeField] private GameObject dotsPrefab;
    [SerializeField] private GameObject dotsParent;
    private GameObject[] dots;

    protected override void Start()
    {
        base.Start();
        GenerateDots();
    }
    protected override void Update()
    {
        base.Update();

        if (Input.GetMouseButton(1))
        {
            for(int i = 0; i < dots.Length; i++)
            {
                dots[i].transform.position = DotPosition(i * dotsBetweenDis);
            }
        }

        if(Input.GetMouseButtonUp(1)) 
            finalDir = new Vector2(launchDir.x * AimDirection().x, launchDir.y * AimDirection().y);
    }

    public void CreateSword()
    {
        GameObject newSword = Instantiate(swordPrefab, _player.transform.position, transform.rotation);
        Skill_Sword_Controller ctrl = newSword.GetComponent<Skill_Sword_Controller>();
        ctrl.SetUpSword(finalDir, swordGravity, _player);

        _player.AssignSword(newSword);
        SetDotsActive(false);
    }

    public Vector2 AimDirection()
    {
        Vector2 playerPos = _player.transform.position;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 aimPos = (mousePos - playerPos);
        aimPos.Normalize();

        return aimPos;
    }

    public void SetDotsActive(bool active)
    {
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i].SetActive(active);
        }
    }

    private void GenerateDots()
    {
        dots = new GameObject[numOfDots];
        for(int i = 0; i < numOfDots; i++)
        {
            dots[i] = Instantiate(dotsPrefab, _player.transform.position, Quaternion.identity, dotsParent.transform);
            dots[i].SetActive(false);
        }
    }

    private Vector2 DotPosition(float t)
    {
        Vector2 position = (Vector2)_player.transform.position + new Vector2(
        AimDirection().x * launchDir.x * t,
        AimDirection().y * launchDir.y * t) + (0.5f * Physics2D.gravity * swordGravity * t * t);
        return position;
    }
}
