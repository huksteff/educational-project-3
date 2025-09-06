// using System;
// using System.Collections;
// using System.Collections.Generic;
// using System.Linq;
// using System.Numerics;
// using TMPro;
// using Unity.Mathematics;
// using UnityEngine;
// using UnityEngine.InputSystem;
// using UnityEngine.Serialization;
// using Vector3 = UnityEngine.Vector3;
// using Vector2 = UnityEngine.Vector2;
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
//         public int UpdateCycles = 10;
//         public int SmoothHeightRange = 1;
//
//         public TMP_Text StatsText;
//
//         private Mesh mesh;
//         public Vector3[] _vertices;
//         private int[] _triangles;
//         [SerializeField] private Vector2[] _uvs;
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
//                 CalculateUVs();
//                 UpdateMesh();
//             }
//
//             if (Input.GetKeyDown(KeyCode.W))
//             {
//                 UpdateMesh();
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
//         private void CalculateUVs()
//         {
//             _uvs = new Vector2[_vertices.Length];
//
//             var xSize = MeshScale.x + 1;
//             var k = Mathf.Clamp01(xSize)/MeshScale.x;
//             var currentRow = 0;
//             var columnIndex = 0;
//             Debug.Log(k);
//             for (int j = 0; j < _vertices.Length; j++)
//             {
//                 // (0, 0)
//                 if (j == 0)
//                 {
//                     _uvs[j] = new Vector2(0, 0);
//                     columnIndex++;
//                     continue;
//                 }
//
//                 if (j < xSize - 1)
//                 {
//                     _uvs[j] = new Vector2(currentRow, k * columnIndex);
//                     columnIndex++;
//                     continue;
//                 }
//                 
//                 // (0, 1)
//                 if (j == xSize - 1)
//                 {
//                     _uvs[j] = new Vector2(0, 1);
//                     columnIndex = 0;
//                     currentRow++;
//                     continue;
//                 }
//
//                 if (j < xSize * xSize - xSize)
//                 {
//                     _uvs[j] = new Vector2(k * currentRow ,k * columnIndex);
//                     columnIndex++;
//                     continue;
//                 }
//
//                 if (j % xSize == 0)
//                 {
//                     currentRow++;
//                     columnIndex = 0;
//                 }
//                 
//                 // (1, 0)
//                 if (j == xSize * xSize - xSize)
//                 {
//                     _uvs[j] = new Vector2(1, 0);
//                     continue;
//                 }
//                 
//                 // (1,1)
//                 if (j == _uvs.Length - 1)
//                 {
//                     _uvs[j] = new Vector2(Mathf.Clamp01(_vertices[^1].x), Mathf.Clamp01(_vertices[^1].z));
//                     break;
//                 }
//             }
//         }
//
//         private void CalculateVerticesHeight()
//         {
//             var xSize = MeshScale.x + 1;
//             var rangeHoriz = SmoothHeightRange;
//             var rangeVert = SmoothHeightRange * xSize;
//             // Debug.Log($"Mas Length: {_vertices.Length}");
//             StatsText.text = ($"xSize {xSize}\nRangeHoriz {rangeHoriz}\nRangeVert {rangeVert}");
//
//             for (int i = 1; i < _vertices.Length - 1; i++)
//             {
//                 if (i % xSize == 0)
//                 {
//                     continue;
//                 }
//
//                 if (_vertices[i - 1].y < _vertices[i].y)
//                 {
//                     _vertices[i - 1].y = _vertices[i].y / 2;
//                 }
//                 else
//                 {
//                     _vertices[i].y = _vertices[i - 1].y / 2;
//                 }
//
//                 if (i - xSize < 0)
//                 {
//                     continue;
//                 }
//
//                 if (_vertices[i - xSize].y < _vertices[i].y)
//                 {
//                     _vertices[i - xSize].y = _vertices[i].y / 2;
//                 }
//                 else
//                 {
//                     _vertices[i].y = _vertices[i - xSize].y / 2;
//                 }
//
//                 if (i + xSize > _vertices.Length)
//                 {
//                     continue;
//                 }
//
//                 if (_vertices[i + xSize].y < _vertices[i].y)
//                 {
//                     _vertices[i + xSize].y = _vertices[i].y / 2;
//                 }
//                 else
//                 {
//                     _vertices[i].y = _vertices[i + xSize].y / 2;
//                 }
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
//
//         private void UpdateMesh()
//         {
//             mesh.Clear();
//
//             mesh.vertices = _vertices;
//             mesh.triangles = _triangles;
//             mesh.uv = _uvs;
//
//             mesh.RecalculateNormals();
//         }
//     }
// }