global using Xunit;
global using FluentAssertions;
global using Kanban.Infra.Tests.DatabaseMocks;
global using Kanban.Model.Dto.Repository.Card;
global using Kanban.Model.Dto.Repository.Client;
global using Kanban.Model.Dto.Repository.Board;
global using MongoDB.Bson;
global using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;

[assembly: ExcludeFromCodeCoverage]