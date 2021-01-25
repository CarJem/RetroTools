using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using RSDKv5;

namespace RetroLayers.Classes
{
    public class EditorLayer
    {
        #region RSDKv5 Properties
        private SceneLayer _layer { get; set; }
        internal SceneLayer Layer { get => _layer; }

        #endregion

        #region Layer Properties
        public string Name
        {
            get
            {

                string internalName = _layer.Name;
                return internalName?.TrimEnd('\0');
            }
            set
            {
                string name = value;
                if (name == null) name = "\0";
                if (!name.EndsWith("\0")) name += "\0";
                _layer.Name = name;
            }
        }
        public byte Behaviour
        {
            get => _layer.Behaviour;
            set => _layer.Behaviour = value;
        }
        public byte DrawingOrder
        {
            get => _layer.DrawingOrder;
            set => _layer.DrawingOrder = value;
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
        public ushort Height { get => _layer.Height; }
        public ushort Width { get => _layer.Width; }
        public IList<HorizontalLayerScroll> HorizontalLayerRules { get; set; }

        #endregion

        #region Constant Variables

        const int TILE_SIZE = 16;

        #endregion

        public EditorLayer(SceneLayer layer)
        {
            _layer = layer;
            HorizontalLayerRules = ReadHorizontalLineRules();
        }


        private IList<HorizontalLayerScroll> ReadHorizontalLineRules()
        {
            var tempList = new List<HorizontalLayerScroll>();
            byte generatedId = 0;
            foreach (var scrollInfo in _layer.ScrollingInfo)
            {
                tempList.Add(new HorizontalLayerScroll(generatedId, scrollInfo));
                ++generatedId;
            }

            var ruleMapCount = _layer.ScrollIndexes.Count();
            int i = 0;
            while (i < ruleMapCount)
            {
                var currentValue = _layer.ScrollIndexes[i];
                var currentRule = _layer.ScrollingInfo[currentValue];
                var currentCount = 0;
                var start = i;
                while (i < ruleMapCount
                       && currentValue == _layer.ScrollIndexes[i])
                {
                    ++currentCount;
                    ++i;
                }

                tempList.First(hlr => hlr.Id == currentValue).AddMapping(start, currentCount);
            }

            return tempList;
        }
        public void WriteHorizontalLineRules()
        {
            var newIndexes = new byte[_layer.ScrollIndexes.Length];
            _layer.ScrollingInfo = HorizontalLayerRules.Select(hlr => hlr.ScrollInfo).ToList();

            // the internal ID may now be inaccurate
            // we were only using it for display purposes anyway
            // generate some correct ones, and use those!
            byte newIndex = 0;
            foreach (var hlr in HorizontalLayerRules)
            {
                foreach (var lml in hlr.LinesMapList)
                {
                    var count = lml.LineCount;
                    int index = lml.StartIndex;
                    for (int i = 0; i < count; i++)
                    {
                        newIndexes[index + i] = newIndex;
                    }
                }
                ++newIndex;
            }

            _layer.ScrollIndexes = newIndexes;
        }
        public void ProduceHorizontalLayerScroll()
        {
            var id = (byte)(HorizontalLayerRules.Select(hlr => hlr.Id).Max() + 1);
            var info = new ScrollInfo();

            _layer.ScrollingInfo.Add(info);
            var hls = new HorizontalLayerScroll(id, info);
            HorizontalLayerRules.Add(hls);
        }

        public void Resize(ushort width, ushort height)
        {
            // first resize the underlying SceneLayer
            _layer.Resize(width, height);
        }

        public EditorLayer Clone()
        {
            var cloneLayer = new RSDKv5.SceneLayer(_layer.Name, _layer.Width, _layer.Height);
            cloneLayer.DrawingOrder = _layer.DrawingOrder;
            cloneLayer.Behaviour = _layer.Behaviour;
            cloneLayer.ConstantSpeed = _layer.ConstantSpeed;
            cloneLayer.RelativeSpeed = _layer.RelativeSpeed;
            cloneLayer.ScrollIndexes = _layer.ScrollIndexes;
            cloneLayer.ScrollingInfo = _layer.ScrollingInfo;
            cloneLayer.Tiles = _layer.Tiles;
            return new EditorLayer(cloneLayer);
        }
    }
}
