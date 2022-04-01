﻿using OpenTK.Graphics.OpenGL4;

namespace gESilk.engine.render.materialSystem;

public class Shader
{
    private readonly int _id;

    public Shader(string data, ShaderType type)
    {
        _id = GL.CreateShader(type);
        GL.ShaderSource(_id, data);
        GL.CompileShader(_id);

        string log = GL.GetShaderInfoLog(_id);
        if (!string.IsNullOrEmpty(log)) Console.WriteLine(log);

        // Oopsie memory leak TODO
        // Free assets when done.
    }

    public int Get()
    {
        return _id;
    }
}