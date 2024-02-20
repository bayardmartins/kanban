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
        public const string Columns = "Columns";
        public const string ColumnCards = "Columns.$.Cards";
        public const string Authentication = "Basic";
        public const string Authorization = "Authorization";

        // Messages
        public const string ClientRegitered = "Client successfully registered";
        public const string MissingAuthorizationKey = "Missing Authorization Key";
        public const string AuthorizationHeaderMalformed = "Authorization header malformed";
        public const string InvalidAuthorizationHeaderFormat = "Invalid authorization header format";
        public const string InvalidIdOrSecret = "Invalid id or secret";
        public const string CardIdMissmatch = "Route Id don't match Card.Id";

        public const string OutOfBoundary = "Index out of boundary";

        public const string CardUpdated = "Card updated";
        public const string CardNotFound = "Card not found";
        public const string CardDeleted = "Card updated";
        public const string UnableToDeleteCard = "Unable to delete card";
        public const string FailedToMoveCard = "Failed to move card";

        public const string BoardDeleted = "Board deleted";
        public const string BoardNotFound = "Board not found";
        public const string BoardInvalid = "BoardId invalid";
        public const string FailedToDeleteBoard = "Failed to delete board";
        public const string BoardWithColumns = "Board with columns can't be deleted. Board has {0} columns. Delete all columns before deleting board";
        public const string BoardIdMissmatch = "Route Id don't match Card.Id";

        public const string ColumnNotFound = "Column not found";
        public const string FailedToUpdateColumn = "Failed to update column";
        public const string OriginNotFound = "Origin Column not found";
        public const string DestinyNotFound = "Destiny Column not found";
        public const string OriginAndDestinyAreTheSame = "Origin and Destiny column are the same";
        public const string ColumnWithCards = "Column with cards can't be deleted. Column has {0} cards. Delete all cards before deleting column";
        public const string FailedToDeleteColumn = "Failed to delete column";
    }
}