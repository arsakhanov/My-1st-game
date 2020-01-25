using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MoveableMonster : Monster {

    [SerializeField]
    private float speed = 2.0F;

    private SpriteRenderer sprite;

    private Vector3 direction;

    
    private Bullet bullet;



    protected override void Awake()
    {
        bullet = Resources.Load<Bullet>("Bullet");
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    protected override void Start()
    {
        direction = transform.right;
    }

    protected override void Update()
    {
        Move();
    }

    protected override void OnTriggerEnter2D(Collider2D collider)
    {
        Unit unit = collider.GetComponent<Unit>();
        if(unit && unit is Character)
        {
            if(Mathf.Abs(unit.transform.position.x - transform.position.x) < 0.3F)RecieveDamage();
            else unit.RecieveDamage();
        }
    }

    private void Move()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position + transform.up * 0.4F + transform.right * direction.x * 0.85F, 0.1F);
        if (colliders.Length > 0 && colliders.All(x => !x.GetComponent<Character>())) { direction *= -1.0F; }
        if (direction.x < 0) { sprite.flipX = true; }
        else { sprite.flipX = false; }
        
                   
        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);
    }
}
