using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode, RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Star : MonoBehaviour
{
    public ColorPoint center;
    public ColorPoint[] points;
    [Range(1, 20)]
    public int frequency = 1;

    private Mesh mesh;
    private Vector3[] vertices;
    private int[] triangles;
    private Color[] colors;

    private void Start()
    {
        UpdateMesh();
    }

    private void Reset()
    {
        UpdateMesh();
    }

    private void OnValidate()
    {
        UpdateMesh();
    }

    public void UpdateMesh()
    {
        if (mesh == null)
        {
            mesh = new Mesh();
            GetComponent<MeshFilter>().mesh = mesh;
            mesh.name = "Star mesh";
            mesh.hideFlags = HideFlags.HideAndDontSave;
        }

        frequency = frequency < 1 ? 1 : frequency;
        if (points == null)
        {
            points = new ColorPoint[0];
        }

        Debug.Log(points[2].position);

        int numberOfPoints = frequency * points.Length;
        //if (vertices == null || vertices.Length != numberOfPoints + 1)
        {
            mesh.Clear();
            SetVerticies(numberOfPoints);
            SetColors(numberOfPoints);
            SetTriangles(numberOfPoints);
        }
    }

    private void SetVerticies(int numberOfPoints)
    {
        vertices = new Vector3[numberOfPoints + 1];
        vertices[0] = center.position;
        var angle = -360f / numberOfPoints;
        for (var rep = 0; rep < frequency; rep++)
        {
            var startVertexIndex = rep * points.Length + 1;
            for (var i = 0; i < points.Length; i++)
            {
                vertices[startVertexIndex + i] = Quaternion.Euler(0f, 0f, angle * (rep * points.Length + i)) * points[i].position;
            }
        }

        mesh.vertices = vertices;
    }

    private void SetColors(int numberOfPoints)
    {
        colors = new Color[numberOfPoints + 1];
        colors[0] = center.color;
        for(var i = 0; i < colors.Length - 1; i++)
        {
            colors[i + 1] = points[i % points.Length].color;
        }
        mesh.colors = colors;
    }

    private void SetTriangles(int numberOfPoints)
    {
        triangles = new int[numberOfPoints * 3];
        for (var i = 0; i < vertices.Length - 1; i++)
        {
            triangles[i * 3] = i + 1;
            triangles[i * 3 + 1] = i + 2;
        }
        triangles[triangles.Length - 2] = 1;

        mesh.triangles = triangles;
    }
}
