using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RetroLayers.Classes;

namespace RetroLayers.Controls
{
    public interface IEditor
    {
        void Load(IEditorClass scene);

        void Unload();

        void Save(string filePath = null);
    }
}
