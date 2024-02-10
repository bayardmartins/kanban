using MongoDB.Bson.Serialization.Serializers;
using System.Net.NetworkInformation;

namespace Kanban.Infra.Tests.DatabaseMocks;

public static class CardMocks
{
    public static string SampleMockOne = "{\"_id\":\"65c6e255a03db52a8056230f\",\"Name\":\"Card Test One\", \"Description\":\"Card One Description\"}";
    public static string SampleMockTwo = "{\"_id\":\"65c77ba67d5a911ae3d662db\",\"Name\":\"Card Test Two\", \"Description\":\"Card Two Description\"}";
    public static string SampleMockThree = "{\"_id\":\"65c77bbb7d5a911ae3d662dc\",\"Name\":\"Card Test Three\", \"Description\":\"Card Three Description\"}";
    public static string SampleMockOneId = "65c6e255a03db52a8056230f";
    public static string NonExistingCardId = "65c6e255a03db52a8056230g";
}
