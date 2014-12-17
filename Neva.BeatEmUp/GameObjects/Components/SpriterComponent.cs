using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Neva.BeatEmUp.GameObjects;
using Neva.BeatEmUp.GameObjects.Components;
using SaNi.Spriter;
using SaNi.Spriter.Data;
using SaNi.Spriter.Loaders;
using SaNi.Spriter.Renderer;

namespace GameObjects.Components
{
    public class SpriterComponent<T> : GameObjectComponent
    {
        
        // TODO HAXXXX
        private readonly static Dictionary<Type, Type> lookup;
        private readonly static Dictionary<Type, Type> lookup2;
        static SpriterComponent()
        {
            lookup = new Dictionary<Type, Type>();
            lookup[typeof(Texture2D)] = typeof(Texture2DLoader);
            lookup2 = new Dictionary<Type, Type>();
            lookup2[typeof(Texture2D)] = typeof(Texture2DRenderer);
        }


        private Vector2 position;


        #region Events

        public event MainlineKeyChangedEventHandler OnMainlineKeyChanged
        {
            add
            {
                player.OnMainlineKeyChanged += value;
            }
            remove
            {
                player.OnMainlineKeyChanged -= value;
            }
        }

        public event PlayerProcessEventHandler OnPreProcess
        {
            add
            {
                player.OnPreProcess += value;
            }
            remove
            {
                player.OnPreProcess -= value;
            }
        }

        public event PlayerProcessEventHandler OnPostProcess
        {
            add
            {
                player.OnPostProcess += value;
            }
            remove
            {
                player.OnPostProcess -= value;
            }
        }

        public event AnimationChangedEventHandler OnAnimationChanged
        {
            add
            {
                player.OnAnimationChanged += value;
            }
            remove
            {
                player.OnAnimationChanged -= value;
            }
        }

        public event AnimationFinishedEventHandler OnAnimationFinished
        {
            add
            {
                player.OnAnimationFinished += value;
            }
            remove
            {
                player.OnAnimationFinished -= value;
            }
        }

        #endregion

        #region Properties

        public float Scale
        {
            get
            {
                return player.ScaleX;
            }
            set
            {
                player.SetScale(value);
            }
        }

        public SpriterAnimation CurrentAnimation
        {
            get { return player.Animation; }
        }

        public Vector2 Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
                player.SetPosition(position.X, position.Y);
            }
        }

        public bool FlipX
        {
            get
            {
                return player.ScaleX < 0f;
            }
            set
            {
                if (value && !FlipX)
                {
                    player.FlipX();
                }
                else if (!value && FlipX)
                {
                    player.FlipX();
                }
            }
        }

        public bool FlipY
        {
            get { throw new NotImplementedException();}
            set { throw new NotImplementedException();}
        }

        public Entity Entity
        {
            get
            {
                return player.Entity;
            }
        }

        public Entity.CharacterMap[] CharacterMaps
        {
            get { return player.CharacterMaps; }
            set
            {
                player.CharacterMaps = value;
            }
        }

        #endregion

        private SpriterData data;
        private readonly string asset;
        private SpriterLoader<T> loader;
        private SpriterAnimationPlayer player;
        private SpriterRenderer<T> renderer;

        public SpriterComponent(GameObject obj, string asset)
            : base(obj, false)
        {
            this.asset = asset;
        }

        protected override void OnInitialize()
        {
            Init(asset);
        }

        private void Init(string file)
        {
            data = owner.Game.Content.Load<SpriterData>(file);
            object[] args = { owner.Game, data };
            Type to = lookup[typeof(T)];
            loader = Activator.CreateInstance(to, args) as SpriterLoader<T>;
            string root = file;
            if (root.Contains(Path.DirectorySeparatorChar))
            {
                root = root.Substring(0, root.LastIndexOf(Path.DirectorySeparatorChar));
            }
            else
            {
                root = "";
            }

            loader.Load(root);
            player = new SpriterAnimationPlayer(data.GetEntity(0));
            player.SetAnimation(0);
            renderer = Activator.CreateInstance(lookup2[typeof(T)], new object[] { loader }) as SpriterRenderer<T>;
        }
        // TODO MOAR HAX
        public static SpriterAnimationPlayer LoadAnimation(Game game, string file, ref SpriterRenderer<T> renderer)
        {
            SpriterData data = game.Content.Load<SpriterData>(file);
            object[] args = { game, data };
            Type to = lookup[typeof(T)];
            SpriterLoader<T> loader = Activator.CreateInstance(to, args) as SpriterLoader<T>;
            string root = file;
            if (root.Contains(Path.DirectorySeparatorChar))
            {
                root = root.Substring(0, root.LastIndexOf(Path.DirectorySeparatorChar));
            }
            else
            {
                root = "";
            }

            loader.Load(root);
            SpriterAnimationPlayer player = new SpriterAnimationPlayer(data.GetEntity(0));
            player.SetAnimation(0);
            renderer = Activator.CreateInstance(lookup2[typeof(T)], new object[] { loader }) as SpriterRenderer<T>;

            return player;
        }

        public void ChangeAnimation(string name)
        {
            player.SetAnimation(name);
        }

        public void ChangeAnimation(int name)
        {
            player.SetAnimation(name);
        }

        public void SetEntity(string name)
        {
            player.SetEntity(data.GetEntity(name));
        }

        public void SetTime(int value)
        {
            player.SetTime(value);
        }
        public void SetSpeed(int speed)
        {
            player.Speed = speed;
        }

        public int Speed
        {
            get
            {
                return player.Speed;
            }
        }
        

        protected override ComponentUpdateResults OnUpdate(GameTime gt, IEnumerable<ComponentUpdateResults> results)
        {
            player.Update();

            return new ComponentUpdateResults(this, true);
        }

        protected override void OnDraw(SpriteBatch sb)
        {
            renderer.Draw(player, sb);
        }
    }
}
