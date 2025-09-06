// using System;
// using System.Collections;
// using System.Collections.Generic;
// using System.Linq;
// using System.Numerics;
// using TMPro;
// using UnityEngine;
// using UnityEngine.InputSystem;
// using UnityEngine.Serialization;
// using Vector3 = UnityEngine.Vector3;
//
// namespace Content.Var3.Scripts
// {
//     [RequireComponent(typeof(MeshFilter))]
//     public class MeshGenerator : MonoBehaviour
//     {
//         [Header("Mesh Settigns")] [Tooltip("Mesh width and height on quads.")]
//         public Vector2Int MeshScale = new Vector2Int(1, 1);
//
//         [Tooltip("Base vertices y position.")] public float BaseVertsHeight = 0;
//
//         [Tooltip("Vertices step at other vertices.")]
//         public float VertsSteps = 1.0f;
//         
//         private Mesh mesh;
//         public Vector3[] _vertices;
//         [SerializeField] private int[] _triangles;
//         private Vector3[] vertices1;
//
//         private void Start()
//         {
//             mesh = new Mesh();
//             GetComponent<MeshFilter>().mesh = mesh;
//         }
//
//         private void Update()
//         {
//             if (Input.GetKeyDown(KeyCode.Q))
//             {
//                 _vertices = CreateMesh();
//                 for (int i = 0; i < 9; i++)
//                 {
//                     Debug.Log(vertices1[i]);
//                     Debug.Log(_vertices[i]);
//                 }
//                 UpdateMesh();
//             }
//
//             if (Input.GetKeyDown(KeyCode.W))
//             {
//                 UpdateMesh();
//             }
//         }
//
//         private Vector3[] CreateMesh()
//         {
//             var width = MeshScale.x;
//             var height = MeshScale.y;
//
//             var widthVerts = 0;
//             var heightVerts = 0;
//             var quadCount = 0;
//             var trisCount = 0;
//
//             var outsideVerts = 0;
//             var summaryVerts = 0;
//
//             if (width == 1 && height == 1)
//             {
//                 quadCount = 1;
//                 heightVerts = 2;
//                 widthVerts = 2;
//                 summaryVerts = widthVerts + heightVerts;
//             }
//             else
//             {
//                 quadCount = (int)(width * height);
//                 widthVerts = (int)(width + 1);
//                 heightVerts = (int)(height + 1);
//                 outsideVerts = (int)(((widthVerts + heightVerts) - 2) * 2);
//                 summaryVerts = (int)(widthVerts * heightVerts);
//             }
//
//             trisCount = quadCount * 2;
//
//             _vertices = new Vector3[summaryVerts];
//             vertices1 = new Vector3[summaryVerts];
//             _triangles = new int[6 * width * height];
//             
//             var a = 0;
//
//             for (int i = 0; i < widthVerts; i++)
//             {
//                 for (int j = 0; j < heightVerts; j++)
//                 {
//                     vertices1[a] = new Vector3((i * VertsSteps), BaseVertsHeight, (j * VertsSteps));
//                     a++;
//                 }
//             }
//
//             if (a == summaryVerts)
//             {
//                 a = 0;
//             }
//             
//             var d = 0;
//             var index = 6 * d;
//             for (int i = 1; i < widthVerts * (heightVerts - 1); i++)
//             {
//                 index = 6 * d;
//
//                 if (i % widthVerts == 0)
//                 {
//                     continue;
//                 }
//                 
//                 _triangles[index] = i;
//                 _triangles[index + 1] = i - 1 + widthVerts;
//                 _triangles[index + 2] = i - 1;
//                 
//                 _triangles[index + 3] = i;
//                 _triangles[index + 4] = i + widthVerts;
//                 _triangles[index + 5] = _triangles[index + 1];
//
//                 d++;
//                 
//                 _vertices[_triangles[index]] = new Vector3((_triangles[index] % widthVerts) * VertsSteps, BaseVertsHeight, (int)(_triangles[index] / widthVerts) * VertsSteps);
//                 _vertices[_triangles[index + 1]] = new Vector3((_triangles[index + 1] % widthVerts) * VertsSteps, BaseVertsHeight, (int)(_triangles[index + 1] /  widthVerts) * VertsSteps);
//                 _vertices[_triangles[index + 2]] = new Vector3((_triangles[index + 2] % widthVerts) * VertsSteps, BaseVertsHeight, (int)(_triangles[index + 2] /  widthVerts) * VertsSteps);
//                 // new Vector3((int)(_triangles[index + 3] /  widthVerts + 1) * VertsSteps, BaseVertsHeight, (_triangles[index + 3] % heightVerts) * VertsSteps);
//                 // new Vector3((int)(_triangles[index + 4]  / widthVerts + 1) * VertsSteps, BaseVertsHeight, (_triangles[index + 4] % heightVerts) * VertsSteps);
//                 _vertices[_triangles[index + 5]] = new Vector3((_triangles[index + 5] % widthVerts) * VertsSteps, BaseVertsHeight, (int)(_triangles[index + 5] /  widthVerts) * VertsSteps);
//             }
//
//             return _vertices;
//         }
//
//         private void UpdateMesh()
//         {
//             mesh.Clear();
//
//             mesh.vertices = _vertices;
//             mesh.triangles = _triangles;
//
//             mesh.RecalculateNormals();
//         }
//     }
// }