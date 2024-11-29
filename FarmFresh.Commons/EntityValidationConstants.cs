namespace FarmFresh.Commons;

public static class EntityValidationConstants
{
    public static class User
    {
        public const int UserFirstNameMinLength = 2;
        public const int UserFirstNameMaxLength = 15;

        public const int UserLastNameMinLength = 3;
        public const int UserLastNameMaxLength = 15;

        public const int UserUserNameMinLength = 5;
        public const int UserUserNameMaxLength = 15;

        public const int PersonPhotoWidth = 300;
        public const int PersonPhotoHeight = 500;
    }

    public static class Farmers
    {
        public const int FarmerDescriptionMinLength = 0;
        public const int FarmerDescriptionMaxLength = 200;

        public const int FarmerLocationMinLegth = 10;
        public const int FarmerLocationMaxLegth = 100;

        public const int FarmerPhoneNumberMinLength = 7;
        public const int FarmerPhoneNumberMaxLength = 14;
    }

    public static class Categories
    {
        public const int CategoryNameMinLength = 3;
        public const int CategoryNameMaxLength = 50;
    }

    public static class Products
    {
        public const int ProductNameMinLength = 5;
        public const int ProductNameMaxLength = 100;

        public const int ProductDescriptionMinLength = 10;
        public const int ProductDescriptionMaxLength = 500;

        public const int ProductOriginMinLength = 3;
        public const int ProductOriginMaxLength = 80;
    }

    public static class Orders
    {
        public const int OrderAdressMinLength = 5;
        public const int OrderAdressMaxLength = 150;
    }

    public static class FarmerLocations
    {
        public const double LatitudeMinValue = -90.00;
        public const double LatitudeMaxValue = 90.00;

        public const double LongitudeMinValue = -180.00;
        public const double LongitudeMaxValue =  180.00;

        public const int TitleMinLength = 3;
        public const int TitleMaxLength = 150;
    }
}
