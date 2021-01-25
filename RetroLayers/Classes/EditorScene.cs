using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using RSDKv5;

namespace RetroLayers.Classes
{
    public class EditorScene : RSDKv5.Scene
    {

        private string OriginalFileName { get; set; }

        #region Layers

        public EditorLayer ForegroundLow
        {
            get => AllLayers.LastOrDefault(el => el.Name.Equals("FG Low") || el.Name.Equals("Playfield"));
        }
        public EditorLayer Scratch
        {
            get => AllLayers.LastOrDefault(el => el.Name.Equals("Scratch"));
        }
        public EditorLayer Move
        {
            get => AllLayers.LastOrDefault(el => el.Name.Equals("Move"));
        }
        public EditorLayer ForegroundHigh
        {
            get => AllLayers.LastOrDefault(el => el.Name.Equals("FG High") || el.Name.Equals("Ring Count"));
        }

        public IList<EditorLayer> AllLayers { get; set; }

        public IEnumerable<EditorLayer> OtherLayers
        {
            get
            {
                return AllLayers.Where(el => el != ForegroundLow && el != ForegroundHigh);
            }
        }

        #endregion

        public EditorScene(string filename) : base(filename)
        {
            OriginalFileName = filename;
            AllLayers = new List<EditorLayer>(Layers.Count);
            foreach (SceneLayer layer in Layers)
            {
                AllLayers.Add(new EditorLayer(layer));
            }
        }
        public EditorLayer ProduceLayer()
        {
            // lets just pick some reasonably safe defaults
            var sceneLayer = new SceneLayer("New Layer", 128, 128);
            var editorLayer = new EditorLayer(sceneLayer);
            return editorLayer;
        }
        public void DeleteLayer(EditorLayer thisLayer)
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
