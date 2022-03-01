using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class CreatorProfileView : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE VIEW CreatorProfiles as
                SELECT T1.Id, T1.Username, T1.UsernameNormalize, T1.Name, T1.Bio, T1.ProfilePictureFilename, T1.DateRegistered, COUNT(T2.Id) As SubscriberCount FROM Creators T1
                LEFT JOIN CreatorSubscribers T2 ON T1.Id = T2.CreatorId
                GROUP BY T1.Id,  T1.Username, T1.UsernameNormalize, T1.Name, T1.Bio,T1.ProfilePictureFilename, T1.DateRegistered");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                drop view CreatorProfiles;
            ");
        }
    }
}
