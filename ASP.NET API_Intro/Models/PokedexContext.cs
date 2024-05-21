using Microsoft.EntityFrameworkCore;

namespace ASP.NET_API_Intro.Models;

public partial class PokedexContext : DbContext
{
    public PokedexContext()
    {
    }

    public PokedexContext(DbContextOptions<PokedexContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Pokemon> Pokemons { get; set; }

    public virtual DbSet<PokemonsTipo> PokemonsTipos { get; set; }

    public virtual DbSet<Tipo> Tipos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Pokemon>(entity =>
        {
            entity.HasKey(e => e.IdPokemon).HasName("PRIMARY");

            entity.ToTable("pokemons");

            entity.Property(e => e.IdPokemon)
                .HasColumnType("int(11)")
                .HasColumnName("id_pokemon");
            entity.Property(e => e.Descripcion)
                .HasColumnType("text")
                .HasColumnName("descripcion");
            entity.Property(e => e.N)
                .HasColumnType("int(11)")
                .HasColumnName("n°");
            entity.Property(e => e.Nombre)
                .HasMaxLength(30)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<PokemonsTipo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("pokemons_tipos");

            entity.HasIndex(e => e.IdPokemon, "Pokemons_Tipos_id_pokemon_Pokemons_id_pokemon");

            entity.HasIndex(e => e.IdTipo, "Pokemons_Tipos_id_tipo_Tipos_id_tipo");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.IdPokemon)
                .HasColumnType("int(11)")
                .HasColumnName("id_pokemon");
            entity.Property(e => e.IdTipo)
                .HasColumnType("int(11)")
                .HasColumnName("id_tipo");

            entity.HasOne(d => d.IdPokemonNavigation).WithMany(p => p.PokemonsTipos)
                .HasForeignKey(d => d.IdPokemon)
                .HasConstraintName("Pokemons_Tipos_id_pokemon_Pokemons_id_pokemon");

            entity.HasOne(d => d.IdTipoNavigation).WithMany(p => p.PokemonsTipos)
                .HasForeignKey(d => d.IdTipo)
                .HasConstraintName("Pokemons_Tipos_id_tipo_Tipos_id_tipo");
        });

        modelBuilder.Entity<Tipo>(entity =>
        {
            entity.HasKey(e => e.IdTipo).HasName("PRIMARY");

            entity.ToTable("tipos");

            entity.Property(e => e.IdTipo)
                .HasColumnType("int(11)")
                .HasColumnName("id_tipo");
            entity.Property(e => e.Nombre)
                .HasMaxLength(30)
                .HasColumnName("nombre");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
