using UnityEngine;

public class BatMovement : MonoBehaviour
{
    public float speed = 2f;

    [Header("Zona")]
    public float moveRange = 3f;
    public float heightRange = 1f;

    [Header("Jugador")]
    public Transform player;
    public float detectionRange = 4f;

    [Header("Ataque")]
    public float attackRange = 1.2f;
    public float attackCooldown = 1.5f;

    private float lastAttackTime;
    private Vector3 startPos;
    private SpriteRenderer sr;
    private float direction = 1f;

    void Start()
    {
        startPos = transform.position;
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (player == null)
        {
            Patrol();
            return;
        }

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance < detectionRange)
            ChasePlayer(distance);
        else
            Patrol();
    }

    // 🟢 PATRULLA SUAVE
    void Patrol()
    {
        float newX = transform.position.x + direction * speed * Time.deltaTime;

        // límites horizontales
        if (newX > startPos.x + moveRange)
        {
            newX = startPos.x + moveRange;
            direction = -1;
        }
        else if (newX < startPos.x - moveRange)
        {
            newX = startPos.x - moveRange;
            direction = 1;
        }

        float newY = startPos.y + Mathf.Sin(Time.time * 2f) * heightRange;

        transform.position = new Vector3(newX, newY, transform.position.z);

        if (sr != null)
            sr.flipX = (direction < 0);
    }

    // 🔥 PERSEGUIR + ATAQUE
    void ChasePlayer(float distance)
    {
        Vector3 newPos = Vector3.MoveTowards(
            transform.position,
            player.position,
            speed * Time.deltaTime
        );

        // límites para que NO se escape
        newPos.x = Mathf.Clamp(newPos.x, startPos.x - moveRange, startPos.x + moveRange);
        newPos.y = Mathf.Clamp(newPos.y, startPos.y - heightRange, startPos.y + heightRange);

        transform.position = newPos;

        float dir = player.position.x - transform.position.x;

        if (sr != null)
            sr.flipX = (dir < 0);

        // 💥 ATAQUE
        if (distance < attackRange && Time.time > lastAttackTime + attackCooldown)
        {
            GameManager.instance.LoseLife();
            lastAttackTime = Time.time;

            Debug.Log("Murciélago atacó 🦇");
        }
    }
}