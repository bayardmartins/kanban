﻿using MongoDB.Driver;

namespace Kanban.Repository.Interfaces;

public interface IMongoContext : IDisposable
{
    void AddCommand(Func<Task> func);

    Task<int> SaveChanges();

    IMongoCollection<T> GetCollection<T>(string name);
}