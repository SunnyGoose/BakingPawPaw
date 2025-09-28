using UnityEngine;

public class ChangeSpriteColor : MonoBehaviour
{
    public SpriteRenderer targetSprite;  // Drag your sprite here
    public Color newColor = Color.green;

    public void ChangeColor()
    {
        if (targetSprite != null)
            targetSprite.color = newColor;
    }
}
