using UnityEngine;
using UnityEngine.AI;

public class AnimalFeedable : MonoBehaviour
{
    public FeederController.FoodType requiredFood;
    private Animator animator;
    private HungerBar hungerBar;
    private Animal animal;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        hungerBar = GetComponentInChildren<HungerBar>();
        animal = GetComponent<Animal>(); 

    }

    public bool IsCorrectFood(FeederController.FoodType food)
    {
        return food == requiredFood;
    }

    public void Feed()
    {
        if (animal != null)
        {
            animal.StopMoving();

        }
        StartCoroutine(FeedingRoutine());
    }

    System.Collections.IEnumerator FeedingRoutine()
    {
        animator.SetBool("Eat_b", true);
        yield return new WaitForSeconds(2f); // 먹는 시간
        animator.SetBool("Eat_b", false);

        if (animal != null)
        {
            animal.ResumeMoving();  
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        FoodProjectile food = other.GetComponent<FoodProjectile>();
        if (food == null) return;

        if (IsCorrectFood(food.foodType))
        {
            Debug.Log($"{gameObject.name}이(가) {food.foodType}을 먹음");
            Feed();
            if (hungerBar != null)
                hungerBar.Feed(20f); // 예시: 20만큼 체력 증가
            Destroy(other.gameObject); // 먹이 제거
        }
        else
        {
            Debug.Log($"{gameObject.name}은(는) {food.foodType}을 먹지 않음");
        }
    }
}
