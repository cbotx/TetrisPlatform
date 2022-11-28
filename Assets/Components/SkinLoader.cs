using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using Assets.Utils;
using Assets.Definitions;
using Assets.Skins;
public static class SkinLoader
{
    public static string s_skinPath = "skins/";

    public static SkinBase LoadSkin(string skinFileName)
    {
        string suffix = skinFileName.Split('.').Last();

        if (suffix == "png") {
            DefaultSkin skin = new();
            Texture2D texture = FileUtils.LoadTextureFromFile(s_skinPath + skinFileName);
            skin.BaseGhostTexture = texture;
            int tileSise = SkinDefinitions.GetDefaultTextureTileSize(texture.width);
            skin.TileWidth = tileSise;
            for (int i = 0; i < 7; ++i)
            {
                skin.s_simpleSprites[i] = Sprite.Create(texture, new Rect(new Vector2(i * (tileSise + 1) + 1, 1), new Vector2(tileSise - 2, tileSise - 2)), new Vector2(0.5f, 0.5f), tileSise - 2);
                skin.s_tiles[i] = ScriptableObject.CreateInstance<Tile>();
                skin.s_tiles[i].sprite = skin.s_simpleSprites[i];
            }
            skin.PostLoading();
            return skin;
        }
        else if (suffix == "gif")
        {
            AnimatedSkin skin = new();
            List<Texture2D> textures = FileUtils.LoadTextureFromGIF(s_skinPath + skinFileName);
            skin.BaseGhostTextures = textures;
            int tileSise = SkinDefinitions.GetDefaultTextureTileSize(textures.First().width);
            skin.TileWidth = tileSise;
            for (int i = 0; i < 7; ++i)
            {
                skin.s_tiles[i] = ScriptableObject.CreateInstance<AnimatedTile>();
                Sprite[] sprites = new Sprite[textures.Count];
                for (int j = 0; j < textures.Count; ++j)
                {
                    sprites[j] = Sprite.Create(textures[j], new Rect(new Vector2(i * (tileSise + 1) + 1, 1), new Vector2(tileSise - 2, tileSise - 2)), new Vector2(0.5f, 0.5f), tileSise - 2);
                }
                skin.s_tiles[i] = ScriptableObject.CreateInstance<AnimatedTile>();
                skin.s_tiles[i].m_AnimatedSprites = sprites;
                skin.s_tiles[i].m_MinSpeed = skin.AnimationSpeed;
                skin.s_tiles[i].m_MaxSpeed = skin.AnimationSpeed;
                skin.s_static_tiles[i] = ScriptableObject.CreateInstance<Tile>();
                skin.s_static_tiles[i].sprite = sprites.First();
            }
            skin.PostLoading();
            return skin;
        }
        else if (suffix == "zip") {
            ConnectedSkin skin = new();
            Dictionary<string, Texture2D> textures = FileUtils.LoadTexturesFromZip(s_skinPath + skinFileName);
            foreach (var item in textures)
            {
                string skinType = item.Key.Split('.').First().Split('_').Last();

                Texture2D texture = item.Value;
                if (skinType == "minos")
                {
                    int tileSize = SkinDefinitions.GetConnectedMinoTextureTileSize(texture.width);
                    skin.TileWidth = tileSize;
                    for (int i = 0; i < 7; ++i)
                    {
                        for (int j = 0; j < 24; ++j)
                        {
                            skin.s_connectedTiles[i, j] = ScriptableObject.CreateInstance<Tile>();
                            skin.s_connectedTiles[i, j].sprite = GetSpriteInConnectedTexture(texture, i, j, tileSize);
                        }
                    }

                }
                else if (skinType == "ghost")
                {
                    skin.BaseGhostTexture = texture;
                }
            }
            skin.PostLoading();
            return skin;
        }
        return null;
    }

    private static Sprite GetSpriteInConnectedTexture(Texture2D texture, int pieceId, int tileIdx, int unitSize)
    {
        int x = (SkinDefinitions.ConnectedTexturePosition[pieceId, 0] + tileIdx % 4) * unitSize;
        int y = texture.height - (SkinDefinitions.ConnectedTexturePosition[pieceId, 1] + tileIdx / 4) * unitSize - unitSize;
        return Sprite.Create(texture, new Rect(new Vector2(x + 1, y + 1), new Vector2(unitSize - 2, unitSize - 2)), new Vector2(0.5f, 0.5f), unitSize - 2);
    }

}
