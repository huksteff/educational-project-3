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
//         [Header("Mesh Settigns")]
//         [Tooltip("Mesh width and height on quads.")]
//         public Vector2Int MeshScale = new Vector2Int(1, 1);
//
//         [Tooltip("Base vertices y position.")]
//         public float BaseVertsHeight = 0;
//
//         [Tooltip("Vertices step at other vertices.")]
//         public float VertsSteps = 1.0f;
//
//         public int UpdateCycles = 10;
//         
//         private Mesh mesh;
//         public Vector3[] _vertices;
//         private int[] _triangles;
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
//                 CreateMesh();
//                 UpdateMesh();
//             }
//
//             if (Input.GetKeyDown(KeyCode.W))
//             {
//                 var checkChangesHeight = CheckChangesHeight();
//                 // Debug.Log(checkChangesHeight);
//                 if (checkChangesHeight == true)
//                 {
//                     // Debug.Log("Has Changes");
//                     for (int i = 0; i < UpdateCycles; i++)
//                     {
//                         //Debug.Log($"Update {i}");
//                         
//                         CalculateVerticesHeight();
//                         UpdateMesh();
//                     }
//                 }
//             }
//
//             if (Input.GetKeyDown(KeyCode.R))
//             {
//                 ResetMesh();
//                 UpdateMesh();
//             }
//         }
//
//         private void CreateMesh()
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
//             _triangles = new int[6 * width * height];
//             
//             var a = 0;
//
//             for (int i = 0; i < widthVerts; i++)
//             {
//                 for (int j = 0; j < heightVerts; j++)
//                 {
//                     _vertices[a] = new Vector3((i * VertsSteps), BaseVertsHeight, (j * VertsSteps));
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
//             }
//         }
//
//         private bool CheckChangesHeight()
//         {
//             var xSize = MeshScale.x + 1;
//             for (int i = 0; i < _vertices.Length; i++)
//             {
//                 if (i + xSize >= _vertices.Length)
//                     continue;
//
//                 if (_vertices[i].y < _vertices[i + 1].y || _vertices[i].y < _vertices[i + xSize].y)
//                     return true;
//             }
//
//             return false;
//         }
//
//         private void CalculateVerticesHeight()
//         {
//             var xSize = MeshScale.x + 1;
//             var currentRow = 0;
//             var nextRow = 1;
//             var index = 0;
//             // Debug.Log($"Mas Length: {_vertices.Length}");
//             for (int i = 0; i < _vertices.Length; i++)
//             {
//                 // Debug.Log(_vertices[i].y);
//                 // Debug.Log($"Current I = {i} |  {_vertices.Length-1}");
//                 if (index == xSize)
//                 {
//                     currentRow++;
//                     nextRow++;
//                     index = 0;
//                 }
//                 Debug.Log($"Row: {currentRow} | Index: {index}");
//                 
//                 
//                 if (_vertices[i + 1].y > _vertices[i].y)
//                 {
//                     if (index <= xSize)
//                     {
//                         _vertices[i].y = _vertices[i + 1].y / 2;
//                         if (_vertices[i].y < BaseVertsHeight)
//                             _vertices[i].y = BaseVertsHeight;   
//                     }
//                 }
//
//                 if (i + xSize >= _vertices.Length)
//                 {
//                     break;
//                 }
//                 
//                 if (_vertices[i + xSize].y > _vertices[i].y)
//                 {
//                     _vertices[i].y = _vertices[i + xSize].y / 2;
//                     if (_vertices[i].y < BaseVertsHeight)
//                         _vertices[i].y = BaseVertsHeight;
//                 }
//
//                 if (_vertices[i + 1].y > _vertices[i + 2].y)
//                 {
//                     if (index + 1 <= xSize)
//                     {
//                         _vertices[i + 2].y = _vertices[i + 1].y / 2;
//                         if (_vertices[i].y < BaseVertsHeight)
//                             _vertices[i].y = BaseVertsHeight;
//                     }
//                 }
//                 
//                 if (i + xSize + xSize >= _vertices.Length)
//                 {
//                     break;
//                 }
//                 if (_vertices[i + xSize].y > _vertices[i + xSize + xSize].y)
//                 {
//                     _vertices[i + xSize + xSize].y = _vertices[i + xSize].y / 2;
//                     if (_vertices[i].y < BaseVertsHeight)
//                         _vertices[i].y = BaseVertsHeight;
//                 }
//                 
//                 
//                 index++; 
//             }
//         }
//
//         private void ResetMesh()
//         {
//             for (int i = 0; i < _vertices.Length; i++)
//             {
//                 _vertices[i].y = BaseVertsHeight;
//             }
//         }
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