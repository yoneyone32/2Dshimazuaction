using System.Runtime.ExceptionServices;
using UnityEngine;
public class EnemyManager : MonoBehaviour
{
    private Rigidbody2D rb;
    private float speed;

    public LayerMask blockLayer;
    public GameObject deathEffect;

    [Header("ˆÚ“®‘¬“x")] public float runSpeed;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        direction = DIRECTION_TYPE.RIGHT;
    }

    public enum DIRECTION_TYPE
    {
        STOP,
        RIGHT,
        LEFT
    }

    DIRECTION_TYPE direction = DIRECTION_TYPE.STOP;

    private void Update()
    {
        if (!IsGround())
        {
            ChangeDirection();
        }        
    }

    private void FixedUpdate()
    {
        switch (direction)
        {
            case DIRECTION_TYPE.STOP:
                speed = 0;
                break;

            case DIRECTION_TYPE.RIGHT:
                speed = runSpeed;
                transform.localScale = new Vector3(1, 1, 1);
                break;

            case DIRECTION_TYPE.LEFT:
                speed = -runSpeed;
                transform.localScale = new Vector3(-1, 1, 1);
                break;
        }
        rb.velocity = new Vector2(speed, rb.velocity.y);
    }

    bool IsGround()
    {
        Vector3 startPoint = transform.position + Vector3.right * 0.5f * transform.localScale.x;
        Vector3 endPoint = startPoint - Vector3.up * 0.5f;
        Debug.DrawLine(startPoint, endPoint);
        return Physics2D.Linecast(startPoint, endPoint, blockLayer);
    }

    void ChangeDirection()
    {
        if(direction == DIRECTION_TYPE.RIGHT)
        {
            direction = DIRECTION_TYPE.LEFT;
        }
        else if(direction == DIRECTION_TYPE.LEFT)
        {
            direction = DIRECTION_TYPE.RIGHT;
        }
    }

    public void DestroyEnemy()
    {
        Instantiate(deathEffect, this.transform.position, this.transform.rotation);
        Destroy(this.gameObject);
    }
}

