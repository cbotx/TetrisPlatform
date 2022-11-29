using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Definitions;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Skins
{
    public interface ISkin
    {
        public SkinType SkinType { get; set; }

        public int TileWidth { get; set; }

        public Color32[] AvgColor { get; set;}
        public List<TileBase> GetPieceTiles(PieceShape shape, BlockType type);


    }

    public interface ISkinConnected
    {
        TileBase GetTileCutTop(TileBase tile);
        TileBase GetTileCutBottom(TileBase tile);
    }

    public static class SkinFactory
    {
        private static Dictionary<string, Type> s_skinTypeMapping = new();

        public static string s_skinPath = "skins/";

        static SkinFactory()
        {
            s_skinTypeMapping.Add("png", typeof(DefaultSkin));
            s_skinTypeMapping.Add("jpg", typeof(DefaultSkin));
            s_skinTypeMapping.Add("zip", typeof(ConnectedSkin));
            s_skinTypeMapping.Add("gif", typeof(AnimatedSkin));
        }
        public static ISkin LoadSkin(string skinFileName)
        {
            string suffix = skinFileName.Split('.').Last();
            if (!s_skinTypeMapping.ContainsKey(suffix)) return null;
            return (ISkin)Activator.CreateInstance(s_skinTypeMapping[suffix], s_skinPath + skinFileName);
        }

    }
}
