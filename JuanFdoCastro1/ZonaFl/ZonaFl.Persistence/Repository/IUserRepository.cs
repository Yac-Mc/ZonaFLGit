﻿using BB.SmsQuiz.Infrastructure.Domain;
using ZonaFl.Persistence.Entities;
namespace ZonaFl.Persistence.Repository
{
    public interface IUserRepository : IRepository<AspNetUsers>
    {
    }
}