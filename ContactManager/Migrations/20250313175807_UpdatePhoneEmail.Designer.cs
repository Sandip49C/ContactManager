﻿// <auto-generated />
using ContactManager.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ContactManager.Migrations
{
    [DbContext(typeof(ContactContext))]
    [Migration("20250313175807_UpdatePhoneEmail")]
    partial class UpdatePhoneEmail
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("ContactManager.Models.Address", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("ContactId")
                        .HasColumnType("int");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("ZipCode")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("ContactId");

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("ContactManager.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Family"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Friends"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Work"
                        });
                });

            modelBuilder.Entity("ContactManager.Models.Contact", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Contacts");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CategoryId = 1,
                            Name = "John Doe"
                        },
                        new
                        {
                            Id = 2,
                            CategoryId = 2,
                            Name = "Jane Smith"
                        });
                });

            modelBuilder.Entity("ContactManager.Models.Email", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("ContactId")
                        .HasColumnType("int");

                    b.Property<string>("EmailAddress")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("IsPrimary")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("Id");

                    b.HasIndex("ContactId");

                    b.ToTable("Emails");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ContactId = 1,
                            EmailAddress = "john.doe@example.com",
                            IsPrimary = true
                        },
                        new
                        {
                            Id = 2,
                            ContactId = 2,
                            EmailAddress = "jane.smith@example.com",
                            IsPrimary = true
                        });
                });

            modelBuilder.Entity("ContactManager.Models.Group", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("ContactManager.Models.GroupMember", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("ContactId")
                        .HasColumnType("int");

                    b.Property<int>("GroupId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ContactId");

                    b.HasIndex("GroupId");

                    b.ToTable("GroupMembers");
                });

            modelBuilder.Entity("ContactManager.Models.Note", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("ContactId")
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("ContactId");

                    b.ToTable("Notes");
                });

            modelBuilder.Entity("ContactManager.Models.Phone", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("ContactId")
                        .HasColumnType("int");

                    b.Property<bool>("IsPrimary")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("ContactId");

                    b.ToTable("Phones");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ContactId = 1,
                            IsPrimary = true,
                            PhoneNumber = "123-456-7890"
                        },
                        new
                        {
                            Id = 2,
                            ContactId = 2,
                            IsPrimary = true,
                            PhoneNumber = "987-654-3210"
                        });
                });

            modelBuilder.Entity("ContactManager.Models.Address", b =>
                {
                    b.HasOne("ContactManager.Models.Contact", "Contact")
                        .WithMany("Addresses")
                        .HasForeignKey("ContactId");

                    b.Navigation("Contact");
                });

            modelBuilder.Entity("ContactManager.Models.Contact", b =>
                {
                    b.HasOne("ContactManager.Models.Category", "Category")
                        .WithMany("Contacts")
                        .HasForeignKey("CategoryId");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("ContactManager.Models.Email", b =>
                {
                    b.HasOne("ContactManager.Models.Contact", "Contact")
                        .WithMany("Emails")
                        .HasForeignKey("ContactId");

                    b.Navigation("Contact");
                });

            modelBuilder.Entity("ContactManager.Models.GroupMember", b =>
                {
                    b.HasOne("ContactManager.Models.Contact", "Contact")
                        .WithMany("GroupMembers")
                        .HasForeignKey("ContactId");

                    b.HasOne("ContactManager.Models.Group", "Group")
                        .WithMany("GroupMembers")
                        .HasForeignKey("GroupId");

                    b.Navigation("Contact");

                    b.Navigation("Group");
                });

            modelBuilder.Entity("ContactManager.Models.Note", b =>
                {
                    b.HasOne("ContactManager.Models.Contact", "Contact")
                        .WithMany("Notes")
                        .HasForeignKey("ContactId");

                    b.Navigation("Contact");
                });

            modelBuilder.Entity("ContactManager.Models.Phone", b =>
                {
                    b.HasOne("ContactManager.Models.Contact", "Contact")
                        .WithMany("Phones")
                        .HasForeignKey("ContactId");

                    b.Navigation("Contact");
                });

            modelBuilder.Entity("ContactManager.Models.Category", b =>
                {
                    b.Navigation("Contacts");
                });

            modelBuilder.Entity("ContactManager.Models.Contact", b =>
                {
                    b.Navigation("Addresses");

                    b.Navigation("Emails");

                    b.Navigation("GroupMembers");

                    b.Navigation("Notes");

                    b.Navigation("Phones");
                });

            modelBuilder.Entity("ContactManager.Models.Group", b =>
                {
                    b.Navigation("GroupMembers");
                });
#pragma warning restore 612, 618
        }
    }
}
