using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserManagmentWithIdentity.Data.Migrations
{
    /// <inheritdoc />
    public partial class AssignAdminUserToRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO [security].[UserRoles] ([UserId] ,[RoleId]) select 'b55d96ca-649e-4bbd-bca1-99bfd6f92cb2',Id from [security].[Roles]");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [security].[UserRoles] WHERE UserId='b55d96ca-649e-4bbd-bca1-99bfd6f92cb2'");
        }
    }
}
