using OzyParkAdmin.Application.Reportes.Generate;
using OzyParkAdmin.Domain.Reportes;
using OzyParkAdmin.Domain.Reportes.DataSources;
using OzyParkAdmin.Domain.Reportes.Filters;
using OzyParkAdmin.Domain.Reportes.Listed;
using OzyParkAdmin.Infrastructure.Reportes.Internals;
using System.Data;
using System.Data.Common;
using System.Data.SqlTypes;

namespace OzyParkAdmin.Infrastructure.Reportes.Generate.Internals;
internal static class DataSetExecutor
{
    public static DataSet ExecuteDataSet(DataSource dataSource, Report report, ReportFilter filter)
    {
        DataSet dataSet = new();
        ExecuteDataTable(dataSource, report, filter, dataSet);
        return dataSet;
    }

    public static void ExecuteDataTable(DataSource dataSource, Report report, ReportFilter filter, DataSet dataSet)
    {
        try
        {
            using DbConnection? connection = TryCreateConnection(dataSource, out DbProviderFactory factory);
            DbCommand? command = PrepareCommand(connection, dataSource, report, filter);

            DbDataAdapter? adapter = factory.CreateDataAdapter();

            if (adapter is not null)
            {
                adapter.SelectCommand = command;
                _ = adapter.Fill(dataSet, dataSource.Name);
            }
        }
        catch (SqlTypeException exception)
        {
            throw new InvalidOperationException(exception.Message, exception);
        }
        catch (Exception exception)
        {
            throw new InvalidOperationException(exception.Message, exception);
        }
    }

    private static DbConnection? TryCreateConnection(DataSource dataSource, out DbProviderFactory factory)
    {
        factory = DbProviderFactories.GetFactory(dataSource.ProviderName);
        DbConnection? connection = factory.CreateConnection();

        if (connection is not null)
        {
            connection.ConnectionString = dataSource.ConnectionString;
        }

        return connection;
    }

    private static DbCommand? PrepareCommand(DbConnection? connection, DataSource dataSource, Report report, ReportFilter reportFilter)
    {
        if (connection is null)
        {
            return null;
        }

        DbCommand command = connection.CreateCommand();
        command.CommandText = dataSource.Script;
        command.CommandType = dataSource.IsStoredProcedure ? CommandType.StoredProcedure : CommandType.Text;

        if (dataSource.Parameters is not null)
        {
            foreach (Parameter parameter in dataSource.Parameters)
            {
                bool otherParameter = false;

                Filter? filter = report.Filters.FirstOrDefault(f => f.Parameter == parameter);

                filter ??= report.Filters.FirstOrDefault(f => string.Equals(f.Parameter.Name, parameter.Name, StringComparison.OrdinalIgnoreCase));

                if (filter is null)
                {
                    filter = report.Filters.Where(f => f.HasMoreParameters).FirstOrDefault(f => f.SupportsParameter(parameter));
                    otherParameter = true;
                }

                if (filter is not null)
                {
                    DbParameter dbParameter = command.CreateParameter();
                    dbParameter.ParameterName = parameter.Name;
                    if (parameter.Type != DbType.Time)
                    {
                        dbParameter.DbType = parameter.Type;
                    }

                    FilterValue? filterValue = reportFilter.FilterValues.FirstOrDefault(x => x.FilterId == filter.Id);

                    object? value = filterValue is not null ? ParameterUtils.ConvertValue(filterValue.Value, parameter.Type, filter, otherParameter) : null;

                    ParameterFactory.SetParameter(parameter, dbParameter, value);

                    _ = command.Parameters.Add(dbParameter);
                }
            }
        }

        if (report.ServerSide && report is PaginatedReport paginatedReport && paginatedReport.IsPaginationInDatabase)
        {
            if (paginatedReport.StartParameter is null || paginatedReport.LengthParameter is null)
            {
                throw new InvalidOperationException($"El reporte {report.Aka} no está bien configurado: Se ejecuta en el lado del servidor, pero no tiene los parámetros de paginación.");
            }

            DbParameter startParameter = command.CreateParameter();
            startParameter.ParameterName = paginatedReport.StartParameter.Name;
            startParameter.DbType = paginatedReport.StartParameter.Type;
            startParameter.Value = reportFilter.Page;
            _ = command.Parameters.Add(startParameter);

            DbParameter lengthParameter = command.CreateParameter();
            lengthParameter.ParameterName = paginatedReport.LengthParameter.Name;
            lengthParameter.DbType = paginatedReport.LengthParameter.Type;
            lengthParameter.Value = reportFilter.PageSize;
            _ = command.Parameters.Add(lengthParameter);
        }

        return command;
    }
}
