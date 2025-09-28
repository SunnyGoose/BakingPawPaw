using System.Collections.Generic;
using UnityEngine;

public class FoodData : MonoBehaviour
{
    public static FoodData Instance;

    public List<string> storedFoods = new List<string>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // survives scene loads
        }
        else
        {
            Destroy(gameObject); // avoid duplicates
        }
    }
}
