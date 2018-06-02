using System;
using System.Diagnostics;
using System.Threading;

namespace GosZakupki.Support
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
        
        /// <summary>
        /// Пауза, случайной продолжительности
        /// </summary>
        /// <param name="from">Минимальная длительность паузы (в миллисекундах)</param>
        /// <param name="to">Максимальная длительность паузы (в миллисекундах)</param>
        public static void _pause(int from, int to)
        {
            _pause(new Random(DateTime.Now.Millisecond).Next(from, to));
        }

        /// <summary>
        /// Возвращает текущее время в формате Unix
        /// </summary>
        /// <returns></returns>
        public static long getUnixTime()
        {
            return (long) (Math.Truncate((DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds));
        }

        /// <summary>
        /// Возвращает время в формате Unix
        /// </summary>
        /// <param name="date">Дата, которую надо преобразовать</param>
        /// <returns></returns>
        public static long getUnixTime(DateTime date)
        {
            return (long) (Math.Truncate((date.ToUniversalTime() - new DateTime(1970, 1, 1)).TotalMilliseconds));
        }
    }
}
