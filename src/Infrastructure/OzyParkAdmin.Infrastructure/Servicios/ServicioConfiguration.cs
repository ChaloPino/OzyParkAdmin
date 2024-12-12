﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OzyParkAdmin.Domain.GruposEtarios;
using OzyParkAdmin.Domain.Servicios;

namespace OzyParkAdmin.Infrastructure.Servicios;
internal sealed class ServicioConfiguration : IEntityTypeConfiguration<Servicio>
{
    public void Configure(EntityTypeBuilder<Servicio> builder)
    {
        builder.ToTable("tkt_Servicios_td");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("ServicioId").ValueGeneratedNever();

        builder.HasOne(x => x.CentroCosto)
            .WithMany()
            .HasForeignKey("CentroCostoId");

        builder.Navigation(x => x.CentroCosto).AutoInclude();

        builder.HasOne(x => x.TipoControl)
            .WithMany()
            .HasForeignKey("TipoControlId");
        builder.Navigation(x => x.TipoControl).AutoInclude();

        builder.HasOne(x => x.TipoDistribucion)
            .WithMany()
            .HasForeignKey("TipoDistribucionId");
        builder.Navigation(x => x.TipoDistribucion).AutoInclude();

        builder.HasOne(x => x.TipoVigencia)
            .WithMany()
            .HasForeignKey("TipoVigenciaId");
        builder.Navigation(x => x.TipoVigencia).AutoInclude();

        builder.OwnsMany<TramoServicio>("_tramosServicio", navBuilder =>
        {
            navBuilder.ToTable("tkt_ServiciosPorTramo_td");
            navBuilder.HasKey("CentroCostoId", "ServicioId", "TramoId");
            navBuilder.HasOne(x => x.CentroCosto).WithMany().HasForeignKey("CentroCostoId");
            navBuilder.Navigation(x => x.CentroCosto).AutoInclude();
            navBuilder.HasOne(x => x.Tramo).WithMany().HasForeignKey("TramoId");
            navBuilder.Navigation(x => x.Tramo).AutoInclude();
            navBuilder.WithOwner().HasForeignKey("ServicioId");
        });

        builder.OwnsMany<CentroCostoServicio>("_centrosCosto", navBuilder =>
        {
            navBuilder.ToTable("tkt_ServiciosPorCentroCosto_td");
            navBuilder.HasKey("CentroCostoId", "ServicioId");
            navBuilder.HasOne(x => x.CentroCosto).WithMany().HasForeignKey("CentroCostoId");
            navBuilder.Navigation(x => x.CentroCosto).AutoInclude();
            navBuilder.WithOwner().HasForeignKey("ServicioId");
        });

        builder.HasMany(x => x.GruposEtarios).WithMany().UsingEntity(
            "tkt_ServiciosPorGruposEtarios_td",
            l => l.HasOne(typeof(GrupoEtario)).WithMany().HasForeignKey("GrupoEtarioId"),
            r => r.HasOne(typeof(Servicio)).WithMany().HasForeignKey("ServicioId"),
            j => j.HasKey("ServicioId", "GrupoEtarioId"));
        builder.Navigation(x => x.GruposEtarios).AutoInclude();

        builder.OwnsMany<ServicioPorCaja>("_serviciosPorCaja", navBuilder =>
        {
            navBuilder.ToTable("tkt_ServiciosPorCaja_td");
            navBuilder.HasKey("CajaId", "ServicioId");
            navBuilder.HasOne(x => x.Caja).WithMany().HasForeignKey("CajaId");
            navBuilder.Navigation(x => x.Caja).AutoInclude();
            navBuilder.WithOwner().HasForeignKey("ServicioId");
        });

        builder.OwnsOne(x => x.Movil, navBuilder =>
        {
            navBuilder.ToTable("tkt_ServiciosMoviles_td");
            navBuilder.HasKey("ServicioId");
            navBuilder.WithOwner().HasForeignKey("ServicioId");
        });

        builder.OwnsOne(x => x.Bus, navBuilder =>
        {
            navBuilder.ToTable("tkt_ServiciosBuses_td");
            navBuilder.HasKey("ServicioId");
            navBuilder.WithOwner().HasForeignKey("ServicioId");
        }); 

        builder.OwnsOne<ServicioPolitica>("_servicioPolitica", navBuilder =>
        {
            navBuilder.ToTable("tkt_ServiciosPoliticas_td");
            navBuilder.HasKey("ServicioId");
            navBuilder.WithOwner().HasForeignKey("ServicioId");
        });

        builder.OwnsOne<ServicioControlParental>("_servicioControlParental", navBuilder =>
        {
            navBuilder.ToTable("tkt_ServiciosControlParental_td");
            navBuilder.HasKey("ServicioId");
            navBuilder.WithOwner().HasForeignKey("ServicioId");
        });

        builder.OwnsMany(x => x.Permisos, navBuilder =>
        {
            navBuilder.ToTable("tkt_PermisoServicios_td");
            navBuilder.HasKey("ServicioId", "TramoId", "CentroCostoId");
            navBuilder.HasOne(x => x.Tramo).WithMany().HasForeignKey("TramoId");
            navBuilder.Navigation(x => x.Tramo).AutoInclude();
            navBuilder.HasOne(x => x.CentroCosto).WithMany().HasForeignKey("CentroCostoId");
            navBuilder.Navigation(x => x.CentroCosto).AutoInclude();
            navBuilder.WithOwner().HasForeignKey("ServicioId");
        });

        builder.Ignore(x => x.Tramos);
        builder.Ignore(x => x.Cajas);
        builder.Ignore(x => x.Politicas);
    }
}
