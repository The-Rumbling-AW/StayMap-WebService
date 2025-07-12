using Microsoft.EntityFrameworkCore;

namespace backendAppsWeb.Shared.Infrastructure.Persistence.EFC.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void UseSnakeCaseNamingConvention(this ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var tableName = entityType.GetTableName();
            if (!string.IsNullOrEmpty(tableName))
            {
                entityType.SetTableName(tableName.ToPlural().ToSnakeCase());
            }
            
            foreach (var property in entityType.GetProperties())
                property.SetColumnName(property.GetColumnName().ToSnakeCase());
            
            foreach (var key in entityType.GetKeys())
            {
                var keyName = key.GetName();
                if (!string.IsNullOrEmpty(keyName)) key.SetName(keyName.ToSnakeCase());
            }
            
            foreach (var foreignKey in entityType.GetForeignKeys())
            {
                var fkName = foreignKey.GetConstraintName();
                if (!string.IsNullOrEmpty(fkName)) foreignKey.SetConstraintName(fkName.ToSnakeCase());
            }
            foreach (var index in entityType.GetIndexes())
            {
                var indexName = index.GetDatabaseName();
                if (!string.IsNullOrEmpty(indexName)) index.SetDatabaseName(indexName.ToSnakeCase());
            }
        }
    }
}