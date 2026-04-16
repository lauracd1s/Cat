using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
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
    private bool isChasing = false;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        target = pointA;
        target = pointB;

    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        // Detecta jugador
         if (distance < detectionRange)
         {
             isChasing = true;
         }

        if (isChasing)
        {
            ChasePlayer(distance);
        }
        else
        {
            Patrol();
        }
    }

    // 👾 PERSEGUIR JUGADOR
    void ChasePlayer(float distance)
    {
        transform.position = Vector2.MoveTowards(
            transform.position,
            player.position,
            speed * Time.deltaTime
        );

        FlipTowards(player.position);

        // ATAQUE
        if (distance < attackRange && Time.time > lastAttackTime + attackCooldown)
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

        transform.position = Vector2.MoveTowards(
            transform.position,
            target.position,
            speed * Time.deltaTime
        );

        float distance = Vector2.Distance(transform.position, target.position);

        if (distance < 0.5f)
        {
            transform.position = target.position;

            if (target == pointA)
                target = pointB;
            else
                target = pointA;

            Debug.Log("CAMBIO DE PUNTO"); // 👈 IMPORTANTE
            Flip();
        }
    }

    // 🔄 VOLTEAR NORMAL
    void Flip()
    {
        sr.flipX = !sr.flipX;
    }

    // 🎯 MIRAR AL JUGADOR
    void FlipTowards(Vector3 targetPos)
    {
        if (targetPos.x > transform.position.x)
            sr.flipX = false;
        else
            sr.flipX = true;
    }
}
