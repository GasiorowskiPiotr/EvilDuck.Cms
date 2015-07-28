using System;
using Microsoft.Data.Entity;
using EvilDuck.Cms.Portal.Framework.Entities;
using Microsoft.Data.Entity.Migrations.Infrastructure;

namespace EvilDuck.Cms.Portal.Migrations
{
    [ContextType(typeof(ApplicationContext))]
    partial class CreateIdentitySchema
    {
        public override string Id
        {
            get { return "00000000000000_CreateIdentitySchema"; }
        }

        public override string ProductVersion
        {
            get { return "7.0.0-beta6"; }
        }

        public override void BuildTargetModel(ModelBuilder builder)
        {
            builder
                .Annotation("SqlServer:ValueGeneration", "Identity");

            builder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .Annotation("OriginalValueIndex", 0);

                    b.Property<string>("ConcurrencyStamp")
                        .ConcurrencyToken()
                        .Annotation("OriginalValueIndex", 1);

                    b.Property<string>("Name")
                        .Annotation("OriginalValueIndex", 2);

                    b.Property<string>("NormalizedName")
                        .Annotation("OriginalValueIndex", 3);

                    b.Key("Id");

                    b.Annotation("Relational:TableName", "AspNetRoles");
                });

            builder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .Annotation("OriginalValueIndex", 0);

                    b.Property<string>("ClaimType")
                        .Annotation("OriginalValueIndex", 1);

                    b.Property<string>("ClaimValue")
                        .Annotation("OriginalValueIndex", 2);

                    b.Property<string>("RoleId")
                        .Annotation("OriginalValueIndex", 3);

                    b.Key("Id");

                    b.Annotation("Relational:TableName", "AspNetRoleClaims");
                });

            builder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .Annotation("OriginalValueIndex", 0);

                    b.Property<string>("ClaimType")
                        .Annotation("OriginalValueIndex", 1);

                    b.Property<string>("ClaimValue")
                        .Annotation("OriginalValueIndex", 2);

                    b.Property<string>("UserId")
                        .Annotation("OriginalValueIndex", 3);

                    b.Key("Id");

                    b.Annotation("Relational:TableName", "AspNetUserClaims");
                });

            builder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .ValueGeneratedOnAdd()
                        .Annotation("OriginalValueIndex", 0);

                    b.Property<string>("ProviderKey")
                        .ValueGeneratedOnAdd()
                        .Annotation("OriginalValueIndex", 1);

                    b.Property<string>("ProviderDisplayName")
                        .Annotation("OriginalValueIndex", 2);

                    b.Property<string>("UserId")
                        .Annotation("OriginalValueIndex", 3);

                    b.Key("LoginProvider", "ProviderKey");

                    b.Annotation("Relational:TableName", "AspNetUserLogins");
                });

            builder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .Annotation("OriginalValueIndex", 0);

                    b.Property<string>("RoleId")
                        .Annotation("OriginalValueIndex", 1);

                    b.Key("UserId", "RoleId");

                    b.Annotation("Relational:TableName", "AspNetUserRoles");
                });

            builder.Entity("EvilDuck.Cms.Portal.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .Annotation("OriginalValueIndex", 0);

                    b.Property<int>("AccessFailedCount")
                        .Annotation("OriginalValueIndex", 1);

                    b.Property<string>("ConcurrencyStamp")
                        .ConcurrencyToken()
                        .Annotation("OriginalValueIndex", 2);

                    b.Property<string>("Email")
                        .Annotation("OriginalValueIndex", 3);

                    b.Property<bool>("EmailConfirmed")
                        .Annotation("OriginalValueIndex", 4);

                    b.Property<bool>("LockoutEnabled")
                        .Annotation("OriginalValueIndex", 5);

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .Annotation("OriginalValueIndex", 6);

                    b.Property<string>("NormalizedEmail")
                        .Annotation("OriginalValueIndex", 7);

                    b.Property<string>("NormalizedUserName")
                        .Annotation("OriginalValueIndex", 8);

                    b.Property<string>("PasswordHash")
                        .Annotation("OriginalValueIndex", 9);

                    b.Property<string>("PhoneNumber")
                        .Annotation("OriginalValueIndex", 10);

                    b.Property<bool>("PhoneNumberConfirmed")
                        .Annotation("OriginalValueIndex", 11);

                    b.Property<string>("SecurityStamp")
                        .Annotation("OriginalValueIndex", 12);

                    b.Property<bool>("TwoFactorEnabled")
                        .Annotation("OriginalValueIndex", 13);

                    b.Property<string>("UserName")
                        .Annotation("OriginalValueIndex", 14);

                    b.Key("Id");

                    b.Annotation("Relational:TableName", "AspNetUsers");
                });

            builder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityRoleClaim<string>", b =>
                {
                    b.Reference("Microsoft.AspNet.Identity.EntityFramework.IdentityRole")
                        .InverseCollection()
                        .ForeignKey("RoleId");
                });

            builder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityUserClaim<string>", b =>
                {
                    b.Reference("EvilDuck.Cms.Portal.Models.ApplicationUser")
                        .InverseCollection()
                        .ForeignKey("UserId");
                });

            builder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityUserLogin<string>", b =>
                {
                    b.Reference("EvilDuck.Cms.Portal.Models.ApplicationUser")
                        .InverseCollection()
                        .ForeignKey("UserId");
                });

            builder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityUserRole<string>", b =>
                {
                    b.Reference("Microsoft.AspNet.Identity.EntityFramework.IdentityRole")
                        .InverseCollection()
                        .ForeignKey("RoleId");

                    b.Reference("EvilDuck.Cms.Portal.Models.ApplicationUser")
                        .InverseCollection()
                        .ForeignKey("UserId");
                });
        }
    }
}
