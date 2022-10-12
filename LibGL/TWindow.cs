using System;
using System.Collections.Generic;
using System.IO;
using System.Drawing.Imaging;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System.Drawing;
using TAO.Engine.LibGL.LibTAOBasic;
using PixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;
using Bitmap = System.Drawing.Bitmap;

namespace TAO.Engine.LibGL
{
    public class TWindow : GameWindow
    {
        //Engine Variables
        //-Actions
        public Action? OnLoadAction;
        public Action? OnUnloadAction;
        public Action? OnRenderFrameAction;
        public Action<KeyboardKeyEventArgs>? OnKeyDownAction;
        public Action<Size>? OnResizeAction;
        public Action<MultipleValue<TObject, Vector3[], Color4[], object>>? OnRenderObjectAction;
        public Action<MultipleValue<TObject, Vector3[], Color4[], object>>? OnEndRenderObjectAction;
        public Action<MouseMoveEventArgs>? OnMoveMouseAction;
        public bool WriteActionInNullMessage = false;
        //-General
        public Color4 BackgroundColor = Color4.CornflowerBlue;
        public List<TObject> Objs = new List<TObject>();
        public Dictionary<string, MultipleValue<int, BitmapData, Bitmap, object>> Textures = new Dictionary<string, MultipleValue<int, BitmapData, Bitmap, object>>();
        public MatrixMode RenderMode = MatrixMode.Modelview;
        public Matrix4 MatrixCamera = Matrix4.Identity;
        //-Time
        public double DeltaTime = 0;
        public float DeltaTimeF = 0;
        //-Misc
        public float Zoom = 0;

        public void BasicStart()
        {
            LibTOS.CheckFiles();
            PreloadTextures();

            Title = LibTOS.GetOS() + " | OpenGL";
            VSync = VSyncMode.On;
            Run();
        }

        public void PreloadTextures()
        {
            Textures.Clear();

            //Textures
            DirectoryInfo texturesInfo = new DirectoryInfo("Assets/Textures/");
            FileInfo[] texturesInfos = texturesInfo.GetFiles();

            for (int i = 0; i < texturesInfos.Length; i++)
            {
                Bitmap bmp = new Bitmap("Assets/Textures/" + texturesInfos[i].Name);
                BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                Textures.Add(texturesInfos[i].Name, new MultipleValue<int, BitmapData, Bitmap, object>(GL.GenTexture(), bmpData, bmp, null));
            }

            //Letters - Normal
            DirectoryInfo charactersInfo = new DirectoryInfo("Assets/Textures/Letters/");
            FileInfo[] charactersInfos = charactersInfo.GetFiles();

            for (int i = 0; i < charactersInfos.Length; i++)
            {
                Bitmap bmp = new Bitmap("Assets/Textures/Letters/" + charactersInfos[i].Name);
                BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                Textures.Add("Letters/" + charactersInfos[i].Name, new MultipleValue<int, BitmapData, Bitmap, object>(GL.GenTexture(), bmpData, bmp, null));
            }

            //Letters - Small
            DirectoryInfo csi = new DirectoryInfo("Assets/Textures/Letters/Small/");
            FileInfo[] csis = csi.GetFiles();

            for (int i = 0; i < csis.Length; i++)
            {
                Bitmap bmp = new Bitmap("Assets/Textures/Letters/Small/" + csis[i].Name);
                BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                Textures.Add("Letters/Small/" + csis[i].Name, new MultipleValue<int, BitmapData, Bitmap, object>(GL.GenTexture(), bmpData, bmp, null));
            }
        }

        public void ExecuteAction(Action? action)
        {
            if (action != null)
            {
                try
                {
                    action.Invoke();
                }
                catch (Exception ex)
                {
                    LibTOS.WriteOnLog("ERROR: " + ex.Message);
                }
            }
            else if (WriteActionInNullMessage)
            {
                LibTOS.WriteOnLog("WARNING: A action is null.");
            }
        }

        public void ExecuteAction<T>(Action<T>? action, T arg1)
        {
            if (action != null)
            {
                try
                {
                    action.Invoke(arg1);
                }
                catch (Exception ex)
                {
                    LibTOS.WriteOnLog("ERROR: " + ex.Message);
                }
            }
            else if (WriteActionInNullMessage)
            {
                LibTOS.WriteOnLog("WARNING: A action is null.");
            }
        }

        public void ExecuteAction<T>(Action<T[]>? action, params T[] args)
        {
            if (action != null)
            {
                try
                {
                    action.Invoke(args);
                }
                catch (Exception ex)
                {
                    LibTOS.WriteOnLog("ERROR: " + ex.Message);
                }
            }
            else if (WriteActionInNullMessage)
            {
                LibTOS.WriteOnLog("WARNING: A action is null.");
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            GL.Viewport(0, 0, Width, Height);

            MatrixCamera = Matrix4.CreatePerspectiveFieldOfView(1, Width / (float)Height, 0.001f, 1000);
            GL.MatrixMode(RenderMode);
            GL.LoadMatrix(ref MatrixCamera);

            ExecuteAction(OnLoadAction);
        }

        protected override void OnUnload(EventArgs e)
        {
            base.OnUnload(e);

            ExecuteAction(OnUnloadAction);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, Width, Height);
            ExecuteAction(OnResizeAction, new Size(Width, Height));
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            base.OnKeyDown(e);

            ExecuteAction(OnKeyDownAction, e);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            DeltaTime = args.Time;
            DeltaTimeF = (float)DeltaTime;

            GL.Enable(EnableCap.Blend);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Texture2D);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.ClearColor(BackgroundColor);

            for (int obj = 0; obj < Objs.Count; obj++)
            {
                Vector3[] vertices = Objs[obj].CalculateVertices();
                Color4[] colors = Objs[obj].CalculateColors();

                Vector3 vZoom;

                if (Zoom != 0)
                {
                    vZoom = Vector3.One * Zoom;
                }
                else
                {
                    vZoom = Vector3.One;
                }

                if (Objs[obj].Texture.Trim() != "")
                {
                    LoadTexture(Objs[obj].Texture);
                }

                GL.Begin(Objs[obj].DrawMode);
                ExecuteAction(OnRenderObjectAction, new MultipleValue<TObject, Vector3[], Color4[], object>(Objs[obj], vertices, colors, null));

                for (int vert = 0; vert < vertices.Length; vert++)
                {
                    if (Objs[obj].Texture.Trim() != "")
                    {
                        if (Objs[obj].TexturePoints.Length <= 0)
                        {
                            GL.TexCoord3(LibTMath.ClampVector(Objs[obj].Vertices[vert], Vector3.Zero, Vector3.One));
                        }
                        else
                        {
                            GL.TexCoord3(Objs[obj].TexturePoints[vert]);
                        }
                    }

                    GL.Color4(colors[vert]);
                    GL.Vertex3(vertices[vert] * vZoom);
                }

                ExecuteAction(OnEndRenderObjectAction, new MultipleValue<TObject, Vector3[], Color4[], object>(Objs[obj], vertices, colors, null));
                GL.End();

                if (Objs[obj].Texture.Trim() != "")
                {
                    UnloadTexture(Objs[obj].Texture);
                }
            }

            ExecuteAction(OnRenderFrameAction);
            SwapBuffers();
        }

        public void LoadTexture(string name)
        {
            MultipleValue<int, BitmapData, Bitmap, object> val;

            if (Textures.TryGetValue(name, out val))
            {
                GL.BindTexture(TextureTarget.Texture2D, val.a);

                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, val.c.Width, val.c.Height, 0, PixelFormat.Bgra, PixelType.UnsignedByte, val.b.Scan0);
            }
            else
            {
                LibTOS.WriteOnLog("Texture '" + name + "' doesn't exists.");
            }
        }

        public void UnloadTexture(string name)
        {
            MultipleValue<int, BitmapData, Bitmap, object> val;

            if (Textures.TryGetValue(name, out val))
            {
                GL.BindTexture(TextureTarget.Texture2D, val.a);
                GL.ClearTexImage(val.a, 0, PixelFormat.Rgba, PixelType.UnsignedByte, val.b.Scan0);
            }
            else
            {
                LibTOS.WriteOnLog("Texture '" + name + "' doesn't exists.");
            }
        }

        public void RotateCamera(Vector3 direction)
        {
            GL.Rotate(1, direction);
        }

        public void MoveCamera(Vector3 direction)
        {
            GL.Translate(direction);
        }

        public void DrawText(string message, float size = 9 / 100, Vector3? offset = null)
        {
            Vector3 ot;

            if (!offset.HasValue)
            {
                offset = Vector3.Zero;
                ot = Vector3.Zero;
            }
            else
            {
                ot = offset.Value;
            }

            if (size <= 0)
            {
                size = 0.09f;
            }

            float distance = size;
            char[] messageChars = message.ToCharArray();
            List<TObject> letters = new List<TObject>();

            for (int i = 0; i < messageChars.Length; i++)
            {
                TObject obj = TObject.Cube2D();
                string lr;

                if (messageChars[i] == ' ')
                {
                    lr = "Space";
                }
                else
                {
                    if (LibTMath.IsLower(messageChars[i]))
                    {
                        lr = "Small/" + messageChars[i].ToString();
                    }
                    else
                    {
                        lr = messageChars[i].ToString();
                    }
                }

                obj.Texture = "Letters/" + lr + ".png";
                obj.Position = Vector3.UnitX * (i * distance - messageChars.Length / 20) + ot;
                obj.Scale = new Vector3(1, 2, 1) * size;
                obj.Colors = new Color4[]
                {
                    Color4.Red,
                    Color4.Red,
                    Color4.Blue,
                    Color4.Blue
                };

                letters.Add(obj);
            }

            for (int i = 0; i < letters.Count; i++)
            {
                Objs.Add(letters[i]);
            }
        }

        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            base.OnMouseMove(e);

            ExecuteAction(OnMoveMouseAction, e);
        }
    }
}
