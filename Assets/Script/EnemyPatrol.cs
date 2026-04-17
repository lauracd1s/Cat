using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Comportamiento")]
    public bool canChase = false;

    [Header("Movimiento")]
    public float speed = 2f;
    public Transform pointA;
    public Transform pointB;

    [Header("Jugador")]
    public Transform player;
    public float detectionRange = 3f;
    public float attackRange = 1.5f;
    public float attackCooldown = 1.5f;

    private float lastAttackTime;
    private Transform target;
    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private bool isChasing = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponentInChildren<SpriteRenderer>();

        if (pointA != null && pointB != null)
        {
            float distA = Mathf.Abs(transform.position.x - pointA.position.x);
            float distB = Mathf.Abs(transform.position.x - pointB.position.x);

            target = (distA < distB) ? pointA : pointB;
        }
    }

    void FixedUpdate()
    {
        if (pointA == null || pointB == null) return;

        // 🔥 DETECCIÓN
        if (canChase && player != null)
        {
            float distanceX = Mathf.Abs(player.position.x - transform.position.x);
            float distanceY = Mathf.Abs(player.position.y - transform.position.y);

            isChasing = (distanceX < detectionRange && distanceY < 1f);
        }
        else
        {
            isChasing = false;
        }

        if (isChasing)
            ChasePlayer();
        else
            Patrol();
    }

    // 👾 PERSEGUIR
    void ChasePlayer()
    {
        float dir = Mathf.Sign(player.position.x - transform.position.x);

        rb.linearVelocity = new Vector2(dir * speed, rb.linearVelocity.y);

        FlipTowards(player.position);

        float distance = Mathf.Abs(transform.position.x - player.position.x);

        if (distance < attackRange && Time.time > lastAttackTime + attackCooldown)
        {
            GameManager.instance.LoseLife();
            lastAttackTime = Time.time;
        }
    }

    // 🚶 PATRULLAR (ARREGLADO)
    void Patrol()
    {
        if (pointA == null || pointB == null) return;

        if (target == null)
            target = pointA;

        float posX = transform.position.x;
        float targetX = target.position.x;

        float dir = (targetX > posX) ? 1f : -1f;

        rb.linearVelocity = new Vector2(dir * speed, rb.linearVelocity.y);

        if (Mathf.Abs(posX - targetX) < 0.2f)
        {
            target = (target == pointA) ? pointB : pointA;
        }

        if (sr != null)
            sr.flipX = (dir < 0);
    }

    // 🎯 MIRAR JUGADOR
    void FlipTowards(Vector3 targetPos)
    {
        if (sr == null) return;
        sr.flipX = (targetPos.x < transform.position.x);
    }
}