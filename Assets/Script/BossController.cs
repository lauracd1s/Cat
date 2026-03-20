using UnityEngine;

public class BossController : MonoBehaviour
{
    public int health = 5;
    public float speed = 2f;

    public Transform pointA;
    public Transform pointB;
    public Transform player;
    public GameObject goal;

    private Transform target;
    private Animator animator;
    private SpriteRenderer sr;
    private bool isAttacking = false;

    private float attackRange = 2f;
    private float attackCooldown = 2f;
    private float lastAttackTime;

    void Start()
    {
        target = pointB; // empieza hacia B
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
{
    float distanceToPlayer = Vector2.Distance(transform.position, player.position);

    // 🔴 ATAQUE
    if (distanceToPlayer < attackRange)
    {
        animator.SetBool("isWalking", false);

        if (Time.time > lastAttackTime + attackCooldown)
        {
            animator.SetTrigger("Attack");
            lastAttackTime = Time.time;
        }

        return; // 👈 IMPORTANTE: evita que se mueva mientras ataca
    }

    // 🟢 MOVIMIENTO
    animator.SetBool("isWalking", true);

    transform.position = Vector2.MoveTowards(
        transform.position,
        target.position,
        speed * Time.deltaTime
    );

    // 🔥 CAMBIO DE DIRECCIÓN (FORMA SEGURA)
    if (Mathf.Abs(transform.position.x - target.position.x) < 0.1f)
    {
        if (target == pointA)
        {
            target = pointB;
        }
        else
        {
            target = pointA;
        }

        Flip();
    }
}

    void Flip()
    {
        sr.flipX = !sr.flipX;
    }

    // 💥 RECIBIR DAÑO
    public void TakeDamage()
{
    health--;
    Debug.Log("Boss recibió daño. Vida: " + health);

    if (health <= 0)
    {
        Debug.Log("Boss muerto");

        if (goal != null)
        {
            goal.SetActive(true); // 🔥 ACTIVA EL GOAL
        }

        Destroy(gameObject);
    }
}

    // 💀 DAÑO AL JUGADOR
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && isAttacking)
{
    GameManager.instance.LoseLife();
}
    }
    public void StartAttack()
{
    isAttacking = true;
}

public void EndAttack()
{
    isAttacking = false;
}
}