[CmdletBinding()]
param(
	[Parameter(Mandatory=$true)]
	[string] $MigrationName
)

process {
	dotnet.exe ef migrations add $MigrationName -s ..\TimeKeeping.Web\Server\ -c Th11s.TimeKeeping.Data.NpgsqlDbContext -o Migrations/PostgreSQL -- --DatabaseProvider "Npgsql"
	dotnet.exe ef migrations add $MigrationName -s ..\TimeKeeping.Web\Server\ -c Th11s.TimeKeeping.Data.SqlServerDbContext -o Migrations/SqlServer --no-build -- --DatabaseProvider "SqlServer"
}