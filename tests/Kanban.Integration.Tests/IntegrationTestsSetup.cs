﻿using Kanban.CrossCutting;
using Kanban.Integration.Tests.DatabaseMocks;
using Kanban.Model.Dto.Repository.Board;
using Kanban.Model.Dto.Repository.Card;
using Kanban.Model.Dto.Repository.Client;
using Kanban.Repository.Settings;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace Kanban.Integration.Tests;

public class IntegrationTestsSetup : IClassFixture<ApiWebApplicationFactory>
{
    protected readonly ApiWebApplicationFactory _factory;
    protected readonly HttpClient _client;
    private readonly MongoSettings _setting;
    private readonly IEnumerable<IMongoClient> _clients;
    private static bool _migrated = false;
    private static object _lock = new object();

    public IntegrationTestsSetup(ApiWebApplicationFactory fixture)
    {
        _factory = fixture;
        _client = _factory.CreateClient();

        _setting = _factory.Configuration.GetSection(Constants.MongoSettings).Get<MongoSettings>();
        var kanbanSettings = MongoClientSettings.FromConnectionString(_setting.KanbanHost.Host); ;
        kanbanSettings.SslSettings = new SslSettings { EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12 };
        var client = new MongoClient(kanbanSettings);
        _clients = new List<IMongoClient> { client };
        if (!_migrated)
        {
            lock (_lock)
            {
                if (!_migrated)
                {
                    this.Dispose();
                    this.Migrate();
                    _migrated = true;
                }
            }
        }        
    }

    private void Dispose()
    {
        var collection = _clients.ElementAt(_setting.KanbanHost.ClusterId).GetDatabase(_setting.KanbanHost.Database).GetCollection<CardDto>(_setting.Collections.Cards);
        collection.DeleteMany(Builders<CardDto>.Filter.Empty);
        var clientCollection = _clients.ElementAt(_setting.KanbanHost.ClusterId).GetDatabase(_setting.KanbanHost.Database).GetCollection<ClientDto>(_setting.Collections.Clients);
        clientCollection.DeleteMany(Builders<ClientDto>.Filter.Empty);
        var boardCollection = _clients.ElementAt(_setting.KanbanHost.ClusterId).GetDatabase(_setting.KanbanHost.Database).GetCollection<BoardDto>(_setting.Collections.Boards);
        boardCollection.DeleteMany(Builders<BoardDto>.Filter.Empty);
    }

    private void Migrate()
    {
        var cardCollection = _clients.ElementAt(_setting.KanbanHost.ClusterId).GetDatabase(_setting.KanbanHost.Database).GetCollection<CardDto>(_setting.Collections.Cards);
        var cardOne = JsonConvert.DeserializeObject<CardDto>(Mocks.SampleMockOne);
        var cardTwo = JsonConvert.DeserializeObject<CardDto>(Mocks.SampleMockTwo);
        var cardThree = JsonConvert.DeserializeObject<CardDto>(Mocks.SampleMockThree);
        var cardFour = JsonConvert.DeserializeObject<CardDto>(Mocks.SampleMockFour);
        var cardOneTwo = JsonConvert.DeserializeObject<CardDto>(Mocks.SampleMockOneTwo);
        var cardTwoTwo = JsonConvert.DeserializeObject<CardDto>(Mocks.SampleMockTwoTwo);
        var cardThreeTwo = JsonConvert.DeserializeObject<CardDto>(Mocks.SampleMockThreeTwo);
        var cardFourTwo = JsonConvert.DeserializeObject<CardDto>(Mocks.SampleMockFourTwo);
        var list = new List<CardDto> { cardOne, cardTwo, cardThree, cardFour, cardOneTwo, cardTwoTwo, cardThreeTwo, cardFourTwo };
        cardCollection.InsertMany(list);
        var clientCollection = _clients.ElementAt(_setting.KanbanHost.ClusterId).GetDatabase(_setting.KanbanHost.Database).GetCollection<ClientDto>(_setting.Collections.Clients);
        var client = JsonConvert.DeserializeObject<ClientDto>(Mocks.ClientMock);
        clientCollection.InsertOne(client);
        var boardCollection = _clients.ElementAt(_setting.KanbanHost.ClusterId).GetDatabase(_setting.KanbanHost.Database).GetCollection<BoardDto>(_setting.Collections.Boards);
        var boardOne = JsonConvert.DeserializeObject<BoardDto>(Mocks.BoardMock);
        var boardTwo = JsonConvert.DeserializeObject<BoardDto>(Mocks.SecondBoardMock);
        var boardEmpty = JsonConvert.DeserializeObject<BoardDto>(Mocks.EmptyBoardMock); 
        boardCollection.InsertMany(new List<BoardDto> { boardOne, boardTwo, boardEmpty });
    }

    internal string GetCredentials()
    {
        var client = JsonConvert.DeserializeObject<Model.Dto.Repository.Client.ClientDto>(Mocks.ClientMock);
        return $"{client._id}:{client.Secret}";
    }
}
