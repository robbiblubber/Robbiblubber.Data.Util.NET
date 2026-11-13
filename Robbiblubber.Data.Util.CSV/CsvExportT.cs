namespace Robbiblubber.Data.Util.CSV;

using System.Globalization;



/// <summary>This class provides CSV export functionality for data.</summary>
/// <typeparam name="T">Type.</typeparam>
public class CsvExport<T>: CsvExport
{
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // constructors                                                                                                 //
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    /// <summary>Creates a new instance of this class.</summary>
    /// <param name="fieldDefinition">Field definition for the export.</param>
    /// <param name="csvOptions">CSV options for the export.</param>
    /// <param name="hasHeader">Whether to include a header row.</param>
    /// <param name="data">Data to export.</param>
    public CsvExport(IEnumerable<DataField<T>>? fieldDefinition = null, CsvOptions? csvOptions = null, bool hasHeader = true, IEnumerable<T>? data = null)
    {
        Data = data;
        HasHeader = hasHeader;
        FieldDefinition = fieldDefinition;
        CsvOptions = csvOptions;
    }



    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // public properties                                                                                            //
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    /// <summary>Gets or sets the data.</summary>
    public IEnumerable<T>? Data
    { 
        get; set;
    }


    /// <summary>Gets or sets whether to include a header.</summary>
    public bool HasHeader
    { 
        get; set; 
    } = true;


    /// <summary>Gets or sets the export data definition.</summary>
    public IEnumerable<DataField<T>>? FieldDefinition
    {
        get; set;
    }


    /// <summary>Gets or sets the CSV export options.</summary>
    public CsvOptions? CsvOptions
    {
        get; set;
    }



    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // public methods                                                                                               //
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    /// <summary>Writes the CSV export to the given text writer.</summary>
    /// <param name="writer">The text writer to write to.</param>
    /// <param name="data">Data.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Returns a task that represents the asynchronous operation.</returns>
    public virtual async Task WriteToTextWriterAsync(TextWriter writer, IEnumerable<T> data, CancellationToken cancellationToken = default)
    {
        Data = data;
        await WriteToTextWriterAsync(writer, cancellationToken).ConfigureAwait(false);
    }


    /// <summary>Writes the CSV export to the given text writer.</summary>
    /// <param name="writer">The text writer to write to.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Returns a task that represents the asynchronous operation.</returns>
    public virtual async Task WriteToTextWriterAsync(TextWriter writer, CancellationToken cancellationToken = default)
    {
        CsvOptions options = CsvOptions ?? CsvOptions.DEFAULT;
        DataField<T>[] definition = FieldDefinition?.ToArray() ?? [];

        if(HasHeader)
        {
            for(int i = 0; i < definition.Length; i++)
            {
                if(i > 0) { await writer.WriteAsync(options.Separator).ConfigureAwait(false); }
                await writer.WriteAsync(_Format(definition[i].HeaderText, options)).ConfigureAwait(false);
            }
            await writer.WriteAsync(options.LineTerminator).ConfigureAwait(false);
        }

        foreach(T k in Data ?? [])
        {
            cancellationToken.ThrowIfCancellationRequested();

            for(int i = 0; i < definition.Length; i++)
            {
                if(i > 0) { await writer.WriteAsync(options.Separator).ConfigureAwait(false); }

                await writer.WriteAsync(_Format(definition[i].ValueFunc?.Invoke(k), options)).ConfigureAwait(false);
            }
            await writer.WriteAsync(options.LineTerminator).ConfigureAwait(false);
        }
    }


    /// <summary>Writes the CSV export to the given text writer.</summary>
    /// <param name="writer">The text writer to write to.</param>
    /// <param name="data">Data.</param>
    public virtual void WriteToTextWriter(TextWriter writer, IEnumerable<T> data)
    {
        Data = data;
        WriteToTextWriter(writer);
    }


    /// <summary>Writes the CSV export to the given text writer.</summary>
    /// <param name="writer">The text writer to write to.</param>
    public virtual void WriteToTextWriter(TextWriter writer)
    {
        CsvOptions options = CsvOptions ?? CsvOptions.DEFAULT;
        DataField<T>[] definition = FieldDefinition?.ToArray() ?? [];

        if(HasHeader)
        {
            for(int i = 0; i < definition.Length; i++)
            {
                if(i > 0) { writer.Write(options.Separator); }
                writer.Write(_Format(definition[i].HeaderText, options));
            }
            writer.Write(options.LineTerminator);
        }

        foreach(T k in Data ?? [])
        {
            for(int i = 0; i < definition.Length; i++)
            {
                if(i > 0) { writer.Write(options.Separator); }

                writer.Write(_Format(definition[i].ValueFunc?.Invoke(k), options));
            }
            writer.Write(options.LineTerminator);
        }
    }


    /// <summary>Writes the CSV export to a string.</summary>
    /// <param name="data">Data.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Returns a task that represents the asynchronous operation, with the CSV export as a string as result.</returns>
    public virtual async Task<string> WriteToStringAsync(IEnumerable<T> data, CancellationToken cancellationToken = default)
    {
        Data = data;
        return await WriteToStringAsync(cancellationToken).ConfigureAwait(false);
    }


    /// <summary>Writes the CSV export to a string.</summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Returns a task that represents the asynchronous operation, with the CSV export as a string as result.</returns>
    public virtual async Task<string> WriteToStringAsync(CancellationToken cancellationToken = default)
    {
        await using StringWriter writer = new StringWriter();
        await WriteToTextWriterAsync(writer, cancellationToken).ConfigureAwait(false);
        
        return writer.ToString();
    }


    /// <summary>Writes the CSV export to a string.</summary>
    /// <returns>Returns the CSV export as a string.</returns>
    public virtual string WriteToString(IEnumerable<T> data)
    {
        Data = data;
        return WriteToString();
    }


    /// <summary>Writes the CSV export to a string.</summary>
    /// <returns>Returns the CSV export as a string.</returns>
    public virtual string WriteToString()
    {
        using StringWriter writer = new StringWriter();
        WriteToTextWriter(writer);
        
        return writer.ToString();
    }


    /// <summary>Writes the CSV export to the given stream.</summary>
    /// <param name="data">Data.</param>
    /// <param name="stream">The stream to write to.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Returns a task that represents the asynchronous operation.</returns>
    public virtual async Task WriteToStreamAsync(Stream stream, IEnumerable<T> data, CancellationToken cancellationToken = default)
    {
        Data = data;
        await WriteToStreamAsync(stream, cancellationToken).ConfigureAwait(false);
    }


    /// <summary>Writes the CSV export to the given stream.</summary>
    /// <param name="stream">The stream to write to.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Returns a task that represents the asynchronous operation.</returns>
    public virtual async Task WriteToStreamAsync(Stream stream, CancellationToken cancellationToken = default)
    {
        await using StreamWriter writer = new StreamWriter(stream, (CsvOptions ?? CsvOptions.DEFAULT).Encoding, 1024, true);
        await WriteToTextWriterAsync(writer, cancellationToken).ConfigureAwait(false);
        await writer.FlushAsync().ConfigureAwait(false);
    }


    /// <summary>Writes the CSV export to the given stream.</summary>
    /// <param name="stream">The stream to write to.</param>
    /// <param name="data">Data.</param>
    public virtual void WriteToStream(Stream stream, IEnumerable<T> data)
    {
        Data = data;
        WriteToStream(stream);
    }


    /// <summary>Writes the CSV export to the given stream.</summary>
    /// <param name="stream">The stream to write to.</param>
    public virtual void WriteToStream(Stream stream)
    {
        using StreamWriter writer = new StreamWriter(stream, (CsvOptions ?? CsvOptions.DEFAULT).Encoding, 1024, true);
        WriteToTextWriter(writer);
        writer.Flush();
    }


    /// <summary>Writes the CSV export to the given file.</summary>
    /// <param name="fileName">The file to write to.</param>
    /// <param name="data">Data.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Returns a task that represents the asynchronous operation.</returns>
    public virtual async Task WriteToFileAsync(string fileName, IEnumerable<T> data, CancellationToken cancellationToken = default)
    {
        Data = data;
        await WriteToFileAsync(fileName, cancellationToken).ConfigureAwait(false);
    }


    /// <summary>Writes the CSV export to the given file.</summary>
    /// <param name="fileName">The file to write to.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Returns a task that represents the asynchronous operation.</returns>
    public virtual async Task WriteToFileAsync(string fileName, CancellationToken cancellationToken = default)
    {
        await using FileStream stream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None, 4096, true);
        await WriteToStreamAsync(stream, cancellationToken).ConfigureAwait(false);
    }


    /// <summary>Writes the CSV export to the given file.</summary>
    /// <param name="data">Data.</param>
    /// <param name="fileName">The file to write to.</param>
    public virtual void WriteToFile(string fileName, IEnumerable<T> data)
    {
        Data = data;
        WriteToFile(fileName);
    }


    /// <summary>Writes the CSV export to the given file.</summary>
    /// <param name="fileName">The file to write to.</param>
    public virtual void WriteToFile(string fileName)
    {
        using FileStream stream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None, 4096, true);
        WriteToStream(stream);
    }


    /// <summary>Clones this instance.</summary>
    /// <returns>Returns a clone of this export.</returns>
    public virtual CsvExport Clone()
    {
        return new CsvExport<T>(FieldDefinition, CsvOptions, HasHeader, Data);
    }


    /// <summary>Clones this instance with the given values.</summary>
    /// <param name="fieldDefinition">Field definition for the export.</param>
    /// <param name="csvOptions">CSV options for the export.</param>
    /// <param name="hasHeader">Whether to include a header row.</param>
    /// <param name="data">Data to export.</param>
    /// <returns>Returns a clone of this export with the given values.</returns>
    public virtual CsvExport Clone(IEnumerable<DataField<T>>? fieldDefinition = null, CsvOptions? csvOptions = null, bool? hasHeader = null, IEnumerable<T>? data = null)
    {
        return new CsvExport<T>(fieldDefinition ?? FieldDefinition, csvOptions ?? CsvOptions, hasHeader ?? HasHeader, data ?? Data);
    }


    /// <summary>Sets the given values on this instance.</summary>
    /// <param name="fieldDefinition">Field definition for the export.</param>
    /// <param name="csvOptions">CSV options for the export.</param>
    /// <param name="hasHeader">Whether to include a header row.</param>
    /// <param name="data">Data to export.</param>
    /// <returns>Returns this instance with the given modifications.</returns>
    public virtual CsvExport Set(IEnumerable<DataField<T>>? fieldDefinition = null, CsvOptions? csvOptions = null, bool? hasHeader = null, IEnumerable<T>? data = null)
    {
        if(fieldDefinition is not null) { FieldDefinition = fieldDefinition; }
        if (csvOptions is not null) { CsvOptions = csvOptions; }
        if (hasHeader is not null) { HasHeader = hasHeader.Value; }
        if (data is not null) { Data = data; }

        return this;
    }



    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // protected methods                                                                                            //
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    /// <summary>Formats a value.</summary>
    /// <param name="value">Value.</param>
    /// <param name="options">Options.</param>
    /// <returns>Returns the formatted value.</returns>
    protected virtual string _Format(object? value, CsvOptions options)
    {
        if(value is null) { return ""; }
        if(value is string s)
        {
            if(options.ReplaceInStrings is not null)
            {
                foreach((string Find, string Replace) in options.ReplaceInStrings) { s = s.Replace(Find, Replace); }
            }
            else if(options.UseQuotes) { s = s.Replace(options.Quote, options.Quote + options.Quote); }
            
            return (options.UseQuotes ? (options.Quote + s + options.Quote) : s);
        }
        if(value is IFormattable f) { return f.ToString(null, CultureInfo.InvariantCulture).Replace(".", options.DecimalSeparator); }
        else
        {
            return value.ToString() ?? "";
        }
    }



    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // [override] object                                                                                            //
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    /// <summary>Returns a string representation of this instance.</summary>
    /// <returns>Returns a string that represents this instance.</returns>
    public override string ToString()
    {
        return WriteToString();
    }
}
