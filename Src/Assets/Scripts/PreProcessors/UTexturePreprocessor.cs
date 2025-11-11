using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UTexturePreprocessor : AssetPostprocessor
{
    public static bool isHD = false;
    void OnPreprocessTexture()
    {
        TextureImporter textureImporter = (TextureImporter)assetImporter;
        textureImporter.isReadable = false;
        if (assetPath.StartsWith("Assets/_UI"))
        {
            textureImporter.mipmapEnabled = false; // Audio disable mipmap
            textureImporter.textureType = TextureImporterType.Sprite;
            textureImporter.crunchedCompression = true;
            textureImporter.compressionQuality = 0;
        }
        else if (assetPath.StartsWith("Assets/Units"))
        {
            textureImporter.textureFormat = TextureImporterFormat.AutomaticCompressed;
        }
        else if (assetPath.StartsWith("Assets/FX/Textures"))
        {
            if (textureImporter.textureFormat == TextureImporterFormat.AutomaticTruecolor
                || textureImporter.textureFormat == TextureImporterFormat.Automatic16bit
                || textureImporter.textureFormat == TextureImporterFormat.Alpha8
                || textureImporter.textureFormat == TextureImporterFormat.ARGB16
                || textureImporter.textureFormat == TextureImporterFormat.RGB24
                || textureImporter.textureFormat == TextureImporterFormat.RGBA32
                || textureImporter.textureFormat == TextureImporterFormat.ARGB32
                || textureImporter.textureFormat == TextureImporterFormat.RGB16
                || textureImporter.textureFormat == TextureImporterFormat.RGBA16
                )
            {
                textureImporter.textureFormat = TextureImporterFormat.AutomaticCompressed;
            }
        }
    }

}
