using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Definitions
{
    public static class MaterialDefinitions
    {
        public static Material material_AlphaMask = new (Shader.Find("AlphaMask"));
    }
}
