using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Sword_Controller : MonoBehaviour
{
    Animator _anim;
    Rigidbody2D _rb;
    Collider2D _col;
    Player _player;

    bool canRotate = true;
    bool isReturn;
    [SerializeField] private float returnSpeed = 12;
    private void Awake()
    {
        _anim = GetComponentInChildren<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _col = GetComponent<Collider2D>();
    }

    public void SetUpSword(Vector2 dir, float gravity, Player player)
    {
        _player = player;
        _rb.velocity = dir;
        _rb.gravityScale = gravity;

        _anim.SetBool("Rotate", true);
    }

    public void ReturnSword()
    {
        _rb.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.parent = null;
        isReturn = true;
    }

    private void FixedUpdate()
    {
        if (canRotate) 
            transform.right = _rb.velocity;

        if (isReturn)
        {
            transform.position = Vector2.MoveTowards(transform.position, _player.transform.position,
                                                     returnSpeed * Time.deltaTime);

            if(Vector2.Distance(transform.position, _player.transform.position) < 1)
                _player.CatchSword();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isReturn) return;

        _anim.SetBool("Rotate", false);
        canRotate = false;
        _col.enabled = false;

        _rb.isKinematic = true;
        _rb.constraints = RigidbodyConstraints2D.FreezeAll;

        transform.parent = collision.transform;
    }
}
