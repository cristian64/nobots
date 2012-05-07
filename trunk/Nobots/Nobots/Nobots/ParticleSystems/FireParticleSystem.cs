#region File Description
//-----------------------------------------------------------------------------
// FireParticleSystem.cs
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
    /// Custom particle system for creating a flame effect.
    /// </summary>
    public class FireParticleSystem : ParticleSystem
    {
        public static FireParticleSystem LastInstance = null;

        public FireParticleSystem(Game game, Scene scene)
            : base(game, scene)
        {
            LastInstance = this;
        }

        protected override void InitializeSettings(ParticleSettings settings)
        {
            settings.TextureName = "fire";

            settings.MaxParticles = 10000;

            settings.Duration = TimeSpan.FromSeconds(0.7f);

            settings.DurationRandomness = 0.3f;

            settings.MinHorizontalVelocity = 0;
            settings.MaxHorizontalVelocity = 0.2f;

            settings.MinVerticalVelocity = -0.6f;
            settings.MaxVerticalVelocity = 0.5f;

            // Set gravity upside down, so the flames will 'fall' upward.
            settings.Gravity = new Vector3(0, -1f, 0);

            settings.MinColor = new Color(255, 255, 255, 40);
            settings.MaxColor = new Color(255, 255, 255, 120);

            settings.MinRotateSpeed = -5.3f;
            settings.MaxRotateSpeed = 5.3f;

            settings.MinStartSize = 110f;
            settings.MaxStartSize = 130f;

            settings.MinEndSize = 30f;
            settings.MaxEndSize = 50f;

            // Use additive blending.
            settings.BlendState = BlendState.Additive;
        }
    }
}
