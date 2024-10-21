using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thunder_Controller : Skill_Controller
{
    float _damage;
    [SerializeField] float speed;

    CharacterStats _targetstats;
    bool _triggered;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    public void SetUp(float damage, CharacterStats targetstats)
    {
        _targetstats = targetstats;
        _damage = damage;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (_triggered || !_targetstats) return;

        transform.position = Vector2.MoveTowards(transform.position, _targetstats.transform.position, speed * Time.deltaTime);
        transform.right = transform.position - _targetstats.transform.position;

        if(Vector2.Distance(transform.position, _targetstats.transform.position) < 0.1f)
        {
            anim.transform.localRotation = Quaternion.identity;
            anim.transform.localPosition = new Vector2(0, 0.3f);
            
            transform.localRotation = Quaternion.identity;
            transform.localScale = new Vector3(2, 2, 2);

            Invoke(nameof(DamageAndDestory), 0.2f);
            _triggered = true;
            anim.SetTrigger("Hit");
            
        }
    }

    public void DamageAndDestory()
    {
        _targetstats.ApplyShock(true);
        _targetstats.TakeDamage(_damage);
        Destroy(gameObject, 0.4f);
    }

}
