using System.Collections.Generic;
using UnityEngine;

public class FridgeLoader : MonoBehaviour
{
    public Transform fridgeContainer;
    public List<FoodButton> foodButtons; // assign all your FoodButtons in Inspector

    private void Start()
    {
        // Make a lookup so we can rebuild prefabs
        Dictionary<string, FoodButton> lookup = new Dictionary<string, FoodButton>();
        foreach (var fb in foodButtons)
        {
            lookup[fb.foodName] = fb;
        }

        FoodButton.RebuildFridge(fridgeContainer, lookup);
    }
}
