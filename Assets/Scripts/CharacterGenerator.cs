using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGenerator : MonoBehaviour
{
    public SpriteRenderer applyTo;
    public Vector2 size = new Vector2(32, 64);
    public Vector2 maxTiles = new Vector2(56, 20);
    public Texture2D[] hairSprites;
    public Texture2D[] accessorySprites;
    public Texture2D[] clothesSprites;
    public Texture2D[] bodySprites;

    public int selectedIndex = 0;
    private Texture2D[] selectedTexture = new Texture2D[] { null, null, null, null };
    private Texture2D mixedTexture;
    private List<Sprite> finalSprites = new List<Sprite>();
    // Start is called before the first frame update
    void Start()
    {
        // Sprites should be ordered by paint order (Accessories last etc...)
        selectedTexture[0] = bodySprites[Random.Range(0, bodySprites.Length - 1)];
        selectedTexture[1] = clothesSprites[Random.Range(0, clothesSprites.Length - 1)];
        selectedTexture[2] = hairSprites[Random.Range(0, hairSprites.Length - 1)];
        selectedTexture[3] = accessorySprites[Random.Range(0, accessorySprites.Length - 1)];

        mixedTexture = new Texture2D(selectedTexture[0].width, selectedTexture[0].height);
        // Clear texture
        for (int x = 0; x < mixedTexture.width; x++)
            for (int y = 0; y < mixedTexture.height; y++)
                mixedTexture.SetPixel(x, y, Color.clear);
        // Overlap every texture into one
        for (int i = 0; i < selectedTexture.Length; i++)
            for (int x = 0; x < selectedTexture[i].width; x++)
                for (int y = 0; y < selectedTexture[i].height; y++)
                    mixedTexture.SetPixel(x, y, selectedTexture[i].GetPixel(x, y).a == 0 ? mixedTexture.GetPixel(x, y) : selectedTexture[i].GetPixel(x, y));

        mixedTexture.filterMode = FilterMode.Point;
        mixedTexture.Apply();
        // Index every
        for (int y = 0; y < Mathf.Floor(mixedTexture.height / size.y); y++)
            for (int x = 0; x < Mathf.Floor(mixedTexture.width / size.x); x++) {
                if (!IsTransparent(mixedTexture, new Rect(new Vector2(x * size.x, y * size.y - 32), size)))
                {
                    finalSprites.Insert(0, Sprite.Create(
                        mixedTexture,
                        new Rect(new Vector2(x * size.x, y * size.y - 32), size),
                        new Vector2(0.5f, 0.5f),
                        32
                    ));
                    finalSprites[x + y].name = "Generated Sprite - " + (x + y);
                }
            }
        applyTo.sprite = finalSprites[selectedIndex];
    }

    private void Update()
    {
       applyTo.sprite = finalSprites[selectedIndex];
    }

    bool IsTransparent(Texture2D tex, Rect rect)
    {
        for (int x = (int) rect.position.x; x < rect.size.x + rect.position.x; x++)
            for (int y = (int) rect.position.y; y < rect.size.y + rect.position.y; y++)
                if (tex.GetPixel(x, y).a != 0)
                    return false;
        return true;
    }
}
