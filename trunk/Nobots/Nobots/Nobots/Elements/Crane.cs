using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using FarseerPhysics.Common;
using FarseerPhysics.Collision.Shapes;

namespace Nobots.Elements
{
    public class Crane : Element, IControllable
    {
        Texture2D leg;
        Texture2D eye;
        Texture2D chain;

        private const float SpiderBodyRadius = 0.5f;
        private bool _kneeFlexed;
        private float _kneeTargetAngle = -0.4f;
        private AngleJoint _leftKneeAngleJoint;
        private AngleJoint _leftShoulderAngleJoint;
        private Vector2 _lowerLegSize = new Vector2(1.5f, 0.3f);

        private AngleJoint _rightKneeAngleJoint;
        private AngleJoint _rightShoulderAngleJoint;
        private bool _shoulderFlexed;
        private float _shoulderTargetAngle = -0.2f;
        private Vector2 _upperLegSize = new Vector2(1.5f, 0.3f);

        private Body _circle;
        private Body _leftLower;
        private Body _leftUpper;
        private Body _rightLower;
        private Body _rightUpper;
        private Body _sensor;

        private Fixture _topSensorFixture;
        private Fixture _bottomSensorFixture;
        private Fixture _rightSensorFixture;
        private Fixture _leftSensorFixture;

        private Joint _joint1, _joint2, _joint3, _joint4;
        private RevoluteJoint _sensorJoint;

        public float LeftShift = 1;
        public float RightShift = 1;
        public float UpShift = 1;
        public float DownShift = 1;
        private Vector2 initialPosition;

        public override float Width
        {
            get
            {
                return (_upperLegSize.X + _lowerLegSize.X) * 2;
            }
            set
            {
            }
        }

        public override float Height
        {
            get
            {
                return SpiderBodyRadius * 2;
            }
            set
            {
            }
        }

        public override Vector2 Position
        {
            get
            {
                return _circle.Position;
            }
            set
            {
                Vector2 difference = value - _circle.Position;
                initialPosition = _circle.Position = value;
                _circle.Awake = _leftLower.Awake = _leftUpper.Awake = _rightLower.Awake = _rightUpper.Awake = true;
                _leftLower.Position += difference;
                _leftUpper.Position += difference;
                _rightLower.Position += difference;
                _rightUpper.Position += difference;
            }
        }

        public override float Rotation
        {
            get
            {
                return 0;
            }
            set
            {
            }
        }

        public Crane(Game game, Scene scene, Vector2 position)
            : base(game, scene)
        {
            ZBuffer = 5;

            //Load bodies
            _circle = BodyFactory.CreateCircle(scene.World, SpiderBodyRadius, 0.1f, position);
            _circle.BodyType = BodyType.Kinematic;
            initialPosition = position;

            //Left upper leg
            _leftUpper = BodyFactory.CreateRectangle(scene.World, _upperLegSize.X, _upperLegSize.Y, 1000,
                                                    _circle.Position - new Vector2(SpiderBodyRadius, 0f) -
                                                    new Vector2(_upperLegSize.X / 2f, 0f));
            _leftUpper.BodyType = BodyType.Dynamic;
            _leftUpper.Friction = float.MaxValue;
            _leftUpper.CollisionCategories = ElementCategory.LEG;

            //Left lower leg
            _leftLower = BodyFactory.CreateRectangle(scene.World, _lowerLegSize.X, _lowerLegSize.Y, 1000,
                                                    _circle.Position - new Vector2(SpiderBodyRadius, 0f) -
                                                    new Vector2(_upperLegSize.X, 0f) -
                                                    new Vector2(_lowerLegSize.X / 2f, 0f));
            _leftLower.BodyType = BodyType.Dynamic;
            _leftLower.Friction = float.MaxValue;
            _leftLower.CollisionCategories = ElementCategory.LEG;

            //Right upper leg
            _rightUpper = BodyFactory.CreateRectangle(scene.World, _upperLegSize.X, _upperLegSize.Y, 1000,
                                                     _circle.Position + new Vector2(SpiderBodyRadius, 0f) +
                                                     new Vector2(_upperLegSize.X / 2f, 0f));
            _rightUpper.BodyType = BodyType.Dynamic;
            _rightUpper.Friction = float.MaxValue;
            _rightUpper.CollisionCategories = ElementCategory.LEG;

            //Right lower leg
            _rightLower = BodyFactory.CreateRectangle(scene.World, _lowerLegSize.X, _lowerLegSize.Y, 1000,
                                                     _circle.Position + new Vector2(SpiderBodyRadius, 0f) +
                                                     new Vector2(_upperLegSize.X, 0f) +
                                                     new Vector2(_lowerLegSize.X / 2f, 0f));
            _rightLower.BodyType = BodyType.Dynamic;
            _rightLower.Friction = float.MaxValue;
            _rightLower.CollisionCategories = ElementCategory.LEG;

            //Sensor body on the eye
            createSensor();

            //Create joints
            _joint1 = JointFactory.CreateRevoluteJoint(scene.World, _circle, _leftUpper, new Vector2(_upperLegSize.X / 2f, 0f));
            _leftShoulderAngleJoint = JointFactory.CreateAngleJoint(scene.World, _circle, _leftUpper);
            _leftShoulderAngleJoint.MaxImpulse = 100;

            _joint2 = JointFactory.CreateRevoluteJoint(scene.World, _circle, _rightUpper, new Vector2(-_upperLegSize.X / 2f, 0f));
            _rightShoulderAngleJoint = JointFactory.CreateAngleJoint(scene.World, _circle, _rightUpper);
            _rightShoulderAngleJoint.MaxImpulse = 100;

            _joint3 = JointFactory.CreateRevoluteJoint(scene.World, _leftUpper, _leftLower, new Vector2(_lowerLegSize.X / 2f, 0f));
            _leftKneeAngleJoint = JointFactory.CreateAngleJoint(scene.World, _leftUpper, _leftLower);
            _leftKneeAngleJoint.MaxImpulse = 100;

            _joint4 = JointFactory.CreateRevoluteJoint(scene.World, _rightUpper, _rightLower, new Vector2(-_lowerLegSize.X / 2f, 0f));
            _rightKneeAngleJoint = JointFactory.CreateAngleJoint(scene.World, _rightUpper, _rightLower);
            _rightKneeAngleJoint.MaxImpulse = 100;

            _circle.IgnoreGravity = true;
            _leftLower.IgnoreGravity = true;
            _leftUpper.IgnoreGravity = true;
            _rightLower.IgnoreGravity = true;
            _rightUpper.IgnoreGravity = true;

            _circle.CollidesWith = _leftLower.CollidesWith = _leftUpper.CollidesWith = _rightLower.CollidesWith = _rightUpper.CollidesWith = Category.All & ~ElementCategory.ENERGY;

            _leftKneeAngleJoint.TargetAngle = _kneeTargetAngle;
            _rightKneeAngleJoint.TargetAngle = -_kneeTargetAngle;

            _leftShoulderAngleJoint.TargetAngle = _shoulderTargetAngle;
            _rightShoulderAngleJoint.TargetAngle = -_shoulderTargetAngle;

            leg = Game.Content.Load<Texture2D>("platform");
            eye = Game.Content.Load<Texture2D>("crane");
            chain = Game.Content.Load<Texture2D>("crane_chain");

            _sensorJoint = new RevoluteJoint(_circle, _sensor, Conversion.ToWorld(new Vector2(-eye.Width / 2, -eye.Height / 2)), Vector2.Zero);
            _sensorJoint.CollideConnected = false;
            scene.World.AddJoint(_sensorJoint);
        }

        private void createSensor()
        {
            _sensor = new Body(scene.World);
            
            //adding the left fixture
            Vertices vertices = new Vertices(4);
            vertices.Add(new Vector2(0, 0.57f));
            vertices.Add(new Vector2(0, 0.41f));
            vertices.Add(new Vector2(0.06f, 0.41f));
            vertices.Add(new Vector2(0.06f, 0.57f));
            PolygonShape p = new PolygonShape(vertices, 100);
            _leftSensorFixture = new Fixture(_sensor, p);

            //adding the top fixture
            vertices.Clear();
            vertices.Add(new Vector2(0.41f, 0));
            vertices.Add(new Vector2(0.57f, 0));
            vertices.Add(new Vector2(0.57f, 0.06f));
            vertices.Add(new Vector2(0.41f, 0.06f));
            p = new PolygonShape(vertices, 100);
            _topSensorFixture = new Fixture(_sensor, p);

            //adding the right fixture
            vertices.Clear();
            vertices.Add(new Vector2(1, 0.41f));
            vertices.Add(new Vector2(1, 0.57f));
            vertices.Add(new Vector2(0.94f, 0.57f));
            vertices.Add(new Vector2(0.94f, 0.41f));
            p = new PolygonShape(vertices, 100);
            _rightSensorFixture = new Fixture(_sensor, p);

            //adding the bottom fixture
            vertices.Clear();
            vertices.Add(new Vector2(0.57f, 1));
            vertices.Add(new Vector2(0.41f, 1));
            vertices.Add(new Vector2(0.41f, 0.94f));
            vertices.Add(new Vector2(0.57f, 0.94f));
            p = new PolygonShape(vertices, 100);
            _bottomSensorFixture = new Fixture(_sensor, p);

            //adding the middle fixture
            vertices.Clear();
            vertices.Add(new Vector2(0.4f, 0.4f));
            vertices.Add(new Vector2(0.6f, 0.4f));
            vertices.Add(new Vector2(0.6f, 0.6f));
            vertices.Add(new Vector2(0.4f, 0.6f));
            p = new PolygonShape(vertices, 100);
            Fixture middle = new Fixture(_sensor, p);

            _sensor.Position = Position;
            _sensor.BodyType = BodyType.Dynamic;
            _sensor.FixedRotation = true;
            _sensor.CollidesWith = Category.None | ElementCategory.FLOOR;
            _sensor.OnCollision += new OnCollisionEventHandler(_sensor_OnCollision);
            _sensor.OnSeparation += new OnSeparationEventHandler(_sensor_OnSeparation);
            _sensor.UserData = this;
        }

        void _sensor_OnSeparation(Fixture fixtureA, Fixture fixtureB)
        {
            if (fixtureA == _topSensorFixture)
                canGoUp = true;
            else if (fixtureA == _bottomSensorFixture)
                canGoDown = true;
            else if (fixtureA == _leftSensorFixture)
                canGoLeft = true;
            else if (fixtureA == _rightSensorFixture)
                canGoRight = true;
        }

        bool canGoUp = true;
        bool canGoDown = true;
        bool canGoLeft = true;
        bool canGoRight = true;
        bool _sensor_OnCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            if (fixtureA == _topSensorFixture)
                canGoUp = false;
            else if (fixtureA == _bottomSensorFixture)
                canGoDown = false;
            else if (fixtureA == _leftSensorFixture)
                canGoLeft = false;
            else if (fixtureA == _rightSensorFixture)
                canGoRight = false;

            return true;
        }

        public override void Update(GameTime gameTime)
        {
            if (scene.InputManager.Character == this)
            {
                scene.ElectricityParticleSystem.AddParticle(_circle.Position, Vector2.Zero);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            float scale = scene.Camera.Scale;
            scene.SpriteBatch.Draw(leg, scale * Conversion.ToDisplay(_leftLower.Position - scene.Camera.Position), null, Color.White, _leftLower.Rotation, new Vector2(leg.Width / 2.0f, leg.Height / 2.0f), scale * _lowerLegSize, SpriteEffects.None, 0);
            scene.SpriteBatch.Draw(leg, scale * Conversion.ToDisplay(_rightLower.Position - scene.Camera.Position), null, Color.White, _rightLower.Rotation, new Vector2(leg.Width / 2.0f, leg.Height / 2.0f), scale * _lowerLegSize, SpriteEffects.None, 0);
            scene.SpriteBatch.Draw(leg, scale * Conversion.ToDisplay(_leftUpper.Position - scene.Camera.Position), null, Color.White, _leftUpper.Rotation, new Vector2(leg.Width / 2.0f, leg.Height / 2.0f), scale * _upperLegSize, SpriteEffects.None, 0);
            scene.SpriteBatch.Draw(leg, scale * Conversion.ToDisplay(_rightUpper.Position - scene.Camera.Position), null, Color.White, _rightUpper.Rotation, new Vector2(leg.Width / 2.0f, leg.Height / 2.0f), scale * _upperLegSize, SpriteEffects.None, 0);
            scene.SpriteBatch.Draw(chain, scale * (Conversion.ToDisplay(_circle.Position - scene.Camera.Position) - Vector2.UnitY * chain.Height / 2.0f), null, Color.White, 0, new Vector2(chain.Width / 2.0f, chain.Height / 2.0f), scale, SpriteEffects.None, 0);
            scene.SpriteBatch.Draw(eye, scale * Conversion.ToDisplay(_circle.Position - scene.Camera.Position), null, Color.White, _circle.Rotation, new Vector2(eye.Width / 2.0f, eye.Height / 2.0f), scale * SpiderBodyRadius * 2, SpriteEffects.None, 0);
        }

        protected override void Dispose(bool disposing)
        {
            _circle.Dispose();
            _leftLower.Dispose();
            _leftUpper.Dispose();
            _rightLower.Dispose();
            _rightUpper.Dispose();
            _sensor.Dispose();

            scene.World.RemoveJoint(_leftKneeAngleJoint);
            scene.World.RemoveJoint(_leftShoulderAngleJoint);
            scene.World.RemoveJoint(_rightKneeAngleJoint);
            scene.World.RemoveJoint(_rightShoulderAngleJoint);

            scene.World.RemoveJoint(_joint1);
            scene.World.RemoveJoint(_joint2);
            scene.World.RemoveJoint(_joint3);
            scene.World.RemoveJoint(_joint4);
            scene.World.RemoveJoint(_sensorJoint);
            base.Dispose(disposing);
        }

        public void AActionStart()
        {
        }

        public void AAction()
        {
        }

        public void AActionStop()
        {
        }

        public void BActionStart()
        {
            _kneeFlexed = !_kneeFlexed;
            _shoulderFlexed = !_shoulderFlexed;

            if (_kneeFlexed)
                _kneeTargetAngle = -1.4f;
            else
                _kneeTargetAngle = -0.4f;

            if (_kneeFlexed)
                _shoulderTargetAngle = -1.2f;
            else
                _shoulderTargetAngle = -0.2f;

            _leftKneeAngleJoint.TargetAngle = _kneeTargetAngle;
            _rightKneeAngleJoint.TargetAngle = -_kneeTargetAngle;

            _leftShoulderAngleJoint.TargetAngle = _shoulderTargetAngle;
            _rightShoulderAngleJoint.TargetAngle = -_shoulderTargetAngle;
        }

        public void BAction()
        {
        }

        public void BActionStop()
        {
        }

        public void XActionStart()
        {
        }

        public void XAction()
        {
        }

        public void XActionStop()
        {
        }

        public void YActionStart()
        {
            _circle.LinearVelocity = Vector2.Zero;
            Energy energy = new Energy(scene.Game, scene, Position);
            energy.State = new FallingCharacterState(scene, energy);
            energy.Position = Position;
            scene.InputManager.Character = energy;
            scene.Camera.Target = energy;
            scene.RespawnElements.Add(energy);
            Random random = new Random();
            for (int j = 0; j < 50; j++)
            {
                scene.PlasmaExplosionParticleSystem.AddParticle(energy.Position - Vector2.UnitY * (float)random.NextDouble() / 2, Vector2.Zero);
                scene.PlasmaExplosionParticleSystem.AddParticle(energy.Position + Vector2.UnitY * (float)random.NextDouble() / 2, Vector2.Zero);
            }
        }

        public void YAction()
        {
        }

        public void YActionStop()
        {
        }

        public void RightActionStart()
        {
        }

        public void RightAction()
        {
            if (initialPosition.X + RightShift >= Position.X && canGoRight)
                _circle.LinearVelocity = Vector2.UnitX*2;
            else
                _circle.LinearVelocity = Vector2.Zero;
        }

        public void RightActionStop()
        {
            _circle.LinearVelocity = Vector2.Zero;
        }

        public void LeftActionStart()
        {
        }

        public void LeftAction()
        {
            if (initialPosition.X - LeftShift <= Position.X && canGoLeft)
                _circle.LinearVelocity = -Vector2.UnitX*2;
            else
                _circle.LinearVelocity = Vector2.Zero;
        }

        public void LeftActionStop()
        {
            _circle.LinearVelocity = Vector2.Zero;
        }

        public void UpActionStart()
        {
        }

        public void UpAction()
        {
            if (initialPosition.Y - UpShift <= Position.Y && canGoUp)
                _circle.LinearVelocity = -Vector2.UnitY*2;
            else
                _circle.LinearVelocity = Vector2.Zero;
        }

        public void UpActionStop()
        {
            _circle.LinearVelocity = Vector2.Zero;
        }

        public void DownActionStart()
        {
        }

        public void DownAction()
        {
            if (initialPosition.Y + DownShift >= Position.Y && canGoDown)
                _circle.LinearVelocity = Vector2.UnitY*2;
            else
                _circle.LinearVelocity = Vector2.Zero;
        }

        public void DownActionStop()
        {
            _circle.LinearVelocity = Vector2.Zero;
        }
    }
}