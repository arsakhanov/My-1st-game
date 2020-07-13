using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : Unit {

    public int lives = 6;
    public float speed = 6.0F; // Скорость передвижения
    public float jumpforce = 18.0F; // Сила прыжка
    public Rigidbody2D playerRigidbody; // Чтобы установить физику прыжка
    public Animator charAnimator; // Для реализации анимации
    public SpriteRenderer sprite;
    

    private bool isGrounded = false;
    private Bullet bullet;

    private void Awake() 
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        charAnimator = GetComponentInChildren<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
      
        bullet = Resources.Load<Bullet>("Bullet");
    }


    private void FixedUpdate ()
    {
        CheckGround();
    }

	void Start ()
    {
		
	}
	
    private void Move()
    {
        Vector3 tempvector = Vector3.right * Input.GetAxis("Horizontal"); //добавление кнопки передвижения
        transform.position = Vector3.MoveTowards(transform.position, transform.position + tempvector, speed * Time.deltaTime); //изменение позиции нашего персонажа
        if(tempvector.x < 0)
        {
            sprite.flipX = true;
        }
        else
        {
            sprite.flipX = false;
        }
    }

    public override void RecieveDamage()
    {

        lives --;
        playerRigidbody.velocity = Vector3.zero;
        playerRigidbody.AddForce(transform.up * 10.0F, ForceMode2D.Impulse);
        playerRigidbody.AddForce(transform.right * (sprite.flipX ? 3.0F : -3.0F), ForceMode2D.Impulse);

        Debug.Log(lives);
    }


    void CheckGround()
    {
        Collider2D[] colladers = Physics2D.OverlapCircleAll(transform.position, 0.8F);
        isGrounded = colladers.Length > 1;
    }

    private void Jump()
    {
        playerRigidbody.AddForce(transform.up * jumpforce, ForceMode2D.Impulse);
    }

    private void Shoot()
    {
        Vector3 position = transform.position;
        position.y += 0.8F;
      
        Bullet newBullet = Instantiate(bullet, position, bullet.transform.rotation) as Bullet;
        newBullet.Parent = gameObject;
        newBullet.Direction = newBullet.transform.right * (sprite.flipX ? -1.0F : 1.0F); 
    }
   

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1")) Shoot();

        if (Input.GetButton("Horizontal"))
        {
            Move();
            charAnimator.SetInteger("State", 1);
        }

        if (isGrounded && Input.GetButtonDown("Jump")) 
        {
            Jump();
            charAnimator.SetInteger("State", 2);
        }

        if (!Input.anyKey)
        {
            charAnimator.SetInteger("State", 0);
        }
	}

    private void OnTriggerEnter2D(Collider2D collider)
    {
        //Unit unit = collider.gameObject.GetComponent<Unit>();
        //if(unit)
        //{
          //  RecieveDamage();
        //}
    }

}
