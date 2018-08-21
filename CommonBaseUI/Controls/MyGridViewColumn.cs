
namespace CommonBaseUI.Controls
{
    public class MyGridViewColumn
    {
        public MyGridViewColumn(MyGridColumnType columnType, string columnName, string displayMemberBinding, double width, bool readOnly = true)
        {
            this.ColumnType = columnType;
            this.ColumnName = columnName;
            this.DisplayMemberBinding = displayMemberBinding;
            this.Width = width;
            this.ReadOnly = readOnly;
        }
        public MyGridViewColumn(MyGridViewCellCheckBox checkBox, string columnName, string displayMemberBinding, double width, bool readOnly = true)
        {
            this.ColumnType = MyGridColumnType.CheckBoxColumn;
            this.ColumnName = columnName;
            this.DisplayMemberBinding = displayMemberBinding;
            this.Width = width;
            this.InputControl = checkBox;
            this.ReadOnly = readOnly;
        }
        public MyGridViewColumn(MyGridViewCellCombBox combBox, string columnName, string displayMemberBinding, double width, bool readOnly = true)
        {
            this.ColumnType = MyGridColumnType.CombBoxColumn;
            this.ColumnName = columnName;
            this.DisplayMemberBinding = displayMemberBinding;
            this.Width = width;
            this.InputControl = combBox;
            this.ReadOnly = readOnly;
        }

        /// <summary>
        /// 列的对象
        /// </summary>
        public MyGridColumnType ColumnType { get; set; }
        /// <summary>
        /// 列头显示的文字
        /// </summary>
        public string ColumnName { get; set; }
        /// <summary>
        /// 该列绑定的字段名
        /// </summary>
        public string DisplayMemberBinding { get; set; }
        /// <summary>
        /// 列的宽度
        /// </summary>
        public double Width { get; set; }
        /// <summary>
        /// 实体控件
        /// </summary>
        public object InputControl { get; set; }
        /// <summary>
        /// 是否只读
        /// </summary>
        public bool ReadOnly { get; set; }
    }
}
