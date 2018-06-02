using GosZakupki.ConstantData;
using GosZakupki.Model;
using GosZakupki.Support;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace GosZakupki.Parser
{
    public class BaseSearchPageParser
    {
        protected string URL;
        protected List<string> links = default(List<string>);

        protected SearchPage page;

        public BaseSearchPageParser(int startFromPage)
        {
            page = new SearchPage(startFromPage);
        }

        protected void parsePage(SearchPage page)
        {
            if (isHaveParsed(page))
            {
                return;
            }

            HttpClient client;
            HtmlDocument document = new HtmlDocument();
            string body = default(string);
            bool hasNavigationMenu;

            using (client = getNewHttpClient())
            {
                body = client.GetStringAsync(URL + page.CURRENT_PAGE).Result;
            }

            if (string.IsNullOrWhiteSpace(body))
            {
                throw new Exception("Empty body");
            }

            document.LoadHtml(body);
            hasNavigationMenu = checkNavigationMenu(document);

            if (!hasNavigationMenu)
            {
                //throw new Exception("No navigation menu");
                Tools._pause(5000, 10000);
                parsePage(page);

                return;
            }

            if (isHaveParsed(page))
            {
                return;
            }

            updateLastPagePosition(document);
            
            HtmlNodeCollection nodes = document.DocumentNode.SelectNodes(Node.TABLE_SEARCH_RESULT);

            if (nodes == null)
            {
                //throw new Exception("No data in body (maybe need enter the captcha)");
                Tools._pause(5000, 10000);
                parsePage(page);

                return;
            }

            links = nodes
                .Select(a => a.GetAttributeValue("href", String.Empty))
                .Where(link => !string.IsNullOrWhiteSpace(link) && link != "javascript:void(0)")
                .ToList();
        }

        protected bool isHaveParsed(SearchPage page)
        {
            return SearchPage.PASSED_PAGE_LIST.Contains(page.CURRENT_PAGE);
        }

        private bool checkNavigationMenu(HtmlDocument document)
        {
            List<string> links = default(List<string>);
            HtmlNodeCollection nodes = document.DocumentNode.SelectNodes(Node.LINK);

            if (nodes != null)
            {
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

        private void updateLastPagePosition(HtmlDocument document)
        {
            IEnumerable<string> lastPageLink = null;
            HtmlNodeCollection nodes = document.DocumentNode.SelectNodes(Node.LINK);

            if (nodes != null)
            {
                lastPageLink = nodes
                    .Select(a => a.GetAttributeValue("href", String.Empty))
                    .Where(link => !string.IsNullOrWhiteSpace(link) && link.Contains("page=") && !link.EndsWith("page=0"));
            }

            if (lastPageLink != null)
            {
                string link = lastPageLink.Last();
                SearchPage.LAST_PAGE = int.Parse(link.Substring(link.LastIndexOf("page=") + "page=".Length));
            }
        }

        private HttpClient getNewHttpClient()
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
