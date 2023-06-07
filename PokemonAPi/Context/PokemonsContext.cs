using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PokemonAPi.Models;

namespace PokemonAPi.Context;

public partial class PokemonsContext : DbContext
{
    public PokemonsContext()
    {
    }

    public PokemonsContext(DbContextOptions<PokemonsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Ability> Abilities { get; set; }

    public virtual DbSet<Abilitypokemon> Abilitypokemons { get; set; }

    public virtual DbSet<Characteristic> Characteristics { get; set; }

    public virtual DbSet<Egggroup> Egggroups { get; set; }

    public virtual DbSet<Gender> Genders { get; set; }

    public virtual DbSet<Growth> Growths { get; set; }

    public virtual DbSet<Pokemon> Pokemons { get; set; }

    public virtual DbSet<Pokemonegggroup> Pokemonegggroups { get; set; }

    public virtual DbSet<Pokemontype> Pokemontypes { get; set; }

    public virtual DbSet<Rating> Ratings { get; set; }

    public virtual DbSet<Stat> Stats { get; set; }

    public virtual DbSet<TypePokemon> TypePokemons { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Database=Pokemons;Username=postgres;password=1812");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Ability>(entity =>
        {
            entity.HasKey(e => e.IdAbility).HasName("ability_pkey");

            entity.ToTable("ability");

            entity.Property(e => e.IdAbility).HasColumnName("id_ability");
            entity.Property(e => e.Ability1)
                .HasColumnType("character varying")
                .HasColumnName("ability");
        });

        modelBuilder.Entity<Abilitypokemon>(entity =>
        {
            entity.HasKey(e => e.IdAbilitypokemon).HasName("abilitypokemon_pkey");

            entity.ToTable("abilitypokemon");

            entity.Property(e => e.IdAbilitypokemon).HasColumnName("id_abilitypokemon");
            entity.Property(e => e.IdAbility).HasColumnName("id_ability");
            entity.Property(e => e.IdPokemon).HasColumnName("id_pokemon");

            entity.HasOne(d => d.IdAbilityNavigation).WithMany(p => p.Abilitypokemons)
                .HasForeignKey(d => d.IdAbility)
                .HasConstraintName("abilitypokemon_id_ability_fkey");

            entity.HasOne(d => d.IdPokemonNavigation).WithMany(p => p.Abilitypokemons)
                .HasForeignKey(d => d.IdPokemon)
                .HasConstraintName("abilitypokemon_id_pokemon_fkey");
        });

        modelBuilder.Entity<Characteristic>(entity =>
        {
            entity.HasKey(e => e.IdCharacteristics).HasName("characteristics__pkey");

            entity.ToTable("characteristics_");

            entity.Property(e => e.IdCharacteristics).HasColumnName("id_characteristics");
            entity.Property(e => e.Height).HasColumnName("height_");
            entity.Property(e => e.Weight).HasColumnName("weight_");
        });

        modelBuilder.Entity<Egggroup>(entity =>
        {
            entity.HasKey(e => e.IdEgggroup).HasName("egggroup_pkey");

            entity.ToTable("egggroup");

            entity.Property(e => e.IdEgggroup).HasColumnName("id_egggroup");
            entity.Property(e => e.Group)
                .HasMaxLength(120)
                .HasColumnName("group_");
        });

        modelBuilder.Entity<Gender>(entity =>
        {
            entity.HasKey(e => e.IdGender).HasName("gender_pkey");

            entity.ToTable("gender");

            entity.Property(e => e.IdGender).HasColumnName("id_gender");
            entity.Property(e => e.Gender1)
                .HasColumnType("character varying")
                .HasColumnName("gender");
        });

        modelBuilder.Entity<Growth>(entity =>
        {
            entity.HasKey(e => e.IdGrowth).HasName("growth_pkey");

            entity.ToTable("growth");

            entity.Property(e => e.IdGrowth).HasColumnName("id_growth");
            entity.Property(e => e.Growth1)
                .HasColumnType("character varying")
                .HasColumnName("growth");
        });

        modelBuilder.Entity<Pokemon>(entity =>
        {
            entity.HasKey(e => e.IdPokemon).HasName("pokemon_pkey");

            entity.ToTable("pokemon");

            entity.Property(e => e.IdPokemon).HasColumnName("id_pokemon");
            entity.Property(e => e.Dateselected).HasColumnName("dateselected");
            entity.Property(e => e.Gen).HasColumnName("gen");
            entity.Property(e => e.Gender).HasColumnName("gender");
            entity.Property(e => e.IdCharacteristics).HasColumnName("id_characteristics_");
            entity.Property(e => e.IdGrowth).HasColumnName("id_growth");
            entity.Property(e => e.IdStats).HasColumnName("id_stats");
            entity.Property(e => e.Image)
                .HasMaxLength(250)
                .HasColumnName("image");
            entity.Property(e => e.Ispokemonofday).HasColumnName("ispokemonofday");
            entity.Property(e => e.NamePokemon)
                .HasMaxLength(120)
                .HasColumnName("name_pokemon");

            entity.HasOne(d => d.GenderNavigation).WithMany(p => p.Pokemons)
                .HasForeignKey(d => d.Gender)
                .HasConstraintName("pokemon_gender_fkey");

            entity.HasOne(d => d.IdCharacteristicsNavigation).WithMany(p => p.Pokemons)
                .HasForeignKey(d => d.IdCharacteristics)
                .HasConstraintName("pokemon_id_characteristics__fkey");

            entity.HasOne(d => d.IdGrowthNavigation).WithMany(p => p.Pokemons)
                .HasForeignKey(d => d.IdGrowth)
                .HasConstraintName("pokemon_id_growth_fkey");

            entity.HasOne(d => d.IdStatsNavigation).WithMany(p => p.Pokemons)
                .HasForeignKey(d => d.IdStats)
                .HasConstraintName("pokemon_id_stats_fkey");
        });

        modelBuilder.Entity<Pokemonegggroup>(entity =>
        {
            entity.HasKey(e => e.IdPokemonegg).HasName("pokemonegggroup_pkey");

            entity.ToTable("pokemonegggroup");

            entity.Property(e => e.IdPokemonegg).HasColumnName("id_pokemonegg");
            entity.Property(e => e.IdEgggroup).HasColumnName("id_egggroup");
            entity.Property(e => e.IdPokemon).HasColumnName("id_pokemon");

            entity.HasOne(d => d.IdEgggroupNavigation).WithMany(p => p.Pokemonegggroups)
                .HasForeignKey(d => d.IdEgggroup)
                .HasConstraintName("pokemonegggroup_id_egggroup_fkey");

            entity.HasOne(d => d.IdPokemonNavigation).WithMany(p => p.Pokemonegggroups)
                .HasForeignKey(d => d.IdPokemon)
                .HasConstraintName("pokemonegggroup_id_pokemon_fkey");
        });

        modelBuilder.Entity<Pokemontype>(entity =>
        {
            entity.HasKey(e => e.IdPokemontype).HasName("pokemontype_pkey");

            entity.ToTable("pokemontype");

            entity.Property(e => e.IdPokemontype).HasColumnName("id_pokemontype");
            entity.Property(e => e.IdPokemon).HasColumnName("id_pokemon");
            entity.Property(e => e.IdType).HasColumnName("id_type");

            entity.HasOne(d => d.IdPokemonNavigation).WithMany(p => p.Pokemontypes)
                .HasForeignKey(d => d.IdPokemon)
                .HasConstraintName("pokemontype_id_pokemon_fkey");

            entity.HasOne(d => d.IdTypeNavigation).WithMany(p => p.Pokemontypes)
                .HasForeignKey(d => d.IdType)
                .HasConstraintName("pokemontype_id_type_fkey");
        });

        modelBuilder.Entity<Rating>(entity =>
        {
            entity.HasKey(e => e.IdUserrating).HasName("rating_pkey");

            entity.ToTable("rating");

            entity.Property(e => e.IdUserrating).HasColumnName("id_userrating");
            entity.Property(e => e.PokemonId).HasColumnName("pokemon_id");
            entity.Property(e => e.Rating1).HasColumnName("rating");
            entity.Property(e => e.Ratingdate).HasColumnName("ratingdate");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Pokemon).WithMany(p => p.Ratings)
                .HasForeignKey(d => d.PokemonId)
                .HasConstraintName("rating_pokemon_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Ratings)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("rating_user_id_fkey");
        });

        modelBuilder.Entity<Stat>(entity =>
        {
            entity.HasKey(e => e.IdStats).HasName("stats_pkey");

            entity.ToTable("stats");

            entity.Property(e => e.IdStats).HasColumnName("id_stats");
            entity.Property(e => e.Attack).HasColumnName("attack");
            entity.Property(e => e.Chancetocache).HasColumnName("chancetocache");
            entity.Property(e => e.Eggcycle).HasColumnName("eggcycle");
            entity.Property(e => e.Exp).HasColumnName("exp_");
            entity.Property(e => e.Health).HasColumnName("health");
            entity.Property(e => e.Protection).HasColumnName("protection");
            entity.Property(e => e.Speed).HasColumnName("speed");
        });

        modelBuilder.Entity<TypePokemon>(entity =>
        {
            entity.HasKey(e => e.IdType).HasName("type_pokemons_pkey");

            entity.ToTable("type_pokemons");

            entity.Property(e => e.IdType).HasColumnName("id_type");
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .HasColumnName("type_");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("users_pkey");

            entity.ToTable("users");

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Password).HasColumnName("password_");
            entity.Property(e => e.Role)
                .HasColumnType("character varying")
                .HasColumnName("role_");
            entity.Property(e => e.Username)
                .HasColumnType("character varying")
                .HasColumnName("username");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
