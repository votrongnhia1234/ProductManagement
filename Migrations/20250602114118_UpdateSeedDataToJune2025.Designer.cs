﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ProductManagement.Data;

#nullable disable

namespace ProductManagement.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250602114118_UpdateSeedDataToJune2025")]
    partial class UpdateSeedDataToJune2025
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);

                    b.HasData(
                        new
                        {
                            Id = "1",
                            Name = "Admin",
                            NormalizedName = "ADMIN"
                        },
                        new
                        {
                            Id = "2",
                            Name = "Manager",
                            NormalizedName = "MANAGER"
                        },
                        new
                        {
                            Id = "3",
                            Name = "Customer",
                            NormalizedName = "CUSTOMER"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("ProductManagement.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("Address")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastLoginAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("ProductManagement.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CreatedAt = new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Thiết bị và tiện ích điện tử",
                            Name = "Điện tử",
                            UpdatedAt = new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 2,
                            CreatedAt = new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Nhiều loại sách và tiểu thuyết",
                            Name = "Sách",
                            UpdatedAt = new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 3,
                            CreatedAt = new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Quần áo và các mặt hàng thời trang",
                            Name = "Thời trang",
                            UpdatedAt = new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 4,
                            CreatedAt = new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Đồ dùng cho nhà cửa và sân vườn",
                            Name = "Nhà cửa & Vườn",
                            UpdatedAt = new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        });
                });

            modelBuilder.Entity("ProductManagement.Models.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("DeliveredDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Notes")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<DateTime>("OrderDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ShippedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ShippingAddress")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<decimal>("TotalAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("TrackingNumber")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("ProductManagement.Models.OrderItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("OrderId")
                        .HasColumnType("int");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.HasIndex("ProductId");

                    b.ToTable("OrderItems");
                });

            modelBuilder.Entity("ProductManagement.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<string>("ImgUrl")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("ProductName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Products");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CategoryId = 1,
                            CreatedAt = new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Máy tính xách tay hiệu năng cao",
                            ImgUrl = "https://via.placeholder.com/300x200?text=Laptop",
                            Price = 1200.00m,
                            ProductName = "Máy tính xách tay",
                            UpdatedAt = new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 2,
                            CategoryId = 1,
                            CreatedAt = new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Điện thoại thông minh mẫu mới nhất",
                            ImgUrl = "https://via.placeholder.com/300x200?text=Smartphone",
                            Price = 800.00m,
                            ProductName = "Điện thoại thông minh",
                            UpdatedAt = new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 3,
                            CategoryId = 2,
                            CreatedAt = new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Tiểu thuyết giả tưởng sử thi",
                            ImgUrl = "https://via.placeholder.com/300x200?text=Book",
                            Price = 20.00m,
                            ProductName = "Chúa tể những chiếc nhẫn",
                            UpdatedAt = new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 4,
                            CategoryId = 2,
                            CreatedAt = new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Khoa học viễn tưởng kinh điển",
                            ImgUrl = "https://via.placeholder.com/300x200?text=Book",
                            Price = 15.00m,
                            ProductName = "Xứ Cát",
                            UpdatedAt = new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 5,
                            CategoryId = 3,
                            CreatedAt = new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Áo phông cotton thoải mái",
                            ImgUrl = "https://via.placeholder.com/300x200?text=T-Shirt",
                            Price = 25.00m,
                            ProductName = "Áo phông",
                            UpdatedAt = new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 6,
                            CategoryId = 3,
                            CreatedAt = new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Quần jean vải bền",
                            ImgUrl = "https://via.placeholder.com/300x200?text=Jeans",
                            Price = 50.00m,
                            ProductName = "Quần jean",
                            UpdatedAt = new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 7,
                            CategoryId = 1,
                            CreatedAt = new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Máy ảnh độ phân giải cao",
                            ImgUrl = "https://via.placeholder.com/300x200?text=Camera",
                            Price = 650.00m,
                            ProductName = "Máy ảnh Kỹ thuật số",
                            UpdatedAt = new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 8,
                            CategoryId = 1,
                            CreatedAt = new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Tai nghe không dây chống ồn",
                            ImgUrl = "https://via.placeholder.com/300x200?text=Headphones",
                            Price = 150.00m,
                            ProductName = "Tai nghe Bluetooth",
                            UpdatedAt = new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 9,
                            CategoryId = 1,
                            CreatedAt = new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Màn hình Full HD 27 inch",
                            ImgUrl = "https://via.placeholder.com/300x200?text=Monitor",
                            Price = 300.00m,
                            ProductName = "Màn hình máy tính",
                            UpdatedAt = new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 10,
                            CategoryId = 1,
                            CreatedAt = new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Bàn phím cơ với đèn nền RGB",
                            ImgUrl = "https://via.placeholder.com/300x200?text=Keyboard",
                            Price = 100.00m,
                            ProductName = "Bàn phím cơ",
                            UpdatedAt = new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 11,
                            CategoryId = 1,
                            CreatedAt = new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Chuột quang không dây",
                            ImgUrl = "https://via.placeholder.com/300x200?text=Mouse",
                            Price = 25.00m,
                            ProductName = "Chuột không dây",
                            UpdatedAt = new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 12,
                            CategoryId = 2,
                            CreatedAt = new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Câu chuyện hấp dẫn đầy bí ẩn",
                            ImgUrl = "https://via.placeholder.com/300x200?text=Mystery+Book",
                            Price = 18.00m,
                            ProductName = "Tiểu thuyết trinh thám",
                            UpdatedAt = new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 13,
                            CategoryId = 2,
                            CreatedAt = new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Tuyển tập công thức món ngon",
                            ImgUrl = "https://via.placeholder.com/300x200?text=Cookbook",
                            Price = 30.00m,
                            ProductName = "Sách nấu ăn",
                            UpdatedAt = new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 14,
                            CategoryId = 2,
                            CreatedAt = new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Tìm hiểu về lịch sử hào hùng",
                            ImgUrl = "https://via.placeholder.com/300x200?text=History+Book",
                            Price = 25.00m,
                            ProductName = "Lịch sử Việt Nam",
                            UpdatedAt = new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 15,
                            CategoryId = 2,
                            CreatedAt = new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Bộ sưu tập truyện tranh vui nhộn",
                            ImgUrl = "https://via.placeholder.com/300x200?text=Comic+Book",
                            Price = 10.00m,
                            ProductName = "Truyện tranh",
                            UpdatedAt = new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 16,
                            CategoryId = 3,
                            CreatedAt = new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Váy thời trang cho nữ",
                            ImgUrl = "https://via.placeholder.com/300x200?text=Dress",
                            Price = 40.00m,
                            ProductName = "Váy đầm",
                            UpdatedAt = new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 17,
                            CategoryId = 3,
                            CreatedAt = new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Áo sơ mi công sở lịch lãm",
                            ImgUrl = "https://via.placeholder.com/300x200?text=Shirt",
                            Price = 35.00m,
                            ProductName = "Áo sơ mi",
                            UpdatedAt = new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 18,
                            CategoryId = 3,
                            CreatedAt = new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Giày thoải mái cho hoạt động thể thao",
                            ImgUrl = "https://via.placeholder.com/300x200?text=Sneakers",
                            Price = 70.00m,
                            ProductName = "Giày thể thao",
                            UpdatedAt = new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 19,
                            CategoryId = 4,
                            CreatedAt = new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Bàn làm việc gỗ hiện đại",
                            ImgUrl = "https://via.placeholder.com/300x200?text=Desk",
                            Price = 150.00m,
                            ProductName = "Bàn làm việc",
                            UpdatedAt = new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 20,
                            CategoryId = 4,
                            CreatedAt = new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Ghế xoay thoải mái",
                            ImgUrl = "https://via.placeholder.com/300x200?text=Office+Chair",
                            Price = 80.00m,
                            ProductName = "Ghế văn phòng",
                            UpdatedAt = new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 21,
                            CategoryId = 4,
                            CreatedAt = new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Đèn LED tiết kiệm điện",
                            ImgUrl = "https://via.placeholder.com/300x200?text=Desk+Lamp",
                            Price = 30.00m,
                            ProductName = "Đèn bàn",
                            UpdatedAt = new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 22,
                            CategoryId = 4,
                            CreatedAt = new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Cây xanh trang trí bàn làm việc",
                            ImgUrl = "https://via.placeholder.com/300x200?text=Plant",
                            Price = 10.00m,
                            ProductName = "Cây cảnh mini",
                            UpdatedAt = new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("ProductManagement.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("ProductManagement.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProductManagement.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("ProductManagement.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ProductManagement.Models.Order", b =>
                {
                    b.HasOne("ProductManagement.Models.ApplicationUser", "User")
                        .WithMany("Orders")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("ProductManagement.Models.OrderItem", b =>
                {
                    b.HasOne("ProductManagement.Models.Order", "Order")
                        .WithMany("OrderItems")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProductManagement.Models.Product", "Product")
                        .WithMany("OrderItems")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Order");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("ProductManagement.Models.Product", b =>
                {
                    b.HasOne("ProductManagement.Models.Category", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("ProductManagement.Models.ApplicationUser", b =>
                {
                    b.Navigation("Orders");
                });

            modelBuilder.Entity("ProductManagement.Models.Category", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("ProductManagement.Models.Order", b =>
                {
                    b.Navigation("OrderItems");
                });

            modelBuilder.Entity("ProductManagement.Models.Product", b =>
                {
                    b.Navigation("OrderItems");
                });
#pragma warning restore 612, 618
        }
    }
}
