using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using SharpGL;
using SharpGL.WPF;
using SharpGL.SceneGraph;
using SharpGL.SceneGraph.Assets;
using System.Windows.Input;

namespace WpfSharpGl
{
    /// <summary>
    /// Логика взаимодействия для SharpGLWin.xaml
    /// </summary>
    public partial class SharpGLWin : Window
    {
        VertexNet vn;
        Texture texture;
        Bitmap texImg;
        
        Delaunay d;
        public SharpGLWin(MainWindow mainWin, VertexNet vn)
        {
            InitializeComponent();
            this.vn = vn;
            texImg = new Bitmap((string)mainWin.fileLabel.Content);

            texture = new Texture();

            List<float> heights = vn.GetHeights();

            List<Point> ps = new List<Point>();

            for (int i = 0; i < vn.LPoints.Count; ++i)
            {
                ps.Add(new Point((double)vn.LPoints[i].First, (double)vn.LPoints[i].Second, heights[i]));
            }

            d = new Delaunay();
            d.AddPoints(ps);
            d.BaseIncremental();
            d.Incremental();
        }

        private float angle = 0f, scale = 1f, volume = 2f;
        bool isTextureInit = false;

        private void OpenGLControl_OpenGLDraw(object sender, OpenGLRoutedEventArgs args)
        {
            var gl = args.OpenGL;
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);

            gl.MatrixMode(OpenGL.GL_PROJECTION);
            gl.LoadIdentity();
            gl.Perspective(60, (double)(Glwin.Width / Glwin.Height), 0.01, 100.0);
            gl.Viewport(0, 0, (int)Glwin.Width, (int)Glwin.Height);
            gl.LookAt(0, 0, -5, 0, 0, 0, 0, 1, 0);

            if (!isTextureInit)
            {
                texture.Create(gl, texImg);
                isTextureInit = true;
            }
            gl.Enable(OpenGL.GL_TEXTURE_2D);
            texture.Bind(gl);

            gl.LoadIdentity();
            gl.PushMatrix();
            gl.Scale(scale, scale, scale);
            gl.Rotate(angle, 0f, 1f, 0f);
            gl.Translate(-0.5f, 0.5f, -1f);

            foreach (Triangle tr in d.triangle)
            {
                gl.PolygonMode(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_FILL);
                gl.Begin(OpenGL.GL_TRIANGLES);
                gl.Color(1f, 1f, 1f);

                gl.TexCoord(tr.A().x, 1f - tr.A().y);
                gl.Vertex(tr.A().x, tr.A().y, volume * tr.A().h);

                gl.TexCoord(tr.B().x, 1f - tr.B().y);
                gl.Vertex(tr.B().x, tr.B().y, volume * tr.B().h);

                gl.TexCoord(tr.C().x, 1f - tr.C().y);
                gl.Vertex(tr.C().x, tr.C().y, volume * tr.C().h);
                gl.End();
            }

            gl.Disable(OpenGL.GL_TEXTURE_2D);
            gl.PopMatrix();
            gl.Flush();
        }

        private void OpenGLControl_OpenGLInitialized(object sender, OpenGLRoutedEventArgs args)
        {
            var gl = args.OpenGL;
            gl.ClearColor(0.3f, 0.3f, 0.3f, 0.3f);
        }

        private void OpenGLControl_Resized(object sender, OpenGLRoutedEventArgs args)
        {
        }

        private void OpenGLControl_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up:
                    angle += (float)Math.PI / 6;
                    break;
                case Key.Down:
                    angle -= (float)Math.PI / 6;
                    break;
                case Key.Right:
                    scale += 0.1f;
                    break;
                case Key.Left:
                    scale -= 0.1f;
                    break;
            }
        }
    }
}
