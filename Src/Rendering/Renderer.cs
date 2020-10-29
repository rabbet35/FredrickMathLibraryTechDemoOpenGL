﻿using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using RabbetGameEngine.Debugging;
using RabbetGameEngine.GUI;
using RabbetGameEngine.Models;
using RabbetGameEngine.SubRendering;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace RabbetGameEngine
{
    /*This class will be responsable for most of the games rendering requests. It will then send the requests to the suitable sub renderers.
      e.g, when the game requests text to be rendered on the screen, the renderer will send a request to the TextRenderer2D.
      e.g, when the game requests entity models to be rendered in the world, the renderer will send a request to the model draw function.
      This class also contains the projection matrix.*/
    public static class Renderer
    {
        private static int privateTotalDrawCallCount;
        private static Matrix4 projectionMatrix;
        private static Matrix4 orthographicMatrix;
        public static readonly bool useOffScreenBuffer = false;
        private static int renderFrame;//used to animate textures (noise texture for now)
        private static Rectangle preFullScreenSize;//used to store the window dimentions before going into full screen
       
        /// <summary>
        /// A list of all requested
        /// </summary>
        private static Dictionary<string, StaticRenderObject> staticDraws;
        
        /*Called before any rendering is done*/
        public static void init()
        {
            ShaderUtil.loadAllFoundShaderFiles();
            TextureUtil.loadAllFoundTextureFiles();
            MeshUtil.loadAllFoundModelFiles();
            Application.infoPrint("OpenGL Version: " + GL.GetString(StringName.Version));
            Application.infoPrint("OpenGL Vendor: " + GL.GetString(StringName.Vendor));
            Application.infoPrint("Shading Language Version: " + GL.GetString(StringName.ShadingLanguageVersion));
            Application.infoPrint("Loaded " + ShaderUtil.getShaderCount() + " shaders.");
            Application.infoPrint("Loaded " + TextureUtil.getTextureCount() + " textures.");
            Application.infoPrint("Loaded " + MeshUtil.getModelCount() + " models.");
            GL.Viewport(preFullScreenSize = GameInstance.get.ClientRectangle);
            GL.Enable(EnableCap.DepthTest);
           // GL.Enable(EnableCap.CullFace);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.LineWidth(3);
            projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(MathUtil.radians(GameSettings.fov), GameInstance.aspectRatio, 0.1F, 1000.0F);
            orthographicMatrix = Matrix4.CreateOrthographic(GameInstance.gameWindowWidth, GameInstance.gameWindowHeight, 0.1F, 1.0F);
            staticDraws = new Dictionary<string, StaticRenderObject>();
            if(useOffScreenBuffer) OffScreen.init();
            SkyboxRenderer.init();
        }

        /*Called each time the game window is resized*/
        public static void onResize()
        {
            GL.Viewport((int)GameInstance.get.Bounds.Center.X, (int)GameInstance.get.Bounds.Center.Y, GameInstance.get.Size.X, GameInstance.get.Size.Y);
            projectionMatrix = Matrix4.CreatePerspectiveFieldOfView((float)MathUtil.radians(GameSettings.fov), GameInstance.aspectRatio, 0.1F, 1000.0F);
            orthographicMatrix = Matrix4.CreateOrthographic(GameInstance.gameWindowWidth, GameInstance.gameWindowHeight, 0.1F, 1.0F);
            GUIManager.onWindowResize();
        }

        /*called once per frame*/
        public static void onFrame()
        {
            
        }

        public static void requestRender(BatchType type, Texture tex, Model mod)
        {
            Profiler.beginEndProfile("batching");
            BatchManager.requestRender(type, tex, mod);
            Profiler.beginEndProfile("batching");
        }

        public static void requestRender(PointCloudModel mod, bool transparency, bool lerp)
        {
            Profiler.beginEndProfile("batching");
            BatchManager.requestRender(mod, transparency, lerp);
            Profiler.beginEndProfile("batching");
        }

        public static void requestRender(PointParticle point, bool transparency, bool lerp)
        {
            Profiler.beginEndProfile("batching");
            BatchManager.requestRender(point, transparency, lerp);
            Profiler.beginEndProfile("batching");
        }

        public static void requestRender(PointParticle point, PointParticle prevTickPoint, bool transparency)
        {
            Profiler.beginEndProfile("batching");
            BatchManager.requestRender(point, prevTickPoint, transparency);
            Profiler.beginEndProfile("batching");
        }
        public static void onTickStart()
        {
            Profiler.beginEndProfile("batching");
            BatchManager.onTickStart();
            Profiler.beginEndProfile("batching");
        }
        public static void onTickEnd()
        {
            Profiler.beginEndProfile("batching");
            BatchManager.onTickEnd();
            Profiler.beginEndProfile("batching");
        }

        /*Called before all draw calls*/
        private static void preRender()
        {
            if (useOffScreenBuffer) OffScreen.prepareToRenderToOffScreenTexture();
            GL.Clear(ClearBufferMask.DepthBufferBit);
            privateTotalDrawCallCount = 0;
        }

        public static void renderAll()
        {
            preRender();
            SkyboxRenderer.drawSkybox(projectionMatrix, GameInstance.get.thePlayer.getViewMatrix());
            drawAllStaticRenderObjects();
            BatchManager.drawAll(GameInstance.get.thePlayer.getViewMatrix(), GameInstance.get.currentPlanet.getFogColor());
            postRender();
        }
        
        /*Called after all draw calls*/
        private static void postRender()
        {
            if (useOffScreenBuffer) OffScreen.renderOffScreenTexture();
            GameInstance.get.SwapBuffers();
            renderFrame++;
            renderFrame %= 4096;
        }

        public static void addStaticDrawTriangles(string name, string textureName, string shaderName, Model data)
        {
            if (staticDraws.TryGetValue(name, out StaticRenderObject s))
            {
                s.delete();
                staticDraws.Remove(name);
            }
            staticDraws.Add(name, StaticRenderObject.createSROTriangles(textureName, shaderName, data));
        }

        public static void addStaticDrawTriangles(string name, string textureName, Model data)
        {
            if (staticDraws.TryGetValue(name, out StaticRenderObject s))
            {
                s.delete();
                staticDraws.Remove(name);
            }
            staticDraws.Add(name, StaticRenderObject.createSROTriangles(textureName, ShaderUtil.trianglesName, data));
        }

        public static void addStaticDrawLines(string name, string textureName, string shaderName, Model data)
        {
            if (staticDraws.TryGetValue(name, out StaticRenderObject s))
            {
                s.delete();
                staticDraws.Remove(name);
            }
            staticDraws.Add(name, StaticRenderObject.createSROLines(textureName, shaderName, data));
        }

        public static void addStaticDrawLines(string name, string textureName, Model data)
        {
            if (staticDraws.TryGetValue(name, out StaticRenderObject s))
            {
                s.delete();
                staticDraws.Remove(name);
            }
            staticDraws.Add(name, StaticRenderObject.createSROLines(textureName, ShaderUtil.linesName, data));
        }

        public static void addStaticDrawPoints(string name, PointParticle[] data, bool transparency)
        {
            if (staticDraws.TryGetValue(name, out StaticRenderObject s))
            {
                s.delete();
                staticDraws.Remove(name);
            }
            staticDraws.Add(name, StaticRenderObject.createSROPoints(data, transparency));
        }
        public static void removeStaticDraw(string name)
        {
            if (staticDraws.TryGetValue(name, out StaticRenderObject s))
            {
                s.delete();
                staticDraws.Remove(name);
            }
        }

        private static void drawAllStaticRenderObjects()
        {
            for(int i = 0; i < staticDraws.Count; ++i)
            { 
                staticDraws.ElementAt(i).Value.draw(GameInstance.get.thePlayer.getViewMatrix(), GameInstance.get.currentPlanet.getFogColor());
                totalDraws++;
            }
        }

        public static void onToggleFullscreen()
        {
            if (GameSettings.fullscreen)
            {
                preFullScreenSize = GameInstance.get.ClientRectangle;
                GameInstance.get.WindowState = WindowState.Fullscreen;
            }
            else
            {
                GameInstance.get.WindowState = WindowState.Normal;
                GameInstance.get.ClientRectangle = preFullScreenSize;
            }

        }

        /*deletes all loaded opengl assets*/
        public static void onClosing()
        {
            foreach (StaticRenderObject s in staticDraws.Values)
            {
                s.delete();
            }
            BatchManager.deleteAll();
            ShaderUtil.deleteAll();
            TextureUtil.deleteAll();
            OffScreen.onClose();
        }

        public static Matrix4 projMatrix { get => projectionMatrix; }
        public static int frame { get => renderFrame; }
        public static int totalDraws { get { return privateTotalDrawCallCount; } set { privateTotalDrawCallCount = value; } }

        public static Matrix4 orthoMatrix { get => orthographicMatrix; }

        public static Vector3 camPos { get => GameInstance.get.thePlayer.getLerpEyePos(); }
    }
}
 