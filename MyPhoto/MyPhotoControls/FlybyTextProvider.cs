using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections;

namespace Manning.MyPhotoControls
{
    [ProvideProperty("FlybyText",typeof(ToolStripMenuItem))]
    public class FlybyTextProvider:Component, IExtenderProvider
    {
        public FlybyTextProvider(IContainer container)
        {
            container.Add(this);
        }

        private Hashtable _flybyTable = new Hashtable();
        private Hashtable FlybyTable
        {
            get { return _flybyTable; }
        }

        private ToolStripStatusLabel _statusLable = null;
        public ToolStripStatusLabel StatusLable
        {
            get { return _statusLable; }
            set { _statusLable = value; }
        }

        private string _currentText = null;
        private string CurrentStatusText
        {
            get { return _currentText; }
            set { _currentText = value; }
        }

        public bool CanExtend(object extendee)
        {
            return (extendee is ToolStripMenuItem);
        }

        public void SetFlybyText(ToolStripMenuItem item, string text)
        {
            if (text == null || text.Length == 0)
            {
                if (FlybyTable.Contains(item))
                {
                    FlybyTable.Remove(item);
                    item.MouseHover -= OnMouseHover;
                    item.MouseLeave -= OnMouseLeave;
                    item.MouseDown -= OnMouseDown;
                }
            }
            else
            {
                FlybyTable[item] = text;
                item.MouseHover += OnMouseHover;
                item.MouseLeave += OnMouseLeave;
                item.MouseDown += OnMouseDown;
            }
        }

        public string GetFlybyText(ToolStripMenuItem item)
        {
            return FlybyTable[item] as string;
        }

        private void ShowFlyby(object item)
        {
            string flybyText = FlybyTable[item] as string;
            if (flybyText != null && StatusLable != null)
            {
                CurrentStatusText = StatusLable.Text;
                StatusLable.Text = flybyText;
            }
        }

        private void RevertFlyby(object item)
        {
            if (StatusLable != null)
            {
                StatusLable.Text = CurrentStatusText;
                CurrentStatusText = null;
            }
        }

        private void OnMouseHover(object sender, EventArgs e)
        {
            //Display flyby text on hover if assigned
            ShowFlyby(sender);
        }

        private void OnMouseLeave(object sender, EventArgs e)
        {
            //Revert to status text when mouse leaves
            RevertFlyby(sender);
        }

        private void OnMouseDown(object sender, EventArgs e)
        {
            //Revert to status text when mouse pressed
            RevertFlyby(sender);
        }

    }
}
