namespace Trucks.Shared
{
    public static class GlobalConstants
    {
        //Client
        public const int ClientNameMinLength = 3;
        public const int ClientNameMaxLength = 40;
        public const int ClientNationalityMinLength = 2;
        public const int ClientNationalityMaxLength = 40;


        //Despatcher
        public const int DespatcherNameMinLength = 2;
        public const int DespatcherNameMaxLength = 40;


        //Truck
        public const int TruckVinNumberLength = 17;
        public const string TruckRegistrationNumberRegex = @"[A-Z]{2}[0-9]{4}[A-Z]{2}$";
    }
}
