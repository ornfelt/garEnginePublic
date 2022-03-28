﻿using gESilk.engine.components;
using gESilk.engine.render.assets;

namespace gESilk.engine.render.materialSystem.settings;

public class GlobalSunPosSetting : ShaderSetting
{
    private new readonly string UniformName;


    public GlobalSunPosSetting(string name) : base(name)
    {
        UniformName = name;
    }

    public override void Use(ShaderProgram program)
    {
        if (RealLocation == -1) RealLocation = program.GetUniform(UniformName);
        program.SetUniform(RealLocation, LightSystem.emitterLight.Owner.GetComponent<Transform>().Model.ExtractTranslation());
    }
}