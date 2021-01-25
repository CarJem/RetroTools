using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using RSDKv5;

namespace RetroLayers.Classes
{
    public interface IEditorClass
    {
        string OriginalFileName { get; set; }
        void Save(string filename = null);
    }
}
