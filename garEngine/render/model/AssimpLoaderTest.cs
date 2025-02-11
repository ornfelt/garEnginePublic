﻿using Assimp;

namespace garEngine.render.model;

public class AssimpLoaderTest
{
    private Scene _scene;
    private List<MeshStruct> _meshes = new List<MeshStruct>();
    public struct Intvec3{
        int x;
        int y;
        int z;

        public Intvec3(int x_, int y_, int z_)
        {
            x = x_;
            y = y_;
            z = z_;
        }
    }

    public struct MaterialStruct
    {
        public string albedo;
        public string normal;
    }
    public struct MeshStruct
    {
        public List<Vector3D> points;
        public List<Vector3D> normal;
        public List<Vector3D> tangents;
        public List<Vector2D> uvs;
        public List<Intvec3> faces;
        public int MaterialIndex;
    }

    public AssimpLoaderTest(string path,
        PostProcessSteps flags = PostProcessSteps.Triangulate | PostProcessSteps.CalculateTangentSpace |
                                 PostProcessSteps.FindInvalidData | PostProcessSteps.OptimizeMeshes | PostProcessSteps.OptimizeGraph)
    {
        _scene = assimpContextClass.get().ImportFile(path, flags);
        if (_scene.SceneFlags.HasFlag(SceneFlags.Incomplete))
        {
            throw new Exception("Error occurred in assimp");
        }

        if (!_scene.HasMeshes)
        {
            throw new Exception("No meshes in the file");
        }
        #if DEBUG
            foreach (var material in _scene.Materials)
            {
                Console.WriteLine(material.Name);
            }
        #endif
        
        
        foreach (Mesh mesh in _scene.Meshes)
        {
            List<Intvec3> tmpfaces = new();
            foreach (var face in mesh.Faces)
            {
                if (face.Indices.Count == 3)
                {
                    tmpfaces.Add(new Intvec3(face.Indices[0], face.Indices[1], face.Indices[2]));
                }
            }

            List<Vector2D> tmpUvs = new();
            foreach (var uv in mesh.TextureCoordinateChannels[0])
            {
                tmpUvs.Add(new Vector2D(uv.X, uv.Y));
            }

            //_scene.Materials[mesh.MaterialIndex].TextureDiffuse.FilePath;
            

            _meshes.Add(new MeshStruct()
            {
                points = mesh.Vertices,
                normal = mesh.Normals,
                faces = tmpfaces,
                tangents = mesh.Tangents,
                uvs = tmpUvs,
                MaterialIndex = mesh.MaterialIndex
            });

        }
    }

    public List<Material> GetMaterials()
    {
        return _scene.Materials;
    }

    public List<MeshStruct> getAllMeshes()
    {
        return _meshes;
    }
    public MeshStruct getMesh(int index)
    {
        return _meshes[index];
    }

    public int MaterialLength()
    {
        return _scene.MaterialCount;
    }

}