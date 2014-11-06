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
    [ScriptAttribute(true)]
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
            LineRenderer renderer = Owner.FirstComponentOfType<LineRenderer>();

            if (Owner.ChildsCount > 0)
            {
                renderer.Texture = Owner.Game.Content.Load<Texture2D>("blank");
                renderer.Destination = Owner.ChildAtIndex(0).Position;
                renderer.Color = Color.Red;
            }

            SpriteRenderer spriteRenderer = Owner.FirstComponentOfType<SpriteRenderer>();

            spriteRenderer.Sprite = new Sprite(Owner.Game.Content.Load<Texture2D>("blank"));
            spriteRenderer.Size = new Vector2(32f);
            spriteRenderer.Position = Owner.Position;

            Owner.Size = new Vector2(32f);
        }
    }
}
