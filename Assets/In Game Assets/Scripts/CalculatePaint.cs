using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Es.InkPainter.Sample;

public class CalculatePaint : MonoBehaviour
{
    Texture2D mainTex;
    Texture2D paintedTex;

    MeshRenderer mesh;

    public bool painting;
    
    int totalPixels;
    int paintedPixels;

    float paintedPerctenage;

    public Text percentageText;
    private void Start()
    {
        mesh = GetComponent<MeshRenderer>();
        mainTex = ToTexture2D(mesh.sharedMaterial.mainTexture as RenderTexture); // Painter tool using Render Texture to paint so getting main texture as Render Texture and convertin to Texture2D.
        totalPixels = mainTex.width * mainTex.height;

        EventManager.PaintingState += GoPaintingState;
        EventManager.StartedPainting += PaintingStarted;
        EventManager.EndedPainting += PaintingEnded;        
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

            paintedPerctenage = Mathf.Round(paintedPixels / (float)totalPixels * 100);
            percentageText.text = $"{paintedPerctenage}%";


            if (paintedPerctenage == 100)            
                EventManager.WinGame();            
        }        
    }

    private void GoPaintingState()
    {
        Camera.main.GetComponent<MousePainter>().enabled = true;
        percentageText.gameObject.SetActive(true);
        paintedPerctenage = 0;
        percentageText.text = $"{paintedPerctenage}%";
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
