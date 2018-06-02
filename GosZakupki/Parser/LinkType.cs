using System;

namespace GosZakupki.Parser
{
    class LinkType
    {
        private const string ADVERT = @"announce";
        private const string LOT = @"subpriceoffer";
        private const string COMPANY = @"show_supplier";

        [Flags]
        public enum Type
        {
            /// <summary>
            /// Объявление
            /// </summary>
            ADVERT = 1,

            /// <summary>
            /// Лот
            /// </summary>
            LOT = 2,

            /// <summary>
            /// Компания
            /// </summary>
            COMPANY = 4,

            /// <summary>
            /// Неизвестный
            /// </summary>
            UNKNOWN = 8
        }

        public static Type getType(string link)
        {
            if (isAdvert(link))
                return Type.ADVERT;

            if (isLot(link))
                return Type.LOT;

            if (isCompany(link))
                return Type.COMPANY;

            return Type.UNKNOWN;
        }

        public static bool isAdvert(string link)
        {
            return link.Contains(ADVERT);
        }

        public static bool isLot(string link)
        {
            return link.Contains(LOT);
        }

        public static bool isCompany(string link)
        {
            return link.Contains(COMPANY);
        }
    }
}
