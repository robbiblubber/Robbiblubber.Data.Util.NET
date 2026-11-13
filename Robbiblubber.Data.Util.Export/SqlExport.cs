namespace Robbiblubber.Data.Util.Export;

using System.Data;



/// <summary>This class provides SQL export functionality.</summary>
public class SqlExport: IDisposable
{
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // protected members                                                                                                //
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    /// <summary>Disposed flag.</summary>
    protected bool _Disposed = false;


    /// <summary>Database connection.</summary>
    protected IDbConnection? _Connection = null;


    /// <summary>Result statements.</summary>
    protected string[] _Statements = Array.Empty<string>();
    


    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // constructors                                                                                                     //
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    /// <summary>Creates a new instance of this object.</summary>
    public SqlExport()
    {}


    /// <summary>Creates a new instance of this object.</summary>
    /// <param name="connection">The database connection to use for exporting data.</param>
    public SqlExport(IDbConnection connection)
    {
        _Connection = connection;
    }


    /// <summary>Creates a new instance of this object.</summary>
    /// <param name="connection">The database connection to use for exporting data.</param>
    /// <param name="tableName">The name of the table to export to.</param>
    /// <param name="query">The SQL query to execute for exporting data.</param>
    public SqlExport(IDbConnection connection, string tableName, string query)
    {
        _Connection = connection;
        TableName = tableName;
        Query = query;
    }



    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // public properties                                                                                                //
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    /// <summary>Gets or sets the name of the table to export to.</summary>
    public string TableName 
    { 
        get; set; 
    } = string.Empty;


    /// <summary>Gets or sets the SQL query to execute for exporting data.</summary>
    public string Query
    {
        get; set;
    } = string.Empty;


    /// <summary>Gets or sets the database connection to use for exporting data.</summary>
    public IDbConnection? Connection
    {
        get
        {
            if(_Disposed) { throw new ObjectDisposedException(nameof(SqlExport)); }
            return _Connection;
        }
        set
        {
            if(_Disposed) { throw new ObjectDisposedException(nameof(SqlExport)); }
            _Connection = value;
        }
    }


    /// <summary>Gets the result statements.</summary>
    public string[] Statements
    {
        get
        {
            if(_Disposed) { throw new ObjectDisposedException(nameof(SqlExport)); }
            return _Statements;
        }
    }



    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // public methods                                                                                                   //
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    /// <summary>Executes the export operation.</summary>
    public virtual void Execute()
    {
        if(_Disposed) { throw new ObjectDisposedException(nameof(SqlExport)); }
        List<string> rval = new();

        using IDbCommand cmd = _Connection!.CreateCommand();
        cmd.CommandText = Query;
        using IDataReader re = cmd.ExecuteReader();

        while(re.Read())
        {
            List<string> values = new();
            for(int i = 0; i < re.FieldCount; i++)
            {
                object val = re.GetValue(i);
                if(val is string) { values.Add($"'{val.ToString()?.Replace("'", "''")}'"); }
                else if(val is DateTime dt) { values.Add($"'{dt:yyyy-MM-dd HH:mm:ss}'"); }
                else if(val is null || val == DBNull.Value) { values.Add("NULL"); }
                else if(val is float || val is double || val is decimal) { values.Add(Convert.ToString(val, System.Globalization.CultureInfo.InvariantCulture) ?? "NULL"); }
                else { values.Add(val.ToString() ?? "NULL"); }
            }
            rval.Add($"INSERT INTO {TableName} ({string.Join(", ", Enumerable.Range(0, re.FieldCount).Select(i => re.GetName(i)))}) VALUES ({string.Join(", ", values)});");
        }
        re.Close();

        _Statements = rval.ToArray();
    }



    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // [override] object                                                                                                //
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    /// <summary>Returns a string that represents the current object.</summary>
    /// <returns>A string with all statements of the export.</returns>
    public override string ToString()
    {
        return string.Join(Environment.NewLine, _Statements);
    }



    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // [interface] IDisposable                                                                                          //
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public void Dispose()
    {
        _Disposed = true;
        _Statements = Array.Empty<string>();
        _Connection = null;
    }
}
