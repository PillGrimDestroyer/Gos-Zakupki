using System.Collections.Generic;

namespace GosZakupki.Model
{
    class Advert
    {
        public long id;

        public Company organizer;
        public Company invitedSupplier;
        public List<Person> competitionCommission;

        public string number;
        public string title;
        public string status;
        public long publishedDate;
        public long startDateDiscussion;
        public long endDateDiscussion;
        public long startDateReceivingApplications;
        public long endDateReceivingApplications;
        public string purchaseMethod;
        public string purchaseType;
        public string methodOfFailedPurchase;
        public string subject;
        public int lotsCount;
        public double purchaseAmount;
        public string symptoms;
    }
}
