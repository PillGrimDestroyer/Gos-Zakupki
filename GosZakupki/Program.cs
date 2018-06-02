using GosZakupki.Data_Base;
using GosZakupki.Model;
using GosZakupki.Parser;
using GosZakupki.Support;
using System;
using System.Threading;

namespace GosZakupki
{
    class Program
    {
        private const int THREAD_COUNT = 1;

        static void Main(string[] args)
        {
            #if DEBUG
                Log.STANDART_TYPE = Log.Type.CONSOLE;
            #else
                Log.STANDART_TYPE = Log.Type.BOTH;
            #endif

            FileOperation.createMyFoldersAndFiles();

            try
            {
                DBWork.createNotExistTables();

                parseCompany();
                //parseAdvertAndLot();
            }
            catch (Exception ex)
            {
                Log.E(ex.Message);
            }
            finally
            {
                Console.WriteLine();
                Console.Write("Done");
                Console.ReadKey();
            }
        }

        /// <summary>
        /// Запуск парсинга по всем компаниям
        /// </summary>
        static void parseCompany()
        {
            SearchPage.LAST_PAGE = 999;
            SearchPage.PASSED_PAGE_LIST.Clear();

            Thread[] threads = new Thread[THREAD_COUNT];
            ManualResetEvent[] handles = new ManualResetEvent[THREAD_COUNT];

            for (int i = 0; i < THREAD_COUNT; i++)
            {
                var searchLotPageParser = new SearchCompanyPageParser(i + 1);

                handles[i] = new ManualResetEvent(false);
                threads[i] = new Thread(new ParameterizedThreadStart(searchLotPageParser.start));
                threads[i].IsBackground = true;
                threads[i].Start(handles[i]);

                Tools._pause(1000, 5000);
            }

            WaitHandle.WaitAll(handles);
        }

        /// <summary>
        /// Запуск парсинга по всем объявлениям и их лотов
        /// </summary>
        static void parseAdvertAndLot()
        {
            SearchPage.LAST_PAGE = 999;
            SearchPage.PASSED_PAGE_LIST.Clear();

            Thread[] threads = new Thread[THREAD_COUNT];
            ManualResetEvent[] handles = new ManualResetEvent[THREAD_COUNT];

            for (int i = 0; i < THREAD_COUNT; i++)
            {
                var searchLotPageParser = new SearchLotPageParser(i + 1);

                handles[i] = new ManualResetEvent(false);
                threads[i] = new Thread(new ParameterizedThreadStart(searchLotPageParser.start));
                threads[i].IsBackground = true;
                threads[i].Start(handles[i]);

                Tools._pause(1000, 5000);
            }

            WaitHandle.WaitAll(handles);
        }
    }
}
