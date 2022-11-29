using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using MG.GIF;

namespace Assets.Utils
{
    public static class FileUtils
    {
        public static Texture2D LoadTextureFromFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                byte[] fileData = File.ReadAllBytes(filePath);
                Texture2D texture = new(2, 2);
                if (texture.LoadImage(fileData))
                {
                    return texture;
                }
            }
            return null;
        }

        public static (List<Texture2D>, int) LoadTextureFromGIF(string filePath)
        {
            if (File.Exists(filePath))
            {
                int totalDelay = 0;
                List<Texture2D> textures = new();
                byte[] data = File.ReadAllBytes(filePath);

                using (var decoder = new MG.GIF.Decoder(data))
                {
                    var img = decoder.NextImage();

                    while (img != null)
                    {
                        textures.Add(img.CreateTexture());
                        totalDelay += img.Delay;
                        img = decoder.NextImage();
                    }
                }
                return (textures, Convert.ToInt32(1000.0f * textures.Count / totalDelay));
            }

            return (null, 0);
        }

        public static Dictionary<string, Texture2D> LoadTexturesFromZip(string filePath)
        {
            Dictionary<string, Texture2D> textures = new();
            using (ZipArchive zip = ZipFile.Open(filePath, ZipArchiveMode.Read))
            {
                foreach (ZipArchiveEntry entry in zip.Entries)
                {
                    using var stream = entry.Open();
                    using var ms = new MemoryStream();
                    stream.CopyTo(ms);
                    ms.Position = 0;
                    Texture2D texture = new(2, 2);
                    if (texture.LoadImage(ms.ToArray()))
                    {
                        textures[entry.Name] = texture;
                    }
                }
            }
            return textures;
        }
    }
}
