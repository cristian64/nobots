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
    public class SmokePlumeParticleSystem : ParticleSystem
    {
        public static SmokePlumeParticleSystem LastInstance = null;

        public SmokePlumeParticleSystem(Game game, Scene scene)
            : base(game, scene)
        {
            LastInstance = this;
        }

        protected override void InitializeSettings(ParticleSettings settings)
        {
            settings.TextureName = "smoke";

            settings.MaxParticles = 10000;

            settings.Duration = TimeSpan.FromSeconds(1);

            settings.MinHorizontalVelocity = 0;
            settings.MaxHorizontalVelocity = 0.1f;

            settings.MinVerticalVelocity = -0.5f;
            settings.MaxVerticalVelocity = -0.2f;

            // Create a wind effect by tilting the gravity vector sideways.
            settings.Gravity = Vector3.Zero;

            settings.EndVelocity = 0.75f;

            settings.MinRotateSpeed = -1;
            settings.MaxRotateSpeed = 1;

            settings.MinStartSize = 15f;
            settings.MaxStartSize = 50f;

            settings.MinEndSize = 100f;
            settings.MaxEndSize = 110;
        }
    }
}
