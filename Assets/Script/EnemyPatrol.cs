using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public float speed = 2f;
    public Transform pointA;
    public Transform pointB;

    private Transform target;
    private bool goingToB = true;

    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        target = pointB; // empieza yendo a B
    }

    void Update()
{
    Vector2 currentPosition = new Vector2(transform.position.x, transform.position.y);
    Vector2 targetPosition = new Vector2(target.position.x, target.position.y);

    transform.position = Vector2.MoveTowards(
        currentPosition,
        targetPosition,
        speed * Time.deltaTime
    );

    if (Vector2.Distance(currentPosition, targetPosition) < 0.2f)
    {

        target = (target == pointA) ? pointB : pointA;

        Flip();
    }
}

    void Flip()
    {
        sr.flipX = !sr.flipX;
    }
}