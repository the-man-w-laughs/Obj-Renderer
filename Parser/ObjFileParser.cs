using Business.Contracts.Parser;
using Domain.ObjClass;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Numerics;

namespace Parser;
public class ObjFileParser : IObjFileParcer
{
    private string usemtl;

    private const int MinPartsLength = 4;
    private const int MinVertexPartsLength = 3;
    private const int MinTextureVertexPartsLength = 2;

    public Obj ParseObjFile(string filePath)
    {
        return LoadObj(File.ReadAllLines(filePath));
    }

    public Obj ParseObjStream(Stream data)
    {
        using (var reader = new StreamReader(data))
        {
            return LoadObj(reader.ReadToEnd().Split(Environment.NewLine.ToCharArray()));
        }
    }

    /// <summary>
    /// Load .obj data from a collection of strings.
    /// </summary>
    /// <param name="data">The collection of strings containing .obj data.</param>
    public Obj LoadObj(IEnumerable<string> data)
    {
        var obj = new Obj();
        foreach (var line in data)
        {
            processLine(line, obj);
        }        

        return obj;
    }

    /// <summary>
    /// Parses and loads a line from an OBJ file.
    /// Currently only supports V, VT, F and MTLLIB prefixes
    /// </summary>		
    private void processLine(string line, Obj obj)
    {
        string[] parts = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);        

        if (parts.Length == 0)
        {
            return; // Empty line, nothing to process.
        }

        string keyword = parts[0].ToLower(); // Convert to lowercase for case-insensitive comparison.

        switch (keyword)
        {
            case "usemtl":
                if (parts.Length >= 2)
                {
                    usemtl = parts[1];
                }
                break;
            case "mtllib":
                if (parts.Length >= 2)
                {
                    obj.Mtl = parts[1];
                }
                break;
            case "v":
                if (parts.Length >= MinPartsLength &&
                    TryParseFloat(parts[1], out float x) &&
                    TryParseFloat(parts[2], out float y) &&
                    TryParseFloat(parts[3], out float z))
                {
                    var v = new Vector3 { X = x, Y = y, Z = z};
                    obj.VertexList.Add(v);                    
                }
                else
                {
                    throw new ArgumentException("Invalid 'v' line format or double values.");
                }
                break;
            case "f":
                if (parts.Length >= MinPartsLength)
                {
                    int vcount = parts.Length - 1;
                    Face f = new Face
                    {
                        VertexIndexList = new int[vcount],
                        TextureVertexIndexList = new int[vcount]
                    };

                    for (int i = 0; i < vcount; i++)
                    {
                        string[] vertexParts = parts[i + 1].Split('/');

                        if (TryParseInt(vertexParts[0], out int vindex))
                        {
                            f.VertexIndexList[i] = vindex;
                        }
                        else
                        {
                            throw new ArgumentException("Invalid 'f' line format or int value.");
                        }
                        
                        if (vertexParts.Length > 1)
                        {
                            if (TryParseInt(vertexParts[1], out int vtindex))
                            {
                                f.TextureVertexIndexList[i] = vtindex;
                            }
                            else
                            {
                                throw new ArgumentException("Invalid 'f' line format or int value.");
                            }
                            
                        }                       
                    }

                    f.UseMtl = usemtl;
                    obj.FaceList.Add(f);
                }
                else
                {
                    throw new ArgumentException("Invalid 'f' line format.");
                }
                break;
            case "vt":
                if (parts.Length >= MinVertexPartsLength &&
                    TryParseFloat(parts[1], out float tx) &&
                    TryParseFloat(parts[2], out float ty))
                {
                    var vt = new Vector2 { X = tx, Y = ty };
                    obj.TextureList.Add(vt);                    
                }
                else
                {
                    throw new ArgumentException("Invalid 'vt' line format or double values.");
                }
                break;
        }
    }


    private bool TryParseFloat(string s, out float result)
    {
        return float.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out result);
    }

    private bool TryParseInt(string s, out int result)
    {
        return int.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out result);
    }

}

