using GosZakupki.Model;
using GosZakupki.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace GosZakupki.Parser
{
    class SearchLotPageParser : BaseSearchPageParser
    {
        private static object lockObject = new object();

        public SearchLotPageParser(int startFromPage) : base(startFromPage)
        {
            URL = @"https://www.goszakup.gov.kz/ru/search/lots?filter[method][0]=3&filter[method][1]=2&filter[method][2]=7&filter[method][3]=6&filter[method][4]=50&filter[method][5]=52&filter[method][6]=22&count_record=50&page=";
        }

        public void start(object objData)
        {
            ManualResetEvent handle = objData as ManualResetEvent;

            try
            {
                while (page.NEXT_PAGE <= SearchPage.LAST_PAGE)
                {
                    parsePage(page);

                    page.CURRENT_PAGE = page.NEXT_PAGE;

                    //todo: Проблема с частыми запросами. Надо найти хорошее решение
                    //Слишком частые запросы приводят к тому, что сайт требует ввести капчу
                    //Сделал временное решение, в виде обычной паузы, но при многопоточности это не работает
                    Tools._pause(1000, 5000);
                }
            }
            catch (Exception ex)
            {
                Log.E(ex.Message);
            }
            finally
            {
                Log.D("Thread Job Done");
                handle.Set();
            }
        }

        private new void parsePage(SearchPage page)
        {
            if (isHaveParsed(page))
                return;

            base.parsePage(page);

            lock (lockObject)
            {
                if (isHaveParsed(page))
                    return;
                
                SearchPage.PASSED_PAGE_LIST.Add(page.CURRENT_PAGE);
            }

            List<string> listOfAdvertLink = links
                .Where(link => LinkType.getType(link) is LinkType.Type.ADVERT)
                .ToList();

            List<string> listOfLotLink = links
                .Where(link => LinkType.getType(link) is LinkType.Type.LOT)
                .ToList();

            Log.D("SearchLotPageParser\r\n"
                + $"\t Current Page = {page.CURRENT_PAGE} Next Page = {page.NEXT_PAGE} Last Page = {SearchPage.LAST_PAGE}\r\n"
                + $"\t\t links count = {links.Count}\r\n"
                + $"\t\t\t Link #1 == {listOfAdvertLink[0]}\tType == {LinkType.getType(listOfAdvertLink[0])}\r\n"
                + $"\t\t\t Link #2 == {listOfLotLink[0]}\tType == {LinkType.getType(listOfLotLink[0])}\r\n");
        }
    }
}
