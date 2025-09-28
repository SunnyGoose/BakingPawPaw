using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlobalAchievements : MonoBehaviour {
    
    //General variables
    public GameObject achNote;
    //add sound if want
    public bool achActive = false;
    public GameObject achTitle;
    public GameObject achDesc;

    //Achievement 01 Specefic
    //public GameObject ach01Image;
    public static int ach01Count;
    public int ach01Trigger = 5;
    public int ach01Code;

    public void CheckAchievements()
    {
        ach01Code = PlayerPrefs.GetInt("Ach01");

        if (ach01Count == ach01Trigger && ach01Code != 12345)
        {
            StartCoroutine(Trigger01Ach());
        }
    }

    IEnumerator Trigger01Ach()
    {
        achActive = true;
        ach01Code = 12345;
        PlayerPrefs.SetInt("Ach01", ach01Code);
        //play sound
        //ach01Image.setactive(true);
        achTitle.GetComponent<Text>().text = "Collection";
        achDesc.GetComponent<Text>().text = "made 5 recipes";
        //sound
        yield return new WaitForSeconds(7);
        //resetting UI
        //ach01Image.SetActive(false);
        //note
        achTitle.GetComponent<Text>().text = "";
        achDesc.GetComponent<Text>().text = "";
        achActive = false;
    }
}
