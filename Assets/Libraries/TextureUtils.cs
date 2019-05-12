using System;
using UnityEngine;

public static class TextureUtils
{

    public static Texture2D LoadTexture_PNG(string path, int width, int height, bool relativePath = true,
                        TextureFormat textureFormat = TextureFormat.ARGB32, bool mipmap = false)
    {
        byte[] byteArray = null;

        if (relativePath)
            byteArray = System.IO.File.ReadAllBytes($"{Application.streamingAssetsPath}/{path}");
        else
            byteArray = System.IO.File.ReadAllBytes(path);


        Texture2D texture = TextureUtils.LoadTexture_PNG(byteArray, width, height, textureFormat, mipmap);

        byteArray = null;
        return texture;
    }


    public static Texture2D LoadTexture_PNG(byte[] byteArray, int width, int height,
                        TextureFormat textureFormat = TextureFormat.ARGB32, bool mipmap = false)
    {

        Texture2D texture = new Texture2D(width, height, textureFormat, mipmap);
        texture.LoadImage(byteArray);
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Repeat;
        texture.Apply();

        return texture;
    }

    /// <summary>
    /// Loads a DDS Texture from Streaming Assets Folder
    /// Image must be Flipped Vertically    
    /// </summary>
    /// <param name="path"></param>
    /// <param name="textureFormat"></param>
    /// <param name="mipmap">whether mipmaps must be generated for this texture</param>
    /// <returns>Texture2D</returns>
    public static Texture2D LoadTexture_DDS(string path,
                    TextureFormat textureFormat = TextureFormat.DXT5, bool mipmap = false)
    {
        byte[] byteArray = System.IO.File.ReadAllBytes($"{Application.streamingAssetsPath}/{path}");

        Texture2D texture = TextureUtils.LoadTexture_DDS(byteArray, textureFormat, mipmap);

        byteArray = null;
        return texture;
    }

    /// <summary>
    /// Loads a DDS Texture from Streaming Assets Folder
    /// Image must be Flipped Vertically
    /// </summary>
    /// <param name="byteArray"></param>
    /// <param name="textureFormat"></param>
    /// <param name="mipmap">whether mipmaps must be generated for this texture</param>
    /// <returns>Texture2D</returns>
    public static Texture2D LoadTexture_DDS(byte[] byteArray,
                    TextureFormat textureFormat = TextureFormat.DXT5, bool mipmap = false)
    {
        if (textureFormat != TextureFormat.DXT1 && textureFormat != TextureFormat.DXT5)
            throw new Exception("Invalid TextureFormat. Only DXT1 and DXT5 formats are supported by this method.");

        byte ddsSizeCheck = byteArray[4];
        if (ddsSizeCheck != 124)
            throw new Exception("Invalid DDS DXTn texture. Unable to read");  //this header byte should be 124 for DDS image files

        int height = byteArray[13] * 256 + byteArray[12];
        int width = byteArray[17] * 256 + byteArray[16];

        const int DDS_HEADER_SIZE = 128;
        byte[] dxtBytes = new byte[byteArray.Length - DDS_HEADER_SIZE];
        Buffer.BlockCopy(byteArray, DDS_HEADER_SIZE, dxtBytes, 0, byteArray.Length - DDS_HEADER_SIZE);

        Texture2D texture = new Texture2D(width, height, textureFormat, mipmap);
        texture.LoadRawTextureData(dxtBytes);
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Repeat;
        texture.Apply();

        dxtBytes = null;
        return (texture);

    }
}
