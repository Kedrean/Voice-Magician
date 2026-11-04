
using UnityEngine;

public class LockedDoor : MonoBehaviour
{
    private Inventory inventory;
    private BoxCollider2D doorCollider;
    private bool isOpen;

    void Start()
    {
        inventory = FindFirstObjectByType<Inventory>();
        doorCollider = GetComponent<BoxCollider2D>();
    }

    public void TryUnlock()
    {
        if (isOpen) return;

        if (inventory == null)
        {
            Debug.LogWarning("No Inventory found!");
            return;
        }

        if (!inventory.HasKey)
        {
            Debug.Log("The door is locked! You need a Key.");
            return;
        }

        isOpen = true;

        if (doorCollider != null)
            doorCollider.enabled = false;

        transform.localScale = new Vector3(
            transform.localScale.x,
            0.15f,
            transform.localScale.z
        );

        Debug.Log("Abracadabra! The door is unlocked!");
    }
}
