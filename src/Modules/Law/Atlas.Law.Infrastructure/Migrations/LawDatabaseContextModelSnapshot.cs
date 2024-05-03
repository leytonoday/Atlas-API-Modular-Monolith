﻿// <auto-generated />
using System;
using Atlas.Law.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Atlas.Law.Infrastructure.Migrations
{
    [DbContext(typeof(LawDatabaseContext))]
    partial class LawDatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("Law")
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Atlas.Law.Domain.Entities.EurLexSumDocumentEntity.EurLexSumDocument", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CelexId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreatedOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("Keywords")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Language")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Reference")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Summary")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedOnUtc")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("CelexId", "Language")
                        .IsUnique();

                    b.ToTable("EurLexSumDocuments", "Law");
                });

            modelBuilder.Entity("Atlas.Law.Domain.Entities.LegalDocumentEntity.LegalDocument", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("FullText")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Language")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MimeType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("LegalDocuments", "Law");
                });

            modelBuilder.Entity("Atlas.Law.Domain.Entities.LegalDocumentSummaryEntity.LegalDocumentSummary", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("Keywords")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("LegalDocumentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("ProcessingStatus")
                        .HasColumnType("int");

                    b.Property<string>("SummarisedText")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SummarizedTitle")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedOnUtc")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("LegalDocumentId")
                        .IsUnique();

                    b.ToTable("LegalDocumentSummaries", "Law");
                });

            modelBuilder.Entity("Atlas.Shared.Application.Queue.QueueMessage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Data")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Error")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("OccurredOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ProcessedOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("QueueMessages", "Law");
                });

            modelBuilder.Entity("Atlas.Shared.Infrastructure.Integration.Inbox.InboxMessage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Data")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("OccurredOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ProcessedOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("PublishError")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("InboxMessages", "Law");
                });

            modelBuilder.Entity("Atlas.Shared.Infrastructure.Integration.Inbox.InboxMessageHandlerAcknowledgement", b =>
                {
                    b.Property<string>("HandlerName")
                        .HasColumnType("nvarchar(450)");

                    b.Property<Guid>("InboxMessageId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("HandlerName", "InboxMessageId");

                    b.ToTable("InboxMessageHandlerAcknowledgements", "Law");
                });

            modelBuilder.Entity("Atlas.Shared.Infrastructure.Integration.Outbox.OutboxMessage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Data")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("OccurredOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ProcessedOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("PublishError")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("OutboxMessages", "Law");
                });

            modelBuilder.Entity("Atlas.Shared.Infrastructure.Queue.QueueMessageHandlerAcknowledgement", b =>
                {
                    b.Property<string>("HandlerName")
                        .HasColumnType("nvarchar(450)");

                    b.Property<Guid>("QueuedCommandId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("HandlerName", "QueuedCommandId");

                    b.ToTable("QueueMessageHandlerAcknowledgements", "Law");
                });

            modelBuilder.Entity("Atlas.Law.Domain.Entities.LegalDocumentSummaryEntity.LegalDocumentSummary", b =>
                {
                    b.HasOne("Atlas.Law.Domain.Entities.LegalDocumentEntity.LegalDocument", "LegalDocument")
                        .WithOne("Summary")
                        .HasForeignKey("Atlas.Law.Domain.Entities.LegalDocumentSummaryEntity.LegalDocumentSummary", "LegalDocumentId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("LegalDocument");
                });

            modelBuilder.Entity("Atlas.Law.Domain.Entities.LegalDocumentEntity.LegalDocument", b =>
                {
                    b.Navigation("Summary");
                });
#pragma warning restore 612, 618
        }
    }
}
