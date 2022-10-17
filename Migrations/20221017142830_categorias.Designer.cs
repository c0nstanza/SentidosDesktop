﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using minimallAPI_rest;

#nullable disable

namespace minimallAPI_rest.Migrations
{
    [DbContext(typeof(RestauranteContext))]
    [Migration("20221017142830_categorias")]
    partial class categorias
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("minimallAPI_rest.modelos.Categoria", b =>
                {
                    b.Property<int>("CategoriaId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CategoriaId"), 1L, 1);

                    b.Property<string>("tipo_categoria")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CategoriaId");

                    b.ToTable("Categoria", (string)null);

                    b.HasData(
                        new
                        {
                            CategoriaId = 1,
                            tipo_categoria = "Cafe"
                        },
                        new
                        {
                            CategoriaId = 2,
                            tipo_categoria = "Te"
                        },
                        new
                        {
                            CategoriaId = 3,
                            tipo_categoria = "Platos Principales"
                        },
                        new
                        {
                            CategoriaId = 4,
                            tipo_categoria = "Factura"
                        },
                        new
                        {
                            CategoriaId = 5,
                            tipo_categoria = "Entradas"
                        },
                        new
                        {
                            CategoriaId = 6,
                            tipo_categoria = "Postres"
                        },
                        new
                        {
                            CategoriaId = 7,
                            tipo_categoria = "Bebidas"
                        },
                        new
                        {
                            CategoriaId = 8,
                            tipo_categoria = "Tragos"
                        });
                });

            modelBuilder.Entity("minimallAPI_rest.modelos.Cliente", b =>
                {
                    b.Property<string>("dni")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("apellido_cliente")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("nombre_cliente")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("dni");

                    b.ToTable("Cliente", (string)null);
                });

            modelBuilder.Entity("minimallAPI_rest.modelos.Pedido_Menu", b =>
                {
                    b.Property<Guid>("ID_PEDIDO_MENU")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ID_MENU")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("pedido_ID")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ID_PEDIDO_MENU");

                    b.HasIndex("ID_MENU");

                    b.HasIndex("pedido_ID");

                    b.ToTable("Pedido_Menu", (string)null);
                });

            modelBuilder.Entity("minimallAPI_rest.Properties.modelos.Empleado", b =>
                {
                    b.Property<Guid>("id_empleado")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("email_empleado")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("nickname_empleado")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("password_empleado")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("rol")
                        .HasColumnType("int");

                    b.HasKey("id_empleado");

                    b.ToTable("Empleado", (string)null);
                });

            modelBuilder.Entity("minimallAPI_rest.Properties.modelos.Factura", b =>
                {
                    b.Property<Guid>("id_factura")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<double>("Total")
                        .HasColumnType("float");

                    b.Property<Guid>("pedidoID")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("id_factura");

                    b.HasIndex("pedidoID")
                        .IsUnique();

                    b.ToTable(" Factura ", (string)null);
                });

            modelBuilder.Entity("minimallAPI_rest.Properties.modelos.Menu", b =>
                {
                    b.Property<Guid>("id_menu")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("comida")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("idcategoria")
                        .HasColumnType("int");

                    b.Property<double>("precio")
                        .HasColumnType("float");

                    b.HasKey("id_menu");

                    b.HasIndex("idcategoria");

                    b.ToTable("Menu", (string)null);

                    b.HasData(
                        new
                        {
                            id_menu = new Guid("0f860902-f798-4c59-a513-1ebd0b39bbe8"),
                            comida = "Medialunas",
                            idcategoria = 1,
                            precio = 100.0
                        });
                });

            modelBuilder.Entity("minimallAPI_rest.Properties.modelos.Mesa", b =>
                {
                    b.Property<Guid>("id_mesa")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("numero_mesa")
                        .HasColumnType("int");

                    b.HasKey("id_mesa");

                    b.ToTable("Mesa", (string)null);

                    b.HasData(
                        new
                        {
                            id_mesa = new Guid("fe2de405-c38e-4c90-ac52-da0540dfb4ef"),
                            numero_mesa = 1
                        },
                        new
                        {
                            id_mesa = new Guid("fe2de405-c38e-4c90-ac52-da0540dfb402"),
                            numero_mesa = 2
                        },
                        new
                        {
                            id_mesa = new Guid("fe2de405-c38e-4c90-ac52-da0540dfb403"),
                            numero_mesa = 3
                        },
                        new
                        {
                            id_mesa = new Guid("fe2de405-c38e-4c90-ac52-da0540dfb404"),
                            numero_mesa = 4
                        },
                        new
                        {
                            id_mesa = new Guid("fe2de405-c38e-4c90-ac52-da0540dfb405"),
                            numero_mesa = 5
                        },
                        new
                        {
                            id_mesa = new Guid("fe2de405-c38e-4c90-ac52-da0540dfb406"),
                            numero_mesa = 6
                        },
                        new
                        {
                            id_mesa = new Guid("fe2de405-c38e-4c90-ac52-da0540dfb407"),
                            numero_mesa = 7
                        },
                        new
                        {
                            id_mesa = new Guid("fe2de405-c38e-4c90-ac52-da0540dfb408"),
                            numero_mesa = 8
                        },
                        new
                        {
                            id_mesa = new Guid("fe2de405-c38e-4c90-ac52-da0540dfb409"),
                            numero_mesa = 9
                        });
                });

            modelBuilder.Entity("minimallAPI_rest.Properties.modelos.Metodo_Pago", b =>
                {
                    b.Property<int>("id_MetodoPago")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id_MetodoPago"), 1L, 1);

                    b.Property<string>("tipo_pago")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id_MetodoPago");

                    b.ToTable("Metodo_Pago", (string)null);

                    b.HasData(
                        new
                        {
                            id_MetodoPago = 1,
                            tipo_pago = "Efectivo"
                        },
                        new
                        {
                            id_MetodoPago = 2,
                            tipo_pago = "Tarjeta de Credito"
                        },
                        new
                        {
                            id_MetodoPago = 3,
                            tipo_pago = "Tarjeta de Debito"
                        });
                });

            modelBuilder.Entity("minimallAPI_rest.Properties.modelos.Pedido", b =>
                {
                    b.Property<Guid>("id_Pedido")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("fecha_pedido")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("hora_pedido")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("id_dni_cliente")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<Guid>("id_mesaa")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("metodo_pago_pedido")
                        .HasColumnType("int");

                    b.HasKey("id_Pedido");

                    b.HasIndex("id_dni_cliente");

                    b.HasIndex("id_mesaa");

                    b.HasIndex("metodo_pago_pedido");

                    b.ToTable("Pedido", (string)null);
                });

            modelBuilder.Entity("minimallAPI_rest.Properties.modelos.Reserva", b =>
                {
                    b.Property<Guid>("id_reservar")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ID_usuario")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("cantidad_personas")
                        .HasColumnType("int");

                    b.Property<bool>("confirmo_reserva")
                        .HasColumnType("bit");

                    b.Property<string>("fecha_de_reseva")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("hora_reserva")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("id_MESA")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("reservado")
                        .HasColumnType("bit");

                    b.HasKey("id_reservar");

                    b.HasIndex("ID_usuario");

                    b.HasIndex("id_MESA");

                    b.ToTable("Reservas", (string)null);
                });

            modelBuilder.Entity("minimallAPI_rest.Properties.modelos.Usuario", b =>
                {
                    b.Property<Guid>("id_usuario")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("dni")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("email")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("nickname")
                        .HasMaxLength(18)
                        .HasColumnType("nvarchar(18)");

                    b.Property<string>("password")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("id_usuario");

                    b.ToTable("usuario", (string)null);
                });

            modelBuilder.Entity("minimallAPI_rest.modelos.Pedido_Menu", b =>
                {
                    b.HasOne("minimallAPI_rest.Properties.modelos.Menu", "menu")
                        .WithMany("pedido_Menus")
                        .HasForeignKey("ID_MENU")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("minimallAPI_rest.Properties.modelos.Pedido", "pedidoss")
                        .WithMany("pedido_MenuS_pedidos")
                        .HasForeignKey("pedido_ID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("menu");

                    b.Navigation("pedidoss");
                });

            modelBuilder.Entity("minimallAPI_rest.Properties.modelos.Factura", b =>
                {
                    b.HasOne("minimallAPI_rest.Properties.modelos.Pedido", "pedido")
                        .WithOne("factura")
                        .HasForeignKey("minimallAPI_rest.Properties.modelos.Factura", "pedidoID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("pedido");
                });

            modelBuilder.Entity("minimallAPI_rest.Properties.modelos.Menu", b =>
                {
                    b.HasOne("minimallAPI_rest.modelos.Categoria", "categoria")
                        .WithMany("menus_categoria")
                        .HasForeignKey("idcategoria")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("categoria");
                });

            modelBuilder.Entity("minimallAPI_rest.Properties.modelos.Pedido", b =>
                {
                    b.HasOne("minimallAPI_rest.modelos.Cliente", "cliente")
                        .WithMany("pedidos")
                        .HasForeignKey("id_dni_cliente")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("minimallAPI_rest.Properties.modelos.Mesa", "mesa_pedido")
                        .WithMany("pedido_mesa")
                        .HasForeignKey("id_mesaa")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("minimallAPI_rest.Properties.modelos.Metodo_Pago", "metodo")
                        .WithMany("pedidos_metodos_pago")
                        .HasForeignKey("metodo_pago_pedido");

                    b.Navigation("cliente");

                    b.Navigation("mesa_pedido");

                    b.Navigation("metodo");
                });

            modelBuilder.Entity("minimallAPI_rest.Properties.modelos.Reserva", b =>
                {
                    b.HasOne("minimallAPI_rest.Properties.modelos.Usuario", "usuario")
                        .WithMany("reservas")
                        .HasForeignKey("ID_usuario");

                    b.HasOne("minimallAPI_rest.Properties.modelos.Mesa", "MESA")
                        .WithMany("reservas_mesas")
                        .HasForeignKey("id_MESA")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MESA");

                    b.Navigation("usuario");
                });

            modelBuilder.Entity("minimallAPI_rest.modelos.Categoria", b =>
                {
                    b.Navigation("menus_categoria");
                });

            modelBuilder.Entity("minimallAPI_rest.modelos.Cliente", b =>
                {
                    b.Navigation("pedidos");
                });

            modelBuilder.Entity("minimallAPI_rest.Properties.modelos.Menu", b =>
                {
                    b.Navigation("pedido_Menus");
                });

            modelBuilder.Entity("minimallAPI_rest.Properties.modelos.Mesa", b =>
                {
                    b.Navigation("pedido_mesa");

                    b.Navigation("reservas_mesas");
                });

            modelBuilder.Entity("minimallAPI_rest.Properties.modelos.Metodo_Pago", b =>
                {
                    b.Navigation("pedidos_metodos_pago");
                });

            modelBuilder.Entity("minimallAPI_rest.Properties.modelos.Pedido", b =>
                {
                    b.Navigation("factura")
                        .IsRequired();

                    b.Navigation("pedido_MenuS_pedidos");
                });

            modelBuilder.Entity("minimallAPI_rest.Properties.modelos.Usuario", b =>
                {
                    b.Navigation("reservas");
                });
#pragma warning restore 612, 618
        }
    }
}
