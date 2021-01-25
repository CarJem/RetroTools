using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetroLayers.Classes.v4
{
    public class ScrollInfo_v4
    {

        private int _ID { get; set; }
        private RSDKvB.BGLayout.ScrollInfo _scrollInfo { get; set; }
        internal RSDKvB.BGLayout.ScrollInfo ScrollInfo { get => _scrollInfo; }

        public byte Behaviour
        {
            get => _scrollInfo.Behaviour;
            set => _scrollInfo.Behaviour = value;
        }
        public short RelativeSpeed
        {
            get => _scrollInfo.RelativeSpeed;
            set => _scrollInfo.RelativeSpeed = value;
        }
        public short ConstantSpeed
        {
            get => _scrollInfo.ConstantSpeed;
            set => _scrollInfo.ConstantSpeed = value;
        }

        public ScrollInfo_v4(RSDKvB.BGLayout.ScrollInfo scrollInfo, int ID = -1)
        {
            _scrollInfo = scrollInfo;
            _ID = ID;
        }

        public override string ToString()
        {
            return _ID.ToString();
        }
    }
}
