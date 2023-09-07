using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ObjParser.Types;

namespace ObjParser
{
    public class Obj
    {
        public List<Vertex> VertexList { get; private set; }
        public List<Face> FaceList { get; private set; }
        public List<TextureVertex> TextureList { get; private set; }

        public Extent Size { get; private set; }

        public string UseMtl { get; private set; }
        public string Mtl { get; private set; }

        /// <summary>
        /// Private constructor to initialize lists.
        /// </summary>
        private Obj()
        {
            VertexList = new List<Vertex>();
            FaceList = new List<Face>();
            TextureList = new List<TextureVertex>();
        }

        /// <summary>
        /// Constructor to load .obj data from a file path.
        /// </summary>
        /// <param name="path">The file path to the .obj file.</param>
        public Obj(string path) : this()
        {
            LoadObj(File.ReadAllLines(path));
        }

        /// <summary>
        /// Constructor to load .obj data from a stream.
        /// </summary>
        /// <param name="data">The stream containing .obj data.</param>
        public Obj(Stream data) : this()
        {
            using (var reader = new StreamReader(data))
            {
                LoadObj(reader.ReadToEnd().Split(Environment.NewLine.ToCharArray()));
            }
        }

        /// <summary>
        /// Load .obj data from a collection of strings.
        /// </summary>
        /// <param name="data">The collection of strings containing .obj data.</param>
        private void LoadObj(IEnumerable<string> data)
        {
            foreach (var line in data)
            {
                processLine(line);
            }

            updateSize();
        }

        /// <summary>
        /// Sets our global object size with an extent object
        /// </summary>
        private void updateSize()
        {
            // If there are no vertices then size should be 0.
            if (VertexList.Count == 0)
            {
                Size = new Extent
                {
                    XMax = 0,
                    XMin = 0,
                    YMax = 0,
                    YMin = 0,
                    ZMax = 0,
                    ZMin = 0
                };

                // Avoid an exception below if VertexList was empty.
                return;
            }

            Size = new Extent
            {
                XMax = VertexList.Max(v => v.X),
                XMin = VertexList.Min(v => v.X),
                YMax = VertexList.Max(v => v.Y),
                YMin = VertexList.Min(v => v.Y),
                ZMax = VertexList.Max(v => v.Z),
                ZMin = VertexList.Min(v => v.Z)
            };
        }

        /// <summary>
        /// Parses and loads a line from an OBJ file.
        /// Currently only supports V, VT, F and MTLLIB prefixes
        /// </summary>		
        private void processLine(string line)
        {
            string[] parts = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length > 0)
            {
                switch (parts[0])
                {
                    case "usemtl":
                        UseMtl = parts[1];
                        break;
                    case "mtllib":
                        Mtl = parts[1];
                        break;
                    case "v":
                        Vertex v = new Vertex();
                        v.LoadFromStringArray(parts);
                        VertexList.Add(v);
                        v.Index = VertexList.Count();
                        break;
                    case "f":
                        Face f = new Face();
                        f.LoadFromStringArray(parts);
                        f.UseMtl = UseMtl;
                        FaceList.Add(f);
                        break;
                    case "vt":
                        TextureVertex vt = new TextureVertex();
                        vt.LoadFromStringArray(parts);
                        TextureList.Add(vt);
                        vt.Index = TextureList.Count();
                        break;

                }
            }
        }

    }
}
