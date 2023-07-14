using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalProject.Migrations
{
    /// <inheritdoc />
    public partial class seedAdminUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Create the admin role if it doesn't already exist
            migrationBuilder.Sql("IF NOT EXISTS (SELECT * FROM [AspNetRoles] WHERE [Name] = 'Admin') " +
                                  "INSERT INTO [AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) " +
                                  "VALUES ('c9d7f038-4d9d-4b2b-9e1f-56c369e2a7f5', 'Admin', 'ADMIN', '1')");

            // Create the admin user if it doesn't already exist
            migrationBuilder.Sql("IF NOT EXISTS (SELECT * FROM [AspNetUsers] WHERE [UserName] = 'admin') " +
                                  "INSERT INTO [AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [Name]) VALUES (N'4e865cce-28e2-46dc-a4b6-9ca0d5aacb09', N'admin', N'ADMIN', N'admin@admin.com', N'ADMIN@ADMIN.COM', 0, N'AQAAAAIAAYagAAAAENJlT9C+5gBJKeVtSbj0owdWRnwHIyw+snwC2IckRX2GAw+8cVoenoa9ciNht3O69w==', N'VC7BP2VT5RRUPBDII4VNQ4QFOKQ4U5PB', N'86314d1c-bc9f-43af-8d49-805b1c7ed591', NULL, 0, 0, NULL, 1, 0, N'Admin User')");

            // Add the admin user to the admin role
            migrationBuilder.Sql("IF NOT EXISTS (SELECT * FROM [AspNetUserRoles] WHERE [UserId] = '4e865cce-28e2-46dc-a4b6-9ca0d5aacb09' AND [RoleId] = 'c9d7f038-4d9d-4b2b-9e1f-56c369e2a7f5') " +
            "INSERT INTO [AspNetUserRoles] ([UserId], [RoleId]) " +
            "VALUES ('4e865cce-28e2-46dc-a4b6-9ca0d5aacb09', 'c9d7f038-4d9d-4b2b-9e1f-56c369e2a7f5')");

            // Assign all permissions to the admin role
            var allPermissions = FinalProject.Constants.Permissions.GenerateAllPermissions();
            foreach (var permission in allPermissions)
            {
                migrationBuilder.Sql($"IF NOT EXISTS (SELECT * FROM [AspNetRoleClaims] WHERE [ClaimType] = 'Permission' AND [ClaimValue] = '{permission}' AND [RoleId] = 'c9d7f038-4d9d-4b2b-9e1f-56c369e2a7f5') " +
                "INSERT INTO [AspNetRoleClaims] ([RoleId], [ClaimType], [ClaimValue]) " +
                                      $"VALUES ('c9d7f038-4d9d-4b2b-9e1f-56c369e2a7f5', 'Permission', '{permission}')");
            }
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove the admin user from the admin role
            migrationBuilder.Sql("DELETE FROM [AspNetUserRoles] WHERE [UserId] = '4e865cce-28e2-46dc-a4b6-9ca0d5aacb09' AND [RoleId] = 'c9d7f038-4d9d-4b2b-9e1f-56c369e2a7f5'");

            // Delete the admin user if it was created by the migration
            migrationBuilder.Sql("DELETE FROM [AspNetUsers] WHERE [UserName] = 'admin'");

            // Delete the admin role if it was created by the migration
            migrationBuilder.Sql("DELETE FROM [AspNetRoles] WHERE [Name] = 'Admin'");

            // Delete all role claims related to permissions if they were created by the migration
            migrationBuilder.Sql("DELETE FROM [AspNetRoleClaims] WHERE [ClaimType] = 'Permission'");
        }
    }
}
