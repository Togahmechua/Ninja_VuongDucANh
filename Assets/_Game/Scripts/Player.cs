using System;
using UnityEngine;

public class Player : Character
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private float speed = 250f;
    [SerializeField] private float jumpForce = 350f;
    [SerializeField] private Kunai kunaiPrefab;
    [SerializeField] private Transform throwPoint;
    [SerializeField] private GameObject attackArea;
    private bool isGrounded = true;
    private bool isJumping = false;
    private bool isAttack = false;
    private bool isDead = false;

    private float horizontal;
    // private float vertical;


    private int coin = 0;

    private Vector3 savePoint;

    private void Awake()
    {
        coin = PlayerPrefs.GetInt("coin", 0);
    }

    void Update()
    {   if (isDead)
        {
            return;
        }
        isGrounded = CheckGrounded();

        // horizontal = Input.GetAxisRaw("Horizontal");
        // vertical = Input.GetAxisRaw("Vertical");
       

        if (isAttack)
        {
            rb.velocity = Vector2.zero;
            return;
           
        }

        if (isGrounded)
        {
            if (isJumping)
            {
                return;
            }


            //jump
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                isJumping = true;
                ChangeAnim("Jump");
                rb.AddForce(jumpForce * Vector2.up);
            }

            //change anim run
            if (Mathf.Abs(horizontal) > 0.1f)
            {
                ChangeAnim("Run");
            }

            //attack
            if (Input.GetKeyDown(KeyCode.C) && isGrounded)
            {
                Attack();
            }
            
            //throw
            if (Input.GetKeyDown(KeyCode.V) && isGrounded)
            {
                Throw();
            }
        }

        //check falling
        if (!isGrounded && rb.velocity.y < 0)
        {
            ChangeAnim("Fall"); 
            isJumping = false;
        }
        
        // if (isDead == false)
        // {
            //Moving
            if (Mathf.Abs(horizontal) > 0.1f)
            {
                rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
                //toan tu nay khi di sang phai thi se tra ve gia tri -> 0 va 180 neu nguoc lai
                transform.rotation = Quaternion.Euler(new Vector3(0, horizontal > 0 ? 0 : 180, 0));
            }
            else if (isGrounded && isJumping == false && isAttack == false)
            {
                ChangeAnim("Idle");
                rb.velocity = Vector2.zero;
            }
        // }
        
        
    }

    public override void OnInit()
    {
        base.OnInit();
        isDead = false;
        isAttack = false;

        transform.position = savePoint;

        ChangeAnim("Idle");
        DeActiveAttack();
        SavePoint();
        UIManager.Instance.SetCoin(coin);
    }

    protected override void OnDeath()
    {
        base.OnDeath();
        isDead = true;
    }

    public override void OnDespawn()
    {
        base.OnDespawn();
        OnInit();
    }
  
    private bool CheckGrounded()
    {
        Debug.DrawLine(transform.position , transform.position + Vector3.down * 1.1f, Color.red);   

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.1f, groundLayer);
        // if (hit.collider != null)
        // {
        //     return true;
        // }
        // else return false;

        return hit.collider != null;
    }

    public void Attack()
    {
        ChangeAnim("Attack");
        isAttack = true;
        Invoke(nameof(ResetAttack), 0.5f);

        ActiveAttack();
        Invoke(nameof(DeActiveAttack), 0.5f);
    }

    private void ResetAttack()
    {
        isAttack = false;
        ChangeAnim("Idle");
    }

    public void Throw()
    {
        isAttack = true;
        ChangeAnim("Throw");
        Invoke(nameof(ResetAttack), 0.6f);

        Instantiate(kunaiPrefab, throwPoint.position , throwPoint.rotation);
    }

    public void Jump()
    {
        isJumping = true;
        ChangeAnim("Jump");
        rb.AddForce(jumpForce * Vector2.up);
    }

    internal void SavePoint()
    {
        savePoint = transform.position;
    }

    private void ActiveAttack()
    {
        attackArea.SetActive(true);
    }

    private void DeActiveAttack()
    {
        attackArea.SetActive(false);
    }

    public void SetMove(float horizontal)
    {
        this.horizontal = horizontal;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Coin"))
        {
            coin++;
            PlayerPrefs.SetInt("coin", coin);
            UIManager.Instance.SetCoin(coin);
            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("DeadZone"))
        {
            ChangeAnim("Die");
            isDead = true;
            Debug.Log("Ded");
            Invoke(nameof(OnInit), 1f);
        }
    }

}
