namespace Robbiblubber.Data.Util.Excel;

using ClosedXML.Excel;



/// <summary>This class provides configuration data types for the Excel export.</summary>
public abstract class ExcelExport
{
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // [class] DataField<T>                                                                                         //
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
    /// <summary>This class provides a data field.</summary>
    /// <typeparam name="T">Type.</typeparam>
    public class DataField<T>
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // constructors                                                                                             //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>Creates a new instance of this class.</summary>
        /// <param name="headerText">The header text.</param>
        /// <param name="valueFunc">The value function.</param>
        /// <param name="customizeCell">The cell action.</param>
        /// <param name="headerFormat">The header format.</param>
        /// <param name="dataFormat">The data format.</param>
        /// <param name="firstRowDataFormat">The first row data format.</param>
        /// <param name="lastRowDataFormat">The last row data format.</param>
        public DataField(string? headerText = null, Func<T, object?>? valueFunc = null, Action<IXLCell, T>? customizeCell = null, CellFormat? headerFormat = null, CellFormat? dataFormat = null, CellFormat? firstRowDataFormat = null, CellFormat? lastRowDataFormat = null)
        {
            HeaderText = headerText;
            ValueFunc = valueFunc;
            CustomizeCell = customizeCell;
            HeaderFormat = headerFormat;
            DataFormat = dataFormat;
            FirstRowDataFormat = firstRowDataFormat;
            LastRowDataFormat = lastRowDataFormat;
        }

        
        /// <summary>Creates a new instance of this class by copying the specified template.</summary>
        /// <param name="template">The template.</param>
        public DataField(DataField<T> template)
        {
            HeaderText = template.HeaderText;
            ValueFunc = template.ValueFunc;
            CustomizeCell = template.CustomizeCell;
            HeaderFormat = template.HeaderFormat;
            DataFormat = template.DataFormat;
            FirstRowDataFormat = template.FirstRowDataFormat;
            LastRowDataFormat = template.LastRowDataFormat;
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
        public Func<T, object?>? ValueFunc
        {
            get; set;
        }


        /// <summary>Gets or sets the customize cell action.</summary>
        public Action<IXLCell, T>? CustomizeCell
        {
            get; set;
        }


        /// <summary>Gets or sets the header format.</summary>
        public CellFormat? HeaderFormat
        {
            get; set;
        }


        /// <summary>Gets or sets the data format.</summary>
        public CellFormat? DataFormat
        {
            get; set;
        }


        /// <summary>Gets or sets the data format for the first row.</summary>
        public CellFormat? FirstRowDataFormat
        {
            get; set;
        }


        /// <summary>Gets or sets the data format for the last row.</summary>
        public CellFormat? LastRowDataFormat
        {
            get; set;
        }



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public methods                                                                                           //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Clones this instance.</summary>
        /// <returns>Returns a new instance that is a copy of this instance.</returns>
        public DataField<T> Clone()
        {
            return new DataField<T>(this);
        }


        /// <summary>Clones this instance, optionally replacing some of the properties.</summary>
        /// <param name="headerText">The new header text.</param>
        /// <param name="valueFunc">The new value function.</param>
        /// <param name="customizeCell">The new cell action.</param>
        /// <param name="headerFormat">The new header format.</param>
        /// <param name="dataFormat">The new data format.</param>
        /// <param name="firstRowDataFormat">The new first row data format.</param>
        /// <param name="lastRowDataFormat">The new last row data format.</param>
        /// <returns>Returns a new instance that is a copy of this instance.</returns>
        public DataField<T> Clone(string? headerText = null, Func<T, object?>? valueFunc = null, Action<IXLCell, T>? customizeCell = null, CellFormat? headerFormat = null, CellFormat? dataFormat = null, CellFormat? firstRowDataFormat = null, CellFormat? lastRowDataFormat = null)
        {
            return new DataField<T>(headerText ?? HeaderText, valueFunc ?? ValueFunc, customizeCell ?? CustomizeCell, headerFormat ?? HeaderFormat, dataFormat ?? DataFormat, firstRowDataFormat ?? FirstRowDataFormat, lastRowDataFormat ?? LastRowDataFormat);
        }


        /// <summary>Sets the properties of this instance.</summary>
        /// <returns>Returns this instance.</returns>
        public DataField<T> Set(string? headerText = null, Func<T, object?>? valueFunc = null, Action<IXLCell, T>? customizeCell = null, CellFormat? headerFormat = null, CellFormat? dataFormat = null, CellFormat? firstRowDataFormat = null, CellFormat? lastRowDataFormat = null)
        {
            if(headerText is not null) { HeaderText = headerText; }
            if(valueFunc is not null) { ValueFunc = valueFunc; }
            if(customizeCell is not null) { CustomizeCell = customizeCell; }
            if(headerFormat is not null) { HeaderFormat = headerFormat; }
            if(dataFormat is not null) { DataFormat = dataFormat; }
            if(firstRowDataFormat is not null) { FirstRowDataFormat = firstRowDataFormat; }
            if(lastRowDataFormat is not null) { LastRowDataFormat = lastRowDataFormat; }

            return this;
        }
    }



    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // [class] CellFormat                                                                                           //
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    /// <summary>This class defines a cell format.</summary>
    public class CellFormat
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // constructors                                                                                             //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Creates a new instance of this class.</summary>
        /// <param name="backgroundColor">Background color.</param>
        /// <param name="fontColor">Font color.</param>
        /// <param name="leftBorder">Left border style.</param>
        /// <param name="rightBorder">Right border style.</param>
        /// <param name="topBorder">Top border style.</param>
        /// <param name="bottomBorder">Bottom border style.</param>
        /// <param name="bold">Bold font.</param>
        /// <param name="italic">Italic font.</param>
        /// <param name="dataType">Data type.</param>

        public CellFormat(XLColor? backgroundColor = null, XLColor? fontColor = null, XLBorderStyleValues? leftBorder = null, XLBorderStyleValues? rightBorder = null, XLBorderStyleValues? topBorder = null, XLBorderStyleValues? bottomBorder = null, bool? bold = null, bool? italic = null, XLDataType? dataType = null)
        {
            BackgroundColor = backgroundColor;
            FontColor = fontColor;
            LeftBorder = leftBorder;
            RightBorder = rightBorder;
            TopBorder = topBorder;
            BottomBorder = bottomBorder;
            Bold = bold;
            Italic = italic;
            DataType = dataType;
        }



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public properties                                                                                        //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Gets or sets the cell background color.</summary>
        public XLColor? BackgroundColor
        {
            get; set;
        }


        /// <summary>Gets or sets the cell font color.</summary>
        public XLColor? FontColor
        {
            get; set;
        }


        /// <summary>Gets or sets the left border style.</summary>
        public XLBorderStyleValues? LeftBorder
        {
            get; set;
        }


        /// <summary>Gets or sets the right border style.</summary>
        public XLBorderStyleValues? RightBorder
        {
            get; set;
        }


        /// <summary>Gets or sets the top border style.</summary>
        public XLBorderStyleValues? TopBorder
        {
            get; set;
        }


        /// <summary>Gets or sets the bottom border style.</summary>
        public XLBorderStyleValues? BottomBorder
        {
            get; set;
        }


        /// <summary>Gets or sets a value indicating whether the cell font is bold.</summary>
        public bool? Bold
        {
            get; set;
        }


        /// <summary>Gets or sets a value indicating whether the cell font is italic.</summary>
        public bool? Italic
        {
            get; set;
        }


        /// <summary>Gets or sets the data type.</summary>
        public XLDataType? DataType
        {
            get; set;
        }



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public methods                                                                                           //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Creates a new CellFormat object that is a copy of the current instance.</summary>
        /// <returns>Returns a new CellFormat object with the same property values as the current instance.</returns>
        public CellFormat Clone()
        {
            return new CellFormat(BackgroundColor, FontColor, LeftBorder, RightBorder, TopBorder, BottomBorder, Bold, Italic, DataType);
        }


        /// <summary>Creates a new CellFormat object that is a copy of the current instance, with optional modifications.</summary>
        /// <param name="backgroundColor">Background color.</param>
        /// <param name="fontColor">Font color.</param>
        /// <param name="leftBorder">Left border style.</param>
        /// <param name="rightBorder">Right border style.</param>
        /// <param name="topBorder">Top border style.</param>
        /// <param name="bottomBorder">Bottom border style.</param>
        /// <param name="bold">Bold font.</param>
        /// <param name="italic">Italic font.</param>
        /// <param name="dataType">Data type.</param>
        /// <returns>Returns a new CellFormat object with the specified property values, or the current values if not specified.</returns>
        public CellFormat Clone(XLColor? backgroundColor = null, XLColor? fontColor = null, XLBorderStyleValues? leftBorder = null, XLBorderStyleValues? rightBorder = null, XLBorderStyleValues? topBorder = null, XLBorderStyleValues? bottomBorder = null, bool? bold = null, bool? italic = null, XLDataType? dataType = null)
        {
            return new CellFormat(backgroundColor ?? BackgroundColor, fontColor ?? FontColor, leftBorder ?? LeftBorder, rightBorder ?? RightBorder, topBorder ?? TopBorder, bottomBorder ?? BottomBorder, bold ?? Bold, italic ?? Italic, dataType ?? DataType);
        }


        /// <summary>Sets values of the cell format.</summary>
        /// <param name="backgroundColor">Background color.</param>
        /// <param name="fontColor">Font color.</param>
        /// <param name="leftBorder">Left border style.</param>
        /// <param name="rightBorder">Right border style.</param>
        /// <param name="topBorder">Top border style.</param>
        /// <param name="bottomBorder">Bottom border style.</param>
        /// <param name="bold">Bold font.</param>
        /// <param name="italic">Italic font.</param>
        /// <param name="dataType">Data type.</param>
        /// <returns>Returns the updated cell format.</returns>
        public CellFormat Set(XLColor? backgroundColor = null, XLColor? fontColor = null, XLBorderStyleValues? leftBorder = null, XLBorderStyleValues? rightBorder = null, XLBorderStyleValues? topBorder = null, XLBorderStyleValues? bottomBorder = null, bool? bold = null, bool? italic = null, XLDataType? dataType = null)
        {
            if(backgroundColor is not null) { BackgroundColor = backgroundColor; }
            if(fontColor is not null) { FontColor = fontColor; }
            if(leftBorder is not null) { LeftBorder = leftBorder; }
            if(rightBorder is not null) { RightBorder = rightBorder; }
            if(topBorder is not null) { TopBorder = topBorder; }
            if(bottomBorder is not null) { BottomBorder = bottomBorder; }
            if(bold is not null) { Bold = bold; }
            if(italic is not null) { Italic = italic; }
            if(dataType is not null) { DataType = dataType; }

            return this;
        }
    }
}
