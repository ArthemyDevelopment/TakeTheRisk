
using System;
using UnityEngine;

public class TriggerDoor : MonoBehaviour
{
    [SerializeField]private bool B_isOpen = false;
    [SerializeField] private Animator An_Door;
    [SerializeField] private DoorsId id;

#if UNITY_EDITOR
    [SerializeField] private bool B_Debug;

    private void Awake()
    {
        if (PlayerPrefs.HasKey("Door" + id))
        {
            PlayerPrefs.DeleteKey("Door"+id);
        }
    }

#endif
    
    private void OnEnable()
    {
        if (PlayerPrefs.HasKey("Door" + id))
        {
            if (PlayerPrefs.GetInt("Door" + id) == 1)
            {
                B_isOpen = true;
                An_Door.SetBool("Open", true);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player/Collider"))
        {
            if (!B_isOpen)
            {
                B_isOpen = true;
                An_Door.SetBool("Open", true);
                PlayerPrefs.SetInt("Door"+id, 1);
                
            }
        }
    }
}
