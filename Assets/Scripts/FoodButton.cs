using System;
using System.Collections.Generic;
using UnityEngine;

public class FoodButton : MonoBehaviour
{
    [Header("Food Setup")]
    public string foodName;              
    public GameObject foodPrefab;        
    public Transform fridgeContainer;    
    public Vector3 fridgeLocalPosition;  
    public Vector3 spawnScale = Vector3.one; 

    // Track spawned objects (only while this scene is active)
    private static Dictionary<string, GameObject> spawnedFoods = new Dictionary<string, GameObject>();

    public static Action OnFoodListChanged;

    public void OnFoodButtonClick()
    {
        if (string.IsNullOrEmpty(foodName) || foodPrefab == null || fridgeContainer == null)
        {
            Debug.LogWarning("FoodButton not fully set up. Assign foodName, prefab, fridgeContainer.");
            return;
        }

        if (FoodData.Instance.storedFoods.Contains(foodName))
            RemoveFood();
        else
            AddFood();
    }

    private void AddFood()
    {
        GameObject spawned = Instantiate(foodPrefab, fridgeContainer);
        spawned.transform.localScale = spawnScale;
        spawned.transform.localPosition = fridgeLocalPosition;
        spawned.transform.localRotation = Quaternion.identity;

        spawnedFoods[foodName] = spawned;
        FoodData.Instance.storedFoods.Add(foodName);

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

        FoodData.Instance.storedFoods.Remove(foodName);

        OnFoodListChanged?.Invoke();
        Debug.Log($"Removed {foodName} from fridge.");
    }

    // Called when reloading the fridge scene to rebuild all foods
    public static void RebuildFridge(Transform fridgeContainer, Dictionary<string, FoodButton> buttonLookup)
    {
        spawnedFoods.Clear();

        foreach (string food in FoodData.Instance.storedFoods)
        {
            if (buttonLookup.TryGetValue(food, out FoodButton fb))
            {
                GameObject spawned = GameObject.Instantiate(fb.foodPrefab, fridgeContainer);
                spawned.transform.localScale = fb.spawnScale;
                spawned.transform.localPosition = fb.fridgeLocalPosition;
                spawned.transform.localRotation = Quaternion.identity;

                spawnedFoods[food] = spawned;
            }
        }
    }
}
