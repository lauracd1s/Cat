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
        sr = GetComponentInChildren<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        // 👇 Elegir el punto más cercano al iniciar
        if (pointA != null && pointB != null)
        {
            float distA = Vector2.Distance(transform.position, pointA.position);
            float distB = Vector2.Distance(transform.position, pointB.position);

            target = (distA < distB) ? pointA : pointB;
        }
    }

    void Update()
    {
        if (canChase && player != null)
        {
            float distanceX = Mathf.Abs(player.position.x - transform.position.x);
            float distanceY = Mathf.Abs(player.position.y - transform.position.y);

            if (distanceX < detectionRange && distanceY < 1f)
            {
                isChasing = true;
            }
            else
            {
                isChasing = false;
            }
        }
        else
        {
            isChasing = false;
        }

        if (isChasing)
        {
            ChasePlayer(); 
}
        else
        {
            Patrol();
        }
    }
    // 👾 PERSEGUIR JUGADOR
    void ChasePlayer()
    {
        float dir = Mathf.Sign(player.position.x - transform.position.x);

        rb.linearVelocity = new Vector2(dir * speed, 0);

        FlipTowards(player.position);

        if (Time.time > lastAttackTime + attackCooldown)
        {
            GameManager.instance.LoseLife();
            lastAttackTime = Time.time;
        }
    }


    // 🚶 PATRULLAR
    void Patrol()
    {
        if (target == null || pointA == null || pointB == null)
            return;

        float dir = Mathf.Sign(target.position.x - transform.position.x);

        rb.linearVelocity = new Vector2(dir * speed, 0);

        float distance = Vector2.Distance(transform.position, target.position);

        if (distance < 0.3f) // 👈 más preciso
        {
            target = (target == pointA) ? pointB : pointA;
            Flip();
        }
    }

    // 🔄 VOLTEAR NORMAL
    void Flip()
    {
        if (sr != null)
            sr.flipX = !sr.flipX;
    }

    // 🎯 MIRAR AL JUGADOR
    void FlipTowards(Vector3 targetPos)
    {
        if (sr == null) return;

        if (targetPos.x > transform.position.x)
            sr.flipX = false;
        else
            sr.flipX = true;
    }
}