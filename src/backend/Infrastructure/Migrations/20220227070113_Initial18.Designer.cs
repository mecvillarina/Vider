// <auto-generated />
using System;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20220227070113_Initial18")]
    partial class Initial18
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Domain.Entities.Creator", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("AccountAddress")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("AccountClassicAddress")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("AccountSecret")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("AccountXAddress")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("Bio")
                        .IsRequired()
                        .HasMaxLength(140)
                        .HasColumnType("nvarchar(140)");

                    b.Property<DateTime>("DateAccountAcquired")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateRegistered")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsAccountValid")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("ProfilePictureFilename")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Salt")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("UsernameNormalize")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("Id");

                    b.HasIndex("Username")
                        .IsUnique();

                    b.HasIndex("UsernameNormalize")
                        .IsUnique();

                    b.ToTable("Creators");
                });

            modelBuilder.Entity("Domain.Entities.CreatorPassword", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("CreatorId")
                        .HasColumnType("int");

                    b.Property<string>("Digest")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Salt")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("CreatorId");

                    b.ToTable("CreatorPasswords");
                });

            modelBuilder.Entity("Domain.Entities.CreatorProfile", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Bio")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateRegistered")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProfilePictureFilename")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SubscriberCount")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UsernameNormalize")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToView("CreatorProfiles");
                });

            modelBuilder.Entity("Domain.Entities.CreatorReward", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("CreatorId")
                        .HasColumnType("int");

                    b.Property<string>("Filename")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Message")
                        .HasMaxLength(512)
                        .HasColumnType("nvarchar(512)");

                    b.Property<string>("Name")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("Taxon")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CreatorId");

                    b.HasIndex("Taxon")
                        .IsUnique();

                    b.ToTable("CreatorRewards");
                });

            modelBuilder.Entity("Domain.Entities.CreatorSubscriber", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("CreatorId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateSubscribed")
                        .HasColumnType("datetime2");

                    b.Property<int?>("SubscriberId")
                        .HasColumnType("int");

                    b.Property<string>("TxHash")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("CreatorId");

                    b.HasIndex("SubscriberId");

                    b.HasIndex("TxHash")
                        .IsUnique();

                    b.ToTable("CreatorSubscribers");
                });

            modelBuilder.Entity("Domain.Entities.FeedPost", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Caption")
                        .IsRequired()
                        .HasMaxLength(140)
                        .HasColumnType("nvarchar(140)");

                    b.Property<DateTime>("DatePosted")
                        .HasColumnType("datetime2");

                    b.Property<string>("Filename")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<int>("PostedById")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PostedById");

                    b.ToTable("FeedPosts");
                });

            modelBuilder.Entity("Domain.Entities.FeedPostLike", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("DateOccured")
                        .HasColumnType("datetime2");

                    b.Property<int?>("LikedById")
                        .HasColumnType("int");

                    b.Property<int?>("PostId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("LikedById");

                    b.HasIndex("PostId");

                    b.ToTable("FeedPostLikes");
                });

            modelBuilder.Entity("Domain.Entities.NFTClaim", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Message")
                        .HasMaxLength(512)
                        .HasColumnType("nvarchar(512)");

                    b.Property<int?>("ReceiverId")
                        .HasColumnType("int");

                    b.Property<string>("SellOfferIndex")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("nvarchar(512)");

                    b.Property<int?>("SenderId")
                        .HasColumnType("int");

                    b.Property<string>("TokenId")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("TokenTaxon")
                        .HasColumnType("int");

                    b.Property<string>("Uri")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("UriHex")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("nvarchar(512)");

                    b.HasKey("Id");

                    b.HasIndex("ReceiverId");

                    b.HasIndex("SellOfferIndex");

                    b.HasIndex("SenderId");

                    b.HasIndex("TokenId");

                    b.HasIndex("Uri");

                    b.HasIndex("UriHex");

                    b.ToTable("NFTClaims");
                });

            modelBuilder.Entity("Domain.Entities.NFTIndex", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Metadata")
                        .IsRequired()
                        .HasMaxLength(2048)
                        .HasColumnType("nvarchar(2048)");

                    b.Property<int>("NftSerial")
                        .HasColumnType("int");

                    b.Property<int>("TokenFlags")
                        .HasColumnType("int");

                    b.Property<string>("TokenId")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("TokenTaxon")
                        .HasColumnType("int");

                    b.Property<string>("Uri")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("UriHex")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("nvarchar(512)");

                    b.HasKey("Id");

                    b.HasIndex("TokenId")
                        .IsUnique();

                    b.HasIndex("Uri")
                        .IsUnique();

                    b.HasIndex("UriHex")
                        .IsUnique();

                    b.ToTable("NFTIndexes");
                });

            modelBuilder.Entity("Domain.Entities.NFTSellOffer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Amount")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DatePosted")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsExclusiveForSubscribers")
                        .HasColumnType("bit");

                    b.Property<string>("SellOfferIndex")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("nvarchar(512)");

                    b.Property<int?>("SellerId")
                        .HasColumnType("int");

                    b.Property<string>("TokenId")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("TokenTaxon")
                        .HasColumnType("int");

                    b.Property<string>("Uri")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("UriHex")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("nvarchar(512)");

                    b.HasKey("Id");

                    b.HasIndex("SellOfferIndex");

                    b.HasIndex("SellerId");

                    b.HasIndex("TokenId");

                    b.HasIndex("Uri");

                    b.HasIndex("UriHex");

                    b.ToTable("NFTSellOffers");
                });

            modelBuilder.Entity("Domain.Entities.NFTSellOfferItem", b =>
                {
                    b.Property<int>("SellOfferId")
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SellOfferId"), 1L, 1);

                    b.Property<string>("Amount")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DatePosted")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsExclusiveForSubscribers")
                        .HasColumnType("bit");

                    b.Property<string>("NFTMetadata")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("NFTSerial")
                        .HasColumnType("int");

                    b.Property<int>("NFTTokenFlags")
                        .HasColumnType("int");

                    b.Property<string>("NFTTokenId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NFTUri")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NFTUriHex")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SellOfferIndex")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("SellerAccountValid")
                        .HasColumnType("bit");

                    b.Property<int>("SellerId")
                        .HasColumnType("int");

                    b.Property<string>("SellerProfilePictureFilename")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SellerUsername")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("SellOfferId");

                    b.ToView("NFTSellOfferView");
                });

            modelBuilder.Entity("Domain.Entities.TransactionLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Action")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("nvarchar(512)");

                    b.Property<int>("CreatorId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateOccured")
                        .HasColumnType("datetime2");

                    b.Property<string>("TxHash")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("CreatorId");

                    b.ToTable("TransactionLogs");
                });

            modelBuilder.Entity("Domain.Entities.CreatorPassword", b =>
                {
                    b.HasOne("Domain.Entities.Creator", "Creator")
                        .WithMany()
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Creator");
                });

            modelBuilder.Entity("Domain.Entities.CreatorReward", b =>
                {
                    b.HasOne("Domain.Entities.Creator", "Creator")
                        .WithMany()
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Creator");
                });

            modelBuilder.Entity("Domain.Entities.CreatorSubscriber", b =>
                {
                    b.HasOne("Domain.Entities.Creator", "Creator")
                        .WithMany()
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.Creator", "Subscriber")
                        .WithMany()
                        .HasForeignKey("SubscriberId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("Creator");

                    b.Navigation("Subscriber");
                });

            modelBuilder.Entity("Domain.Entities.FeedPost", b =>
                {
                    b.HasOne("Domain.Entities.Creator", "PostedBy")
                        .WithMany()
                        .HasForeignKey("PostedById")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PostedBy");
                });

            modelBuilder.Entity("Domain.Entities.FeedPostLike", b =>
                {
                    b.HasOne("Domain.Entities.Creator", "LikedBy")
                        .WithMany()
                        .HasForeignKey("LikedById");

                    b.HasOne("Domain.Entities.FeedPost", "Post")
                        .WithMany()
                        .HasForeignKey("PostId");

                    b.Navigation("LikedBy");

                    b.Navigation("Post");
                });

            modelBuilder.Entity("Domain.Entities.NFTClaim", b =>
                {
                    b.HasOne("Domain.Entities.Creator", "Receiver")
                        .WithMany()
                        .HasForeignKey("ReceiverId");

                    b.HasOne("Domain.Entities.Creator", "Sender")
                        .WithMany()
                        .HasForeignKey("SenderId");

                    b.Navigation("Receiver");

                    b.Navigation("Sender");
                });

            modelBuilder.Entity("Domain.Entities.NFTSellOffer", b =>
                {
                    b.HasOne("Domain.Entities.Creator", "Seller")
                        .WithMany()
                        .HasForeignKey("SellerId");

                    b.Navigation("Seller");
                });

            modelBuilder.Entity("Domain.Entities.TransactionLog", b =>
                {
                    b.HasOne("Domain.Entities.Creator", "Creator")
                        .WithMany()
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Creator");
                });
#pragma warning restore 612, 618
        }
    }
}
