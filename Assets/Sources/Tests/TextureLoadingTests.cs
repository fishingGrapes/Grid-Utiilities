using System;
using UnityEngine;

public class TextureLoadingTests : MonoBehaviour
{
    private Texture2D PNGTexture;
    private Texture2D DDSTexture;

    private void Awake()
    {
        this.LoadTextures();
    }

    private void LoadTextures()
    {

        System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
        stopWatch.Reset();

        stopWatch.Start();

        Unity.Collections.NativeArray<byte> data = new Unity.Collections.NativeArray<byte>(System.IO.File.ReadAllBytes($"Texture_PNG.png"), Unity.Collections.Allocator.Temp);

        PNGTexture = new Texture2D(912, 513, TextureFormat.ARGB32, true);
        PNGTexture.LoadImage(data.ToArray());
        PNGTexture.Apply();



        data.Dispose();

        stopWatch.Stop();
        Debug.Log($"<color=red>Time taken for PNG :</color> {stopWatch.ElapsedMilliseconds} ms");

        stopWatch.Reset();
        stopWatch.Start();

        DDSTexture = TextureUtils.LoadTexture_DDS(System.IO.File.ReadAllBytes($"Texture_DDS.DDS"), TextureFormat.DXT5);
        stopWatch.Stop();
        Debug.Log($"<color=red>Time taken for DDS :</color> {stopWatch.ElapsedMilliseconds} ms");

    }




    private void OnDrawGizmos()
    {
        if (DDSTexture != null)
            Graphics.DrawTexture(new Rect(0, 0, 400, 250), DDSTexture);

        if (PNGTexture != null)
            Graphics.DrawTexture(new Rect(500, 0, 400, 250), PNGTexture);
    }
}
