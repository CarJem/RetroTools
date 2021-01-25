using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using RSDKvB;

namespace RetroLayers.Classes.v4
{
    public class EditorLayer_v4
    {
        private RSDKvB.BGLayout.BGLayer _layer { get; set; }
        internal RSDKvB.BGLayout.BGLayer Layer { get => _layer; }
        public ushort Height { get => _layer.height; }
        public ushort Width { get => _layer.width; }
        public ushort WorkingHeight { get; set; }
        public ushort WorkingWidth { get; set; }
        private int _ID { get; set; }

        const int TILE_SIZE = 128;

        public byte Behaviour
        {
            get => _layer.Behaviour;
            set => _layer.Behaviour = value;
        }
        public short RelativeSpeed
        {
            get => _layer.RelativeSpeed;
            set => _layer.RelativeSpeed = value;
        }
        public short ConstantSpeed
        {
            get => _layer.ConstantSpeed;
            set => _layer.ConstantSpeed = value;
        }
        public byte[] LineIndexes
        {
            get => _layer.LineIndexes;
            set => _layer.LineIndexes = value;
        }

        public EditorLayer_v4(RSDKvB.BGLayout.BGLayer layer, int ID = -1)
        {
            _layer = layer;
            _ID = ID;
        }

        public void Resize(ushort width, ushort height)
        {
            // first resize the underlying SceneLayer
            //_layer.Resize(width, height);
        }

        public EditorLayer_v4 Clone()
        {
            return new EditorLayer_v4(_layer);
        }

        public override string ToString()
        {
            return _ID.ToString();
        }
    }
}
