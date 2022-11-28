using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class MaterialAnimator : MonoBehaviour
{
    private Material _material;
    string _textureName;
    private int _frameCount;
    private int _fps;
    private void Update()
    {
        float time = Time.time;
        int frameIndex = (int)(time * _fps) % _frameCount;
        _material.SetTextureOffset(_textureName, new Vector2(0, 1.0f * frameIndex / _frameCount));
    }

    public void Initialize(Material material, string textureName, int frameCount, int fps)
    {
        _material = material;
        _textureName = textureName;
        _frameCount = frameCount;
        _fps = fps;
        _material.SetTextureScale(textureName, new Vector2(1, 1.0f / frameCount));
    }
};