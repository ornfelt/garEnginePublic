﻿using gESilk.engine.components;
using gESilk.engine.render.assets;
using gESilk.engine.render.materialSystem.settings;
using gESilk.engine.window;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using static gESilk.engine.Globals;

namespace gESilk.engine.render.materialSystem;

public class Material
{
    private readonly Application _application;
    private readonly DepthFunction _function;
    private readonly ShaderProgram _program;
    private readonly List<ShaderSetting> _settings = new();
    private CullFaceMode _cullFaceMode;
    private readonly int _model;
    private readonly int _view;
    private readonly int _projection;
    private readonly int _viewPos;
    private readonly int _lightProj;
    private readonly int _lightView;
    private readonly int _skybox;
    private readonly int _shadowMap;


    public Material(ShaderProgram program, Application application, DepthFunction function = DepthFunction.Less,
        CullFaceMode cullFaceMode = CullFaceMode.Back)
    {
        _program = program;
        _application = application;
        _function = function;
        _cullFaceMode = cullFaceMode;

        _model = _program.GetUniform("model");
        _view = _program.GetUniform("view");
        _projection = _program.GetUniform("projection");
        _viewPos = _program.GetUniform("viewPos");
        _lightProj = _program.GetUniform("lightProjection");
        _lightView = _program.GetUniform("lightView");
        _skybox = _program.GetUniform("skyBox");
        _shadowMap = _program.GetUniform("shadowMap");
    }

    public void SetCullMode(CullFaceMode mode)
    {
        _cullFaceMode = mode;
    }

    public DepthFunction GetDepthFunction()
    {
        return _function;
    }


    public void Use(Matrix4 model, bool clearTranslation, DepthFunction? function = null)
    {
        GL.DepthFunc(function ?? _function);
        _program.Use();
        GL.CullFace(_cullFaceMode);
        _program.SetUniform(_model, model);
        var state = _application.State();
        if (state == EngineState.RenderShadowState)
        {
            _program.SetUniform(_view, ShadowView);
            _program.SetUniform(_projection, ShadowProjection);
        }
        else
        {
            _program.SetUniform(_view, clearTranslation
                ? CameraSystem.CurrentCamera.View.ClearTranslation()
                : CameraSystem.CurrentCamera.View);
            _program.SetUniform(_projection, CameraSystem.CurrentCamera.Projection);
        }

        _program.SetUniform(_viewPos, CameraSystem.CurrentCamera.Owner.GetComponent<Transform>().Location);
        _program.SetUniform(_lightProj, ShadowProjection);
        _program.SetUniform(_lightView, ShadowView);
        _program.SetUniform(_shadowMap, _application._shadowTex.Use(TextureSlotManager.GetUnit()));
        _program.SetUniform(_skybox,
            state == EngineState.GenerateCubemapState
                ? _application.Skybox.Use(TextureSlotManager.GetUnit())
                : CubemapCaptureManager.GetNearest(model.ExtractTranslation()).Get().Use(TextureSlotManager.GetUnit()));
        foreach (var setting in _settings) setting.Use(_program);
    }

    public void Cleanup()
    {
        TextureSlotManager.ResetUnit();
        foreach (var setting in _settings) setting.Cleanup(_program);
    }

    public void AddSetting(ShaderSetting setting)
    {
        _settings.Add(setting);
    }

    public void EditSetting(int index, ShaderSetting setting)
    {
        _settings[index] = setting;
    }
}