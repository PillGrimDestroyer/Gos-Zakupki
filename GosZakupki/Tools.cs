using System.Diagnostics;
using System.Threading;

namespace GosZakupki
{
    class Tools
    {

        /// <summary>
        /// Пауза
        /// </summary>
        /// <param name="value">Длительность паузы (в миллисекундах)</param>
        public static void _pause(int value)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            while (sw.ElapsedMilliseconds < value)
            {
                //Application.DoEvents();
                Thread.Sleep(5);
            }
            sw.Reset();
        }
    }
}
