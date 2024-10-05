namespace MoviesTicket.Infrastructure.EntityConfigurations
{
    internal class MoviesEntityTypeConfiguration : IEntityTypeConfiguration<Movies>
    {
        public void Configure(EntityTypeBuilder<Movies> builder)
        {
            builder.ToTable("Movies");

            builder.HasKey(o => o.Id);

            builder.Ignore(b => b.DomainEvents);

            builder.Property(o => o.Id)
                .UseHiLo("movieseq");
            builder.Property(b => b.Title);


            builder.Property(m => m.MovieGUID)
                .IsRequired();
            builder.HasIndex(m => m.MovieGUID)
             .IsUnique();
            builder.Property(m => m.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(m => m.ReleaseDate)
                .IsRequired();

            builder.OwnsOne(m => m.Genres);


            builder.Property(m => m.Runtime)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(m => m.Director)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(m => m.Synopsis)
                .HasMaxLength(1000);

            builder.Property(m => m.IsActive)
                .IsRequired();

            builder.OwnsMany(m => m.ShowsTimes, a =>
            {
                a.WithOwner().HasForeignKey("MovieId");

                a.Property(o => o.Id)
               .UseHiLo("showtimeseq");

                a.HasKey(o => o.Id);

                a.Ignore(b => b.DomainEvents);

                a.Property(st => st.ShowsTimeGUID)
                    .IsRequired();
                a.HasIndex(st => st.ShowsTimeGUID)
                       .IsUnique();
                a.Property(st => st.ShowDate)
                    .IsRequired();

                a.Property(st => st.Time)
                    .IsRequired()
                    .HasMaxLength(50);

                a.Property(st => st.IsDeleted)
                    .IsRequired();

                a.OwnsMany(st => st.Reservation, r =>
                {
                    r.WithOwner().HasForeignKey("ShowsTimeGUID");
                    r.Property<int>("ReservationId").UseHiLo("reservationseq", "dbo");
                    r.HasKey("ReservationId");

                    r.Property(res => res.FirstName)
                        .IsRequired()
                        .HasMaxLength(100);

                    r.Property(res => res.LastName)
                        .IsRequired()
                        .HasMaxLength(100);

                    r.Property(res => res.Email)
                        .IsRequired()
                        .HasMaxLength(200);
                    r.ToTable("ShowsTime_Reservation");
                });
                a.ToTable("ShowsTime");
            });


        }
    }
}

