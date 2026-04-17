using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Comportamiento")]
    public bool canChase = false;

    [Header("Movimiento")]
    public float speed = 2f;
    public Transform pointA;
    public Transform pointB;

    [Header("Suelo (ANTI CAÍDA)")]
    public Transform groundCheck;
    public float groundCheckDistance = 0.5f;
    public LayerMask groundLayer;

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

        // DETECCIÓN
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

    // 🚶 PATRULLAR
    void Patrol()
    {
        if (target == null)
            target = pointA;

        float posX = rb.position.x;
        float targetX = target.position.x;

        float dir = Mathf.Sign(targetX - posX);

        // 🔥 DETECTAR BORDE (CLAVE)
        if (!IsGroundAhead(dir))
        {
            target = (target == pointA) ? pointB : pointA;
            dir *= -1;
        }

        // mover solo en X
        rb.linearVelocity = new Vector2(dir * speed, rb.linearVelocity.y);

        // cambio de dirección normal
        if (Vector2.Distance(rb.position, target.position) < 0.5f)
        {
            target = (target == pointA) ? pointB : pointA;
        }

        // limitar entre A y B
        float minX = Mathf.Min(pointA.position.x, pointB.position.x);
        float maxX = Mathf.Max(pointA.position.x, pointB.position.x);

        if (posX < minX)
        {
            rb.position = new Vector2(minX, rb.position.y);
            target = pointB;
        }

        if (posX > maxX)
        {
            rb.position = new Vector2(maxX, rb.position.y);
            target = pointA;
        }

        // volteo
        if (sr != null)
            sr.flipX = (dir < 0);
    }

    // 🔍 DETECTAR SUELO ADELANTE
    bool IsGroundAhead(float dir)
    {
        if (groundCheck == null) return true;

        Vector2 origin = groundCheck.position;
        Vector2 direction = new Vector2(dir, -1);

        RaycastHit2D hit = Physics2D.Raycast(origin, direction, groundCheckDistance, groundLayer);

        return hit.collider != null;
    }

    // 🎯 MIRAR JUGADOR
    void FlipTowards(Vector3 targetPos)
    {
        if (sr == null) return;
        sr.flipX = (targetPos.x < transform.position.x);
    }
}