namespace GosZakupki.Model
{
    class Company
    {
        public long id;

        public Person director;
        public Contacts contacts;
        public Company reportingAdministrator;

        public string nameInKaz;
        public string nameInRus;
        public long registrationDate;
        public long dateOfLastUpdate;
        public string haveRoles;
        public bool haveInRegisterOfStateCustomers;
        public long iin;
        public long bin;
        public long rnn;
        public long kato;
        public string seriesAndNumberCertificateOfStateRegistration;
        public string dateCertificateOfStateRegistration;
    }
}
