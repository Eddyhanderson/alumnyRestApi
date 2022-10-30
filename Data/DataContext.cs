using alumni.Domain;
using Alumni.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
        public DbSet<AuthConfigTokens> RefreshTokens { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<AuthConfigTokens> AuthConfigTokens { get; set; }

        public DbSet<Comment> Comments { get; set; }
        public DbSet<Commentable> Commentables { get; set; }

        public DbSet<Certificate> Certificates { get; set; }
        public DbSet<Formation> Formations { get; set; }
        public DbSet<FormationEvent> FormationEvents { get; set; }
        public DbSet<FormationRequest> FormationRequests { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<Organ> Organ { get; set; }
        public DbSet<Post> Posts { get; set; }        
        public DbSet<School> Schools { get; set; }
        public DbSet<Question> Questions { get; set; }

        public DbSet<Studant> Studants { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }

        public DbSet<Video> Videos { get; set; }

        private void CompositeKeyBuilder(ModelBuilder builder)
        {
           
        }
    }
}
