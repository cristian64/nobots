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
    public class LaserParticleSystem : ParticleSystem
    {
        public static LaserParticleSystem LastInstance = null;

        public LaserParticleSystem(Game game, Scene scene)
            : base(game, scene)
        {
            LastInstance = this;
        }

        protected override void InitializeSettings(ParticleSettings settings)
        {
            settings.TextureName = "laser";

            settings.MaxParticles = 10000;

            settings.Duration = TimeSpan.FromSeconds(3f);
            settings.DurationRandomness = 0;

            settings.MinHorizontalVelocity = -0.1f;
            settings.MaxHorizontalVelocity = 0.1f;

            settings.MinVerticalVelocity = -0.1f;
            settings.MaxVerticalVelocity = 0.1f;

            settings.EndVelocity = 0;

            settings.MinColor = Color.White * 0.3f;
            settings.MaxColor = Color.White * 0.3f;

            settings.MinRotateSpeed = -5f;
            settings.MaxRotateSpeed = 5f;

            settings.MinStartSize = 30;
            settings.MaxStartSize = 35;

            settings.MinEndSize = 40;
            settings.MaxEndSize = 45;

            // Use additive blending.
            settings.BlendState = BlendState.AlphaBlend;
        }
    }
}
