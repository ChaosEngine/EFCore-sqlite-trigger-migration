using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;

namespace EFCore_sqlite_trigger_migration.Helpers
{
	public static class TriggerMigrationExtensions
	{
		internal static MigrationBuilder CreateTimestampTrigger(this MigrationBuilder migrationBuilder,
			IEntityType entityType,
			string timeStampColumnName,
			string primaryKey)
		{

			switch (migrationBuilder.ActiveProvider)
			{
				case "Microsoft.EntityFrameworkCore.Sqlite":
					var tableName = entityType.GetTableName();
					string command =
						$"""
						CREATE TRIGGER IF NOT EXISTS "{tableName}_update_{timeStampColumnName}_Trigger"
							AFTER UPDATE ON "{tableName}"
						BEGIN
							UPDATE "{tableName}" SET
							"{timeStampColumnName}" = datetime(CURRENT_TIMESTAMP, 'UTC')
							WHERE "{primaryKey}" = NEW."{primaryKey}";
						END;
						""";
					//Console.Error.WriteLine($"executing '{command}'");
					migrationBuilder.Sql(command);
					break;

				default:
					throw new Exception("Unexpected provider.");
			}

			return migrationBuilder;
		}

		internal static MigrationBuilder DropTimestampTrigger(this MigrationBuilder migrationBuilder, IEntityType entityType, string timeStampColumnName)
		{

			switch (migrationBuilder.ActiveProvider)
			{
				case "Microsoft.EntityFrameworkCore.Sqlite":
					var tableName = entityType.GetTableName();
					string command = $"""DROP TRIGGER IF EXISTS "{tableName}_update_{timeStampColumnName}_Trigger";""";

					//Console.Error.WriteLine($"executing '{command}'");
					migrationBuilder.Sql(command);
					break;

				default:
					throw new Exception("Unexpected provider.");
			}

			return migrationBuilder;
		}
	}
}
