using System.Collections;

namespace GosZakupki.Model
{
    public class SearchPage
    {
        public static ArrayList PASSED_PAGE_LIST = new ArrayList();
        public static int LAST_PAGE = 999;

        public int CURRENT_PAGE;
        public int NEXT_PAGE => CURRENT_PAGE + 1;

        public SearchPage(int startFromPage)
        {
            CURRENT_PAGE = startFromPage;
        }
    }
}
