using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manning.MyPhotoAlbum
{
    public class PhotoAlbum : Collection<Photograph>, IDisposable
    {
        private bool _hasChange = false;
        public bool HasChange
        {
            get
            {
                if(_hasChange) return true;

                foreach(Photograph p in this)
                    if(p.HasChanged) return true;

                return false;
            }
            set
            {
                _hasChange=value;
                if(value==false)
                    foreach(Photograph p in this)
                        p.HasChanged=false;
            }
        }

        public Photograph Add(string filename)
        {
            Photograph p = new Photograph(filename);
            base.Add(p);
            return p;
        }

        protected override void ClearItems()
        {
            if (Count > 0)
            {
                Dispose();
                base.ClearItems();
                HasChange = true;
            }
        }

        protected override void InsertItem(int index, Photograph item)
        {
            base.InsertItem(index, item);
            HasChange = true;
        }

        protected override void RemoveItem(int index)
        {
            Items[index].Dispose();
            base.RemoveItem(index);
            HasChange = true;
        }

        protected override void SetItem(int index, Photograph item)
        {
            base.SetItem(index, item);
            HasChange = true;
        }

        public void Dispose()
        {
            foreach (Photograph p in this)
                p.Dispose();
        }
   
    }
}
