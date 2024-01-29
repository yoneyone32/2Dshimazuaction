using UnityEngine;
public class PlayerManager : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    //SE
    private AudioSource audioSource;
    public AudioClip stepOnSE;
    public AudioClip getItemSE;
    public AudioClip jumpSE;

    private float speed;
    private bool isDead = false;

    public GameManager gameManager;
    public LayerMask blockLayer;
    [Header("移動速度")] public float runSpeed;
    [Header("ジャンプの高さ")]public float jumpPower;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    public enum DIRECTION_TYPE
    {
        STOP,
        RIGHT,
        LEFT
    }

    DIRECTION_TYPE direction = DIRECTION_TYPE.STOP;

    void Update()
    {
        if (isDead)
        {
            return;
        }

        float x = Input.GetAxis("Horizontal");

        animator.SetFloat("speed", Mathf.Abs(x));

        if (x == 0)
        {
            //止まっている
            direction = DIRECTION_TYPE.STOP;
        }
        else if (x > 0)
        {
            //右に動く
            direction = DIRECTION_TYPE.RIGHT;
        }
        else if (x < 0)
        {
            //左に動く
            direction = DIRECTION_TYPE.LEFT;
        }

        if (IsGround())
        {
            if (Input.GetKeyDown("space"))
            {
                Jump();
            }
            else
            {
                animator.SetBool("isJumping", false);
            }
        }
    }

    private void Jump()
    {
        rb.AddForce(Vector2.up * jumpPower);
        animator.SetBool("isJumping", true);
        audioSource.PlayOneShot(jumpSE);
    }

    bool IsGround()
    {
        Vector3 leftStartPoint = transform.position - Vector3.right * 0.2f;
        Vector3 rightStartPoint = transform.position + Vector3.right * 0.2f;
        Vector3 endPoint = transform.position - Vector3.up * 0.1f;
        Debug.DrawLine(rightStartPoint, endPoint);
        Debug.DrawLine(leftStartPoint, endPoint);
        return Physics2D.Linecast(leftStartPoint, endPoint, blockLayer)
            || Physics2D.Linecast(rightStartPoint, endPoint, blockLayer);
    }

    private void FixedUpdate()
    {
        if (isDead)
        {
            return;
        }

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead)
        {
            return;
        }

            if (collision.tag == "Trap")
        {
            Debug.Log("ゲームオーバー");
            Death();
        }

        if (collision.tag == "Finish")
        {
            Debug.Log("ゲームクリア");
            gameManager.GameClear();
        }

        if (collision.tag == "Item")
        {
            Debug.Log("ゲット");
            collision.gameObject.GetComponent<ItemManager>().GetItem();
            audioSource.PlayOneShot(getItemSE);
        }

        if(collision.tag == "Enemy")
        {
            EnemyManager enemy = collision.gameObject.GetComponent<EnemyManager>();
            if (this.transform.position.y > enemy.transform.position.y)
            {
                //上から踏んだ時
                //敵を削除
                rb.velocity = new Vector2(rb.velocity.x, 0);
                Jump();
                enemy.DestroyEnemy();
                audioSource.PlayOneShot(stepOnSE);
            }
            else
            {
                //横からぶつかったら
                Death();
            }
        }
    }

    void Death()
    {
        isDead = true;
        rb.velocity = new Vector2(0, 0);
        rb.AddForce(Vector2.up * jumpPower);
        BoxCollider2D boxCollider2D = GetComponent<BoxCollider2D>();
        Destroy(boxCollider2D);
        animator.Play("player_death_Animation");
        gameManager.GameOver();
    }
}
