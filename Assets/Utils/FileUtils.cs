﻿using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


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