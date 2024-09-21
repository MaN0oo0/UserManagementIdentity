using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserManagmentWithIdentity.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAdminUserSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT [security].[Users] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [FirstName], [LastName], [ProfilePicture]) VALUES (N'b55d96ca-649e-4bbd-bca1-99bfd6f92cb2', N'admin', N'ADMIN', N'admin@test.com', N'ADMIN@TEST.COM', 0, N'AQAAAAIAAYagAAAAEFx7hRor7gaSR/fg0L2gRzJFE9nqcsa46d30sMpxxVQUSUWagkAtjvTqKHce86SIkg==', N'ULLBQESCVBDMSFU73VNWUXUBBAUAUH6A', N'e623f4a9-655f-4ea7-a436-f8052f8c55a0', NULL, 0, 0, NULL, 1, 0, N'Mohamed', N'Ahmed', NULL)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [security].[Users] WHERE Id='b55d96ca-649e-4bbd-bca1-99bfd6f92cb2'");
        }
    }
}
