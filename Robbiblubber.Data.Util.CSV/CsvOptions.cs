namespace Robbiblubber.Data.Util.CSV;

using System.Text;



/// <summary>This class defines CSV export options.</summary>
public class CsvOptions
{
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // public constants                                                                                             //
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    /// <summary>Default CSV options.</summary>
    public static readonly CsvOptions DEFAULT = new();

    /// <summary>CSV options for German locale.</summary>
    public static readonly CsvOptions GERMAN = new() { Separator = ";", DecimalSeparator = "," };



    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // constructors                                                                                                 //
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    /// <summary>Creates a new instance of this class.</summary>
    public CsvOptions()
    {}


    /// <summary>Creates a new instance of this class by copying the values from the given template.</summary>
    /// <param name="template">Template to copy from.</param>
    public CsvOptions(CsvOptions template)
    {
        Separator = template.Separator;
        DecimalSeparator = template.DecimalSeparator;
        Quote = template.Quote;
        LineTerminator = template.LineTerminator;
        UseQuotes = template.UseQuotes;
        Encoding = template.Encoding;
        DateTimeFormat = template.DateTimeFormat;
        TimeSpanFormat = template.TimeSpanFormat;
        BooleanValues = template.BooleanValues;
        ReplaceInStrings = template.ReplaceInStrings;
    }


    /// <summary>Creates a new instance of this class with the given values.</summary>
    /// <param name="separator">Separator.</param>
    /// <param name="decimalSeparator">Decimal separator.</param>
    /// <param name="quote">Quote character.</param>
    /// <param name="lineTerminator">Line terminator.</param>
    /// <param name="useQuotes">Whether to use quotes.</param>
    /// <param name="encoding">Encoding.</param>
    /// <param name="dateTimeFormat">Date time format.</param>
    /// <param name="timeSpanFormat">Time span format.</param>
    /// <param name="booleanValues">Boolean values for true and false.</param>
    /// <param name="replaceInStrings">Replacements in strings.</param>
    public CsvOptions(string separator, string decimalSeparator, string quote, string lineTerminator, bool useQuotes, Encoding encoding, string dateTimeFormat, string timeSpanFormat, (string True, string False) booleanValues, IEnumerable<(string Find, string Replace)>? replaceInStrings)
    {
        Separator = separator;
        DecimalSeparator = decimalSeparator;
        Quote = quote;
        LineTerminator = lineTerminator;
        UseQuotes = useQuotes;
        Encoding = encoding;
        DateTimeFormat = dateTimeFormat;
        TimeSpanFormat = timeSpanFormat;
        BooleanValues = booleanValues;
        ReplaceInStrings = replaceInStrings;
    }



    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // public properties                                                                                            //
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    /// <summary>Gets or sets the separator.</summary>
    public string Separator
    {
        get; set;
    } = ",";


    /// <summary>Gets or sets the decimal separator.</summary>
    public string DecimalSeparator
    {
        get; set;
    } = ".";


    /// <summary>Gets or sets the quote character.</summary>
    public string Quote
    {
        get; set;
    } = "\"";


    /// <summary>Gets or sets the line terminator.</summary>
    public string LineTerminator
    {
        get; set;
    } = "\n";


    /// <summary>Gets or sets whether to use quotes.</summary>
    public bool UseQuotes
    {
        get; set;
    } = true;


    /// <summary>Gets or sets the encoding.</summary>
    public Encoding Encoding
    {
        get; set;
    } = Encoding.UTF8;


    /// <summary>Gets or sets the date time format.</summary>
    public string DateTimeFormat
    {
        get; set;
    } = "yyyy-MM-dd HH:mm:ss";


    /// <summary>Gets or sets the time span format.</summary>
    public string TimeSpanFormat
    {
        get; set;
    } = "hh:mm:ss";


    /// <summary>Gets or sets the boolean values.</summary>
    public (string True, string False) BooleanValues
    {
        get; set;
    } = ("true", "false");


    /// <summary>Gets or sets the replacements in strings.</summary>
    /// <remarks>If set, this replaces the escape sequences, even when using quotes.</remarks>
    public IEnumerable<(string Find, string Replace)>? ReplaceInStrings
    {
        get; set;
    }



    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // public methods                                                                                               //
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
     /// <summary>Clones this instance.</summary>
     /// <returns>Returns a clone of this instance.</returns>
     public CsvOptions Clone()
     {
         return new CsvOptions(Separator, DecimalSeparator, Quote, LineTerminator, UseQuotes, Encoding, DateTimeFormat, TimeSpanFormat, BooleanValues, ReplaceInStrings);
     }


    /// <summary>Clones this instance with the given modifications.</summary>
    /// <param name="separator">Separator.</param>
    /// <param name="decimalSeparator">Decimal separator.</param>
    /// <param name="quote">Quote character.</param>
    /// <param name="lineTerminator">Line terminator.</param>
    /// <param name="useQuotes">Whether to use quotes.</param>
    /// <param name="encoding">Encoding.</param>
    /// <param name="dateTimeFormat">Date time format.</param>
    /// <param name="timeSpanFormat">Time span format.</param>
    /// <param name="booleanValues">Boolean values.</param>
    /// <param name="replaceInStrings">Replacements in strings.</param>
    /// <returns>Returns a clone of this instance with the given modifications.</returns>
    public CsvOptions Clone(string? separator = null, string? decimalSeparator = null, string? quote = null, string? lineTerminator = null, bool? useQuotes = null, Encoding? encoding = null, string? dateTimeFormat = null, string? timeSpanFormat = null, (string True, string False)? booleanValues = null, IEnumerable<(string Find, string Replace)>? replaceInStrings = null)
    {
        return new CsvOptions(separator ?? Separator, decimalSeparator ?? DecimalSeparator, quote ?? Quote, lineTerminator ?? LineTerminator, useQuotes ?? UseQuotes, encoding ?? Encoding, dateTimeFormat ?? DateTimeFormat, timeSpanFormat ?? TimeSpanFormat, booleanValues ?? BooleanValues, replaceInStrings ?? ReplaceInStrings);
    }


    /// <summary>Sets the given modifications on this instance.</summary>
    /// <param name="separator">Separator.</param>
    /// <param name="decimalSeparator">Decimal separator.</param>
    /// <param name="quote">Quote character.</param>
    /// <param name="lineTerminator">Line terminator.</param>
    /// <param name="useQuotes">Whether to use quotes.</param>
    /// <param name="encoding">Encoding.</param>
    /// <param name="dateTimeFormat">Date time format.</param>
    /// <param name="timeSpanFormat">Time span format.</param>
    /// <param name="booleanValues">Boolean values.</param>
    /// <param name="replaceInStrings">Replacements in strings.</param>
    /// <returns>Returns this instance with the given modifications.</returns>
    public virtual CsvOptions Set(string? separator = null, string? decimalSeparator = null, string? quote = null, string? lineTerminator = null, bool? useQuotes = null, Encoding? encoding = null, string? dateTimeFormat = null, string? timeSpanFormat = null, (string True, string False)? booleanValues = null, IEnumerable<(string Find, string Replace)>? replaceInStrings = null)
    {
        if (separator != null) Separator = separator;
        if (decimalSeparator != null) DecimalSeparator = decimalSeparator;
        if (quote != null) Quote = quote;
        if (lineTerminator != null) LineTerminator = lineTerminator;
        if (useQuotes != null) UseQuotes = useQuotes.Value;
        if (encoding != null) Encoding = encoding;
        if (dateTimeFormat != null) DateTimeFormat = dateTimeFormat;
        if (timeSpanFormat != null) TimeSpanFormat = timeSpanFormat;
        if (booleanValues != null) BooleanValues = booleanValues.Value;
        if (replaceInStrings != null) ReplaceInStrings = replaceInStrings;

        return this;
    }
}
