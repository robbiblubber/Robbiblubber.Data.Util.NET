namespace Robbiblubber.Data.Util.Excel;

using ClosedXML.Excel;



/// <summary>This class provides Excel export functionality for data.</summary>
/// <typeparam name="T">Type.</typeparam>
public class ExcelExport<T>: ExcelExport
{
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // constructors                                                                                                 //
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    /// <summary>Creates a new instance of this class.</summary>
    public ExcelExport(IEnumerable<DataField<T>>? fieldDefinition = null, IEnumerable<double>? columnWidths = null, string? worksheetName = null, bool hasHeader = true, IEnumerable<T>? data = null)
    {
        FieldDefinition = fieldDefinition;
        ColumnWidths = columnWidths;
        WorksheetName = worksheetName;
        HasHeader = hasHeader;
        Data = data;
    }


    /// <summary>Creates a new instance of this class by copying the properties of the specified template.</summary>
    /// <param name="template">The template to copy the properties from.</param>
    public ExcelExport(ExcelExport<T> template)
    {
        FieldDefinition = template.FieldDefinition;
        ColumnWidths = template.ColumnWidths;
        WorksheetName = template.WorksheetName;
        HasHeader = template.HasHeader;
        Data = template.Data;
    }



    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // public properties                                                                                            //
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    /// <summary>Gets or sets the data.</summary>
    public IEnumerable<T>? Data
    {
        get; set;
    } = Array.Empty<T>();


    /// <summary>Gets or sets whether to include a header.</summary>
    public bool HasHeader
    {
        get; set;
    } = true;


    /// <summary>Gets or sets the worksheet name.</summary>
    public string? WorksheetName
    {
        get; set;
    }

    /// <summary>Gets or sets the export data definition.</summary>
    public IEnumerable<DataField<T>>? FieldDefinition
    {
        get; set;
    }


    /// <summary>Gets or sets the column widths (only for Excel export).</summary>
    public IEnumerable<double>? ColumnWidths
    {
        get; set;
    }



    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // public methods                                                                                               //
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    /// <summary>Writes the export data to a file.</summary>
    /// <param name="fileName">The name of the file to write to.</param>
    public virtual void WriteToFile(string fileName)
    {
        using XLWorkbook wb = _BuildWorkbook();
        wb.SaveAs(fileName);
    }


    /// <summary>Writes the export data to a file.</summary>
    /// <param name="fileName">The name of the file to write to.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Returns a task that represents the asynchronous operation.</returns>
    public virtual async Task WriteToFileAsync(string fileName, CancellationToken cancellationToken = default)
    {
        await Task.Run(() => WriteToFile(fileName), cancellationToken).ConfigureAwait(true);
    }


    /// <summary>Writes the export data to a file.</summary>
    /// <param name="fileName">The name of the file to write to.</param>
    /// <param name="data">Data.</param>
    public virtual void WriteToFile(string fileName, IEnumerable<T> data)
    {
        Data = data;
        WriteToFile(fileName);
    }


    /// <summary>Writes the export data to a file.</summary>
    /// <param name="fileName">The name of the file to write to.</param>
    /// <param name="data">Data.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Returns a task that represents the asynchronous operation.</returns>
    public virtual async Task WriteToFileAsync(string fileName, IEnumerable<T> data, CancellationToken cancellationToken = default)
    {
        await Task.Run(() => WriteToFile(fileName, data), cancellationToken).ConfigureAwait(true);
    }


    /// <summary>Writes the export data to a stream.</summary>
    /// <param name="stream">The stream to write to.</param>
    public virtual void WriteToStream(Stream stream)
    {
        using XLWorkbook wb = _BuildWorkbook();
        wb.SaveAs(stream);
    }


    /// <summary>Writes the export data to a stream.</summary>
    /// <param name="stream">The stream to write to.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Returns a task that represents the asynchronous operation.</returns>
    public virtual async Task WriteToStreamAsync(Stream stream, CancellationToken cancellationToken = default)
    {
        await Task.Run(() => WriteToStream(stream), cancellationToken).ConfigureAwait(true);
    }


    /// <summary>Writes the export data to a stream.</summary>
    /// <param name="stream">The stream to write to.</param>
    /// <param name="data">Data.</param>
    public virtual void WriteToStream(Stream stream, IEnumerable<T> data)
    {
        Data = data;
        WriteToStream(stream);
    }


    /// <summary>Writes the export data to a stream.</summary>
    /// <param name="stream">The stream to write to.</param>
    /// <param name="data">Data.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Returns a task that represents the asynchronous operation.</returns>
    public virtual async Task WriteToStreamAsync(Stream stream, IEnumerable<T> data, CancellationToken cancellationToken = default)
    {
        await Task.Run(() => WriteToStream(stream, data), cancellationToken).ConfigureAwait(true);
    }


    /// <summary>Creates a new instance of this class by copying the properties of this instance.</summary>
    /// <returns>Returns a cloned instance of this class.</returns>
    public virtual ExcelExport<T> Clone()
    {
        return new ExcelExport<T>(this);
    }


    /// <summary>Creates a new instance of this class by copying the properties of this instance and overriding the specified properties.</summary>
    /// <param name="fieldDefinition">The field definition to override.</param>
    /// <param name="columnWidths">The column widths to override.</param>
    /// <param name="worksheetName">The worksheet name to override.</param>
    /// <param name="hasHeader">Whether to include a header.</param>
    /// <param name="data">The data to override.</param>
    /// <returns>Returns a cloned instance of this class.</returns>
    public virtual ExcelExport<T> Clone(IEnumerable<DataField<T>>? fieldDefinition = null, IEnumerable<double>? columnWidths = null, string? worksheetName = null, bool? hasHeader = null, IEnumerable<T>? data = null)
    {
        return new ExcelExport<T>(fieldDefinition ?? FieldDefinition, columnWidths ?? ColumnWidths, worksheetName ?? WorksheetName, hasHeader ?? HasHeader, data ?? Data);
    }


    /// <summary>Sets the specified properties of this instance.</summary>
    /// <param name="fieldDefinition">The field definition to set.</param>
    /// <param name="columnWidths">The column widths to set.</param>
    /// <param name="worksheetName">The worksheet name to set.</param>
    /// <param name="hasHeader">Whether to include a header.</param>
    /// <param name="data">The data to set.</param>
    /// <returns>Returns this instance.</returns>
    public virtual ExcelExport Set(IEnumerable<DataField<T>>? fieldDefinition = null, IEnumerable<double>? columnWidths = null, string? worksheetName = null, bool? hasHeader = null, IEnumerable<T>? data = null)
    {
        if(fieldDefinition is not null) { FieldDefinition = fieldDefinition; }
        if(columnWidths is not null) { ColumnWidths = columnWidths; }
        if(worksheetName is not null) { WorksheetName = worksheetName; }
        if(hasHeader is not null) { HasHeader = hasHeader.Value; }
        if(data is not null) { Data = data; }

        return this;
    }



    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // protected methods                                                                                            //
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    /// <summary>Builds the Excel workbook.</summary>
    /// <returns>Returns the built workbook.</returns>
    protected virtual XLWorkbook _BuildWorkbook()
    {
        DataField<T>[] definition = FieldDefinition?.ToArray() ?? [];

        XLWorkbook wb = new();
        IXLWorksheet ws;
        if(string.IsNullOrEmpty(WorksheetName)) { ws = wb.Worksheets.Add(); }
        else { ws = wb.Worksheets.Add(WorksheetName); }

        int row = 1;

        if(HasHeader)
        {
            for(int i = 0; i < definition.Length; i++)
            {
                _FillCell(ws.Cell(row, i + 1), definition[i].HeaderText, definition[i].HeaderFormat);
            }
            row++;
        }

        T[] data = (Data ?? []).ToArray();
        for(int k = 0; k < data.Length; k++)
        {
            for(int i = 0; i < definition.Length; i++)
            {
                CellFormat? f = definition[i].DataFormat;
                if(k == 0) { f = definition[i].FirstRowDataFormat ?? f; }
                if(k == (data.Length - 1)) { f = definition[i].LastRowDataFormat ?? f; }

                _FillCell(ws.Cell(row, i + 1), definition[i].ValueFunc?.Invoke(data[k]), f);
                if(definition[i].CustomizeCell is not null)
                {
                    try { definition[i].CustomizeCell?.Invoke(ws.Cell(row, i + 1), data[k]); } catch(Exception) {}
                }
            }
            row++;
        }

        if(ColumnWidths is not null)
        {
            double[] widths = ColumnWidths.ToArray();
            for(int i = 0; i < definition.Length; i++) { ws.Column(i + 1).Width = widths[i]; }
        }

        return wb;
    }



    /// <summary>Fills a cell.</summary>
    /// <param name="cell">Cell.</param>
    /// <param name="value">Cell value.</param>
    /// <param name="f">Cell format.</param>
    protected virtual void _FillCell(IXLCell cell, object? value, CellFormat? f)
    {
        if(f?.BackgroundColor is not null) { cell.Style.Fill.BackgroundColor = f.BackgroundColor; }
        if(f?.FontColor is not null) { cell.Style.Font.FontColor = f.FontColor; }
        if(f?.LeftBorder is not null) { cell.Style.Border.LeftBorder = f.LeftBorder.Value; }
        if(f?.RightBorder is not null) { cell.Style.Border.RightBorder = f.RightBorder.Value; }
        if(f?.TopBorder is not null) { cell.Style.Border.TopBorder = f.TopBorder.Value; }
        if(f?.BottomBorder is not null) { cell.Style.Border.BottomBorder = f.BottomBorder.Value; }

        if(f?.Bold ?? false) { cell.Style.Font.Bold = true; }
        if(f?.Italic ?? false) { cell.Style.Font.Italic = true; }

        if(f?.DataType is null)
        {
            if(value is null) { cell.Value = string.Empty; }
            else if(value is bool) { cell.Value = (bool) value; }
            else if(value is DateTime) { cell.Value = (DateTime) value; }
            else if(value is TimeSpan) { cell.Value = (TimeSpan) value; }
            else if(value is double || value is float || value is decimal) { cell.Value = Convert.ToDouble(value); }
            else if(value is int || value is long || value is short || value is byte) { cell.Value = Convert.ToInt64(value); }
            else { cell.Value = value.ToString(); }
        }
        else
        {
            switch(f.DataType)
            {
                case XLDataType.Blank: cell.Value = string.Empty; break;
                case XLDataType.Boolean:
                    if(value is bool b) { cell.Value = b; break; }
                    cell.Value = value?.ToString() ?? string.Empty; break;
                case XLDataType.DateTime:
                    if(value is DateTime dt) { cell.Value = dt; break; }
                    cell.Value = value?.ToString() ?? string.Empty; break;
                case XLDataType.TimeSpan:
                    if(value is TimeSpan ts) { cell.Value = ts; break; }
                    cell.Value = value?.ToString() ?? string.Empty; break;
                case XLDataType.Number:
                    if(value is double || value is float || value is decimal) { cell.Value = Convert.ToDouble(value); break; }
                    if(value is int || value is long || value is short || value is byte) { cell.Value = Convert.ToInt64(value); break; }
                    cell.Value = value?.ToString() ?? string.Empty; break;
                default:
                    cell.Value = value?.ToString() ?? string.Empty; break;
            }
        }
    }
}