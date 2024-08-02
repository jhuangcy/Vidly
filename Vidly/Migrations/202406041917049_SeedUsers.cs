namespace Vidly2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeedUsers : DbMigration
    {
        public override void Up()
        {
            // Password1!
            Sql(@"
                INSERT INTO [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'a8eff572-7dce-4a73-be40-c26784d7dac0', N'guest@gmail.com', 0, N'AH1TIB5fPScTFyt4MoO3z8x/mWpd4wTrqsYcJPxgWYLO3/amr7dwKXYB+gkKWhkLSw==', N'd6664e30-5306-4ca1-884c-3185c6967db4', NULL, 0, 0, NULL, 1, 0, N'guest@gmail.com')
                INSERT INTO [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'ef6b0302-5a82-4151-9c0c-ebd5d27b011c', N'admin@gmail.com', 0, N'AI4q0VS/lOXOs8DY108yhCOeEMBtkier4LxhqbmdFOwRmZPb/UZpmvJcH/n0lI3WLg==', N'6cb89149-159e-4094-8834-ef8ad72d95b0', NULL, 0, 0, NULL, 1, 0, N'admin@gmail.com')
                INSERT INTO [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'df4bedbf-0b12-4622-9146-5fbf0b33d32c', N'CanManageMovies')
                INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'ef6b0302-5a82-4151-9c0c-ebd5d27b011c', N'df4bedbf-0b12-4622-9146-5fbf0b33d32c')
            ");
        }
        
        public override void Down()
        {
        }
    }
}
