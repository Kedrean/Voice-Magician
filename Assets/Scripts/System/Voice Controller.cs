
using UnityEngine;
using UnityEngine.Windows.Speech;
using System;
using System.Collections.Generic;

public class VoiceController : MonoBehaviour
{
    private KeywordRecognizer recognizer;

    [Header("Voice Settings")]
    public float collectionRange = 1.5f;
    public Transform playerTransform;

    [Header("Spell Settings")]
    public GameObject fireballPrefab;
    public float spellSpawnDistance = 0.6f;
    private PlayerMovement playerMovement;

    void Start()
    {
        string[] keywords =
        {
            "Key",
            "Potion",
            "Fireball",
            "Abracadabra"
        };

        recognizer = new KeywordRecognizer(
            keywords,
            ConfidenceLevel.Medium
        );

        recognizer.OnPhraseRecognized += OnPhraseRecognized;
        recognizer.Start();

        playerMovement = playerTransform.GetComponent<PlayerMovement>();

        Debug.Log("Voice recognition started. Say a command!");
    }

    void OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        string command = args.text.ToLower();

        Debug.Log("Voice command: " + command);

        switch (command)
        {
            case "key":
                CollectItem("Key");
                break;

            case "potion":
                CollectItem("Potion");
                break;

            case "fireball":
                CastFireball();
                break;

            case "abracadabra":
                UnlockDoor();
                break;
        }
    }

    void CollectItem(string itemName)
    {
        Collectible[] items =
            FindObjectsByType<Collectible>(FindObjectsSortMode.None);

        Collectible nearestItem = null;
        float nearestDistance = collectionRange;

        foreach (Collectible item in items)
        {
            if (!item.itemName.Equals(
                itemName,
                StringComparison.OrdinalIgnoreCase))
                continue;

            float distance = Vector2.Distance(
                playerTransform.position,
                item.transform.position
            );

            if (distance <= nearestDistance)
            {
                nearestDistance = distance;
                nearestItem = item;
            }
        }

        if (nearestItem != null)
        {
            nearestItem.Collect();
            Debug.Log("Voice collected: " + itemName);
        }
        else
        {
            Debug.Log("No " + itemName + " nearby.");
        }
    }

    void CastFireball()
    {
        if (fireballPrefab == null || playerMovement == null)
        {
            Debug.LogWarning("Fireball prefab or PlayerMovement is missing!");
            return;
        }

        Vector2 direction = playerMovement.FacingDirection;

        Vector3 spawnPosition = playerTransform.position
            + (Vector3)(direction * spellSpawnDistance);

        GameObject fireball = Instantiate(
            fireballPrefab,
            spawnPosition,
            Quaternion.identity
        );

        fireball.GetComponent<FireballProjectile>().Launch(direction);

        Debug.Log("Fireball cast!");
    }

    
    void UnlockDoor()
    {
        LockedDoor[] doors =
            FindObjectsByType<LockedDoor>(FindObjectsSortMode.None);

        LockedDoor nearestDoor = null;
        float nearestDistance = 2.5f;

        foreach (LockedDoor door in doors)
        {
            float distance = Vector2.Distance(
                playerTransform.position,
                door.transform.position
            );

            if (distance <= nearestDistance)
            {
                nearestDistance = distance;
                nearestDoor = door;
            }
        }

        if (nearestDoor != null)
        {
            nearestDoor.TryUnlock();
        }
        else
        {
            Debug.Log("No door nearby!");
        }
    }

    void OnDestroy()
    {
        if (recognizer != null)
        {
            recognizer.OnPhraseRecognized -= OnPhraseRecognized;

            if (recognizer.IsRunning)
                recognizer.Stop();

            recognizer.Dispose();
        }
    }
}
