using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Capstone.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Album> Albums { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<GenreArtist> GenreArtists { get; set; }
        public DbSet<GenreTrack> GenreTracks { get; set; }
        public DbSet<Listener> Listeners { get; set; }
        public DbSet<ListenerArtist> ListenerArtists { get; set; }
        public DbSet<ListenerGenre> ListenerGenres { get; set; }
        public DbSet<ListenerTrack> ListenerTracks { get; set; }
        public DbSet<Mood> Moods { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<Track> Tracks { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Genre>().Property(g => g.GenreId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<Genre>().
              HasOptional(e => e.ParentGenre).
              WithMany(g => g.Children).
              HasForeignKey(m => m.ParentGenreId).
              WillCascadeOnDelete(false);
            //modelBuilder.Entity<Genre>().
            //  HasOptional(e => e.Children).
            //  WithMany().
            //  HasForeignKey(m => m.ChildGenreId);
            modelBuilder.Entity<IdentityUserRole>()
            .HasKey(r => new { r.UserId, r.RoleId })
            .ToTable("AspNetUserRoles");

            modelBuilder.Entity<IdentityUserLogin>()
                        .HasKey(l => new { l.LoginProvider, l.ProviderKey, l.UserId })
                        .ToTable("AspNetUserLogins");
        }
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}