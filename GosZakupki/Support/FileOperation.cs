using GosZakupki.ConstantData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GosZakupki.Support
{
    public class FileOperation
    {
        private static object LOCKOBJEC = new object();

        public FileOperation()
        {

        }

        public FileOperation(string filePath)
        {
            file = filePath;
        }

        /// <summary>
        /// Путь к файлу
        /// </summary>
        public string file { get; set; }

        /// <summary>
        /// Считать первую строку из указанного файла
        /// </summary>
        /// <param name="filePath">Путь к файлу</param>
        /// <returns></returns>
        public static string readLine(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($@"Файл ""{filePath}"" не найден");
            }

            lock (LOCKOBJEC)
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    return reader.ReadLine();
                }
            }
        }

        /// <summary>
        /// Считать первую строку из файла
        /// </summary>
        /// <returns></returns>
        public string readLine()
        {
            return readLine(file);
        }

        /// <summary>
        /// Считать все строки из указанного файла
        /// </summary>
        /// <param name="filePath">Путь к файлу</param>
        /// <returns></returns>
        public static string readAllText(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($@"Файл ""{filePath}"" не найден");
            }

            lock (LOCKOBJEC)
            {
                return File.ReadAllText(filePath, Encoding.UTF8);
            }
        }

        /// <summary>
        /// Считать все строки
        /// </summary>
        /// <returns></returns>
        public string readAllText()
        {
            return readAllText(file);
        }

        /// <summary>
        /// Удалить первую строку в указанном файле
        /// </summary>
        /// <param name="filePath">Путь к файлу</param>
        /// <returns></returns>
        public static void deliteFirstLine(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($@"Файл ""{filePath}"" не найден");
            }

            lock (LOCKOBJEC)
            {
                string[] fileData = File.ReadAllLines(filePath);
                using (StreamWriter writer = new StreamWriter(filePath, false))
                {
                    for (int i = 0; i < fileData.Length; i++)
                    {
                        if (i != 0)
                            writer.WriteLine(fileData[i]);
                    }
                }
            }
        }

        /// <summary>
        /// Удалить первую строку в файле
        /// </summary>
        public void deliteFirstLine()
        {
            deliteFirstLine(file);
        }

        /// <summary>
        /// Запись в файл, по указанному пути
        /// </summary>
        /// <param name="filePath">Путь к файлу</param>
        /// <param name="data">Данные на запись</param>
        /// <param name="rewrite">Режим записи (true = перезапись, false = дописывание в конец)</param>
        public static void write(string filePath, string data, bool rewrite = false)
        {
            lock (LOCKOBJEC)
            {
                if (!File.Exists(filePath))
                {
                    FileStream fileStream = File.Create(filePath);
                    fileStream.Dispose();
                }

                using (StreamWriter writer = new StreamWriter(filePath, !rewrite, Encoding.UTF8))
                {
                    writer.WriteLine(data);
                }
            }
        }

        /// <summary>
        /// Запись в файл, по указанному пути
        /// </summary>
        /// <param name="filePath">Путь к файлу</param>
        /// <param name="data">Данные на запись</param>
        public static void write(string filePath, IEnumerable<string> data, bool rewrite = true)
        {
            lock (LOCKOBJEC)
            {
                if (!File.Exists(filePath))
                {
                    File.Create(filePath).Dispose();
                }

                if (rewrite)
                {
                    File.WriteAllLines(filePath, data, Encoding.UTF8);
                }
                else
                {
                    File.AppendAllLines(filePath, data, Encoding.UTF8);
                }
            }
        }

        /// <summary>
        /// Запись в файл, по указанному пути
        /// </summary>
        /// <param name="data">Данные на запись</param>
        /// <param name="rewrite">Режим записи (true = перезапись, false = дописывание в конец)</param>
        public void write(string data, bool rewrite = false)
        {
            write(file, data, rewrite);
        }

        /// <summary>
        /// Считывание всех строк из файла
        /// </summary>
        /// <param name="filePath">Путь до файла</param>
        /// <returns></returns>
        public static List<String> readAllLines(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($@"Файл ""{filePath}"" не найден");
            }

            lock (LOCKOBJEC)
            {
                return new List<String>(File.ReadAllLines(filePath));
            }
        }

        /// <summary>
        /// Считывание всех строк из файла
        /// </summary>
        /// <returns></returns>
        public List<String> readAllLines()
        {
            return readAllLines(file);
        }

        /// <summary>
        /// Создает все папки, которые нужны для работы программы
        /// </summary>
        public static void createMyFoldersAndFiles()
        {
            checkAndCreateFolder(FilePath.Folder.SYSTEM);

            checkAndCreateFolder($@"{FilePath.Folder.SYSTEM}\{FilePath.Folder.LOG}");
        }

        /// <summary>
        /// Проверить существование указанного пути (относительного). Если отсутствует то создаёт его
        /// </summary>
        /// <param name="folder">Относительный путь</param>
        public static void checkAndCreateFolder(string folder)
        {
            if (!Directory.Exists(Directory.GetCurrentDirectory() + @"\" + folder))
            {
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + @"\" + folder);
            }
        }

        /// <summary>
        /// Проверить на существование указанного файла. Если отсутсвует то создаёт его
        /// </summary>
        /// <param name="file">Путь к файлу</param>
        public static void checkAndCreateFile(string file)
        {
            if (!File.Exists(file))
            {
                File.Create(file).Close();
            }
        }

        /// <summary>
        /// Проверить на существование файла. Если отсутсвует то создаёт его
        /// </summary>
        public void checkAndCreateFile()
        {
            if (!File.Exists(file))
            {
                File.Create(file).Close();
            }
        }
    }
}
