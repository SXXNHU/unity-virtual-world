using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public Camera firstPersonCam;
    public Camera thirdPersonCam;

    private bool isFirstPerson = true;

    void Start()
    {
        ActivateFirstPerson();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isFirstPerson = !isFirstPerson;

            if (isFirstPerson)
                ActivateFirstPerson();
            else
                ActivateThirdPerson();
        }
    }

    void ActivateFirstPerson()
    {
        firstPersonCam.enabled = true;
        thirdPersonCam.enabled = false;
    }

    void ActivateThirdPerson()
    {
        firstPersonCam.enabled = false;
        thirdPersonCam.enabled = true;
    }
}
