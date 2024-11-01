
namespace BT.Shared
{
    public class AppConstants
    {
        public static string ApiGateway = "API-Gateway";
        /// <summary>
        /// Attempt to access service API directly. No access granted;
        /// </summary>
        public static string ServiceIsUnavailable503 = "Sorry, service is unavailable.";

        public static string ParamsMissingError_Id = "Id parameter missing.";


        public static string CreateDatabaseEntitySuccess = "Successfully created database entity.";
        public static string DeleteDatabaseEntitySuccess = "Successfully deleted database entity.";

        public static string CreateDatabaseEntityFailed = "Create database entity failed.";
        public static string UpdateDatabaseEntityFailed = "Updating database entity failed.";
        public static string DeleteDatabaseEntityFailed = "Delete database entity failed.";


        
        public static string ErrorRetrievingEntity = "Error retrieving entity.";


        public static string DatabaseEntityNotFound = "Database enetity not found.";

    }
}
