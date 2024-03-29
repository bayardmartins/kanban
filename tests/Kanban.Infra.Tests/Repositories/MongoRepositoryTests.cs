﻿using System.Linq;
using System.Runtime.CompilerServices;
using Kanban.Model.Dto.Repository.Column;

namespace Kanban.Infra.Tests.Repositories;

public class ClientRepositoryTests : MongoRepositoryTestsSetup
{
    #region Cards
    [Fact]
    public async Task GetById_ShouldReturnCard_WhenValidCardIsSearched()
    {
        // Act
        var response = await this.cardWorker.GetCardById(Mocks.SampleMockOneId);

        // Assert
        response._id.Should().Be(Mocks.SampleMockOneId);
    }

    [Fact]
    public async Task GetById_ShouldNotReturnCard_WhenInvalidCardIsSearched()
    {
        // Act
        var response = await this.cardWorker.GetCardById(ObjectId.GenerateNewId().ToString());

        // Assert
        response.Should().BeNull();
    }

    [Fact]
    public async Task GetAll_ShouldReturnCards_WhenThereAreCardsInDatabase()
    {
        // Act
        var response = await this.cardWorker.GetAllCards(new string[] { Mocks.SampleMockOneId, Mocks.SampleMockTwoId });

        // Assert
        response.Should().NotBeNull();
        response.Count.Should().Be(2);
        response.First(x => x._id == Mocks.SampleMockOneId).Should().NotBeNull();
        response.First(x => x._id == Mocks.SampleMockTwoId).Should().NotBeNull();
    }

    [Fact]
    public async Task GetAll_ShouldNotReturnCards_WhenThereAreInvalidCardsInList()
    {
        // Act
        var response = await this.cardWorker.GetAllCards(new string[] { Mocks.SampleMockOneId, Mocks.InvalidId });

        // Assert
        response.Should().BeNull();
    }

    [Fact]
    public async Task Insert_ShouldInsertCard_WhenCardIsGiven()
    {
        // Arrange
        var card = JsonConvert.DeserializeObject<CardDto>(Mocks.InsertMockObject);

        // Act
        var response = await this.cardWorker.InsertCard(card);

        // Assert
        response.Should().NotBeNull();
        response._id.Should().NotBe(card._id);
    }

    [Fact]
    public async Task Update_ShouldUpdateCard_WhenValidCardIsGiven()
    {
        // Arrange
        var card = JsonConvert.DeserializeObject<CardDto>(Mocks.UpdateMockObject);
        card = await this.cardWorker.InsertCard(card);
        card.Name = "New Name";

        // Act
        var response = await this.cardWorker.UpdateCard(card);
        var updatedCard = await this.cardWorker.GetCardById(card._id);
        // Assert
        response.Should().NotBeNull();
        response._id.Should().Be(card._id);
        updatedCard.Name.Should().Be("New Name");
    }

    [Theory]
    [InlineData(Mocks.NonexistingMockObject)]
    [InlineData(Mocks.InvalidMockObject)]
    public async Task Update_ShouldNotUpdateCard_WhenNonExistingCardIsGiven(string mock)
    {
        // Arrange
        var card = JsonConvert.DeserializeObject<CardDto>(mock);

        // Act
        var response = await this.cardWorker.UpdateCard(card);

        // Assert
        response.Should().BeNull();
    }

    [Fact]
    public async Task UpdateMany_ShouldUpdateManyCardDescriptions_WhenValidCardIdsAreGiven()
    {
        // Arrange
        var ids = new List<string> { Mocks.SampleMockOneId, Mocks.SampleMockTwoId };
        var newDescription = "New Description";

        // Act
        var response = await this.cardWorker.UpdateManyDescriptions(ids, newDescription);

        // Assert
        response.Should().Be(2);
    }

    [Fact]
    public async Task UpdateMany_ShouldNotUpdateAnyCardDescriptions_WhenAnInvalidValidCardIdsIsGiven()
    {
        // Arrange
        var ids = new List<string> { Mocks.SampleMockOneId, Mocks.SampleMockTwoId, Mocks.NonexistingMockObject };
        var newDescription = "New Description";

        // Act
        var response = await this.cardWorker.UpdateManyDescriptions(ids, newDescription);

        // Assert
        response.Should().Be(0);
    }

    [Fact]
    public async Task Delete_ShouldDeleteCard_WhenValidCardIdIsGiven()
    {
        // Arrange
        var id = Mocks.SampleMockOneId;

        // Act
        var cardBeforeDelete = await this.cardWorker.GetCardById(id);
        var response = await this.cardWorker.DeleteById(id);
        var deletedCard = await this.cardWorker.GetCardById(id);

        // Assert
        response.Should().BeTrue();
        deletedCard.Should().BeNull();
        cardBeforeDelete.Should().NotBeNull();
        var card = JsonConvert.DeserializeObject<CardDto>(Mocks.SampleMockOne);
        await this.cardWorker.InsertCard(card);
    }

    [Theory]
    [InlineData(Mocks.NonExistingCardId)]
    [InlineData(Mocks.InvalidId)]
    public async Task Delete_ShouldNotDeleteCard_WhenInvalidCardIdIsGiven(string id)
    {
        // Act
        var preDelete = await this.cardWorker.GetCardById(id);
        var response = await this.cardWorker.DeleteById(id);
        var deletedCard = await this.cardWorker.GetCardById(id);

        // Assert
        preDelete.Should().BeNull();
        response.Should().BeFalse();
        deletedCard.Should().BeNull();
    }

    #endregion

    #region Auth
    [Fact]
    public async Task GetById_ShouldGetClient_WhenValidIdAreSend()
    {
        // Act
        var result = await this.authWorker.GetClientById("client");

        // Assert
        result.Should().NotBeNull();
        result.Secret.Should().Be("secret");
    }

    [Fact]
    public async Task GetById_ShouldNotGetClient_WhenInvalidIdAreSend()
    {
        // Act
        var result = await this.authWorker.GetClientById("fake_client");

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task Register_ShouldRegisterClient_WhenValidCredentialsAreSend()
    {
        // Arrange
        var client = JsonConvert.DeserializeObject<ClientDto>(Mocks.NewClientMock);

        // Act
        await this.authWorker.RegisterClient(client);
        var result = await this.authWorker.GetClientById(client._id);

        // Assert
        result.Should().NotBeNull();
        result.Secret.Should().Be("newsecret");
    }
    #endregion

    #region Boards
    [Fact]
    public async Task GetById_ShouldReturnBoard_WhenValidBoardIsSearched()
    {
        // Act
        var response = await this.boardWorker.GetBoardById(Mocks.BoardOneId);

        // Assert
        response._id.Should().Be(Mocks.BoardOneId);
    }

    [Fact]
    public async Task GetAll_ShouldReturnBoards_WhereThereAreBoardsInDatabase()
    {
        // Act
        var response = await this.boardWorker.GetAllBoards();

        // Assert
        response.Should().NotBeNull();
        response.Count.Should().Be(2);
    }

    [Theory]
    [InlineData("65c6e255a03db52a8056230f")]
    [InlineData("65c6e255a03db52a80562")]
    public async Task GetById_ShouldNotReturnBoard_WhenInvalidBoardIsSearched(string id)
    {
        // Act
        var response = await this.boardWorker.GetBoardById(id);

        // Assert
        response.Should().BeNull();
    }

    [Fact]
    public async Task Insert_ShouldInsertBoard_WhenBoardIsGiven()
    {
        // Arrange
        var board = JsonConvert.DeserializeObject<BoardDto>(Mocks.InsertBoardMockObject);

        // Act
        var response = await this.boardWorker.InsertBoard(board);

        // Assert
        response.Should().NotBeNull();
        response.Name.Should().Be(board.Name);
        response._id.Should().NotBe(board._id);
        response.Columns.Length.Should().Be(0);
    }

    [Fact]
    public async Task Updade_ShouldUpdateBoard_WhenValidRequestIsGiven()
    {
        // Arrange
        var board = JsonConvert.DeserializeObject<BoardDto>(Mocks.UpdateBoardMockObject);

        // Act
        var response = await this.boardWorker.UpdateBoard(board);

        // Assert
        response.Should().NotBeNull();
        response.Name.Should().Be(board.Name);
        response._id.Should().Be(board._id);
        response.Columns.Length.Should().Be(0);
    }

    [Theory]
    [InlineData(Mocks.InvalidUpdateBoardMockObject)]
    [InlineData(Mocks.NonexistingBoardMockObject)]
    public async Task Updade_ShouldNotUpdateBoard_WhenInvalidRequestIsGiven(string mock)
    {
        // Arrange
        var board = JsonConvert.DeserializeObject<BoardDto>(mock);

        // Act
        var response = await this.boardWorker.UpdateBoard(board);

        // Assert
        response.Should().BeNull();
    }

    [Fact]
    public async Task Delete_ShouldDeleteBoard_WhenValidBoardIdIsGiven()
    {
        // Arrange
        var id = Mocks.BoardTwoId;

        // Act
        var cardBeforeDelete = await this.boardWorker.GetBoardById(id);
        var response = await this.boardWorker.DeleteById(id);
        var deletedCard = await this.boardWorker.GetBoardById(id);

        // Assert
        cardBeforeDelete.Should().NotBeNull();
        response.Should().BeTrue();
        deletedCard.Should().BeNull();
        var card = JsonConvert.DeserializeObject<BoardDto>(Mocks.SecondBoardMock);
        await this.boardWorker.InsertBoard(card);
    }

    [Theory]
    [InlineData(Mocks.NonexistingBoardId)]
    [InlineData(Mocks.InvalidBoardId)]
    public async Task Delete_ShouldNotDeleteBoard_WhenInvalidBoardIdIsGiven(string id)
    {
        // Act
        var cardBeforeDelete = await this.boardWorker.GetBoardById(id);
        var response = await this.boardWorker.DeleteById(id);
        var deletedCard = await this.boardWorker.GetBoardById(id);

        // Assert
        cardBeforeDelete.Should().BeNull();
        response.Should().BeFalse();
        deletedCard.Should().BeNull();
    }
    #endregion

    #region Columns

    [Fact]
    public async Task AddColumn_ShouldAddColumnToBoard_WhenValidBoardIsGiven()
    {
        // Arrange
        var boardMock = JsonConvert.DeserializeObject<BoardDto>(Mocks.SecondBoardMock);
        var columnCount = boardMock.Columns.Length;
        var column = JsonConvert.DeserializeObject<ColumnDto>(Mocks.ColumnAddMock);
        boardMock.Columns = boardMock.Columns.Append(column).ToArray();

        // Act
        var response = await this.boardWorker.UpdateBoardColumns(boardMock, boardMock.Columns.Length-1);
        var newBoard = await this.boardWorker.GetBoardById(boardMock._id);

        // Assert
        response.Should().NotBeNullOrEmpty();
        newBoard.Columns.Length.Should().Be(columnCount + 1);
    }

    [Theory]
    [InlineData(Mocks.NonexistingBoardMockObject)]
    [InlineData(Mocks.InvalidUpdateBoardMockObject)]
    public async Task AddColumn(string mock)
    {
        // Arrange
        var boardMock = JsonConvert.DeserializeObject<BoardDto>(mock);
        var column = JsonConvert.DeserializeObject<ColumnDto>(Mocks.ColumnAddMock);
        boardMock.Columns = boardMock.Columns.Append(column).ToArray();

        // Act
        var response = await this.boardWorker.UpdateBoardColumns(boardMock, 0);

        // Assert
        response.Should().BeNullOrEmpty();
    }

    [Theory]
    [MemberData(nameof(GetUpdateBoardColumn))]
    public async Task UpdateBoardColumnName(string mock, bool? result)
    {
        // Arrange
        var request = JsonConvert.DeserializeObject<UpdateColumnRequest>(mock);

        // Act
        var response = await this.boardWorker.UpdateBoardColumnName(request);

        // Assert
        response.Should().Be(result);
    }

    [Theory]
    [MemberData(nameof(GetColumnDeleteParams))]
    public async Task DeleteColumn(string boardId, string columnId, bool? result)
    {
        // Act
        var response = await this.boardWorker.DeleteColumn(boardId, columnId);

        // Assert
        response.Should().Be(result);
    }

    [Theory]
    [MemberData(nameof(GetUpdateColumnCardsParams))]
    public async Task UpdateColumnCards(string boardId, ColumnDto column, bool? result)
    {
        // Act
        var response = await this.boardWorker.UpdateColumnCards(boardId, column);

        // Assert
        response.Should().Be(result);
    }

    private static IEnumerable<object[]> GetUpdateBoardColumn() => new List<object[]>
    {
        new object[] { Mocks.UpdColumnReqInvalidBoard, null },
        new object[] { Mocks.UpdColumnReqNotFoundBoard, false },
        new object[] { Mocks.UpdColumnReqSuccess, true },
    };

    private static IEnumerable<object[]> GetColumnDeleteParams() => new List<object[]>
    {
        new object[] { Mocks.BoardTwoId, Mocks.ColumnToDelete, true },
        new object[] { Mocks.BoardOneId, Mocks.ColumnToDelete, false },
        new object[] { Mocks.InvalidBoardId, Mocks.ColumnToDelete, null },
    };

    private static IEnumerable<object[]> GetUpdateColumnCardsParams()
    {
        var board = JsonConvert.DeserializeObject<BoardDto>(Mocks.BoardMock);

        var cardOne = board.Columns.First().Cards[0];
        var cardTwo = board.Columns.First().Cards[1];

        board.Columns.First().Cards = new string[] { cardTwo, cardOne };
        var boardInvalid = JsonConvert.DeserializeObject<BoardDto>(Mocks.NonexistingBoardMockObject); 

        return new List<object[]>
        {
            new object[] { Mocks.InvalidBoardId, board.Columns.First(), null },
            new object[] { Mocks.BoardOneId, board.Columns.First(), true },
            new object[] { Mocks.BoardOneId, boardInvalid.Columns.First(), false },
        };
    }
    #endregion
}
