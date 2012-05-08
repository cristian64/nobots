#region File Description
//-----------------------------------------------------------------------------
// ExplosionParticleSystem.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace Nobots.ParticleSystem
{
    /// <summary>
    /// Custom particle system for creating the fiery part of the explosions.
    /// </summary>
    public class ElectricityParticleSystem : ParticleSystem
    {
        public static ElectricityParticleSystem LastInstance = null;

        public ElectricityParticleSystem(Game game, Scene scene)
            : base(game, scene)
        {
            LastInstance = this;
        }

        protected override void InitializeSettings(ParticleSettings settings)
        {
            settings.TextureName = "electricity";

            settings.MaxParticles = 10000;

            settings.Duration = TimeSpan.FromSeconds(0.2f);
            settings.DurationRandomness = 1;

            settings.MinHorizontalVelocity = -0.5f;
            settings.MaxHorizontalVelocity = 0.5f;

            settings.MinVerticalVelocity = -0.5f;
            settings.MaxVerticalVelocity = 0.5f;

            settings.EndVelocity = 0;

            settings.MinColor = Color.White;
            settings.MaxColor = Color.White;

            settings.MinRotateSpeed = -20;
            settings.MaxRotateSpeed = 0;

            settings.MinStartSize = 140f;
            settings.MaxStartSize = 140f;

            settings.MinEndSize = 160f;
            settings.MaxEndSize = 170f;

            // Use additive blending.
            settings.BlendState = BlendState.Additive;
        }
    }
}
