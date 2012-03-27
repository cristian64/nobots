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
    public class VortexParticleSystem : ParticleSystem
    {
        public static VortexParticleSystem LastInstance = null;

        public VortexParticleSystem(Game game, Scene scene)
            : base(game, scene)
        {
            LastInstance = this;
        }

        protected override void InitializeSettings(ParticleSettings settings)
        {
            settings.TextureName = "vortex";

            settings.MaxParticles = 10000;

            settings.Duration = TimeSpan.FromSeconds(2);
            settings.DurationRandomness = 1;

            settings.MinHorizontalVelocity = 0.00f;
            settings.MaxHorizontalVelocity = 0.00f;

            settings.MinVerticalVelocity = -0.0f;
            settings.MaxVerticalVelocity = 0.0f;

            settings.EndVelocity = 0;

            settings.MinColor = Color.DarkGray;
            settings.MaxColor = Color.Gray;

            settings.MinColor = Color.White;
            settings.MaxColor = Color.White;

            settings.MinRotateSpeed = -20;
            settings.MaxRotateSpeed = 20;

            settings.MinStartSize = 200f;
            settings.MaxStartSize = 200f;

            settings.MinEndSize = 0;
            settings.MaxEndSize = 0f;

            // Use additive blending.
            settings.BlendState = BlendState.Additive;
        }
    }
}
