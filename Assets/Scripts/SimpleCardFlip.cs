using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Card flip Animation
public class SimpleCardFlip : MonoBehaviour
{
    public float flipSpeed = 500f; // Speed of the flip
    private bool isFlipping = false; // To prevent multiple flips at the same time
    private bool isFaceUp = false; // Track the card's state (flipped or not)

    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnCardClick()
    {
        // Start the flip if it's not already flipping
        if (!isFlipping)
        {
            StartCoroutine(FlipCard());
        }
    }

    private System.Collections.IEnumerator FlipCard()
    {
        isFlipping = true;

        float rotation = 0f;
        while (rotation < 180f)
        {
            float step = flipSpeed * Time.deltaTime;
            rectTransform.Rotate(Vector3.up, step); // Rotate around the Y-axis
            rotation += step;

            yield return null;
        }

        // Correct the rotation after the flip is complete
        rectTransform.localRotation = Quaternion.Euler(0, isFaceUp ? 0 : 0, 0);
        isFlipping = false;
        isFaceUp = !isFaceUp;
    }
}
