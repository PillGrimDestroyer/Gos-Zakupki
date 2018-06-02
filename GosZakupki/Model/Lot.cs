namespace GosZakupki.Model
{
    class Lot
    {
        public long id;

        public Company customer;
        public Person customerRepresentative;

        public string number;
        public string status;
        public long startDateReceivingApplications;
        public long endDateReceivingApplications;
        public string truCode;
        public string truName;
        public string shortDescription;
        public string description;
        public string financingSource;
        public double pricePerUnit;
        public string measureUnit;
        public int count;
        public double oneYearPrice;
        public double twoYearPrice;
        public double threeYearPrice;
        public double plannedPrice;
        public float advancePayment;
        public long kato;
        public string deliveryPlace;
        public string deliveryTime;
        public string deliveryTerms;
    }
}
