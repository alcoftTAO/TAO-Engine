using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace TAO.Engine.LibGL
{
    public class TObject
    {
        public static TObject Triangle2D()
        {
            return MakeBasic(new Vector3[]
            {
                new Vector3(-0.5f, -0.5f, 0),
                new Vector3(0.5f, -0.5f, 0),
                new Vector3(0, 0.5f, 0)
            }, null, PrimitiveType.Triangles, new Vector3[]
            {
                new Vector3(0, 1, 0),
                new Vector3(1, 1, 0),
                new Vector3(0, 0.5f, 0)
            });
        }

        public static TObject Cube2D()
        {
            return MakeBasic(new Vector3[]
            {
                new Vector3(-0.5f, -0.5f, 0),
                new Vector3(0.5f, -0.5f, 0),
                new Vector3(0.5f, 0.5f, 0),
                new Vector3(-0.5f, 0.5f, 0)
            }, null, PrimitiveType.Quads, new Vector3[]
            {
                new Vector3(0, 1, 0),
                new Vector3(1, 1, 0),
                new Vector3(1, 0, 0),
                new Vector3(0, 0, 0)
            });
        }

        public static TObject Cube2DInclined()
        {
            return MakeBasic(new Vector3[]
            {
                new Vector3(-0.5f, 0, -0.5f),
                new Vector3(0.5f, 0, -0.5f),
                new Vector3(0.5f, 0, 0.5f),
                new Vector3(-0.5f, 0, 0.5f)
            }, null, PrimitiveType.Quads, new Vector3[]
            {
                new Vector3(0, 1, 0),
                new Vector3(1, 1, 0),
                new Vector3(1, 0, 0),
                new Vector3(0, 0, 0)
            });
        }

        public static TObject Circle2D(Color4? color = null)
        {
            if (color == null)
            {
                color = Color4.White;
            }

            List<Vector3> verts = new List<Vector3>();
            List<Color4> colrs = new List<Color4>();

            for (int i = 0; i < 360; i++)
            {
                verts.Add(new Vector3((float)Math.Cos(i), (float)Math.Sin(i), 0));
                colrs.Add(color.Value);
            }

            return MakeBasic(verts.ToArray(), colrs.ToArray(), PrimitiveType.Polygon);
        }

        public static TObject Cube3D()
        {
            return MakeBasic(new Vector3[]
            {
                new Vector3(-0.5f, -0.5f, -0.5f),
                new Vector3(-0.5f, 0.5f, -0.5f),
                new Vector3(0.5f, 0.5f, -0.5f),
                new Vector3(0.5f, -0.5f, -0.5f),

                new Vector3(0.5f, -0.5f, -0.5f),
                new Vector3(0.5f, 0.5f, -0.5f),
                new Vector3(0.5f, 0.5f, 0.5f),
                new Vector3(0.5f, -0.5f, 0.5f),

                new Vector3(0.5f, -0.5f, 0.5f),
                new Vector3(0.5f, 0.5f, 0.5f),
                new Vector3(-0.5f, 0.5f, 0.5f),
                new Vector3(-0.5f, -0.5f, 0.5f),

                new Vector3(-0.5f, -0.5f, 0.5f),
                new Vector3(-0.5f, 0.5f, 0.5f),
                new Vector3(-0.5f, 0.5f, -0.5f),
                new Vector3(-0.5f, -0.5f, -0.5f),

                new Vector3(-0.5f, -0.5f, -0.5f),
                new Vector3(0.5f, -0.5f, -0.5f),
                new Vector3(0.5f, -0.5f, 0.5f),
                new Vector3(-0.5f, -0.5f, 0.5f),

                new Vector3(-0.5f, 0.5f, -0.5f),
                new Vector3(0.5f, 0.5f, -0.5f),
                new Vector3(0.5f, 0.5f, 0.5f),
                new Vector3(-0.5f, 0.5f, 0.5f),
            }, null, PrimitiveType.Quads, new Vector3[]
            {
                new Vector3(0, 1, 0),
                new Vector3(0, 0, 0),
                new Vector3(1, 0, 0),
                new Vector3(1, 1, 0),

                new Vector3(1, 1, 0),
                new Vector3(1, 0, 0),
                new Vector3(1, 0, 1),
                new Vector3(1, 1, 1),

                new Vector3(1, 1, 1),
                new Vector3(1, 0, 1),
                new Vector3(0, 0, 1),
                new Vector3(0, 1, 1),

                new Vector3(0, 1, 1),
                new Vector3(0, 0, 1),
                new Vector3(0, 0, 0),
                new Vector3(0, 1, 0),

                new Vector3(0, 0, 0),
                new Vector3(1, 0, 0),
                new Vector3(1, 1, 1),
                new Vector3(0, 1, 1),

                new Vector3(0, 0, 0),
                new Vector3(1, 0, 0),
                new Vector3(1, 1, 1),
                new Vector3(0, 1, 1)
            });
        }

        public TObject? Parent;
        public Vector3[] Vertices = new Vector3[0];
        public Color4[] Colors = new Color4[0];
        public Vector3[] TexturePoints = new Vector3[0];
        public string Texture = "";
        public Vector3 Position = Vector3.Zero;
        public Vector3 Scale = Vector3.One;
        public PrimitiveType DrawMode = PrimitiveType.Polygon;
        public float Zoom = 0;

        public static TObject MakeBasic(Vector3[] Vertices, Color4[] Colors, PrimitiveType DrawMode, Vector3[] TexturePoints = null, string Texture = "", TObject? Parent = null)
        {
            if (TexturePoints == null)
            {
                TexturePoints = new Vector3[0];
            }

            return new TObject()
            {
                Vertices = Vertices,
                Colors = Colors,
                DrawMode = DrawMode,
                TexturePoints = TexturePoints,
                Texture = Texture,
                Parent = Parent
            };
        }

        public Vector3[] CalculateVertices()
        {
            List<Vector3> verts = new List<Vector3>();

            Vector3 vZoom;
            Vector3 pvZoom;

            if (Zoom != 0)
            {
                vZoom = Vector3.One * Zoom;
            }
            else
            {
                vZoom = Vector3.One;
            }

            if (Parent != null && Parent.Zoom != 0)
            {
                pvZoom = Vector3.One * Parent.Zoom;
            }
            else
            {
                pvZoom = Vector3.One;
            }

            for (int i = 0; i < Vertices.Length; i++)
            {
                if (Parent == null)
                {
                    verts.Add((Vertices[i] * Scale + Position) * vZoom);
                }
                else
                {
                    verts.Add((((Vertices[i] * Scale + Position) * Parent.Scale + Parent.Position) * pvZoom) * vZoom);
                }
            }

            return verts.ToArray();
        }

        public Color4[] CalculateColors()
        {
            List<Color4> colrs = new List<Color4>();

            for (int i = 0; i < Vertices.Length; i++)
            {
                if (Colors != null && i < Colors.Length)
                {
                    colrs.Add(Colors[i]);
                }
                else
                {
                    colrs.Add(Color4.White);
                }
            }

            return colrs.ToArray();
        }
    }
}
