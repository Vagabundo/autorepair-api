namespace Autorepair.Test.Data
{
    public static class TestConstants
    {
        public static class JobIds
        {
            public const int TYRES_ID = 1;
            public const int BRAKESDISCS_ID = 2;
            public const int BRAKESPADS_ID = 3;
            public const int OIL_ID = 4;
            public const int EXHAUST_ID = 5;
        }

        public static class TyrePosition
        {
            public const int FRONT_LEFT = 1;
            public const int FRONT_RIGHT = 2;
            public const int BACK_LEFT = 3;
            public const int BACK_RIGHT = 4;
        }

        public static class AnswerCodes
        {
            public const int DECLINED = -1;
            public const int REFERRED = 0;
            public const int APPROVED = 1;
        }

        public static class AnswerMessages
        {
            public const string ERROR_TYRE = "Tyre jobs do not follow the rules";
            public const string ERROR_BRAKES = "Brake jobs do not follow the rules";
            public const string ERROR_EXHAUST = "Exhaust jobs do not follow the rules";
            public const string ERROR_HOUR = "Total hours exceed the refence hours labour";
            public const string ERROR_PRICE = "The total price exceeds the reference price";
            public const string REFER = "Jobsheet has been referred";
            public const string APPROVE = "Jobsheet aproved successfully";
        }
    }
}