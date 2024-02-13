using System.Net;

namespace Kanban.CrossCutting
{
    public static class Constants
    {
        public const string MongoDbId = "_id";
        public const string HostSetting = "MongoSettings:KanbanHost:Host";
        public const string MongoSettings = "MongoSettings";
        public const string Name = "Name";
        public const string Description = "Description";
        public const string Authentication = "Basic";
        public const string Authorization = "Authorization";

        // Messages
        public const string ClientRegitered = "Client successfully registered";
        public const string MissingAuthorizationKey = "Missing Authorization Key";
        public const string AuthorizationHeaderMalformed = "Authorization header malformed";
        public const string InvalidAuthorizationHeaderFormat = "Invalid authorization header format";
        public const string InvalidIdOrSecret = "Invalid id or secret";
        public const string CardIdMissmatch = "Route Id don't match Card.Id";
        public const string CardUpdated = "Card updated";
        public const string CardNotFound = "Card not found";
        public const string CardDeleted = "Card updated";
    }
}