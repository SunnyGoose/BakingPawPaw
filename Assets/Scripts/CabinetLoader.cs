using System.Collections.Generic;
using UnityEngine;

public class CabinetLoader : MonoBehaviour
{
    [Header("Cabinet Setup")]
    [SerializeField] private Transform cabinetContainer; // Parent for spawned prefabs
    [SerializeField] private List<FoodButton> allFoodButtons; // Reference to all food buttons/prefabs

    private void Start()
    {
        SpawnStoredFoods();
    }

    private void SpawnStoredFoods()
    {
        if (FoodData.Instance == null)
        {
            Debug.LogError("FoodData.Instance is null! Make sure FoodData exists in the first scene.");
            return;
        }

        foreach (string foodName in FoodData.Instance.storedFoods)
        {
            // Find the corresponding FoodButton to get the prefab and spawn info
            FoodButton button = allFoodButtons.Find(fb => fb.foodName == foodName);
            if (button != null && button.foodPrefab != null)
            {
                GameObject spawned = Instantiate(button.foodPrefab, cabinetContainer);
                spawned.transform.localPosition = button.fridgeLocalPosition; // Could rename to spawnLocalPosition
                spawned.transform.localRotation = Quaternion.identity;
                spawned.transform.localScale = button.spawnScale;
            }
            else
            {
                Debug.LogWarning($"No prefab found for stored food: {foodName}");
            }
        }
    }
}
