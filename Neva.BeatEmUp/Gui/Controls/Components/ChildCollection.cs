using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neva.BeatEmUp.Gui.Controls.Components
{
    public sealed class ChildCollection
    {
        #region Vars
        private readonly List<Control> childs;
        private readonly Control owner;
        #endregion

        #region Events
        public event GuiComponentEventHandler<GuiChildComponentEventArgs> ChildAdded;
        public event GuiComponentEventHandler<GuiChildComponentEventArgs> ChildRemoved;
        #endregion

        #region Indexer
        public Control this[int index]
        {
            get
            {
                return childs[index];
            }
        }
        public int ChildsCount
        {
            get
            {
                return childs.Count;
            }
        }
        #endregion

        public ChildCollection(Control owner)
        {
            this.owner = owner;

            childs = new List<Control>();

            ChildAdded += delegate { };
            ChildRemoved += delegate { };
        }

        private void CheckBeforeAdding(Control child)
        {
            if (ContainsChild(child))
            {
                throw new InvalidGuiOperationException("Container already contains reference of this child.");
            }
            if (child.HasParent)
            {
                throw new InvalidGuiOperationException("Trying to add control with parent to another control.");
            }
        }
        private void CheckBeforeRemoving(Control child)
        {
            if (!child.HasParent || !ReferenceEquals(child.Parent, owner))
            {
                throw new InvalidGuiOperationException("Trying to remove child from wrong parent.");
            }
        }

        public void AddChild(Control control)
        {
            if (control == null)
            {
                return;
            }

            CheckBeforeAdding(control);

            childs.Add(control);

            ChildAdded(new GuiChildComponentEventArgs(control), this);
        }
        public void RemoveChild(Control control)
        {
            if (control == null || !childs.Contains(control))
            {
                return;
            }

            CheckBeforeRemoving(control);

            childs.Remove(control);

            ChildRemoved(new GuiChildComponentEventArgs(control), this);
        }
        public void RemoveChilds(IEnumerable<Control> controls)
        {
            foreach (Control control in controls)
            {
                RemoveChild(control);
            }
        }
        public void RemoveChildAt(int index)
        {
            childs.RemoveAt(index);
        }

        public bool ContainsChild(Control control)
        {
            return childs.Contains(control);
        }
        
        public void OrderChildsByDrawOrder()
        {
            childs.OrderByDescending(c => c.DrawOrder);
        }

        public List<Control> RemoveDisposedControls()
        {
            List<Control> disposedControls = new List<Control>();

            for (int i = 0; i < childs.Count; i++)
            {
                if (childs[i].Disposed)
                {
                    Control control = childs[i];
                    childs.RemoveAt(i);

                    disposedControls.Add(control);
                }
            }

            return disposedControls;
        }
        public Control FirstOrDefaultChild(Predicate<Control> predicate)
        {
            return childs.FirstOrDefault(c => predicate(c));
        }
        public IEnumerable<Control> Childs()
        {
            for (int i = 0; i < childs.Count; i++)
            {
                yield return childs[i];
            }
        }
    }
}
