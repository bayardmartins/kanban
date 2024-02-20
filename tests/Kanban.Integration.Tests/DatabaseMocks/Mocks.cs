namespace Kanban.Integration.Tests.DatabaseMocks;

public class Mocks
{
    public const string SampleMockOne = "{\"_id\":\"65c6e255a03db52a8056230f\",\"Name\":\"Card Test One\", \"Description\":\"Card One Description\"}";
    public const string SampleMockTwo = "{\"_id\":\"65c77ba67d5a911ae3d662db\",\"Name\":\"Card Test Two\", \"Description\":\"Card Two Description\"}";
    public const string SampleMockThree = "{\"_id\":\"65c77bbb7d5a911ae3d662dc\",\"Name\":\"Card Test Three\", \"Description\":\"Card Three Description\"}";
    public const string SampleMockFour = "{\"_id\":\"65c89bcd3adb8be079b61e88\",\"Name\":\"Card Test Four\", \"Description\":\"Card Four Description\"}";

    public const string SampleMockOneTwo = "{\"_id\":\"65ce9af265a3b4fbed694612\",\"Name\":\"Card Test One\", \"Description\":\"Card One Description\"}";
    public const string SampleMockTwoTwo = "{\"_id\":\"65ce9af765a3b4fbed694613\",\"Name\":\"Card Test Two\", \"Description\":\"Card Two Description\"}";
    public const string SampleMockThreeTwo = "{\"_id\":\"65ce9afd65a3b4fbed694614\",\"Name\":\"Card Test Three\", \"Description\":\"Card Three Description\"}";
    public const string SampleMockFourTwo = "{\"_id\":\"65ce9b0265a3b4fbed694615\",\"Name\":\"Card Test Four\", \"Description\":\"Card Four Description\"}";

    public const string UpdateMock = "{\"Id\":\"65c77ba67d5a911ae3d662db\",\"Name\":\"Card Test Two\", \"Description\":\"Card Two Description\"}";

    public const string NonexistingMockObject = "{\"Id\":\"65c806377d5a911ae3d662f0\",\"Name\":\"Card Test Update\", \"Description\":\"Card Update Description\"}";

    public const string ClientMock = "{\"_id\":\"client\", \"secret\":\"secret\"}";

    public const string InsertMockObject = "{\"Name\":\"Card Test Insert\", \"Description\":\"Card Insert Description\"}";

    public const string BoardMock = "{\"_id\":\"65cbec3865a3b4fbed6945aa\",\"name\":\"Boardmock\",\"columns\":[{\"_id\":\"65cbec6d65a3b4fbed6945ab\",\"name\":\"column1\",\"cards\":[\"65c6e255a03db52a8056230f\",\"65c77ba67d5a911ae3d662db\",\"65d3b1bdd34d409353458cf5\"]},{\"_id\":\"65cbed1365a3b4fbed6945ac\",\"name\":\"column2\",\"cards\":[\"65c77bbb7d5a911ae3d662dc\",\"65c89bcd3adb8be079b61e88\"]},{\"_id\":\"65cfafb85550509ebdc7f82c\",\"name\":\"column empty\",\"cards\":[]}]}";

    public const string SecondBoardMock = "{\"_id\":\"65ccabdb65a3b4fbed6945af\",\"name\":\"Boardmock number two\",\"columns\":[{\"_id\":\"65cbec6d65a3b4fbed6945ab\",\"name\":\"column1\",\"cards\":[\"65ce9af265a3b4fbed694612\",\"65ce9af765a3b4fbed694613\"]},{\"_id\":\"65cbed1365a3b4fbed6945ac\",\"name\":\"column2\",\"cards\":[]}]}";

    public const string EmptyBoardMock = "{\"_id\":\"65cf4e805550509ebdc7f82a\",\"name\":\"Boardmock empty\",\"columns\":[]}";


    public const string BoardOneId = "65cbec3865a3b4fbed6945aa";
    public const string BoardTwoId = "65ccabdb65a3b4fbed6945af";
    public const string EmptyBoardId = "65cf4e805550509ebdc7f82a";
    public const string NonexistingBoardId = "65ccad2765a3b4fbed6945b0";

    public const string CreateBoardRequest = "{\"name\":\"Create Board\"}";
    public const string UpdateBoardRequest = "{\"Id\":\"65cbec3865a3b4fbed6945aa\",\"name\":\"Update Board\"}";
    public const string NonexistingUpdateBoardRequest = "{\"Id\":\"65cca89665a3b4fbed6945ae\",\"name\":\"Update Board\"}";
    public const string NonexistingUpdateBoardId = "65cca89665a3b4fbed6945ae";


    public const string AddColumnRequestOne = "{\"columnName\":\"column added one\",\"position\":0}";
    public const string AddColumnRequestTwo = "{\"columnName\":\"column added two\",\"position\":1}";
    public const string AddColumnRequestThree = "{\"columnName\":\"column added three\",\"position\":3}";

    public const string UpdColumnReqSuccess = "{\"columnName\":\"new name update\"}";
    public const string ExistingColumn = "65cbec6d65a3b4fbed6945ab";
    public const string ColumnWithoutCards = "65cfafb85550509ebdc7f82c";
}
