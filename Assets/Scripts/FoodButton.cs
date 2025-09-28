using System;
using System.Collections.Generic;
using UnityEngine;

public class FoodButton : MonoBehaviour
{
    [Header("Food Setup")]
    public string foodName;              // e.g. "Apple"
    public GameObject foodPrefab;        // Prefab to spawn
    public Transform fridgeContainer;    // Parent inside fridge
    public Vector3 fridgeLocalPosition;  // Exact position in fridge
    public Vector3 spawnScale = Vector3.one; // Custom scale for this prefab

    // Shared state (accessible anywhere)
    public static List<string> storedFoods = new List<string>();
    private static Dictionary<string, GameObject> spawnedFoods = new Dictionary<string, GameObject>();

    // Event so UI can update when list changes
    public static Action OnFoodListChanged;

    // Called by the UI Button OnClick()
    public void OnFoodButtonClick()
    {
        if (string.IsNullOrEmpty(foodName) || foodPrefab == null || fridgeContainer == null)
        {
            Debug.LogWarning("FoodButton not fully set up. Assign foodName, prefab, fridgeContainer.");
            return;
        }

        if (spawnedFoods.ContainsKey(foodName))
            RemoveFood();
        else
            AddFood();
    }

    private void AddFood()
    {
        // Instantiate prefab under fridge container
        GameObject spawned = Instantiate(foodPrefab, fridgeContainer);

        // Set custom scale and position
        spawned.transform.localScale = spawnScale;
        spawned.transform.localPosition = fridgeLocalPosition;
        spawned.transform.localRotation = Quaternion.identity;

        spawnedFoods[foodName] = spawned;
        storedFoods.Add(foodName);

        OnFoodListChanged?.Invoke();
        Debug.Log($"Added {foodName} at {fridgeLocalPosition} with scale {spawnScale}");
    }

    private void RemoveFood()
    {
        if (spawnedFoods.TryGetValue(foodName, out GameObject spawned))
        {
            Destroy(spawned);
            spawnedFoods.Remove(foodName);
        }

        storedFoods.Remove(foodName);
        OnFoodListChanged?.Invoke();
        Debug.Log($"Removed {foodName} from fridge.");
    }

#if UNITY_EDITOR
    // Optional: Spawn prefab in editor for positioning
    [ContextMenu("Spawn Prefab In Editor")]
    private void SpawnInEditor()
    {
        if (foodPrefab != null && fridgeContainer != null)
        {
            GameObject spawned = UnityEditor.PrefabUtility.InstantiatePrefab(foodPrefab, fridgeContainer) as GameObject;
            spawned.transform.localPosition = fridgeLocalPosition;
            spawned.transform.localScale = spawnScale;
            spawned.transform.localRotation = Quaternion.identity;
        }
    }
#endif
}
