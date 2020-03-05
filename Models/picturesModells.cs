namespace ModellsUp.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class picturesModells : DbContext
    {
        public picturesModells()
            : base("name=picturesModells")
        {
        }

       
        public virtual DbSet<category> category { get; set; }
       
        public virtual DbSet<picture> picture { get; set; }
        
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
       
            modelBuilder.Entity<category>()
                .Property(e => e.categoryName)
                .IsUnicode(false);

            modelBuilder.Entity<category>()
                .HasMany(e => e.picture)
                .WithRequired(e => e.category)
                .WillCascadeOnDelete(false);

        

            modelBuilder.Entity<picture>()
                .Property(e => e.pictureTitle)
                .IsUnicode(false);

            modelBuilder.Entity<picture>()
                .Property(e => e.pictureAlternateTitle)
                .IsUnicode(false);

            modelBuilder.Entity<picture>()
                .Property(e => e.pictureDescription)
                .IsUnicode(false);

            modelBuilder.Entity<picture>()
                .Property(e => e.pictureStandardUrl)
                .IsUnicode(false);

            modelBuilder.Entity<picture>()
                .HasMany(e => e.comments)
                .WithRequired(e => e.picture)
                .WillCascadeOnDelete(false);

        }
    }
}