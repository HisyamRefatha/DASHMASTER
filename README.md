# DASHMASTER
# SCAFFOLDING
dotnet ef dbcontext scaffold "Data Source=localhost,1433;Database=DASHMASTER;User Id=LocalAuth;Password=local123; TrustServerCertificate=Yes" Microsoft.EntityFrameworkCore.SqlServer --output-dir "../DASHMASTER.DATA/Model" -c ApplicationDBContext --context-dir "../DASHMASTER.DATA" --namespace "DASHMASTER.DATA.Model" --context-namespace "DASHMASTER.DATA" --no-pluralize -f --no-onconfiguring --schema "dbo"
