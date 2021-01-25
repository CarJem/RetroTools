using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RSDKv5;

namespace RetroLayers.Classes.v5
{
    public class EditorClass_v5 : RSDKv5.Scene, IEditorClass
    {
        public string OriginalFileName { get; set; }
        public IList<EditorLayer_v5> AllLayers { get; set; }


        public EditorClass_v5(string filename) : base(filename)
        {
            OriginalFileName = filename;
            AllLayers = new List<EditorLayer_v5>(Layers.Count);
            foreach (SceneLayer layer in Layers)
            {
                AllLayers.Add(new EditorLayer_v5(layer));
            }
        }
        public EditorLayer_v5 ProduceLayer()
        {
            // lets just pick some reasonably safe defaults
            var sceneLayer = new SceneLayer("New Layer", 128, 128);
            var editorLayer = new EditorLayer_v5(sceneLayer);
            return editorLayer;
        }
        public void DeleteLayer(EditorLayer_v5 thisLayer)
        {
            AllLayers.Remove(thisLayer);
        }
        public void Save(string filename = null)
        {
            // save any changes made to the scrolling horizontal rules
            foreach (var el in AllLayers)
            {
                el.WriteHorizontalLineRules();
            }
            Layers = AllLayers.Select(el => el.Layer).ToList();
            Write(filename == null ? OriginalFileName : filename);
            if (filename != null) OriginalFileName = filename;
        }
    }
}
