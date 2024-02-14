namespace Kanban.Integration.Tests.DatabaseMocks;

public class Mocks
{
    public const string SampleMockOne = "{\"_id\":\"65c6e255a03db52a8056230f\",\"Name\":\"Card Test One\", \"Description\":\"Card One Description\"}";
    public const string SampleMockTwo = "{\"_id\":\"65c77ba67d5a911ae3d662db\",\"Name\":\"Card Test Two\", \"Description\":\"Card Two Description\"}";
    public const string SampleMockThree = "{\"_id\":\"65c77bbb7d5a911ae3d662dc\",\"Name\":\"Card Test Three\", \"Description\":\"Card Three Description\"}";

    public const string UpdateMock = "{\"Id\":\"65c77ba67d5a911ae3d662db\",\"Name\":\"Card Test Two\", \"Description\":\"Card Two Description\"}";

    public const string NonexistingMockObject = "{\"Id\":\"65c806377d5a911ae3d662f0\",\"Name\":\"Card Test Update\", \"Description\":\"Card Update Description\"}";

    public const string ClientMock = "{\"_id\":\"client\", \"secret\":\"secret\"}";

    public const string InsertMockObject = "{\"Name\":\"Card Test Insert\", \"Description\":\"Card Insert Description\"}";

    public const string BoardMock = "{\"_id\":\"65cbec3865a3b4fbed6945aa\",\"name\":\"Boardmock\",\"columns\":[{\"_id\":\"65cbec6d65a3b4fbed6945ab\",\"name\":\"column1\",\"cards\":[\"65c6e255a03db52a8056230f\",\"65c77ba67d5a911ae3d662db\"]},{\"_id\":\"65cbed1365a3b4fbed6945ac\",\"name\":\"column2\",\"cards\":[\"65c77bbb7d5a911ae3d662dc\",\"65c89bcd3adb8be079b61e88\"]}]}";

    public const string BoardId = "65cbec3865a3b4fbed6945aa";

    public const string CreateBoardRequest = "{\"name\":\"Create Board\"}";

    public const string UpdateBoardRequest = "{\"Id\":\"65cbec3865a3b4fbed6945aa\",\"name\":\"Update Board\"}";
    public const string NonexistingUpdateBoardRequest = "{\"Id\":\"65cca89665a3b4fbed6945ae\",\"name\":\"Update Board\"}";
}
