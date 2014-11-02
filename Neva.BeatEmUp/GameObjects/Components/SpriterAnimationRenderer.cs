using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BrashMonkeyContentPipelineExtension;
using BrashMonkeySpriter;
using BrashMonkeySpriter.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Neva.BeatEmUp.GameObjects.Components
{
    public class SpriterAnimationRenderer : GameObjectComponent
    {
        private CharaterAnimator animator;

        public SpriterAnimationRenderer(GameObject owner) : base(owner, false)
        {
            
        }

        protected override void OnInitialize()
        {

            if (string.IsNullOrEmpty(Entity))
                throw new ArgumentException("Entity", "Entity cannot be null or empty!");

            if (string.IsNullOrEmpty(FilePath))
                throw new ArgumentException("FilePath", "FilePath cannot be null or empty!");

            SpriterReader reader = new SpriterReader();
            SpriterImporter importer = new SpriterImporter();
            var model = reader.Read(importer.Import(Path.Combine(@"Content", FilePath)), null,
                owner.Game.Content, owner.Game.GraphicsDevice);
            animator = new CharaterAnimator(model, Entity);

            Enable();
            Show();
        }

        protected override ComponentUpdateResults OnUpdate(GameTime gameTime, IEnumerable<ComponentUpdateResults> results)
        {
            animator.Update(gameTime);
            animator.Location = owner.Position;
            return new ComponentUpdateResults(this, true);
        }

        protected override void OnDraw(SpriteBatch spriteBatch)
        {

            animator.Draw(spriteBatch);
        }

        /// <summary>
        /// Animaatiofilun paikka, relatiivisesti ilman content kansiota
        /// </summary>
        public string FilePath
        {
            get;
            set;
        }

        /// <summary>
        /// Entityn nimi
        /// </summary>
        public string Entity
        {
            get;
            set;
        }

        public void ChangeAnimation(string name)
        {
            animator.ChangeAnimation(name);
        }
    }
}
