using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MobyLabWebProgramming.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class IsThisTheEnd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Artwork_User_UserId",
                table: "Artwork");

            migrationBuilder.DropForeignKey(
                name: "FK_ArtworkReference_Artwork_ArtworkId",
                table: "ArtworkReference");

            migrationBuilder.DropForeignKey(
                name: "FK_ArtworkReference_Reference_ReferenceId",
                table: "ArtworkReference");

            migrationBuilder.DropForeignKey(
                name: "FK_ArtworkTag_Artwork_ArtworkId",
                table: "ArtworkTag");

            migrationBuilder.DropForeignKey(
                name: "FK_ArtworkTag_Tag_TagId",
                table: "ArtworkTag");

            migrationBuilder.DropForeignKey(
                name: "FK_ReferenceTag_Reference_ReferenceId",
                table: "ReferenceTag");

            migrationBuilder.DropForeignKey(
                name: "FK_ReferenceTag_Tag_TagId",
                table: "ReferenceTag");

            migrationBuilder.DropForeignKey(
                name: "FK_UserFollows_User_FollowedId",
                table: "UserFollows");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Tag_Name",
                table: "Tag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tag",
                table: "Tag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ReferenceTag",
                table: "ReferenceTag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Reference",
                table: "Reference");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ArtworkTag",
                table: "ArtworkTag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ArtworkReference",
                table: "ArtworkReference");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Artwork",
                table: "Artwork");

            migrationBuilder.RenameTable(
                name: "Tag",
                newName: "Tags");

            migrationBuilder.RenameTable(
                name: "ReferenceTag",
                newName: "ReferenceTags");

            migrationBuilder.RenameTable(
                name: "Reference",
                newName: "References");

            migrationBuilder.RenameTable(
                name: "ArtworkTag",
                newName: "ArtworkTags");

            migrationBuilder.RenameTable(
                name: "ArtworkReference",
                newName: "ArtworkReferences");

            migrationBuilder.RenameTable(
                name: "Artwork",
                newName: "Artworks");

            migrationBuilder.RenameColumn(
                name: "FollowedId",
                table: "UserFollows",
                newName: "FollowingId");

            migrationBuilder.RenameIndex(
                name: "IX_UserFollows_FollowedId",
                table: "UserFollows",
                newName: "IX_UserFollows_FollowingId");

            migrationBuilder.RenameIndex(
                name: "IX_ReferenceTag_TagId",
                table: "ReferenceTags",
                newName: "IX_ReferenceTags_TagId");

            migrationBuilder.RenameIndex(
                name: "IX_ReferenceTag_ReferenceId_TagId",
                table: "ReferenceTags",
                newName: "IX_ReferenceTags_ReferenceId_TagId");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "References",
                newName: "Title");

            migrationBuilder.RenameIndex(
                name: "IX_ArtworkTag_TagId",
                table: "ArtworkTags",
                newName: "IX_ArtworkTags_TagId");

            migrationBuilder.RenameIndex(
                name: "IX_ArtworkTag_ArtworkId_TagId",
                table: "ArtworkTags",
                newName: "IX_ArtworkTags_ArtworkId_TagId");

            migrationBuilder.RenameIndex(
                name: "IX_ArtworkReference_ReferenceId",
                table: "ArtworkReferences",
                newName: "IX_ArtworkReferences_ReferenceId");

            migrationBuilder.RenameIndex(
                name: "IX_ArtworkReference_ArtworkId",
                table: "ArtworkReferences",
                newName: "IX_ArtworkReferences_ArtworkId");

            migrationBuilder.RenameIndex(
                name: "IX_Artwork_UserId",
                table: "Artworks",
                newName: "IX_Artworks_UserId");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "References",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "MediumId",
                table: "Artworks",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Tags_Name",
                table: "Tags",
                column: "Name");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tags",
                table: "Tags",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReferenceTags",
                table: "ReferenceTags",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_References",
                table: "References",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ArtworkTags",
                table: "ArtworkTags",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ArtworkReferences",
                table: "ArtworkReferences",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Artworks",
                table: "Artworks",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Mediums",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mediums", x => x.Id);
                    table.UniqueConstraint("AK_Mediums_Name", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "SavedReference",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ReferenceId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SavedReference", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SavedReference_References_ReferenceId",
                        column: x => x.ReferenceId,
                        principalTable: "References",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SavedReference_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserFollowedTag",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    TagId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFollowedTag", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserFollowedTag_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserFollowedTag_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_References_UserId",
                table: "References",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Artworks_MediumId",
                table: "Artworks",
                column: "MediumId");

            migrationBuilder.CreateIndex(
                name: "IX_SavedReference_ReferenceId",
                table: "SavedReference",
                column: "ReferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_SavedReference_UserId_ReferenceId",
                table: "SavedReference",
                columns: new[] { "UserId", "ReferenceId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserFollowedTag_TagId",
                table: "UserFollowedTag",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFollowedTag_UserId",
                table: "UserFollowedTag",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ArtworkReferences_Artworks_ArtworkId",
                table: "ArtworkReferences",
                column: "ArtworkId",
                principalTable: "Artworks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ArtworkReferences_References_ReferenceId",
                table: "ArtworkReferences",
                column: "ReferenceId",
                principalTable: "References",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Artworks_Mediums_MediumId",
                table: "Artworks",
                column: "MediumId",
                principalTable: "Mediums",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Artworks_User_UserId",
                table: "Artworks",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ArtworkTags_Artworks_ArtworkId",
                table: "ArtworkTags",
                column: "ArtworkId",
                principalTable: "Artworks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ArtworkTags_Tags_TagId",
                table: "ArtworkTags",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_References_User_UserId",
                table: "References",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReferenceTags_References_ReferenceId",
                table: "ReferenceTags",
                column: "ReferenceId",
                principalTable: "References",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReferenceTags_Tags_TagId",
                table: "ReferenceTags",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserFollows_User_FollowingId",
                table: "UserFollows",
                column: "FollowingId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArtworkReferences_Artworks_ArtworkId",
                table: "ArtworkReferences");

            migrationBuilder.DropForeignKey(
                name: "FK_ArtworkReferences_References_ReferenceId",
                table: "ArtworkReferences");

            migrationBuilder.DropForeignKey(
                name: "FK_Artworks_Mediums_MediumId",
                table: "Artworks");

            migrationBuilder.DropForeignKey(
                name: "FK_Artworks_User_UserId",
                table: "Artworks");

            migrationBuilder.DropForeignKey(
                name: "FK_ArtworkTags_Artworks_ArtworkId",
                table: "ArtworkTags");

            migrationBuilder.DropForeignKey(
                name: "FK_ArtworkTags_Tags_TagId",
                table: "ArtworkTags");

            migrationBuilder.DropForeignKey(
                name: "FK_References_User_UserId",
                table: "References");

            migrationBuilder.DropForeignKey(
                name: "FK_ReferenceTags_References_ReferenceId",
                table: "ReferenceTags");

            migrationBuilder.DropForeignKey(
                name: "FK_ReferenceTags_Tags_TagId",
                table: "ReferenceTags");

            migrationBuilder.DropForeignKey(
                name: "FK_UserFollows_User_FollowingId",
                table: "UserFollows");

            migrationBuilder.DropTable(
                name: "Mediums");

            migrationBuilder.DropTable(
                name: "SavedReference");

            migrationBuilder.DropTable(
                name: "UserFollowedTag");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Tags_Name",
                table: "Tags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tags",
                table: "Tags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ReferenceTags",
                table: "ReferenceTags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_References",
                table: "References");

            migrationBuilder.DropIndex(
                name: "IX_References_UserId",
                table: "References");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ArtworkTags",
                table: "ArtworkTags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Artworks",
                table: "Artworks");

            migrationBuilder.DropIndex(
                name: "IX_Artworks_MediumId",
                table: "Artworks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ArtworkReferences",
                table: "ArtworkReferences");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "References");

            migrationBuilder.DropColumn(
                name: "MediumId",
                table: "Artworks");

            migrationBuilder.RenameTable(
                name: "Tags",
                newName: "Tag");

            migrationBuilder.RenameTable(
                name: "ReferenceTags",
                newName: "ReferenceTag");

            migrationBuilder.RenameTable(
                name: "References",
                newName: "Reference");

            migrationBuilder.RenameTable(
                name: "ArtworkTags",
                newName: "ArtworkTag");

            migrationBuilder.RenameTable(
                name: "Artworks",
                newName: "Artwork");

            migrationBuilder.RenameTable(
                name: "ArtworkReferences",
                newName: "ArtworkReference");

            migrationBuilder.RenameColumn(
                name: "FollowingId",
                table: "UserFollows",
                newName: "FollowedId");

            migrationBuilder.RenameIndex(
                name: "IX_UserFollows_FollowingId",
                table: "UserFollows",
                newName: "IX_UserFollows_FollowedId");

            migrationBuilder.RenameIndex(
                name: "IX_ReferenceTags_TagId",
                table: "ReferenceTag",
                newName: "IX_ReferenceTag_TagId");

            migrationBuilder.RenameIndex(
                name: "IX_ReferenceTags_ReferenceId_TagId",
                table: "ReferenceTag",
                newName: "IX_ReferenceTag_ReferenceId_TagId");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Reference",
                newName: "Name");

            migrationBuilder.RenameIndex(
                name: "IX_ArtworkTags_TagId",
                table: "ArtworkTag",
                newName: "IX_ArtworkTag_TagId");

            migrationBuilder.RenameIndex(
                name: "IX_ArtworkTags_ArtworkId_TagId",
                table: "ArtworkTag",
                newName: "IX_ArtworkTag_ArtworkId_TagId");

            migrationBuilder.RenameIndex(
                name: "IX_Artworks_UserId",
                table: "Artwork",
                newName: "IX_Artwork_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ArtworkReferences_ReferenceId",
                table: "ArtworkReference",
                newName: "IX_ArtworkReference_ReferenceId");

            migrationBuilder.RenameIndex(
                name: "IX_ArtworkReferences_ArtworkId",
                table: "ArtworkReference",
                newName: "IX_ArtworkReference_ArtworkId");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Tag_Name",
                table: "Tag",
                column: "Name");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tag",
                table: "Tag",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReferenceTag",
                table: "ReferenceTag",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reference",
                table: "Reference",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ArtworkTag",
                table: "ArtworkTag",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Artwork",
                table: "Artwork",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ArtworkReference",
                table: "ArtworkReference",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Artwork_User_UserId",
                table: "Artwork",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ArtworkReference_Artwork_ArtworkId",
                table: "ArtworkReference",
                column: "ArtworkId",
                principalTable: "Artwork",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ArtworkReference_Reference_ReferenceId",
                table: "ArtworkReference",
                column: "ReferenceId",
                principalTable: "Reference",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ArtworkTag_Artwork_ArtworkId",
                table: "ArtworkTag",
                column: "ArtworkId",
                principalTable: "Artwork",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ArtworkTag_Tag_TagId",
                table: "ArtworkTag",
                column: "TagId",
                principalTable: "Tag",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReferenceTag_Reference_ReferenceId",
                table: "ReferenceTag",
                column: "ReferenceId",
                principalTable: "Reference",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReferenceTag_Tag_TagId",
                table: "ReferenceTag",
                column: "TagId",
                principalTable: "Tag",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserFollows_User_FollowedId",
                table: "UserFollows",
                column: "FollowedId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
