using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 12f;
    public Vector2 respawnPoint = new Vector2(7.31f, -5.66f);
    public Transform attackPoint;
public float attackRange = 1f;
public LayerMask enemyLayers;

    private Rigidbody2D rb;
    private Animator animator;
    private bool isClimbing = false;
    public float climbSpeed = 3f;

    private bool isGrounded;
    private bool isDead = false;

    void Attack()
{
    animator.SetTrigger("Attack");

    Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(
        transform.position,
        2.5f
    );

    foreach (Collider2D col in hitEnemies)
    {
        Debug.Log("Detectó: " + col.name);

        BossController boss = col.GetComponent<BossController>();

        if (boss != null)
        {
            Debug.Log("🔥 DAÑO AL BOSS 🔥");
            boss.TakeDamage();
        }
    }
}
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float move = Input.GetAxis("Horizontal");

        rb.linearVelocity = new Vector2(move * speed, rb.linearVelocity.y);

        animator.SetFloat("Speed", Mathf.Abs(move));

        // VOLTEAR PERSONAJE
        if (move > 0)
        {
            transform.localScale = new Vector3(5, 5, 1);
        }
        else if (move < 0)
        {
            transform.localScale = new Vector3(-5, 5, 1);
        }

        // SALTO
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
        float vertical = Input.GetAxis("Vertical");

        if (isClimbing)
        {
            rb.gravityScale = 0; // sin gravedad
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, vertical * climbSpeed);
        }
        else
        {
            rb.gravityScale = 3; // normal (ajústalo a tu valor)
        }
        if (Input.GetKeyDown(KeyCode.J))
{
    Attack();
}
if (attackPoint != null)
{
    float direction = Mathf.Sign(transform.localScale.x);
attackPoint.localPosition = new Vector3(direction * 1.5f, 1f, 0f);}

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
        if (collision.gameObject.CompareTag("Enemy"))
{
    if (rb.linearVelocity.y < 0)
    {
        Destroy(collision.gameObject); // matas enemigo
    }
    else
    {
        GameManager.instance.LoseLife();
        transform.position = respawnPoint;
    }
}

    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    // 🔥 DETECCIÓN DE MUERTE (AGUA)
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
    {
        isClimbing = true;
    }
        if (collision.CompareTag("Death") && !isDead)
    {
        isDead = true;
        GameManager.instance.LoseLife();
        // MOVER AL PUNTO DE RESPAWN
        transform.position = respawnPoint;

        // RESETEAR VELOCIDAD
        rb.linearVelocity = Vector2.zero;

        // PERMITIR MORIR OTRA VEZ DESPUÉS
        Invoke("ResetDeath", 0.5f);
    }
    if (collision.CompareTag("Goal"))
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    }
    void OnTriggerExit2D(Collider2D collision)
{
    if (collision.CompareTag("Ladder"))
    {
        isClimbing = false;
    }
}    void ResetDeath()
{
    isDead = false;
}
void OnDrawGizmosSelected()
{
    if (attackPoint == null) return;

    Gizmos.color = Color.red;
    Gizmos.DrawWireCube(attackPoint.position, new Vector3(1.5f, 1f, 1));
}
public void DoDamage()
{
    Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(
    transform.position,
    new Vector2(6f, 6f),
    0f
);

    foreach (Collider2D col in hitEnemies)
    {
        BossController boss = col.GetComponent<BossController>();

        if (boss != null)
        {
            Debug.Log("Golpeó al boss");
            boss.TakeDamage();
        }
    }
}
}