using JurayKV.Domain.Aggregates.BucketAggregate;
using JurayKV.Domain.Aggregates.GenericRepositoryInterface;
using JurayKV.Domain.Aggregates.ImageAggregate;
using JurayKV.Domain.Aggregates.UserMessageAggregate;
using JurayKV.Persistence.RelationalDB.Repositories.GenericRepositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace JurayKV.Persistence.RelationalDB.Repositories
{
     internal sealed class UserMessageRepository : GenericRepository<UserMessage>, IUserMessageRepository
    {
        private readonly JurayDbContext _dbContext;

        public UserMessageRepository(JurayDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public Task Create(UserMessage entity)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(UserMessage userMessage)
        {
            if (userMessage == null)
            {
                throw new ArgumentException("userMessage cannot be empty.", nameof(userMessage));
            }

            _dbContext.Remove(userMessage);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<UserMessage> GetByIdAsync(Guid userMessageId)
        {
            if (userMessageId == Guid.Empty)
            {
                throw new ArgumentException("userMessageId cannot be empty.", nameof(userMessageId));
            }
            UserMessage data = await _dbContext.UserMessages.Include(x=>x.User).FirstOrDefaultAsync(x => x.Id == userMessageId);
            return data;
        }

        public async Task InsertAsync(UserMessage userMessage)
        {
            if (userMessage == null)
            {
                throw new ArgumentException("userMessage cannot be empty.", nameof(userMessage));
            }

            await _dbContext.AddAsync(userMessage);
            await _dbContext.SaveChangesAsync();
        }

        public Task<List<UserMessage>> ListAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<List<UserMessage>> ListAllByUserIdAsync(Guid userId)
        {
            return await _dbContext.UserMessages.Where(x=>x.UserId == userId).ToListAsync();
        }

        public async Task Update(Guid id, UserMessage entity)
        {
            if (entity == null)
            {
                throw new ArgumentException("userMessage cannot be empty.", nameof(entity));
            }

            EntityEntry<UserMessage> trackedEntity = _dbContext.ChangeTracker.Entries<UserMessage>()
                .FirstOrDefault(x => x.Entity == entity);

            if (trackedEntity == null)
            {
                _dbContext.Update(entity);
            }

            await _dbContext.SaveChangesAsync();
        }

        public Task UpdateAsync(UserMessage userMessage)
        {
            throw new NotImplementedException();
        }

        IQueryable<UserMessage> IGenericRepository<UserMessage>.GetAll()
        {
            throw new NotImplementedException();
        }

        Task<UserMessage> IGenericRepository<UserMessage>.GetById(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
