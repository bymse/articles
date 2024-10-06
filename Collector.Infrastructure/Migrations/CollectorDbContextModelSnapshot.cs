﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Collector.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Collector.Infrastructure.Migrations
{
    [DbContext(typeof(CollectorDbContext))]
    partial class CollectorDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Collector.Application.Entities.Mailbox", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("character varying(26)")
                        .HasColumnName("id");

                    b.Property<long?>("LastUid")
                        .HasColumnType("bigint")
                        .HasColumnName("last_uid");

                    b.Property<long?>("UidValidity")
                        .HasColumnType("bigint")
                        .HasColumnName("uid_validity");

                    b.HasKey("Id")
                        .HasName("pk_mailbox");

                    b.ToTable("mailbox", (string)null);
                });

            modelBuilder.Entity("Collector.Application.Entities.ReceivedEmail", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("character varying(26)")
                        .HasColumnName("id");

                    b.Property<string>("FromEmail")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("from_email");

                    b.Property<string>("HtmlBody")
                        .HasColumnType("text")
                        .HasColumnName("html_body");

                    b.Property<string>("MailboxId")
                        .IsRequired()
                        .HasColumnType("character varying(26)")
                        .HasColumnName("mailbox_id");

                    b.Property<DateTimeOffset>("ReceivedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("received_at");

                    b.Property<string>("Subject")
                        .HasColumnType("text")
                        .HasColumnName("subject");

                    b.Property<string>("TextBody")
                        .HasColumnType("text")
                        .HasColumnName("text_body");

                    b.Property<string>("ToEmail")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("to_email");

                    b.Property<long>("Uid")
                        .HasColumnType("bigint")
                        .HasColumnName("uid");

                    b.Property<long>("UidValidity")
                        .HasColumnType("bigint")
                        .HasColumnName("uid_validity");

                    b.HasKey("Id")
                        .HasName("pk_received_email");

                    b.HasIndex("MailboxId")
                        .HasDatabaseName("ix_received_email_mailbox_id");

                    b.ToTable("received_email", (string)null);
                });

            modelBuilder.Entity("Collector.Application.Entities.Source", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("character varying(26)")
                        .HasColumnName("id");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<int>("State")
                        .HasColumnType("integer")
                        .HasColumnName("state");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("title");

                    b.Property<string>("WebPage")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("web_page");

                    b.ComplexProperty<Dictionary<string, object>>("Tenant", "Collector.Application.Entities.Source.Tenant#Tenant", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Id")
                                .IsRequired()
                                .HasColumnType("character varying(26)")
                                .HasColumnName("tenant_id");

                            b1.Property<int>("Type")
                                .HasColumnType("integer")
                                .HasColumnName("tenant_type");
                        });

                    b.HasKey("Id")
                        .HasName("pk_sources");

                    b.ToTable("sources", (string)null);

                    b.HasDiscriminator<int>("State");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Collector.Application.Entities.ConfirmedSource", b =>
                {
                    b.HasBaseType("Collector.Application.Entities.Source");

                    b.Property<DateTimeOffset>("ConfirmedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("confirmed_at");

                    b.ToTable("sources", (string)null);

                    b.HasDiscriminator().HasValue(1);
                });

            modelBuilder.Entity("Collector.Application.Entities.UnconfirmedSource", b =>
                {
                    b.HasBaseType("Collector.Application.Entities.Source");

                    b.ToTable("sources", (string)null);

                    b.HasDiscriminator().HasValue(0);
                });

            modelBuilder.Entity("Collector.Application.Entities.ReceivedEmail", b =>
                {
                    b.HasOne("Collector.Application.Entities.Mailbox", null)
                        .WithMany()
                        .HasForeignKey("MailboxId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_received_email_mailbox_mailbox_id");
                });

            modelBuilder.Entity("Collector.Application.Entities.Source", b =>
                {
                    b.OwnsOne("Collector.Application.Entities.Receiver", "Receiver", b1 =>
                        {
                            b1.Property<string>("SourceId")
                                .HasColumnType("character varying(26)")
                                .HasColumnName("id");

                            b1.Property<string>("Email")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("receiver_email");

                            b1.HasKey("SourceId");

                            b1.HasIndex("Email")
                                .IsUnique()
                                .HasDatabaseName("ix_sources_receiver_email");

                            b1.ToTable("sources");

                            b1.WithOwner()
                                .HasForeignKey("SourceId")
                                .HasConstraintName("fk_sources_sources_id");
                        });

                    b.Navigation("Receiver")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
