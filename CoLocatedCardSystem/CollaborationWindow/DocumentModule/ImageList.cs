using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoLocatedCardSystem.CollaborationWindow.DocumentModule
{
    class ImageList
    {
        Dictionary<string, ImageVector> list = new Dictionary<string, ImageVector>();
        internal void AddImage(ImageVector vector)
        {
            if (!list.ContainsKey(vector.Id))
            {
                list.Add(vector.Id, vector);
            }
        }
        internal ImageVector GetImageVector(string imageName) {
            if (list.ContainsKey(imageName))
            {
                return list[imageName];
            }
            else return null;
        }

        internal IEnumerable<string> GetImage()
        {
            return list.Keys;
        }
    }
}
