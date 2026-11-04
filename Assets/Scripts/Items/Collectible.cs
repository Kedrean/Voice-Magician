
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Collectible : MonoBehaviour
{
    public string itemName = "Key";

    private bool playerNearby;
    private bool collected;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;
            Debug.Log($"Nearby: {itemName}. Press E to collect.");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
        }
    }

    void Update()
    {
        if (playerNearby && !collected && Input.GetKeyDown(KeyCode.E))
        {
            Collect();
        }
    }

    
    public void Collect()
    {
        if (collected) return;

        collected = true;

        Inventory inventory = FindFirstObjectByType<Inventory>();

        if (inventory != null)
        {
            inventory.AddItem(itemName);
        }

        Debug.Log("Collected: " + itemName + "!");
        Destroy(gameObject);
    }
}
