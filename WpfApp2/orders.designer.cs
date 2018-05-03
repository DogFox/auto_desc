﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WpfApp2
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="auto76")]
	public partial class OrdersDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Определения метода расширяемости
    partial void OnCreated();
    partial void Insertorder(order instance);
    partial void Updateorder(order instance);
    partial void Deleteorder(order instance);
    #endregion
		
		public OrdersDataContext() : 
				base(global::WpfApp2.Properties.Settings.Default.auto76ConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public OrdersDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public OrdersDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public OrdersDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public OrdersDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<order> orders
		{
			get
			{
				return this.GetTable<order>();
			}
		}
	}

    [global::System.Data.Linq.Mapping.TableAttribute(Name = "dbo.orders")]
    public partial class order : INotifyPropertyChanging, INotifyPropertyChanged
    {

        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        private int _id;

        private string _number;

        private int _cust_id;

        private int _type;

        private System.Nullable<double> _summ;

        private System.Nullable<int> _count;

        private string _comment;

        private System.Nullable<int> _status;

        private System.Nullable<System.DateTime> _date;

        private string _author_id;

        #region Определения метода расширяемости
        partial void OnLoaded();
        partial void OnValidate(System.Data.Linq.ChangeAction action);
        partial void OnCreated();
        partial void OnidChanging(int value);
        partial void OnidChanged();
        partial void OnnumberChanging(string value);
        partial void OnnumberChanged();
        partial void Oncust_idChanging(int value);
        partial void Oncust_idChanged();
        partial void OnsummChanging(System.Nullable<double> value);
        partial void OnsummChanged();
        partial void OncountChanging(System.Nullable<int> value);
        partial void OncountChanged();
        partial void OncommentChanging(string value);
        partial void OncommentChanged();
        partial void OnstatusChanging(System.Nullable<int> value);
        partial void OnstatusChanged();
        partial void OndateChanging(System.Nullable<System.DateTime> value);
        partial void OndateChanged();
        partial void OnauthorChanging(string value);
        partial void OnauthorChanged();
        #endregion

        public order()
        {
            OnCreated();
        }

        public order( DataRowView drv )
        {
            this._id =(int) drv["id"];
            this._number = (string)drv["number"];
            this._cust_id = (int)drv["cust_id"];
            this._comment = (string)drv["comment"];
            this._status = (int)drv["status"];
            this._date = (DateTime)drv["date"];

        }
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_id", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int id
		{
			get
			{
				return this._id;
			}
			set
			{
				if ((this._id != value))
				{
					this.OnidChanging(value);
					this.SendPropertyChanging();
					this._id = value;
					this.SendPropertyChanged("id");
					this.OnidChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_number", DbType="VarChar(MAX) NOT NULL", CanBeNull=false)]
		public string number
		{
			get
			{
				return this._number;
			}
			set
			{
				if ((this._number != value))
				{
					this.OnnumberChanging(value);
					this.SendPropertyChanging();
					this._number = value;
					this.SendPropertyChanged("number");
					this.OnnumberChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_cust_id", DbType="Int NOT NULL")]
		public int cust_id
		{
			get
			{
				return this._cust_id;
			}
			set
			{
				if ((this._cust_id != value))
				{
					this.Oncust_idChanging(value);
					this.SendPropertyChanging();
					this._cust_id = value;
					this.SendPropertyChanged("cust_id");
					this.Oncust_idChanged();
				}
			}
		}


        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_type", DbType = "Int")]
        public int type
        {
            get
            {
                return this._type;
            }
            set
            {
                if ((this._type != value))
                {
                    this.Oncust_idChanging(value);
                    this.SendPropertyChanging();
                    this._type = value;
                    this.SendPropertyChanged("type");
                    this.Oncust_idChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_summ", DbType="Float")]
		public System.Nullable<double> summ
		{
			get
			{
				return this._summ;
			}
			set
			{
				if ((this._summ != value))
				{
					this.OnsummChanging(value);
					this.SendPropertyChanging();
					this._summ = value;
					this.SendPropertyChanged("summ");
					this.OnsummChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_count", DbType="Int")]
		public System.Nullable<int> count
		{
			get
			{
				return this._count;
			}
			set
			{
				if ((this._count != value))
				{
					this.OncountChanging(value);
					this.SendPropertyChanging();
					this._count = value;
					this.SendPropertyChanged("count");
					this.OncountChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_comment", DbType="VarChar(MAX)")]
		public string comment
		{
			get
			{
				return this._comment;
			}
			set
			{
				if ((this._comment != value))
				{
					this.OncommentChanging(value);
					this.SendPropertyChanging();
					this._comment = value;
					this.SendPropertyChanged("comment");
					this.OncommentChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_status", DbType="Int")]
		public System.Nullable<int> status
		{
			get
			{
				return this._status;
			}
			set
			{
				if ((this._status != value))
				{
					this.OnstatusChanging(value);
					this.SendPropertyChanging();
					this._status = value;
					this.SendPropertyChanged("status");
					this.OnstatusChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_date", DbType="DateTime")]
		public System.Nullable<System.DateTime> date
		{
			get
			{
				return this._date;
			}
			set
			{
				if ((this._date != value))
				{
					this.OndateChanging(value);
					this.SendPropertyChanging();
					this._date = value;
					this.SendPropertyChanged("date");
					this.OndateChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_author_id", CanBeNull=false)]
		public string author
		{
			get
			{
				return this._author_id;
			}
			set
			{
				if ((this._author_id != value))
				{
					this.OnauthorChanging(value);
					this.SendPropertyChanging();
					this._author_id = value;
					this.SendPropertyChanged("author");
					this.OnauthorChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
#pragma warning restore 1591
