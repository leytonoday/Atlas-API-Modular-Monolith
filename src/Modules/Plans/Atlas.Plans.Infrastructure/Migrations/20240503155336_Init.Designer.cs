﻿// <auto-generated />
using System;
using Atlas.Plans.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Atlas.Plans.Infrastructure.Migrations
{
    [DbContext(typeof(PlansDatabaseContext))]
    [Migration("20240503155336_Init")]
    partial class Init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("Plans")
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Atlas.Plans.Domain.Entities.CreditTrackerEntity.CreditTracker", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<int>("CurrentCreditCount")
                        .HasColumnType("int");

                    b.Property<int>("MaxCreditCount")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedOnUtc")
                        .HasColumnType("datetime2");

                    b.HasKey("UserId");

                    b.ToTable("CreditTrackers", "Plans");
                });

            modelBuilder.Entity("Atlas.Plans.Domain.Entities.FeatureEntity.Feature", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsHidden")
                        .HasColumnType("bit");

                    b.Property<bool>("IsInheritable")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime?>("UpdatedOnUtc")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Features", "Plans");
                });

            modelBuilder.Entity("Atlas.Plans.Domain.Entities.PlanEntity.Plan", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<int>("AnnualPrice")
                        .HasColumnType("int");

                    b.Property<string>("BackgroundColour")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Icon")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IconColour")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("InheritsFromId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("IsoCurrencyCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("MonthlyPrice")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("StripeProductId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Tag")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TextColour")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TrialPeriodDays")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedOnUtc")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Plans", "Plans");
                });

            modelBuilder.Entity("Atlas.Plans.Domain.Entities.PlanFeatureEntity.PlanFeature", b =>
                {
                    b.Property<Guid>("FeatureId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("PlanId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("UpdatedOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("FeatureId", "PlanId");

                    b.HasIndex("PlanId");

                    b.ToTable("PlanFeatures", "Plans");
                });

            modelBuilder.Entity("Atlas.Plans.Domain.Entities.StripeCardFingerprintEntity.StripeCardFingerprint", b =>
                {
                    b.Property<string>("Fingerprint")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreatedOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("UpdatedOnUtc")
                        .HasColumnType("datetime2");

                    b.HasKey("Fingerprint");

                    b.ToTable("StripeCardFingerprints", "Plans");
                });

            modelBuilder.Entity("Atlas.Plans.Domain.Entities.StripeCustomerEntity.StripeCustomer", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("StripeCustomerId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreatedOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("UpdatedOnUtc")
                        .HasColumnType("datetime2");

                    b.HasKey("UserId", "StripeCustomerId");

                    b.ToTable("StripeCustomers", "Plans");
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

                    b.ToTable("QueueMessages", "Plans");
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

                    b.ToTable("InboxMessages", "Plans");
                });

            modelBuilder.Entity("Atlas.Shared.Infrastructure.Integration.Inbox.InboxMessageHandlerAcknowledgement", b =>
                {
                    b.Property<string>("HandlerName")
                        .HasColumnType("nvarchar(450)");

                    b.Property<Guid>("InboxMessageId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("HandlerName", "InboxMessageId");

                    b.ToTable("InboxMessageHandlerAcknowledgements", "Plans");
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

                    b.ToTable("OutboxMessages", "Plans");
                });

            modelBuilder.Entity("Atlas.Shared.Infrastructure.Queue.QueueMessageHandlerAcknowledgement", b =>
                {
                    b.Property<string>("HandlerName")
                        .HasColumnType("nvarchar(450)");

                    b.Property<Guid>("QueuedCommandId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("HandlerName", "QueuedCommandId");

                    b.ToTable("QueueMessageHandlerAcknowledgements", "Plans");
                });

            modelBuilder.Entity("Atlas.Plans.Domain.Entities.PlanFeatureEntity.PlanFeature", b =>
                {
                    b.HasOne("Atlas.Plans.Domain.Entities.FeatureEntity.Feature", null)
                        .WithMany()
                        .HasForeignKey("FeatureId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Atlas.Plans.Domain.Entities.PlanEntity.Plan", null)
                        .WithMany("PlanFeatures")
                        .HasForeignKey("PlanId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Atlas.Plans.Domain.Entities.PlanEntity.Plan", b =>
                {
                    b.Navigation("PlanFeatures");
                });
#pragma warning restore 612, 618
        }
    }
}
