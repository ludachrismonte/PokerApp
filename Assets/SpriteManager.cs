using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : MonoBehaviour {

    public static SpriteManager sprite_manager;

    private Dictionary<string, Sprite> Sprites;

    private void Awake()
    {
        if (SpriteManager.sprite_manager == null)
        {
            SpriteManager.sprite_manager = this;
        }
        else
        {
            if (SpriteManager.sprite_manager != this)
            {
                Destroy(this.gameObject);
            }
        }

        Sprite[] SpritesData = Resources.LoadAll<Sprite>("cards");
        Sprites = new Dictionary<string, Sprite>();

        for (int i = 0; i < SpritesData.Length; i++)
        {
            Sprites.Add(SpritesData[i].name, SpritesData[i]);
        }
    }

    public Sprite GetSpriteByName(string name)
    {
        if (Sprites.ContainsKey(name))
            return Sprites[name];
        else
            return null;
    }
}
