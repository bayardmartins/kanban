using MongoDB.Bson.Serialization.Serializers;
using System.Net.NetworkInformation;

namespace Kanban.Infra.Tests.DatabaseMocks;

public static class Mocks
{
    public static string SampleMockOne = "{\"_id\":\"65c6e255a03db52a8056230f\",\"Name\":\"Card Test One\", \"Description\":\"Card One Description\"}";
    public static string SampleMockTwo = "{\"_id\":\"65c77ba67d5a911ae3d662db\",\"Name\":\"Card Test Two\", \"Description\":\"Card Two Description\"}";
    public static string SampleMockThree = "{\"_id\":\"65c77bbb7d5a911ae3d662dc\",\"Name\":\"Card Test Three\", \"Description\":\"Card Three Description\"}";

    public static string InsertMockObject = "{\"Name\":\"Card Test Insert\", \"Description\":\"Card Insert Description\"}";

    public static string UpdateMockObject = "{\"_id\":\"65c89bcd3adb8be079b61e88\",\"Name\":\"Card Test Update\", \"Description\":\"Card Update Description\"}";

    public static string NonexistingMockObject = "{\"_id\":\"65c806377d5a911ae3d662f0\",\"Name\":\"Card Test Update\", \"Description\":\"Card Update Description\"}";

    public static string SampleMockOneId = "65c6e255a03db52a8056230f";
    public static string SampleMockTwoId = "65c77ba67d5a911ae3d662db";
    public static string NonExistingCardId = "65c7c4ea7d5a911ae3d662e4";

    public static string ClientMock = "{\"_id\":\"client\", \"secret\":\"secret\"}";
    public static string NewClientMock = "{\"_id\":\"newclient\", \"secret\":\"newsecret\"}";
}
