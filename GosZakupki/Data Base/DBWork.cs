namespace GosZakupki.Data_Base
{
    class DBWork
    {
        private static DBO dbo = new DBO();

        public static void createNotExistTables()
        {
            createContactsTable();
            createPersonTable();
            createCompanyTable();
        }

        private static void createCompanyTable()
        {
            dbo.executeNonQuery(@"IF OBJECT_ID(N'dbo.Company', N'U') IS NULL
                                BEGIN
                                    CREATE TABLE Company (
	                                    id INT IDENTITY(1,1) PRIMARY KEY,
	                                    nameInKaz char(500) UNIQUE,
	                                    nameInRus char(500) UNIQUE,
	                                    registrationDate bigint,
	                                    dateOfLastUpdate bigint,
	                                    haveRoles text,
                                        haveInRegisterOfStateCustomers BIT NOT NULL,
	                                    iin bigint UNIQUE,
	                                    bin bigint UNIQUE,
	                                    rnn bigint UNIQUE,
	                                    kato bigint,
	                                    seriesAndNumberCertificateOfStateRegistration text,
	                                    dateCertificateOfStateRegistration text,
	                                    director int,
	                                    contacts int,
	                                    reportingAdministrator int,
	                                    FOREIGN KEY (director) REFERENCES Person(id),
	                                    FOREIGN KEY (contacts) REFERENCES Contacts(id),
	                                    FOREIGN KEY (reportingAdministrator) REFERENCES Company(id)
                                    )
                                END", null);
        }

        private static void createPersonTable()
        {
            dbo.executeNonQuery(@"IF OBJECT_ID(N'dbo.Person', N'U') IS NULL
                                BEGIN
                                    CREATE TABLE Person (
	                                    id INT IDENTITY(1,1) PRIMARY KEY,
	                                    iin bigint UNIQUE,
	                                    rnn bigint UNIQUE,
	                                    fullName char(70) UNIQUE,
	                                    personRole text,
	                                    bankDetails text,
	                                    contacts int,
	                                    FOREIGN KEY (contacts) REFERENCES Contacts(id)
                                    )
                                END", null);
        }

        private static void createContactsTable()
        {
            dbo.executeNonQuery(@"IF OBJECT_ID(N'dbo.Contacts', N'U') IS NULL
                                BEGIN
                                    CREATE TABLE Contacts (
	                                     id INT IDENTITY(1,1) PRIMARY KEY,
	                                     postcode INT,
	                                     addressInRuss text,
	                                     addressInKaz text,
	                                     addressType text,
	                                     residency text,
	                                     region text,
	                                     website text,
	                                     email text,
	                                     phone text
                                     )
                                END", null);
        }
    }
}
