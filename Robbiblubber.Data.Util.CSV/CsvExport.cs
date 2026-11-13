namespace Robbiblubber.Data.Util.CSV;



/// <summary>This class provides configuration data types for the CSV export.</summary>
public abstract class CsvExport
{
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // [class] DataField<T>                                                                                         //
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    /// <summary>This class ^defines a data field.</summary>
    /// <typeparam name="T">Type.</typeparam>
    public class DataField<T>
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // constructors                                                                                             //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Creates a new instance of this class.</summary>
        public DataField()
        { }


        /// <summary>Creates a new instance of this class by copying the values from the given template.</summary>
        /// <param name="template">Template to copy from.</param>
        public DataField(DataField<T> template)
        {
            HeaderText = template.HeaderText;
            ValueFunc = template.ValueFunc;
        }


        /// <summary>Creates a new instance of this class.</summary>
        /// <param name="headerText">Header text.</param>
        /// <param name="valueFunc">Value function.</param>
        public DataField(string? headerText, Func<T, object?>? valueFunc)
        {
            HeaderText = headerText;
            ValueFunc = valueFunc;
        }



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public properties                                                                                        //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Gets or sets the header text.</summary>
        public string? HeaderText
        {
            get; set;
        }


        /// <summary>Gets or sets the value function.</summary>
        /// <remarks>This method is called for each cell in the column. It provides the value for the cell.</remarks>
        public Func<T, object?>? ValueFunc
        {
            get; set;
        }



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public methods                                                                                           //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Clones this instance.</summary>
        /// <returns>Returns a clone of this instance.</returns>
        public virtual DataField<T> Clone()
        {
            return new DataField<T>(HeaderText, ValueFunc);
        }


        /// <summary>Clones this instance with new values.</summary>
        /// <param name="headerText">Header text.</param>
        /// <param name="valueFunc">Value function.</param>
        /// <returns>Returns a clone of this instance with new values.</returns>
        public virtual DataField<T> Clone(string? headerText, Func<T, object?>? valueFunc)
        {
            return new DataField<T>(headerText ?? HeaderText, valueFunc ?? ValueFunc);
        }


        /// <summary>Allows to set values in a fluent manner.</summary>
        /// <param name="headerText">Header text.</param>
        /// <param name="valueFunc">Value function.</param>
        /// <returns>Returns this instance with the given modifications.</returns>
        public virtual DataField<T> Set(string? headerText, Func<T, object?>? valueFunc)
        {
            if(headerText is not null) { HeaderText = headerText; }
            if(valueFunc is not null) { ValueFunc = valueFunc; }

            return this;
        }
    }
}
