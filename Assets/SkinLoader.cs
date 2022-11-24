using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkinLoader : MonoBehaviour
{
    public static string s_skinFileName = "jstris7.png";
    public static Sprite[] s_simpleSprites = new Sprite[8];
    private void Awake()
    {
        LoadSkin();
    }

    public void LoadSkin()
    {
        string[] splitName = s_skinFileName.Split('.');
        switch (splitName.Last()) {
            case "png":
                Texture2D texture = Resources.Load(splitName[0]) as Texture2D;
                for (int i = 0; i < 8; ++i)
                {
                    s_simpleSprites[i] = Sprite.Create(texture, new Rect(new Vector2(i * 31, 0), new Vector2(30, 30)), new Vector2(0.5f, 0.5f), 30);
                }
                break;
            default:
                break;
        }

    }
}
