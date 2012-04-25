#region File Description
//-----------------------------------------------------------------------------
// SmokePlumeParticleSystem.cs
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
    /// Custom particle system for creating a giant plume of long lasting smoke.
    /// </summary>
    public class SteamParticleSystem : ParticleSystem
    {
        public static SteamParticleSystem LastInstance = null;

        public SteamParticleSystem(Game game, Scene scene)
            : base(game, scene)
        {
            LastInstance = this;
        }

        protected override void InitializeSettings(ParticleSettings settings)
        {
            settings.TextureName = "whitesmoke";

            settings.MaxParticles = 10000;

            settings.Duration = TimeSpan.FromSeconds(1);

            settings.MinHorizontalVelocity = 0;
            settings.MaxHorizontalVelocity = 0.1f;

            settings.MinVerticalVelocity = -3.5f;
            settings.MaxVerticalVelocity = -2.9f;

            // Create a wind effect by tilting the gravity vector sideways.
            settings.Gravity = Vector3.Zero;

            settings.EndVelocity = 0.75f;

            settings.MinRotateSpeed = -1;
            settings.MaxRotateSpeed = 1;

            settings.MinStartSize = 5;
            settings.MaxStartSize = 20;

            settings.MinEndSize = 100;
            settings.MaxEndSize = 110;

            settings.BlendState = BlendState.Additive;
        }
    }
}
