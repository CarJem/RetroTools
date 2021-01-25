using System;
using SFML.Graphics;

namespace RetroTile.Events
{
    public delegate void RenderEventHandlerSFML(object sender, DeviceEventArgsSFML e);
    public delegate void CreateDeviceEventHandlerSFML(object sender, DeviceEventArgsSFML e);

    public class DeviceEventArgsSFML : EventArgs
    {
        private RenderWindow _device;

        public RenderWindow Device
        {
            get
            {
                return _device;
            }
        }

        public DeviceEventArgsSFML(RenderWindow device)
        {
            _device = device;
        }
    }

}
