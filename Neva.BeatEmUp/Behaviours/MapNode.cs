using GameObjects.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Neva.BeatEmUp.GameObjects;
using Neva.BeatEmUp.GameObjects.Components;
using Neva.BeatEmUp.Scripts.CSharpScriptEngine.ScriptClasses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Neva.BeatEmUp.Behaviours
{
    [ScriptAttribute(false)]
    public sealed class MapNode : Behaviour
    {
        #region Vars
        private string mapName;
        private bool canEnter;
        #endregion

        #region Properties
        public bool MapExists
        {
            get
            {
                return File.Exists(@"Content\Maps\" + mapName);
            }
        }
        public string MapName
        {
            get
            {
                return mapName;
            }
        }
        public bool CanEnter
        {
            get
            {
                return canEnter;
            }
        }
        #endregion

        public MapNode(GameObject owner, string mapName, bool canEnter)
            : base(owner)
        {
            this.mapName = mapName;
            this.canEnter = canEnter;
        }

        protected override void OnInitialize()
        {
            for (int i = 0; i < Owner.ChildsCount; i++)
            {
                LineRenderer renderer = new LineRenderer(Owner)
                {
                    Texture = Owner.Game.Content.Load<Texture2D>("blank"),
                    Destination = Owner.ChildAtIndex(i).Position,
                    Color = new Color(Color.Gray, 125),
                    DrawOrder = 0
                };

                Owner.AddComponent(renderer);
            }


            FloatingSpriteRenderer spriteRenderer = new FloatingSpriteRenderer(Owner, Owner.Position.Y - Owner.Game.Random.Next(5, 15), Owner.Position.Y + Owner.Game.Random.Next(5, 15));
            spriteRenderer.FollowOwner = false;
            spriteRenderer.Sprite = new Sprite(Owner.Game.Content.Load<Texture2D>("greennode"));
            spriteRenderer.Position = Owner.Position;
            spriteRenderer.DrawOrder = 1;

            Owner.Size = spriteRenderer.Size;

            Owner.AddComponent(spriteRenderer);

            Owner.InitializeComponents();
        }
    }
}
