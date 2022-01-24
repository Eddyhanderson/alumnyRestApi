using alumni.Data;
using alumni.Domain;
using alumni.IServices;
using Alumni.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Services
{
    public class CommentableService : ICommentableService
    {
        private readonly DataContext dataContext;

        public CommentableService(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public async Task<CreationResult<Commentable>> CreateAsync()
        {
            var newCommentable = new Commentable
            {
                Id = Guid.NewGuid().ToString()
            };

            await dataContext.Commentables.AddAsync(newCommentable);

            try
            {
                await dataContext.SaveChangesAsync();

                return new CreationResult<Commentable>
                {
                    Data = newCommentable,
                    Succeded = true
                };
            }
            catch (DbUpdateException e)
            {
                Console.WriteLine(e.Message);

                return FailCreation();
            }
        }

        public async Task<bool> ObjectExists(string id)
        {
            return await dataContext.Commentables.AnyAsync(c => c.Id == id);
        }

        private CreationResult<Commentable> FailCreation()
        {
            return new CreationResult<Commentable>
            {
                Succeded = false,
                Errors = new[] { Constants.ModelMessages.FailModelCreated }
            };
        }
    }
}
