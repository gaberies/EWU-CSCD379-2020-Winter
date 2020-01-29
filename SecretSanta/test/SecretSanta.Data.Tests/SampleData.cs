namespace SecretSanta.Data.Tests
{
    static public class SampleData
    {
        public const string Inigo = "Inigo";
        public const string Montoya = "Montoya";

        public const string Princess = "Princess";
        public const string Buttercup = "Buttercup";

        public const string life = "life";
        public const string overrated = "overrated";
        public const string amazon = "amazon";

        public const string love = "love";
        public const string isnt = "isnt free";
        public const string ebay = "ebay.com";

        public const string teamlove = "teamlove";
        public const string teamlife = "teamlife";



        static public User CreateInigoMontoya() => new User(Inigo, Montoya);
        static public User CreatePrincessButtercup() => new User(Princess, Buttercup);
        static public Gift CreateLifeGift() => new Gift(life, overrated, amazon, CreateInigoMontoya());
        static public Gift CreateLoveGift() => new Gift(love, isnt, ebay, CreatePrincessButtercup());
        static public Group CreateLoveTeam() => new Group(teamlove);
        static public Group CreateLifeTeam() => new Group(teamlife);


    }
}