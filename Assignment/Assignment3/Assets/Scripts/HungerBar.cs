using UnityEngine;
using UnityEngine.UI;

public class HungerBar : MonoBehaviour
{
    public Image fillImage;
    private float maxHunger = 100f;
    private float currentHunger;
    private Transform cameraTransform;
    private Animal parentAnimal;

    public void Initialize(Animal animal, Transform cameraT)
    {
        parentAnimal = animal;
        cameraTransform = cameraT;
        currentHunger = maxHunger;
        UpdateBar();
    }

    void Update()
    {
        if (cameraTransform == null || parentAnimal == null) return;
        currentHunger -= Time.deltaTime * 5f;

        if (currentHunger <= 0f)
        {
            Destroy(parentAnimal.gameObject);
            Destroy(gameObject); 
        }
        UpdateBar();
    }

    void LateUpdate()
    {
        if (cameraTransform == null) return;
        Vector3 lookDirection = transform.position - cameraTransform.position;
        transform.rotation = Quaternion.LookRotation(lookDirection, Vector3.up);
    }

    public void Feed(float amount)
    {
        currentHunger = Mathf.Min(currentHunger + amount, maxHunger);
        UpdateBar();
    }

    private void UpdateBar()
    {
        if (fillImage != null)
            fillImage.fillAmount = currentHunger / maxHunger;
    }
}
