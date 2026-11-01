using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FeederController : MonoBehaviour
{
    public enum FoodType { Bone, Apple, Carrot }
    public FoodType currentFood = FoodType.Bone;

    public GameObject bonePrefab;
    public GameObject applePrefab;
    public GameObject carrotPrefab;

    public Transform foodSpawnPoint;
    public float throwForce = 10f;

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            CycleFoodType();
            Debug.Log($"[Feeder] 현재 음식: {currentFood}");
        }

        if (Input.GetMouseButtonDown(0))
        {
            ThrowFood();
        }
    }

    private void CycleFoodType()
    {
        currentFood = (FoodType)(((int)currentFood + 1) % 3);
    }

    private void ThrowFood()
    {
        GameObject foodObj = Instantiate(GetFoodPrefab(currentFood), foodSpawnPoint.position, Quaternion.identity);

        Rigidbody rb = foodObj.GetComponent<Rigidbody>();
        if (rb == null)
            rb = foodObj.AddComponent<Rigidbody>();

        rb.useGravity = true;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.AddForce(foodSpawnPoint.forward * throwForce, ForceMode.VelocityChange);

        FoodProjectile projectile = foodObj.GetComponent<FoodProjectile>();
        if (projectile == null)
            projectile = foodObj.AddComponent<FoodProjectile>();

        projectile.foodType = currentFood;

        Destroy(foodObj, 3f);
    }


    private GameObject GetFoodPrefab(FoodType food)
    {
        return food switch
        {
            FoodType.Bone => bonePrefab,
            FoodType.Apple => applePrefab,
            FoodType.Carrot => carrotPrefab,
            _ => null
        };
    }

    public FoodType GetCurrentFoodType()
    {
        return currentFood;
    }

}
