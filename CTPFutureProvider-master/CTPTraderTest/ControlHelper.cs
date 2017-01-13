using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;
using System.Windows.Forms;
using System.Collections;
using System.Diagnostics;
using CalmBeltFund.Trading.CTP;

namespace CTPTraderTest
{
  public partial class MainForm
  {
    static object printLocker = new object();

    protected static void PrintObject(object obj, bool output = false)
    {
      if (output == false)
        return;


      StringBuilder buffer = new StringBuilder();

      if (obj == null)
      {
        buffer.AppendLine("NULL");
      }
      else
      {
        Type type = obj.GetType();

        buffer.AppendLine(type.FullName);

        FieldInfo[] fields = type.GetFields();

        foreach (FieldInfo field in fields)
        {
          buffer.AppendLine(string.Format("  {0}[{1}]:{2}", field.Name, field.FieldType.Name, field.GetValue(obj)));
        }

        buffer.AppendLine();
      }

      lock (printLocker)
      {
        Debug.Write(buffer.ToString());
      }
    }

    protected static void PrintObjectList(IEnumerable list)
    {
      foreach (var item in list)
      {
        PrintObject(item);
      }
    }

    protected static DataTable BindObjectList(DataGridView dataGridView, IEnumerable list)
    {
      DataTable table = null;

      foreach (var obj in list)
      {
        if (table == null)
        {
          table = CreateObjectFiledTable(obj);
        }

        DataRow row = ConvertObjectToRow(table, obj);

        table.Rows.Add(row);
      }


      if (dataGridView != null)
      {
        dataGridView.Invoke(new Action(() =>
        {
          dataGridView.AutoGenerateColumns = true;
          dataGridView.DataSource = table;
        }));
      }

      return table;
    }

    protected static DataTable CreateObjectFiledTable(object obj)
    {
      DataTable table = new DataTable();

      Type type = obj.GetType();

      foreach (FieldInfo field in type.GetFields())
      {

        if (field.FieldType == typeof(byte[]))
        {
          //Byte[]类型的字段，显示时使用String
          table.Columns.Add(field.Name, typeof(string));
        }
        else
        {
          table.Columns.Add(field.Name, field.FieldType);
        }
      }

      foreach (PropertyInfo pro in type.GetProperties())
      {
        table.Columns.Add(pro.Name, pro.PropertyType);
      }

      return table;
    }

    protected static void UpdateBindTable(DataGridView view, object obj, Func<DataRow, object, bool> compre = null)
    {

      DataTable table = null;

      if (view.DataSource == null)
      {
        table = CreateObjectFiledTable(obj);
        view.DataSource = table;
      }
      else
      {
        table = view.DataSource as DataTable;
      }

      if (compre == null)
      {
        DataRow row = ConvertObjectToRow(table, obj);
        table.Rows.Add(row);
      }
      else
      {
        lock (table)
        {

          bool handle = false;

          foreach (DataRow item in table.Rows)
          {
            if (compre(item, obj) == true)
            {
              UpdateObjectToRow(item, obj);
              handle = true;
              break;
            }
          }

          if (handle == false)
          {
            DataRow row = ConvertObjectToRow(table, obj);
            table.Rows.Add(row);
          }
        }
      }
    }

    protected static DataRow ConvertObjectToRow(DataTable table, object obj)
    {
      DataRow row = table.NewRow();

      UpdateObjectToRow(row, obj);

      return row;
    }

    protected static DataRow UpdateObjectToRow(DataRow row, object obj)
    {
      Type type = obj.GetType();

      foreach (FieldInfo field in type.GetFields())
      {

        if (field.FieldType == typeof(byte[]))
        {
          //Byte[]类型的字段，显示时使用String
          row[field.Name] = PInvokeUtility.GetUnicodeString((byte[])field.GetValue(obj));
        }
        else
        {
          row[field.Name] = field.GetValue(obj);
        }
      }

      foreach (PropertyInfo pro in type.GetProperties())
      {
        row[pro.Name] = pro.GetValue(obj, null);
      }

      return row;
    }

  }
}
