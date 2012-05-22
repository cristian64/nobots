#region File Description
//-----------------------------------------------------------------------------
// ExplosionSmokeParticleSystem.cs
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
    /// Custom particle system for creating the smokey part of the explosions.
    /// </summary>
    public class ExplosionSmokeParticleSystem : ParticleSystem
    {
        public static ExplosionSmokeParticleSystem LastInstance = null;

        public ExplosionSmokeParticleSystem(Game game, Scene scene)
            : base(game, scene)
        {
            LastInstance = this;
        }

        protected override void InitializeSettings(ParticleSettings settings)
        {
            settings.TextureName = "smoke";

            settings.MaxParticles = 10000;

            settings.Duration = TimeSpan.FromSeconds(0.5f);

            settings.MinHorizontalVelocity = 0;
            settings.MaxHorizontalVelocity = 0.7f;

            settings.MinVerticalVelocity = -0.5f;
            settings.MaxVerticalVelocity = 1;

            settings.Gravity = new Vector3(0, 0.5f, 0);

            settings.EndVelocity = 0;

            settings.MinColor = Color.Black;
            settings.MaxColor = Color.Gray;

            settings.MinRotateSpeed = -1;
            settings.MaxRotateSpeed = 1;

            settings.MinStartSize = 175f;
            settings.MaxStartSize = 195;

            settings.MinEndSize = 200f;
            settings.MaxEndSize = 225f;
        }
    }
}
