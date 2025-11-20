using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ufshop.Migrations
{
    /// <inheritdoc />
    public partial class _20251105_1014 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Discriminator = table.Column<string>(type: "TEXT", maxLength: 21, nullable: false),
                    UserName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: true),
                    SecurityStamp = table.Column<string>(type: "TEXT", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Carts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomerInfos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    FirstName = table.Column<string>(type: "TEXT", nullable: false),
                    LastName = table.Column<string>(type: "TEXT", nullable: false),
                    StreetName = table.Column<string>(type: "TEXT", nullable: false),
                    StreetNo = table.Column<string>(type: "TEXT", nullable: false),
                    ZipCode = table.Column<string>(type: "TEXT", nullable: false),
                    City = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Phone = table.Column<string>(type: "TEXT", nullable: false),
                    Info = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerInfos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShopSocialMedias",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Facebook = table.Column<string>(type: "TEXT", nullable: false),
                    Instagram = table.Column<string>(type: "TEXT", nullable: false),
                    LinkedIn = table.Column<string>(type: "TEXT", nullable: false),
                    TikTok = table.Column<string>(type: "TEXT", nullable: false),
                    YouTube = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopSocialMedias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RoleId = table.Column<string>(type: "TEXT", nullable: false),
                    ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "TEXT", nullable: false),
                    ProviderKey = table.Column<string>(type: "TEXT", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "TEXT", nullable: true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    RoleId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    LoginProvider = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Value = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    CartId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CustomerInfoId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customer_Carts_CartId",
                        column: x => x.CartId,
                        principalTable: "Carts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Customer_CustomerInfos_CustomerInfoId",
                        column: x => x.CustomerInfoId,
                        principalTable: "CustomerInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShopContactInfos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    MobileNumber = table.Column<string>(type: "TEXT", nullable: false),
                    SocialMediaId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopContactInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShopContactInfos_ShopSocialMedias_SocialMediaId",
                        column: x => x.SocialMediaId,
                        principalTable: "ShopSocialMedias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CartItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ShopItemId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Amount = table.Column<int>(type: "INTEGER", nullable: false),
                    CartId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CartItems_Carts_CartId",
                        column: x => x.CartId,
                        principalTable: "Carts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ShopImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Filename = table.Column<string>(type: "TEXT", nullable: false),
                    AltText = table.Column<string>(type: "TEXT", nullable: false),
                    ShopItemId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopImages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShopSettings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    SwishNumber = table.Column<string>(type: "TEXT", nullable: false),
                    BaseShippingPrice = table.Column<int>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Layout = table.Column<string>(type: "TEXT", nullable: false),
                    Theme = table.Column<string>(type: "TEXT", nullable: false),
                    ContactInfoId = table.Column<Guid>(type: "TEXT", nullable: false),
                    LogoImageId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShopSettings_ShopContactInfos_ContactInfoId",
                        column: x => x.ContactInfoId,
                        principalTable: "ShopContactInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShopSettings_ShopImages_LogoImageId",
                        column: x => x.LogoImageId,
                        principalTable: "ShopImages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Shops",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Prefix = table.Column<string>(type: "TEXT", nullable: false),
                    OwnerId = table.Column<string>(type: "TEXT", nullable: false),
                    SettingsId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shops", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Shops_AspNetUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Shops_ShopSettings_SettingsId",
                        column: x => x.SettingsId,
                        principalTable: "ShopSettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShopItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    ItemsAvailable = table.Column<int>(type: "INTEGER", nullable: false),
                    Price = table.Column<int>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Order = table.Column<int>(type: "INTEGER", nullable: false),
                    PrimaryImageId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ShopId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShopItems_ShopImages_PrimaryImageId",
                        column: x => x.PrimaryImageId,
                        principalTable: "ShopImages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShopItems_Shops_ShopId",
                        column: x => x.ShopId,
                        principalTable: "Shops",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ShopOrders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ShopOrderStatus = table.Column<int>(type: "INTEGER", nullable: false),
                    CustomerId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ShopId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShopOrders_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShopOrders_Shops_ShopId",
                        column: x => x.ShopId,
                        principalTable: "Shops",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ShopPages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Header = table.Column<string>(type: "TEXT", nullable: false),
                    Order = table.Column<int>(type: "INTEGER", nullable: false),
                    ShopId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopPages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShopPages_Shops_ShopId",
                        column: x => x.ShopId,
                        principalTable: "Shops",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ShopItemProperties",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Uploadable = table.Column<bool>(type: "INTEGER", nullable: false),
                    ImageId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CartId = table.Column<Guid>(type: "TEXT", nullable: true),
                    ShopItemId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopItemProperties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShopItemProperties_Carts_CartId",
                        column: x => x.CartId,
                        principalTable: "Carts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ShopItemProperties_ShopImages_ImageId",
                        column: x => x.ImageId,
                        principalTable: "ShopImages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShopItemProperties_ShopItems_ShopItemId",
                        column: x => x.ShopItemId,
                        principalTable: "ShopItems",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ShopPageFragments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Header = table.Column<string>(type: "TEXT", nullable: false),
                    Paragraph = table.Column<string>(type: "TEXT", nullable: false),
                    Order = table.Column<int>(type: "INTEGER", nullable: false),
                    ImageId = table.Column<Guid>(type: "TEXT", nullable: true),
                    ShopPageId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopPageFragments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShopPageFragments_ShopImages_ImageId",
                        column: x => x.ImageId,
                        principalTable: "ShopImages",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ShopPageFragments_ShopPages_ShopPageId",
                        column: x => x.ShopPageId,
                        principalTable: "ShopPages",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ShopItemPropertyOptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Value = table.Column<string>(type: "TEXT", nullable: false),
                    ShopItemPropertyId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopItemPropertyOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShopItemPropertyOptions_ShopItemProperties_ShopItemPropertyId",
                        column: x => x.ShopItemPropertyId,
                        principalTable: "ShopItemProperties",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_CartId",
                table: "CartItems",
                column: "CartId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_ShopItemId",
                table: "CartItems",
                column: "ShopItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_CartId",
                table: "Customer",
                column: "CartId");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_CustomerInfoId",
                table: "Customer",
                column: "CustomerInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopContactInfos_SocialMediaId",
                table: "ShopContactInfos",
                column: "SocialMediaId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopImages_ShopItemId",
                table: "ShopImages",
                column: "ShopItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopItemProperties_CartId",
                table: "ShopItemProperties",
                column: "CartId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopItemProperties_ImageId",
                table: "ShopItemProperties",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopItemProperties_ShopItemId",
                table: "ShopItemProperties",
                column: "ShopItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopItemPropertyOptions_ShopItemPropertyId",
                table: "ShopItemPropertyOptions",
                column: "ShopItemPropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopItems_PrimaryImageId",
                table: "ShopItems",
                column: "PrimaryImageId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopItems_ShopId",
                table: "ShopItems",
                column: "ShopId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopOrders_CustomerId",
                table: "ShopOrders",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopOrders_ShopId",
                table: "ShopOrders",
                column: "ShopId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopPageFragments_ImageId",
                table: "ShopPageFragments",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopPageFragments_ShopPageId",
                table: "ShopPageFragments",
                column: "ShopPageId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopPages_ShopId",
                table: "ShopPages",
                column: "ShopId");

            migrationBuilder.CreateIndex(
                name: "IX_Shops_OwnerId",
                table: "Shops",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Shops_SettingsId",
                table: "Shops",
                column: "SettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopSettings_ContactInfoId",
                table: "ShopSettings",
                column: "ContactInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopSettings_LogoImageId",
                table: "ShopSettings",
                column: "LogoImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_ShopItems_ShopItemId",
                table: "CartItems",
                column: "ShopItemId",
                principalTable: "ShopItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShopImages_ShopItems_ShopItemId",
                table: "ShopImages",
                column: "ShopItemId",
                principalTable: "ShopItems",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shops_AspNetUsers_OwnerId",
                table: "Shops");

            migrationBuilder.DropForeignKey(
                name: "FK_ShopImages_ShopItems_ShopItemId",
                table: "ShopImages");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "CartItems");

            migrationBuilder.DropTable(
                name: "ShopItemPropertyOptions");

            migrationBuilder.DropTable(
                name: "ShopOrders");

            migrationBuilder.DropTable(
                name: "ShopPageFragments");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "ShopItemProperties");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropTable(
                name: "ShopPages");

            migrationBuilder.DropTable(
                name: "Carts");

            migrationBuilder.DropTable(
                name: "CustomerInfos");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "ShopItems");

            migrationBuilder.DropTable(
                name: "Shops");

            migrationBuilder.DropTable(
                name: "ShopSettings");

            migrationBuilder.DropTable(
                name: "ShopContactInfos");

            migrationBuilder.DropTable(
                name: "ShopImages");

            migrationBuilder.DropTable(
                name: "ShopSocialMedias");
        }
    }
}
