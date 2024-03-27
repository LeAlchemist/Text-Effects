using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextEffects : MonoBehaviour
{
    public int charCount;
    public TMP_Text textMesh;
    public Mesh mesh;
    public Vector3[] vertices;
    public Color32[] colors;
    public Gradient rainbow;

    // Start is called before the first frame update
    void Start()
    {
        textMesh = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    { 
        //Color32[] colors;
        charCount = textMesh.textInfo.characterCount;
        textMesh.ForceMeshUpdate();
        mesh = textMesh.mesh;
        vertices = mesh.vertices;

        for (int i = 0; i < textMesh.textInfo.characterInfo.Length ; i++)
        {
            TMP_CharacterInfo charInfo = textMesh.textInfo.characterInfo[i];
            int materialIndex = charInfo.materialReferenceIndex;

            colors = textMesh.textInfo.meshInfo[materialIndex].colors32;
            vertices = textMesh.textInfo.meshInfo[materialIndex].vertices;
        }

        foreach (TMP_LinkInfo link in textMesh.textInfo.linkInfo) 
        {
            if (link.GetLinkID() == "rainbow")
            {
                Rainbow(link);
            }
            if (link.GetLinkID() == "wobble")
            {
                Wobble(link);
            }
            if (link.GetLinkID() == "wave")
            {
                Wave(link);
            }
        }

        mesh.vertices = vertices;
        mesh.colors32 = colors;
        textMesh.canvasRenderer.SetMesh(mesh);
    }

    public void Wobble(TMP_LinkInfo link)
    {
        for (int i = link.linkTextfirstCharacterIndex; i < link.linkTextfirstCharacterIndex + link.linkTextLength; i++)
        {
            TMP_CharacterInfo charInfo = textMesh.textInfo.characterInfo[i];
            int materialIndex = charInfo.materialReferenceIndex;

            colors = textMesh.textInfo.meshInfo[materialIndex].colors32;
            vertices = textMesh.textInfo.meshInfo[materialIndex].vertices;

            for (int j = 0; j < 4; j++)
            {
                if (charInfo.character == ' ') continue; // Skips spaces
                int vertexIndex = charInfo.vertexIndex + j;

                Vector3 offset = Wobble(Time.time + j);

                vertices[vertexIndex] = vertices[vertexIndex] + offset;
                //colors[vertexIndex] = Color.red;
            }
        }
    }

    public void Wave(TMP_LinkInfo link)
    {
        for (int i = link.linkTextfirstCharacterIndex; i < link.linkTextfirstCharacterIndex + link.linkTextLength; i++)
        {
            Vector2 movementStrength = new(0.1f, 0.1f);
            float movementSpeed = 1f;

            TMP_CharacterInfo charInfo = textMesh.textInfo.characterInfo[i]; // Gets info on the current character
            int materialIndex = charInfo.materialReferenceIndex; // Gets the index of the current character material

            colors = textMesh.textInfo.meshInfo[materialIndex].colors32;
            vertices = textMesh.textInfo.meshInfo[materialIndex].vertices;

            // Loop all vertexes of the current characters
            for (int j = 0; j < 4; j++)
            {
                if (charInfo.character == ' ') continue; // Skips spaces
                int vertexIndex = charInfo.vertexIndex + j;

                // Offset and Rainbow effects, replace it with any other effect you want.
                Vector3 offset = new Vector2(Mathf.Sin((Time.realtimeSinceStartup * movementSpeed) + (vertexIndex * movementStrength.x)), Mathf.Cos((Time.realtimeSinceStartup * movementSpeed) + (vertexIndex * movementStrength.y))) * 10f;

                // Sets the new effects
                vertices[vertexIndex] += offset;
            }
        }
    }

    public void Rainbow(TMP_LinkInfo link)
    {
        for (int i = link.linkTextfirstCharacterIndex; i < link.linkTextfirstCharacterIndex + link.linkTextLength; i++)
        {
            TMP_CharacterInfo charInfo = textMesh.textInfo.characterInfo[i];
            int materialIndex = charInfo.materialReferenceIndex;

            colors = textMesh.textInfo.meshInfo[materialIndex].colors32;
            vertices = textMesh.textInfo.meshInfo[materialIndex].vertices;

            for (int j = 0; j < 1; j++)
            {
                if (charInfo.character == ' ') continue; // Skips spaces
                int vertexIndex = charInfo.vertexIndex + j;

                colors[vertexIndex] = rainbow.Evaluate(Mathf.Repeat(Time.time + vertices[vertexIndex].x*0.01f, 1f));
                colors[vertexIndex + 1] = rainbow.Evaluate(Mathf.Repeat(Time.time + vertices[vertexIndex + 1].x*0.01f, 1f));
                colors[vertexIndex + 2] = rainbow.Evaluate(Mathf.Repeat(Time.time + vertices[vertexIndex + 2].x*0.01f, 1f));
                colors[vertexIndex + 3] = rainbow.Evaluate(Mathf.Repeat(Time.time + vertices[vertexIndex + 3].x*0.01f, 1f));
            }
        }
    }
    
    Vector2 Wobble(float time)
    {
        return new Vector2(Mathf.Sin(time*3.3f), Mathf.Cos(time*2.5f));
    }
}