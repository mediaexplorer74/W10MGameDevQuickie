using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Input;

namespace DialPong
{
    public class World
    {
        public static Random random;
        public static RadialController dial;
        public static RadialControllerConfiguration dialConfig;

        static World()
        {
            random = new Random();
            dial = RadialController.CreateForCurrentView();
            dialConfig = RadialControllerConfiguration.GetForCurrentView();
        }
    }
}
