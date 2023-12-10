    using System;
using EFCore_sqlite_trigger_migration.Helpers;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFCoresqlitetriggermigration.Migrations
{
    /// <inheritdoc />
    public partial class TimestampAndTrigger : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
			//
			//Not working :-(
			//Throws:
			//          Failed executing DbCommand (2ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
			//          ALTER TABLE "Pets" ADD "UpdatedAt" TIMESTAMP NULL DEFAULT (CURRENT_TIMESTAMP);
			//          Microsoft.Data.Sqlite.SqliteException (0x80004005): SQLite Error 1: 'Cannot add a column with non-constant default'.
			//
			migrationBuilder.AddColumn<DateTime>(
				name: "UpdatedAt",
				table: "Pets",
				type: "TIMESTAMP",
				nullable: true
				//, defaultValueSql: "CURRENT_TIMESTAMP"
				);

			var entity = TargetModel.FindEntityType(typeof(Pet));
			if (entity != null && entity.Name == typeof(Pet).FullName)
			{
				migrationBuilder.CreateTimestampTrigger(entity, nameof(Pet.UpdatedAt), nameof(Pet.ID));
			}

			//overcomming problem/bug:
			//Microsoft.Data.Sqlite.SqliteException (0x80004005): SQLite Error 1: 'Cannot add a column with non-constant default'
			//https://stackoverflow.com/a/61967099/4429828
			//OMG, I am loosing hope in sqlite
			migrationBuilder.Sql(
                $"""
                    UPDATE "Pets" SET
                    "UpdatedAt" = datetime(CURRENT_TIMESTAMP, 'UTC')
                    WHERE "UpdatedAt" IS NULL
                """
                );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
			var entity = TargetModel.FindEntityType(typeof(Pet));
			if (entity != null && entity.Name == typeof(Pet).FullName)
			{
				migrationBuilder.DropTimestampTrigger(entity, nameof(Pet.UpdatedAt));
			}


			migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Pets");
        }
    }
}
