using HtmlAgilityPack;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;

namespace GosZakupki
{
    class Program
    {
        private const int THREAD_COUNT = 1;
        private static int LAST_PAGE = 999;
        private const string URL = @"https://www.goszakup.gov.kz/ru/search/lots?filter[method][0]=3&filter[method][1]=2&filter[method][2]=7&filter[method][3]=6&filter[method][4]=50&filter[method][5]=52&filter[method][6]=22&count_record=50&page=";
        private static ArrayList PASSED_PAGE_LIST = new ArrayList();

        static void Main(string[] args)
        {
            Thread[] threads = new Thread[THREAD_COUNT];
            ManualResetEvent[] handles = new ManualResetEvent[THREAD_COUNT];

            for (int i = 0; i < THREAD_COUNT; i++)
            {
                handles[i] = new ManualResetEvent(false);
                threads[i] = new Thread(new ParameterizedThreadStart(threadJob));
                threads[i].IsBackground = true;
                threads[i].Start(new object[] { handles[i], i + 1 });
            }

            WaitHandle.WaitAll(handles);

            Console.WriteLine();
            Console.Write("Done");
            Console.ReadKey();
        }

        private static void threadJob(object objData)
        {
            object[] objMass = objData as object[];
            ManualResetEvent handle = objMass[0] as ManualResetEvent;
            int page = (int) objMass[1];
            int nextPage = page + 1;

            try
            {
                while (nextPage <= LAST_PAGE)
                {
                    parsePage(page);

                    page = nextPage;
                    nextPage++;

                    //todo: Проблема с частыми запросами. Надо найти хорошее решение
                    //Слишком частые запросы приводят к тому, что сайт требует ввести капчу
                    //Сделал временное решение, в виде обычной паузы, но при многопоточности это не работает
                    Tools._pause(new Random(DateTime.Now.Millisecond).Next(1, 5) * 1000);
                }
            }
            catch (Exception ex)
            {
                //todo: Убрать
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.WriteLine("Thread Job Done");
                handle.Set();
            }
        }

        private static void parsePage(int page)
        {
            HttpClient client;
            HtmlDocument document = new HtmlDocument();
            string body = default(string);
            bool hasNavigationMenu;

            using (client = getNewHttpClient())
            {
                body = client.GetStringAsync(URL + page).Result;
            }

            if (string.IsNullOrWhiteSpace(body))
            {
                throw new Exception("Empty body");
            }

            document.LoadHtml(body);
            hasNavigationMenu = checkNavigationMenu(document);

            if (hasNavigationMenu)
            {
                if (PASSED_PAGE_LIST.Contains(page))
                {
                    return;
                }

                PASSED_PAGE_LIST.Add(page);

                updateLastPagePosition(document);
                //todo: Убрать
                Console.WriteLine($"Current Page = {page} Next Page = {page + 1} Last Page = {LAST_PAGE}");


                List<string> links = default(List<string>);
                HtmlNodeCollection nodes = document.DocumentNode.SelectNodes("//table[@id='search-result']//*//td//a[@href]");
                if (nodes != null)
                {
                    links = nodes
                        .Select(a => a.GetAttributeValue("href", String.Empty))
                        .Where(link => !string.IsNullOrWhiteSpace(link) && link != "javascript:void(0)")
                        .ToList();
                }

                links.ForEach(Console.WriteLine);
            }
            else
            {
                throw new Exception("No navigation menu");
            }
        }

        private static bool checkNavigationMenu(HtmlDocument document)
        {
            List<string> links = default(List<string>);
            HtmlNodeCollection nodes = document.DocumentNode.SelectNodes("//a[@href]");

            if (nodes != null) {
                links = nodes    
                    .Select(a => a.GetAttributeValue("href", String.Empty))
                    .Where(link => !string.IsNullOrWhiteSpace(link) && link.Contains("page=") && !link.EndsWith("page=0"))
                    .ToList();
            }

            if (links != null)
            {
                return links.Count > 0;
            }

            return false;
        }

        private static void updateLastPagePosition(HtmlDocument document)
        {
            IEnumerable<string> lastPageLink = null;
            HtmlNodeCollection nodes = document.DocumentNode.SelectNodes("//a[@href]");

            if (nodes != null)
            {
                lastPageLink = nodes
                    .Select(a => a.GetAttributeValue("href", String.Empty))
                    .Where(link => !string.IsNullOrWhiteSpace(link) && link.Contains("page=") && !link.EndsWith("page=0"));
            }

            if (lastPageLink != null)
            {
                string link = lastPageLink.Last();
                LAST_PAGE = int.Parse(link.Substring(link.LastIndexOf("page=") + "page=".Length));
            }
        }

        private static HttpClient getNewHttpClient()
        {
            HttpClient client;
            HttpClientHandler handler;

            handler = new HttpClientHandler
            {
                ClientCertificateOptions = ClientCertificateOption.Manual,
                ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, cetChain, policyErrors) => { return true; }
            };

            client = new HttpClient(new RetryHandler(handler));

            return client;
        }
    }
}
