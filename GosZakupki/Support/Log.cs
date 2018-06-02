using GosZakupki.ConstantData;
using System;

namespace GosZakupki.Support
{
    class Log
    {
        private static object LOG_OBJECT = new object();

        public static Type STANDART_TYPE;

        private static string LOG_FILE_PATH => $@"{FilePath.Folder.SYSTEM}\{FilePath.Folder.LOG}\{DateTime.Now.ToString("yyyy-MM-dd")}.txt";
        private static string TIME_NOW => DateTime.Now.ToString("HH:mm:ss");

        [Flags]
        public enum Type
        {
            /// <summary>
            /// Вывод сообщения в консоль
            /// </summary>
            CONSOLE = 1,

            /// <summary>
            /// Запись сообщения в файл
            /// </summary>
            FILE = 2,

            /// <summary>
            /// Вывод сообщения в консоль и записывание его в файл
            /// </summary>
            BOTH = 4
        }

        /// <summary>
        /// Debug mode
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <param name="type">Тип логирования</param>
        public static void D(string message, Type type)
        {
            message = $@"{TIME_NOW} {message}";

            lock (LOG_OBJECT)
            {
                switch (type)
                {
                    case Type.CONSOLE:
                        Console.WriteLine(message);
                        break;

                    case Type.FILE:
                        FileOperation.write(LOG_FILE_PATH, message);
                        break;

                    case Type.BOTH:
                        Console.WriteLine(message);
                        FileOperation.write(LOG_FILE_PATH, message);
                        break;
                }
            }
        }

        /// <summary>
        /// Debug mode
        /// </summary>
        /// <param name="message">Сообщение</param>
        public static void D(string message)
        {
            D(message, STANDART_TYPE);
        }

        /// <summary>
        /// Error mode
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <param name="type">Тип логирования</param>
        public static void E(string message, Type type)
        {
            message = $@"{TIME_NOW} {message}";

            lock (LOG_OBJECT)
            {
                switch (type)
                {
                    case Type.CONSOLE:
                        Console.WriteLine(message);
                        break;

                    case Type.FILE:
                        FileOperation.write(LOG_FILE_PATH, message);
                        break;

                    case Type.BOTH:
                        Console.WriteLine(message);
                        FileOperation.write(LOG_FILE_PATH, message);
                        break;
                }
            }
        }

        /// <summary>
        /// Error mode
        /// </summary>
        /// <param name="message">Сообщение</param>
        public static void E(string message)
        {
            E(message, STANDART_TYPE);
        }
    }
}
