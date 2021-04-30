using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculatePaint : MonoBehaviour
{
    Texture2D mainTex;
    Texture2D paintedTex;

    MeshRenderer mesh;

    public bool painting;
    
    int totalPixels;
    int paintedPixels;
    private void Start()
    {
        mesh = GetComponent<MeshRenderer>();
        mainTex = mesh.sharedMaterial.GetTexture("_BaseMap") as Texture2D;
        totalPixels = mainTex.width * mainTex.height;           
    }

    private void Update()
    {        
        if (painting)
        {
            paintedPixels = 0;
            paintedTex = ToTexture2D(mesh.material.mainTexture as RenderTexture);            

            for (int i = 0; i < mainTex.width; i++)
            {
                for (int j = 0; j < mainTex.height; j++)
                {
                    if (paintedTex.GetPixel(i, j) != mainTex.GetPixel(i, j))
                        paintedPixels++;
                }
            }
            Debug.Log($"Paint perctenge= {Mathf.Round((float)paintedPixels / (float)totalPixels * 100)}%");
        }
    }

    private void PaintingStarted()
    {
        painting = true;
    }
    private void PaintingEnded()
    {
        painting = false;
    }


    Texture2D ToTexture2D(RenderTexture rTex)
    {
        Texture2D tex = new Texture2D(128, 128, TextureFormat.RGB24, false);
        // ReadPixels looks at the active RenderTexture.
        RenderTexture.active = rTex;
        tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
        tex.Apply();
        return tex;
    }
}
