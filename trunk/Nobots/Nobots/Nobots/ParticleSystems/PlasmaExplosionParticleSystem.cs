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
    public class PlasmaExplosionParticleSystem : ParticleSystem
    {
        public static PlasmaExplosionParticleSystem LastInstance = null;

        public PlasmaExplosionParticleSystem(Game game, Scene scene)
            : base(game, scene)
        {
            LastInstance = this;
        }

        protected override void InitializeSettings(ParticleSettings settings)
        {
            settings.TextureName = "plasmaexplosion";

            settings.MaxParticles = 50000;

            settings.Duration = TimeSpan.FromSeconds(2);
            settings.DurationRandomness = 1;

            settings.MinHorizontalVelocity = 0.01f;
            settings.MaxHorizontalVelocity = 0.05f;

            settings.MinVerticalVelocity = -0.1f;
            settings.MaxVerticalVelocity = 0.1f;

            settings.EndVelocity = 0;

            settings.MinColor = Color.White * 0.7f;
            settings.MaxColor = Color.White;

            settings.MinRotateSpeed = -5;
            settings.MaxRotateSpeed = 5;

            settings.MinStartSize = 70f;
            settings.MaxStartSize = 80f;

            settings.MinEndSize = 85f;
            settings.MaxEndSize = 100f;

            // Use additive blending.
            settings.BlendState = BlendState.Additive;
        }
    }
}
