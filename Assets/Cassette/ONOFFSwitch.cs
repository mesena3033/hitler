using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;


public class ONOFFSwitch : MonoBehaviour
{
    [SerializeField]
    private GameObject gameObject;

    void Start()
    {
        if (gameObject != null)
        {
            gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if(Keyboard.current.tabKey.wasPressedThisFrame)
        {
            if (gameObject != null)
            {
                gameObject.SetActive(!gameObject.activeSelf);
            }
        }
    }
}
