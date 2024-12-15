namespace FarmFresh.Commons;

public static class MessagesConstants
{
    public static string RejectProductEmailBody = "Dear Farmer,\n\nYour product '{0}' has been rejected.\n\nFor more information or assistance, feel free to contact us at:\n\nEmail: support@farmfresh2024.com\nPhone: +1-234-567-890\n\nRegards,\nAdmin";
    public static string ProductRejectNotification = "Product Rejection Notification";
    public static class Farmers
    {
        public static string SuccessfullyBecomeAFarmer = "Congratulations! You have successfully applied to become a farmer. The administrator will review your application, and you will receive a response as soon as possible.";
    }

    public static class Products
    {
        public static string SuccessfullyCreateProduct = "Congratulations! You successfully create product with name {0}. The administrator will review your product, and you will receive a response as soon as possible.";
    }

    public static class Users
    {
        public static string UserNotFound = "User profile not found. Please try again later or contact the administrator.";
        public static string SuccessfullyRegister = "Hello {0}, you have successfully registered! Welcome aboard.";
        public static string SendEmailForResetingPassword = "An email with a link to reset your password has been sent to the provided email address {0}. Please check your inbox and follow the instructions to reset your password.";
        public static string SuccessfullyResetThePassword = "Your password has been successfully reset! You can now log in using your new password.";
    }
}
