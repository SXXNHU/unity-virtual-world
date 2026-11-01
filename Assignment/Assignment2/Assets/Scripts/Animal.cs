using UnityEngine;

public class Animal : MonoBehaviour
{
    private HungerBar hungerBar;
    private Animator animator;
    private bool isEating = false;

    public float feedAmount = 20f;

    public float moveSpeed = 1.5f;
    public float changeDirectionInterval = 3f;

    private Vector3 moveDirection;
    private float timer;


    void Start()
    {
        ChooseNewDirection();

    }
    void Update()
    {
        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        timer -= Time.deltaTime;
        if (timer <= 0f && !isEating) 
        {
            ChooseNewDirection();
        }
    }


    void ChooseNewDirection()
    {
        Vector2 random2D = Random.insideUnitCircle.normalized;
        moveDirection = new Vector3(random2D.x, 0, random2D.y);

        if (moveDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveDirection);
        }

        timer = changeDirectionInterval + Random.Range(-1f, 1f);
    }

    void Awake()
    {
        hungerBar = GetComponentInChildren<HungerBar>();
        animator = GetComponent<Animator>();
    }

    public void SetHungerBar(HungerBar bar)
    {
        hungerBar = bar;
    }

    public bool IsEating => isEating;

    public void FeedAnimal()
    {
        if (isEating) return;

        if (hungerBar != null)
        {
            isEating = true;
            hungerBar.Feed(feedAmount);

            animator.SetBool("Eat_b", true);
            StopMoving(); 

            StartCoroutine(EndEatingAfterAnimation());
        }
    }

    public void Move()
    {
        moveSpeed = 1.5f;
    }
    public void StopMoving()
    {
        moveSpeed = 0f;
    }
    public void ResumeMoving()
    {   
        moveSpeed = 1.5f;
    }

    private System.Collections.IEnumerator EndEatingAfterAnimation()
    {
        yield return new WaitForSeconds(2f);
        isEating = false;
        animator.SetBool("Eat_b", false);
        ResumeMoving();
    }
}
