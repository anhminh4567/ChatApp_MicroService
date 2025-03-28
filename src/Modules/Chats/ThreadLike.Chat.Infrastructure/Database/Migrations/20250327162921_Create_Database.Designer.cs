﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using ThreadLike.Chat.Infrastructure.Database;

#nullable disable

namespace ThreadLike.Chat.Infrastructure.Database.Migrations
{
    [DbContext(typeof(ChatDbContext))]
    [Migration("20250327162921_Create_Database")]
    partial class Create_Database
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("chat")
                .HasAnnotation("ProductVersion", "8.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ThreadLike.Chat.Domain.Group.Entities.UserGroupRole", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<Guid>("GroupId")
                        .HasColumnType("uuid");

                    b.Property<string>("RoleName")
                        .HasColumnType("text");

                    b.HasKey("UserId", "GroupId", "RoleName");

                    b.HasIndex("GroupId");

                    b.HasIndex("RoleName");

                    b.ToTable("UserGroupRoles", "chat");
                });

            modelBuilder.Entity("ThreadLike.Chat.Domain.GroupMessages.Entities.GroupMessageReaction", b =>
                {
                    b.Property<Guid>("MesssageId")
                        .HasColumnType("uuid");

                    b.Property<string>("ReactionId")
                        .HasColumnType("text");

                    b.Property<Guid?>("GroupMessageId")
                        .HasColumnType("uuid");

                    b.Property<List<string>>("ReactorIds")
                        .IsRequired()
                        .HasColumnType("text[]");

                    b.HasKey("MesssageId", "ReactionId");

                    b.HasIndex("GroupMessageId");

                    b.HasIndex("ReactionId");

                    b.ToTable("GroupMessageReaction", "chat");
                });

            modelBuilder.Entity("ThreadLike.Chat.Domain.GroupMessages.GroupMessage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("GroupId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("ReferenceId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("RefrenceMessageId")
                        .HasColumnType("uuid");

                    b.Property<string>("SenderId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.HasIndex("ReferenceId");

                    b.HasIndex("RefrenceMessageId");

                    b.HasIndex("SenderId");

                    b.HasIndex("Id", "GroupId")
                        .IsUnique();

                    b.ToTable("GroupMessage", "chat");
                });

            modelBuilder.Entity("ThreadLike.Chat.Domain.GroupRoles.GroupRole", b =>
                {
                    b.Property<string>("Role")
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsDefault")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsSystem")
                        .HasColumnType("boolean");

                    b.HasKey("Role");

                    b.ToTable("GroupRoles", "chat");

                    b.HasData(
                        new
                        {
                            Role = "Guest",
                            Description = "Guest",
                            IsDefault = true,
                            IsSystem = true
                        },
                        new
                        {
                            Role = "GroupLeader",
                            Description = "GroupLeader",
                            IsDefault = true,
                            IsSystem = true
                        },
                        new
                        {
                            Role = "Member",
                            Description = "Member",
                            IsDefault = true,
                            IsSystem = true
                        });
                });

            modelBuilder.Entity("ThreadLike.Chat.Domain.Groups.Group", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreatorId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CreatorId");

                    b.ToTable("Groups", "chat");
                });

            modelBuilder.Entity("ThreadLike.Chat.Domain.Messages.Entities.MessageReaction", b =>
                {
                    b.Property<Guid>("MesssageId")
                        .HasColumnType("uuid");

                    b.Property<string>("ReactionId")
                        .HasColumnType("text");

                    b.Property<Guid?>("MessageId")
                        .HasColumnType("uuid");

                    b.Property<List<string>>("ReactorIds")
                        .IsRequired()
                        .HasColumnType("text[]");

                    b.HasKey("MesssageId", "ReactionId");

                    b.HasIndex("MessageId");

                    b.HasIndex("ReactionId");

                    b.ToTable("MessageReaction", "chat");
                });

            modelBuilder.Entity("ThreadLike.Chat.Domain.Messages.Message", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("ReceiverId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid?>("ReferenceId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("RefrenceMessageId")
                        .HasColumnType("uuid");

                    b.Property<string>("SenderId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("RefrenceMessageId");

                    b.HasIndex("SenderId");

                    b.HasIndex("UserId");

                    b.ToTable("Message", "chat");
                });

            modelBuilder.Entity("ThreadLike.Chat.Domain.Reactions.Reaction", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<bool>("IsSystem")
                        .HasColumnType("boolean");

                    b.Property<string>("MimeType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ReactionDescription")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Reactions", "chat");
                });

            modelBuilder.Entity("ThreadLike.Chat.Domain.Users.Entities.UserFriend", b =>
                {
                    b.Property<string>("FriendId")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.HasKey("FriendId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("UserFriends", "chat");
                });

            modelBuilder.Entity("ThreadLike.Chat.Domain.Users.Entities.UserLetter", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("ExpireTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsHtml")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsRead")
                        .HasColumnType("boolean");

                    b.Property<int>("LetterType")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("ProcessedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("ReceiverId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("SenderId")
                        .HasColumnType("text");

                    b.Property<DateTime>("SentAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("ReceiverId");

                    b.HasIndex("SenderId");

                    b.ToTable("UserLetters", "chat");
                });

            modelBuilder.Entity("ThreadLike.Chat.Domain.Users.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("AvatarUri")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("IdentityId")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<bool>("IsVerified")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("IdentityId")
                        .IsUnique();

                    b.ToTable("Users", "chat");
                });

            modelBuilder.Entity("ThreadLike.Common.Infrastructure.Inbox.InboxMessage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("jsonb");

                    b.Property<string>("Error")
                        .HasColumnType("text");

                    b.Property<DateTime>("OccurredOnUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("ProcessedOnUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("InboxMessages", "chat");
                });

            modelBuilder.Entity("ThreadLike.Common.Infrastructure.Inbox.InboxMessageConsumer", b =>
                {
                    b.Property<Guid>("InboxMessageId")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("InboxMessageId", "Name");

                    b.ToTable("InboxMessageConsumers", "chat");
                });

            modelBuilder.Entity("ThreadLike.Common.Infrastructure.Outbox.OutboxMessage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("jsonb");

                    b.Property<string>("Error")
                        .HasColumnType("text");

                    b.Property<DateTime>("OccurredOnUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("ProcessedOnUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("OutboxMessages", "chat");
                });

            modelBuilder.Entity("ThreadLike.Common.Infrastructure.Outbox.OutboxMessageConsumer", b =>
                {
                    b.Property<Guid>("OutboxMessageId")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("OutboxMessageId", "Name");

                    b.ToTable("OutboxMessageConsumers", "chat");
                });

            modelBuilder.Entity("ThreadLike.Chat.Domain.Group.Entities.UserGroupRole", b =>
                {
                    b.HasOne("ThreadLike.Chat.Domain.Groups.Group", null)
                        .WithMany("UserGroupRoles")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ThreadLike.Chat.Domain.GroupRoles.GroupRole", null)
                        .WithMany()
                        .HasForeignKey("RoleName")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired();

                    b.HasOne("ThreadLike.Chat.Domain.Users.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ThreadLike.Chat.Domain.GroupMessages.Entities.GroupMessageReaction", b =>
                {
                    b.HasOne("ThreadLike.Chat.Domain.GroupMessages.GroupMessage", null)
                        .WithMany("Reactions")
                        .HasForeignKey("GroupMessageId");

                    b.HasOne("ThreadLike.Chat.Domain.Reactions.Reaction", null)
                        .WithMany()
                        .HasForeignKey("ReactionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ThreadLike.Chat.Domain.GroupMessages.GroupMessage", b =>
                {
                    b.HasOne("ThreadLike.Chat.Domain.Groups.Group", null)
                        .WithMany("Messages")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ThreadLike.Chat.Domain.GroupMessages.GroupMessage", null)
                        .WithMany()
                        .HasForeignKey("ReferenceId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("ThreadLike.Chat.Domain.GroupMessages.GroupMessage", "RefrenceMessage")
                        .WithMany()
                        .HasForeignKey("RefrenceMessageId");

                    b.HasOne("ThreadLike.Chat.Domain.Users.User", null)
                        .WithMany("GroupMessages")
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("RefrenceMessage");
                });

            modelBuilder.Entity("ThreadLike.Chat.Domain.Groups.Group", b =>
                {
                    b.HasOne("ThreadLike.Chat.Domain.Users.User", "Creator")
                        .WithMany()
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Creator");
                });

            modelBuilder.Entity("ThreadLike.Chat.Domain.Messages.Entities.MessageReaction", b =>
                {
                    b.HasOne("ThreadLike.Chat.Domain.Messages.Message", null)
                        .WithMany("MessageReactions")
                        .HasForeignKey("MessageId");

                    b.HasOne("ThreadLike.Chat.Domain.Reactions.Reaction", null)
                        .WithMany()
                        .HasForeignKey("ReactionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ThreadLike.Chat.Domain.Messages.Message", b =>
                {
                    b.HasOne("ThreadLike.Chat.Domain.Messages.Message", "RefrenceMessage")
                        .WithMany()
                        .HasForeignKey("RefrenceMessageId");

                    b.HasOne("ThreadLike.Chat.Domain.Users.User", null)
                        .WithMany("PeerReceivedMessages")
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ThreadLike.Chat.Domain.Users.User", null)
                        .WithMany("PeerSendMessages")
                        .HasForeignKey("UserId");

                    b.Navigation("RefrenceMessage");
                });

            modelBuilder.Entity("ThreadLike.Chat.Domain.Users.Entities.UserFriend", b =>
                {
                    b.HasOne("ThreadLike.Chat.Domain.Users.User", null)
                        .WithMany()
                        .HasForeignKey("FriendId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("ThreadLike.Chat.Domain.Users.User", null)
                        .WithMany("Friends")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("ThreadLike.Chat.Domain.Users.Entities.UserLetter", b =>
                {
                    b.HasOne("ThreadLike.Chat.Domain.Users.User", null)
                        .WithMany("Letters")
                        .HasForeignKey("ReceiverId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ThreadLike.Chat.Domain.Users.User", null)
                        .WithMany()
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ThreadLike.Common.Infrastructure.Inbox.InboxMessageConsumer", b =>
                {
                    b.HasOne("ThreadLike.Common.Infrastructure.Inbox.InboxMessage", null)
                        .WithMany()
                        .HasForeignKey("InboxMessageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ThreadLike.Common.Infrastructure.Outbox.OutboxMessageConsumer", b =>
                {
                    b.HasOne("ThreadLike.Common.Infrastructure.Outbox.OutboxMessage", null)
                        .WithMany()
                        .HasForeignKey("OutboxMessageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ThreadLike.Chat.Domain.GroupMessages.GroupMessage", b =>
                {
                    b.Navigation("Reactions");
                });

            modelBuilder.Entity("ThreadLike.Chat.Domain.Groups.Group", b =>
                {
                    b.Navigation("Messages");

                    b.Navigation("UserGroupRoles");
                });

            modelBuilder.Entity("ThreadLike.Chat.Domain.Messages.Message", b =>
                {
                    b.Navigation("MessageReactions");
                });

            modelBuilder.Entity("ThreadLike.Chat.Domain.Users.User", b =>
                {
                    b.Navigation("Friends");

                    b.Navigation("GroupMessages");

                    b.Navigation("Letters");

                    b.Navigation("PeerReceivedMessages");

                    b.Navigation("PeerSendMessages");
                });
#pragma warning restore 612, 618
        }
    }
}
