using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetroLayers.Classes.v4
{
    public class EditorClass_v4 : RSDKvB.BGLayout, IEditorClass
    {
        public string OriginalFileName { get; set; }

        public List<ScrollInfo_v4> Editor_HLines { get; set; } = new List<ScrollInfo_v4>();
        public List<ScrollInfo_v4> Editor_VLines { get; set; } = new List<ScrollInfo_v4>();
        public List<EditorLayer_v4> Editor_Layers { get; set; } = new List<EditorLayer_v4>();

        public EditorClass_v4(string filename) : base(filename)
        {
            for (int i = 0; i < base.HLines.Count; i++) Editor_HLines.Add(new ScrollInfo_v4(base.HLines[i], i));
            for (int i = 0; i < base.VLines.Count; i++) Editor_VLines.Add(new ScrollInfo_v4(base.VLines[i], i));
            for (int i = 0; i < base.Layers.Count; i++) Editor_Layers.Add(new EditorLayer_v4(base.Layers[i], i));
        }

        public void Save(string filename = null)
        {
            base.HLines.Clear();
            base.VLines.Clear();
            base.Layers.Clear();

            for (int i = 0; i < Editor_HLines.Count; i++) base.HLines.Add(Editor_HLines[i].ScrollInfo);
            for (int i = 0; i < Editor_VLines.Count; i++) base.VLines.Add(Editor_VLines[i].ScrollInfo);
            for (int i = 0; i < Editor_Layers.Count; i++) base.Layers.Add(Editor_Layers[i].Layer);

            Write(filename == null ? OriginalFileName : filename);
            if (filename != null) OriginalFileName = filename;
        }
    }
}
