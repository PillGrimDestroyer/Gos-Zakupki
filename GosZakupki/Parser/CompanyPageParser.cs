using GosZakupki.ConstantData;
using GosZakupki.Model;
using GosZakupki.Support;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace GosZakupki.Parser
{
    public class CompanyPageParser : BasePageParser
    {
        private Company company;
        private HttpClient client;
        private HtmlDocument document;

        public CompanyPageParser(string link) : base(link)
        {
            company = new Company();

            company.director = new Person();
            company.contacts = new Contacts();
        }

        public void parse()
        {
            Log.D("CompanyPageParser\r\n"
                + $"\t Link == {link}\r\n"
                + "\t START parsing\r\n");
            
            document = new HtmlDocument();
            string body = default(string);

            using (client = getNewHttpClient())
            {
                body = client.GetStringAsync(link).Result;
            }

            if (string.IsNullOrWhiteSpace(body))
            {
                throw new Exception("Empty body");
            }

            document.LoadHtml(body);

            HtmlNodeCollection nodes = document.DocumentNode.SelectNodes(Node.DEFAULT_PANEL);

            if (nodes == null)
            {
                //throw new Exception("No data in body (maybe need enter the captcha)");
                Tools._pause(5000, 10000);
                parse();

                return;
            }

            //todo: Убрать
            //Просто проверяю, могут ли отличаться данные от компании к компании
            if (nodes.Count != 4)
            {
                throw new Exception("Таблиц НЕ 4!!!");
            }

            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            for (int i = 0; i < nodes.Count; i++)
            {
                HtmlNode node = nodes[i];

                var header = document.DocumentNode.SelectSingleNode(node.XPath + Node.PANEL_HEADER);

                if (header != null)
                {
                    string innerText = header.InnerText;

                    if (innerText == CompanyHook.CONTACTS)
                    {
                        contactsTableParse(node, dictionary);
                        continue;
                    }
                }

                HtmlNodeCollection rowsAndColumns = document.DocumentNode.SelectNodes(node.XPath + "//tr");

                foreach (var item in rowsAndColumns)
                {
                    fillModel(item, dictionary);
                }
            }

            company.registrationDate = Tools.getUnixTime(DateTime.ParseExact(dictionary[CompanyHook.REGISTER_DATE], "yyyy-MM-dd HH:mm:ss", null));
            company.dateOfLastUpdate = Tools.getUnixTime(DateTime.ParseExact(dictionary[CompanyHook.LAST_UPDATE_DATE], "yyyy-MM-dd HH:mm:ss", null));
            company.haveRoles = dictionary[CompanyHook.ROLES];
            company.haveInRegisterOfStateCustomers = dictionary[CompanyHook.HAVE_IN_REGISTER_OF_STATE_CUSTOMERS] != "Не состоит";
            long.TryParse(dictionary[CompanyHook.IIN], out company.iin);
            long.TryParse(dictionary[CompanyHook.BIN], out company.bin);
            long.TryParse(dictionary[CompanyHook.RNN], out company.rnn);
            company.nameInKaz = dictionary[CompanyHook.NAME_IN_KAZ];
            company.nameInRus = dictionary[CompanyHook.NAME_IN_RUS];
            long.TryParse(dictionary[CompanyHook.KATO], out company.kato);
            company.contacts.region = dictionary[CompanyHook.REGION];
            company.contacts.website = dictionary[CompanyHook.WEBSITE];
            company.contacts.email = dictionary[CompanyHook.EMAIL];
            company.contacts.phone = dictionary[CompanyHook.TELEPHONE];
            company.seriesAndNumberCertificateOfStateRegistration = dictionary[CompanyHook.SERIES_AND_NUMBER_CERTIFICATE_OF_STATE_REGISTRATION];
            company.dateCertificateOfStateRegistration = dictionary[CompanyHook.DATE_CERTIFICATE_OF_STATE_REGISTRATION];
            //company.reportingAdministrator = dictionary[CompanyHook.REPORTING_ADMINISTRATOR]; //todo: сделать понормальному (тоесть, чтобы записывалось ID компании)



            Log.D("CompanyPageParser\r\n"
                + $"\t Link == {link}\r\n"
                + "\t END parsing\r\n");
        }

        private void fillModel(HtmlNode node, Dictionary<string, string> dictionary)
        {
            string header = document.DocumentNode.SelectSingleNode(node.XPath + "//th")?.InnerHtml;
            string data = document.DocumentNode.SelectSingleNode(node.XPath + "//td")?.InnerHtml;

            header = cleanUp(header);
            data = cleanUp(data);

            dictionary.Add(header, data);
        }

        private void contactsTableParse(HtmlNode node, Dictionary<string, string> dictionary)
        {

        }

        private string cleanUp(string text)
        {
            if (text != null)
            {
                if (text.StartsWith("\n"))
                {
                    text = text.Substring(2);
                }

                if (text.EndsWith("\n"))
                {
                    text = text.Remove(text.Length - 2);
                }

                text = text.Trim();
            }

            return text;
        }
    }
}
