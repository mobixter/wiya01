using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using DbAcces.Data;

namespace ContentAdmin
{
    public class CDevices : Base
    {
        private DataTable TbDevices = new DataTable();


        public DataTable TableDevices2(int ParamModel)
        {
            DataTable TbDevicesN = new DataTable();
            try
            {
                DataSet objResultado = new DataSet();
                SelectData objInsert = new SelectData();
                DbParameterCollection objParameterCollection = new DbParameterCollection();
                objInsert.Connection = base.ConnectionStrings;
                objInsert.StoreProcedure = "dbo.usp_devices";
                objParameterCollection.Add(new DbParameter("opt", 4));
                objParameterCollection.Add(new DbParameter("devicemodel", ParamModel));
                objInsert.ParameterCollection = objParameterCollection;
                objResultado = objInsert.Select();
                if (objResultado != null)
                {
                    if (objResultado.Tables["data"].Rows.Count > 0)
                        TbDevicesN = objResultado.Tables["data"];
                    else
                        TbDevicesN = null;
                }
                else
                    TbDevicesN = null;
                return TbDevicesN;
            }
            catch (Exception ex)
            {
                TbDevicesN = null;
                return TbDevicesN;
            }
        }



        public DataTable TableDevices234(int ParamModel)
        {
            DataTable TbDevicesN = new DataTable();
            try
            {
                DataSet objResultado = new DataSet();
                SelectData objInsert = new SelectData();
                DbParameterCollection objParameterCollection = new DbParameterCollection();
                objInsert.Connection = base.ConnectionStrings;
                objInsert.StoreProcedure = "dbo.usp_devices";
                objParameterCollection.Add(new DbParameter("opt", 8));
                objParameterCollection.Add(new DbParameter("devicemodel", ParamModel));
                objInsert.ParameterCollection = objParameterCollection;
                objResultado = objInsert.Select();
                if (objResultado != null)
                {
                    if (objResultado.Tables["data"].Rows.Count > 0)
                        TbDevicesN = objResultado.Tables["data"];
                    else
                        TbDevicesN = null;
                }
                else
                    TbDevicesN = null;
                return TbDevicesN;
            }
            catch (Exception ex)
            {
                TbDevicesN = null;
                return TbDevicesN;
            }
        }





        public DataTable TableDevices
        {
            get
            {
                if (TbDevices != null)
                {
                    if (this.TbDevices.Rows.Count != 0)
                        return this.TbDevices;
                }

                DataSet objResultado = new DataSet();
                SelectData objSelect = new SelectData();
                DbParameterCollection objParameterCollection = new DbParameterCollection();
                objSelect.Connection = base.ConnectionStrings;
                objSelect.StoreProcedure = "dbo.usp_devices";
                objParameterCollection.Add(new DbParameter("opt", 1));
                objSelect.ParameterCollection = objParameterCollection;

                objResultado = objSelect.Select();
                if (objResultado != null)
                {
                    if (objResultado.Tables["data"].Rows.Count > 0)
                        this.TbDevices = objResultado.Tables["data"];
                    else
                        this.TbDevices = null;
                }
                else
                    this.TbDevices = null;
                return this.TbDevices;
            }
        }

        public void InsertCompatibilidad(string handsetname)
        {
            InsertData objSelect = new InsertData();
            DbParameterCollection objParameterCollection = new DbParameterCollection();
            objSelect.Connection = base.ConnectionStrings;
            objSelect.StoreProcedure = "dbo.usp_devices";
            objParameterCollection.Add(new DbParameter("opt", 3));
            objParameterCollection.Add(new DbParameter("HandsetName", handsetname));
            objSelect.ParameterCollection = objParameterCollection;
            objSelect.Insert();
        }

        public void deleteCompatibilidad(int handsetname)
        {
            DeleteData objSelect = new DeleteData();
            DbParameterCollection objParameterCollection = new DbParameterCollection();
            objSelect.Connection = base.ConnectionStrings;
            objSelect.StoreProcedure = "dbo.usp_devices";
            objParameterCollection.Add(new DbParameter("opt", 10));
            objParameterCollection.Add(new DbParameter("devicemodel", handsetname));
            objSelect.ParameterCollection = objParameterCollection;
            objSelect.Delete();
        }


        public void InsertCompatibilidad22(int group,int handsetname)
        {
            InsertData objSelect = new InsertData();
            DbParameterCollection objParameterCollection = new DbParameterCollection();
            objSelect.Connection = base.ConnectionStrings;
            objSelect.StoreProcedure = "dbo.usp_devices";
            objParameterCollection.Add(new DbParameter("opt", 6));
            objParameterCollection.Add(new DbParameter("devicemodel", group));
            objParameterCollection.Add(new DbParameter("HandsetCompId", handsetname));
            objSelect.ParameterCollection = objParameterCollection;
            objSelect.Insert();
        }


        public DataTable TableDevices22(string ParamModel)
        {
            DataTable TbDevicesN = new DataTable();
            try
            {
                DataSet objResultado = new DataSet();
                SelectData objInsert = new SelectData();
                DbParameterCollection objParameterCollection = new DbParameterCollection();
                objInsert.Connection = base.ConnectionStrings;
                objInsert.StoreProcedure = "dbo.usp_devices";
                objParameterCollection.Add(new DbParameter("opt", 4));
                objParameterCollection.Add(new DbParameter("HandsetName", ParamModel));
                objInsert.ParameterCollection = objParameterCollection;
                objResultado = objInsert.Select();
                if (objResultado != null)
                {
                    if (objResultado.Tables["data"].Rows.Count > 0)
                        TbDevicesN = objResultado.Tables["data"];
                    else
                        TbDevicesN = null;
                }
                else
                    TbDevicesN = null;
                return TbDevicesN;
            }
            catch (Exception ex)
            {
                TbDevicesN = null;
                return TbDevicesN;
            }
        }



        public DataTable TableDevices221(string ParamModel)
        {
            DataTable TbDevicesN = new DataTable();
            try
            {
                DataSet objResultado = new DataSet();
                SelectData objInsert = new SelectData();
                DbParameterCollection objParameterCollection = new DbParameterCollection();
                objInsert.Connection = base.ConnectionStrings;
                objInsert.StoreProcedure = "dbo.usp_devices";
                objParameterCollection.Add(new DbParameter("opt", 7));
                objParameterCollection.Add(new DbParameter("HandsetName", ParamModel));
                objInsert.ParameterCollection = objParameterCollection;
                objResultado = objInsert.Select();
                if (objResultado != null)
                {
                    if (objResultado.Tables["data"].Rows.Count > 0)
                        TbDevicesN = objResultado.Tables["data"];
                    else
                        TbDevicesN = null;
                }
                else
                    TbDevicesN = null;
                return TbDevicesN;
            }
            catch (Exception ex)
            {
                TbDevicesN = null;
                return TbDevicesN;
            }
        }

        public DataTable TableDevices22H()
        {
            DataTable TbDevicesN = new DataTable();
            try
            {
                DataSet objResultado = new DataSet();
                SelectData objInsert = new SelectData();
                DbParameterCollection objParameterCollection = new DbParameterCollection();
                objInsert.Connection = base.ConnectionStrings;
                objInsert.StoreProcedure = "dbo.usp_Devices";
                objParameterCollection.Add(new DbParameter("opt", 5));
                objInsert.ParameterCollection = objParameterCollection;
                objResultado = objInsert.Select();
                if (objResultado != null)
                {
                    if (objResultado.Tables["data"].Rows.Count > 0)
                        TbDevicesN = objResultado.Tables["data"];
                    else
                        TbDevicesN = null;
                }
                else
                    TbDevicesN = null;
                return TbDevicesN;
            }
            catch (Exception ex)
            {
                TbDevicesN = null;
                return TbDevicesN;
            }
        }


    }
}