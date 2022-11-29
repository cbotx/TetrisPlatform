using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Utils
{
    public static class GraphicsUtils
    {
        public static Color32 AverageColorFromTexture(Texture2D tex)
        {
            Color32[] texColors = tex.GetPixels32();

            int total = texColors.Length;

            float r = 0;
            float g = 0;
            float b = 0;

            for (int i = 0; i < total; i++)
            {
                r += texColors[i].r;
                g += texColors[i].g;
                b += texColors[i].b;
            }
            return new Color32((byte)(r / total), (byte)(g / total), (byte)(b / total), 0);
        }
        public static Color32 AverageColorFromTexture(Texture2D tex, RectInt rect)
        {
            Color32[] texColors = tex.GetPixels32();

            int total = rect.width * rect.height;

            float r = 0;
            float g = 0;
            float b = 0;

            for (int i = 0; i < rect.height; i++)
            {

                for (int j = 0; j < rect.width; j++)
                {
                    int idx = tex.width * (i + rect.y) + j + rect.x;
                    r += texColors[idx].r;
                    g += texColors[idx].g;
                    b += texColors[idx].b;
                }
            }
            return new Color32((byte)(r / total), (byte)(g / total), (byte)(b / total), 255);
        }
    }
}
