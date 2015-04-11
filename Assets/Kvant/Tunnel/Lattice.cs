//
// Lattice Builder
//
// Builds a triangular lattice for GPGPU geometry construction.
//
// It has two submeshes in order to store two types of vertex orders.
//
// 1st submesh: A-B-C
// 2nd submesh: A-C-D
//
// B   C
// .---.---.
//  \ / \ /
//   .---.--
//   A   D
//
// Vertex attribute usage:
//
// POS - not in use
// UV1 - texcoord for position buffer
// UV2 - texcoord for normal vector buffer
//

using UnityEngine;
using System.Collections;

namespace Kvant {

public partial class Tunnel
{
    [System.Serializable]
    public class Lattice
    {
        Mesh[] _meshes;

        public Mesh[] meshes {
            get { return _meshes; }
        }

        public Lattice(int columns, int rows)
        {
            Build(columns, rows);
        }

        public void Rebuild(int columns, int rows)
        {
            Release();
            Build(columns, rows);
        }

        public void Release()
        {
            if (_meshes != null) {
                foreach (var m in _meshes) Object.DestroyImmediate(m);
                _meshes = null;
            }
        }

        void Build(int columns, int rows)
        {
            // Estimate total count of vertices.
            var totalVC = columns * rows * 6;

            if (totalVC <= 60000)
            {
                // < 60000: It needs just one mesh.
                _meshes = new Mesh[1] { BuildMesh(columns, rows, 0, rows) };
            }
            else
            {
                // > 60000: Split into segments.
                var segments = totalVC / 60000 + 1;
                _meshes = new Mesh[segments];

                // Have an even number of rows in a segment.
                var rowsSegment = (rows / segments / 2 + 1) * 2;

                // Build each segments excluding the last one.
                for (var i = 0; i < segments - 1; i++)
                    _meshes[i] = BuildMesh(columns, rowsSegment, rowsSegment * i, rows);

                // Build the last segment.
                var last = rowsSegment * (segments - 1);
                _meshes[segments - 1] = BuildMesh(columns, rows - last, last, rows);
            }
        }

        Mesh BuildMesh(int columns, int rows, int startRow, int totalRows)
        {
            var Nx = columns;
            var Ny = rows + 1;

            var Sx = 1.0f / Nx;
            var Sy = 1.0f / (totalRows + 1);

            var Oy = Sy * startRow;

            // Texcoord Array for UV1 and UV2.
            var TA1 = new Vector2[Nx * (Ny - 1) * 6];
            var TA2 = new Vector2[Nx * (Ny - 1) * 6];
            var iTA = 0;

            // 1st submesh (A-B-C triangles).
            for (var Iy = 0; Iy < Ny - 1; Iy++)
            {
                for (var Ix = 0; Ix < Nx; Ix++, iTA += 3)
                {
                    var Ix2 = Ix + 0.5f * (Iy & 1);
                    // UVs for position.
                    TA1[iTA + 0] = new Vector2(Sx * (Ix2 + 0.0f), Oy + Sy * (Iy + 0));
                    TA1[iTA + 1] = new Vector2(Sx * (Ix2 - 0.5f), Oy + Sy * (Iy + 1));
                    TA1[iTA + 2] = new Vector2(Sx * (Ix2 + 0.5f), Oy + Sy * (Iy + 1));
                    // UVs for normal vector.
                    TA2[iTA] = TA2[iTA + 1] = TA2[iTA + 2] = TA1[iTA];
                }
            }

            // 2nd submesh (A-C-D triangls).
            for (var Iy = 0; Iy < Ny - 1; Iy++)
            {
                for (var Ix = 0; Ix < Nx; Ix++, iTA += 3)
                {
                    var Ix2 = Ix + 0.5f * (Iy & 1);
                    // UVs for position.
                    TA1[iTA + 0] = new Vector2(Sx * (Ix2 + 0.0f), Oy + Sy * (Iy + 0));
                    TA1[iTA + 1] = new Vector2(Sx * (Ix2 + 0.5f), Oy + Sy * (Iy + 1));
                    TA1[iTA + 2] = new Vector2(Sx * (Ix2 + 1.0f), Oy + Sy * (Iy + 0));
                    // UVs for normal vector.
                    TA2[iTA] = TA2[iTA + 1] = TA2[iTA + 2] = TA1[iTA];
                }
            }

            // Index arrays for the 1st and 2nd submesh (surfaces).
            var IA1 = new int[Nx * (Ny - 1) * 3];
            var IA2 = new int[Nx * (Ny - 1) * 3];
            for (var iIA = 0; iIA < IA1.Length; iIA++)
            {
                IA1[iIA] = iIA;
                IA2[iIA] = iIA + IA1.Length;
            }

            // Index array for the 3rd submesh (lines).
            var IA3 = new int[Nx * (Ny - 1) * 6];
            var iIA3a = 0;
            var iIA3b = 0;
            for (var Iy = 0; Iy < Ny - 1; Iy++)
            {
                for (var Ix = 0; Ix < Nx; Ix++, iIA3a += 6, iIA3b += 3)
                {
                    IA3[iIA3a + 0] = iIA3b;
                    IA3[iIA3a + 1] = iIA3b + 1;
                    IA3[iIA3a + 2] = iIA3b;
                    IA3[iIA3a + 3] = iIA3b + 2;
                    IA3[iIA3a + 4] = iIA3b;
                    IA3[iIA3a + 5] = iIA3b + IA1.Length + 2;
                }
            }

            // Construct a mesh.
            var mesh = new Mesh();
            mesh.subMeshCount = 3;
            mesh.vertices = new Vector3[TA1.Length];
            mesh.uv = TA1;
            mesh.uv2 = TA2;
            mesh.SetIndices(IA1, MeshTopology.Triangles, 0);
            mesh.SetIndices(IA2, MeshTopology.Triangles, 1);
            mesh.SetIndices(IA3, MeshTopology.Lines, 2);
            mesh.Optimize();

            // Avoid being culled.
            mesh.bounds = new Bounds(Vector3.zero, Vector3.one * 100);

            // This only for temporary use. Don't save.
            mesh.hideFlags = HideFlags.DontSave;

            return mesh;
        }
    }
}

} // namespace Kvant
