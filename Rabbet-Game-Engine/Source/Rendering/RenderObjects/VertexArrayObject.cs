﻿using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using System.Linq;

namespace RabbetGameEngine
{
    public class VertexArrayObject
    {
        private int vaoID;

        private List<VertexBufferObject> vbos;

        private IndexBufferObject ibo;
        private IndirectBufferObject indbo;
        private InstanceBufferObject instbo;

        private bool usesIndices = false;
        private bool hasSetUpIndices = false;

        private bool usesIndirect = false;
        private bool hasSetUpIndirect = false;

        private bool usesInstancing = false;
        private bool hasSetUpInstancing = false;


        private int debugTotalAttributes = 0;

        /// <summary>
        /// not currently used for anything but may come in handy
        /// </summary>
        public PrimitiveType drawType;

        public VertexArrayObject()
        {
            vaoID = GL.GenVertexArray();
            vbos = new List<VertexBufferObject>();
        }

        /// <summary>
        /// should be called before adding any objects to this vao.
        /// objects should be added in specific orders and accessed in using indexes into the list of vbos. Order is important!.
        /// </summary>
        public void beginBuilding()
        {
            GL.BindVertexArray(vaoID);
        }

        public int getName()
        {
            return vaoID;
        }
        public void addIndicesBuffer(uint[] indices)
        {
            if (hasSetUpIndices) return;
            ibo = new IndexBufferObject();
            ibo.initStatic(indices);
            hasSetUpIndices = true;
            usesIndices = true;
        }

        public void addInstanceBuffer<T2>(T2[] data, int sizeOfType, VertexBufferLayout layout) where T2 : struct
        {
            if (hasSetUpInstancing) return;
            instbo = new InstanceBufferObject(layout);
            instbo.init<T2>(data, sizeOfType);
            hasSetUpInstancing = true;
            usesInstancing = true;
        }

        public void addIndicesBufferDynamic(int initialCount)
        {
            if (hasSetUpIndices) return;
            ibo = new IndexBufferObject();
            ibo.initDynamic(initialCount * sizeof(uint));
            hasSetUpIndices = true;
            usesIndices = true;
        }

        public void addIndirectBuffer(int initialCount)
        {
            if (hasSetUpIndirect) return;
            indbo = new IndirectBufferObject();
            indbo.init(initialCount * DrawCommand.SIZE_BYTES);
            hasSetUpIndirect = true;
            usesIndirect = true;
        }


        public void addBuffer<T2>(T2[] data, int sizeOfType, VertexBufferLayout layout) where T2 : struct
        {
            VertexBufferObject vbo = new VertexBufferObject(layout);
            vbo.initStatic<T2>(data, sizeOfType);
            vbos.Add(vbo);
        }

        public void addBufferDynamic(int initialByteSize, VertexBufferLayout layout)
        {
            VertexBufferObject vbo = new VertexBufferObject(layout);
            vbo.initDynamic(initialByteSize);
            vbos.Add(vbo);
        }

        /// <summary>
        /// should be called after adding all objects to this vao and before using this vao to render.
        /// </summary>
        public void finishBuilding()
        {
            //build vertex attrib pointers based on requested layouts, etc.
            int attribItterator = 0;
            int offset = 0;
            VertexBufferElement element;
            VertexBufferObject vbo;
            for (int i = 0; i < vbos.Count; i++)
            {
                vbo = vbos.ElementAt(i);
                vbo.bind();
                offset = 0;
                for (int j = 0; j < vbo.layout.elements.Count; j++)
                {
                    element = vbo.layout.elements.ElementAt(j);
                    GL.EnableVertexAttribArray(attribItterator);

                    if (element.isInteger)
                        GL.VertexAttribIPointer(attribItterator, element.count, (VertexAttribIntegerType)element.type, vbo.layout.getStride(), (System.IntPtr)offset);
                    else
                        GL.VertexAttribPointer(attribItterator, element.count, (VertexAttribPointerType)element.type, element.normalized, vbo.layout.getStride(), offset);

                    if (vbo.layout.instancedData)
                    {
                        GL.VertexAttribDivisor(attribItterator, element.divisor);
                    }

                    if (element.isInteger)
                        offset += element.count * VertexBufferElement.getSizeOfType((VertexAttribIntegerType)element.type);
                    else
                        offset += element.count * VertexBufferElement.getSizeOfType((VertexAttribPointerType)element.type);

                    attribItterator++;
                }
            }

            if (usesInstancing && hasSetUpInstancing)
            {
                offset = 0;
                instbo.bind();
                for (int j = 0; j < instbo.layout.elements.Count; j++)
                {
                    element = instbo.layout.elements.ElementAt(j);
                    GL.EnableVertexAttribArray(attribItterator);

                    if (element.isInteger)
                        GL.VertexAttribIPointer(attribItterator, element.count, (VertexAttribIntegerType)element.type, instbo.layout.getStride(), (System.IntPtr)offset);
                    else
                        GL.VertexAttribPointer(attribItterator, element.count, (VertexAttribPointerType)element.type, element.normalized, instbo.layout.getStride(), offset);

                    if (element.isInteger)
                        offset += element.count * VertexBufferElement.getSizeOfType((VertexAttribIntegerType)element.type);
                    else
                        offset += element.count * VertexBufferElement.getSizeOfType((VertexAttribPointerType)element.type);

                    attribItterator++;
                }
            }
            debugTotalAttributes = attribItterator;
        }

        public void resizeBuffer(int vboIndex, int newSizeBytes)
        {
            vbos.ElementAt(vboIndex).resizeBuffer(newSizeBytes);
        }

        public void resizeIndices(int newCount)
        {
            ibo.resizeBuffer(newCount);
        }

        public void resizeIndirect(int newCount)
        {
            indbo.resizeBuffer(newCount);
        }

        public void updateBuffer<T2>(int vboIndex, T2[] data, int sizeToUpdate) where T2 : struct
        {
            vbos.ElementAt(vboIndex).updateBuffer(data, sizeToUpdate);
        }

        public void updateIndices(uint[] data, int count)
        {
            ibo.updateData(data, count);
        }

        public void updateIndirectBuffer(DrawCommand[] data, int countToUpdate)
        {
            indbo.updateData(data, countToUpdate);
        }

        public void bind()
        {
            GL.BindVertexArray(vaoID);
            if (usesIndices) ibo.bind();
            if (usesIndirect) indbo.bind();
            if (usesInstancing) instbo.bind();
            foreach (VertexBufferObject vb in vbos)
            {
                vb.bind();
            }
        }

        public int getVBOIDAt(int index)
        {
            return vbos.ElementAt(index).id;
        }

        public void unBind()
        {
            if (usesIndices) GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            if (usesIndirect) GL.BindBuffer(BufferTarget.DrawIndirectBuffer, 0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
        }

        public void delete()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            foreach (VertexBufferObject b in vbos)
            {
                b.delete();
            }

            if (usesIndices && hasSetUpIndices) ibo.delete();
            if (usesIndirect && hasSetUpIndirect) indbo.delete();
            if (usesInstancing && hasSetUpInstancing) instbo.delete();

            GL.DeleteVertexArray(vaoID);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
        }
    }
}
