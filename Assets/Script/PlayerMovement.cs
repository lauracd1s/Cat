using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movimiento")]
    public float speed = 5f;
    public float jumpForce = 12f;

    [Header("Escalada")]
    public float climbSpeed = 3f;

    [Header("Combate")]
    public Transform attackPoint;
    public float attackRange = 1f;
    public LayerMask enemyLayers;
    public float attackCooldown = 0.5f;

    [Header("Respawn")]
    public Vector2 respawnPoint;

    private Rigidbody2D rb;
    private Animator animator;

    private bool isGrounded;
    private bool isClimbing = false;
    private bool isDead = false;

    private float lastAttackTime;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // 🔥 APLICAR POWERUPS
        speed += GameManager.instance.speedLevel * 1.5f;
    }

    void Update()
    {
        Move();
        Jump();
        Climb();
        Attack();
        UpdateAttackPoint();
    }

    void Move()
    {
        float move = Input.GetAxis("Horizontal");

        rb.linearVelocity = new Vector2(move * speed, rb.linearVelocity.y);

        animator.SetFloat("Speed", Mathf.Abs(move));

        if (move != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Sign(move) * Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    void Climb()
    {
        float vertical = Input.GetAxis("Vertical");

        if (isClimbing)
        {
            rb.gravityScale = 0;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, vertical * climbSpeed);
        }
        else
        {
            rb.gravityScale = 3;
        }
    }

    void Attack()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            if (Time.time < lastAttackTime + attackCooldown) return;

            lastAttackTime = Time.time;

            animator.SetTrigger("Attack");

            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(
                attackPoint.position,
                attackRange,
                enemyLayers
            );

            foreach (Collider2D enemy in hitEnemies)
            {
                EnemyHealth eh = enemy.GetComponent<EnemyHealth>();
                if (eh != null)
                {
                    eh.TakeDamage(GameManager.instance.damageLevel);
                }

                BossController boss = enemy.GetComponent<BossController>();
                if (boss != null)
                {
                    boss.TakeDamage();
                }
            }
        }
    }

    void UpdateAttackPoint()
    {
        if (attackPoint == null) return;

        float direction = Mathf.Sign(transform.localScale.x);
        attackPoint.localPosition = new Vector3(direction * 1.5f, 1f, 0f);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = true;

        if (collision.gameObject.CompareTag("Enemy"))
        {
            GameManager.instance.LoseLife();
            Respawn();
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = false;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
            isClimbing = true;

        if (collision.CompareTag("Death") && !isDead)
        {
            isDead = true;
            GameManager.instance.LoseLife();
            Respawn();
            Invoke(nameof(ResetDeath), 0.5f);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
            isClimbing = false;
    }

    void Respawn()
    {
        transform.position = respawnPoint;
        rb.linearVelocity = Vector2.zero;
    }

    void ResetDeath()
    {
        isDead = false;
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
