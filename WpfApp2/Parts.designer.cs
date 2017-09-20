﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
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
	public partial class PartsDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();


        public IEnumerable<part> GetAllParts()
        {
            var items = this.parts.Select(item => item).OrderBy(item => item.name);

            return items;
        }

        #region Extensibility Method Definitions
        partial void OnCreated();
    partial void Insertpart(part instance);
    partial void Updatepart(part instance);
    partial void Deletepart(part instance);
    #endregion
		
		public PartsDataContext() : 
				base(global::WpfApp2.Properties.Settings.Default.auto76ConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public PartsDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public PartsDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public PartsDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public PartsDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<part> parts
		{
			get
			{
				return this.GetTable<part>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.parts")]
	public partial class part : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _id;
		
		private string _name;
		
		private string _part_number;
		
		private float _sup_price;
		
		private int _sup_id;
		
		private float _price;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnidChanging(int value);
    partial void OnidChanged();
    partial void OnnameChanging(string value);
    partial void OnnameChanged();
    partial void Onpart_numberChanging(string value);
    partial void Onpart_numberChanged();
    partial void Onsup_priceChanging(System.Nullable<double> value);
    partial void Onsup_priceChanged();
    partial void Onsup_idChanging(System.Nullable<int> value);
    partial void Onsup_idChanged();
    partial void OnpriceChanging(System.Nullable<double> value);
    partial void OnpriceChanged();
    #endregion
		
		public part()
		{
			OnCreated();
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
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_name", DbType="VarChar(MAX)")]
		public string name
		{
			get
			{
				return this._name;
			}
			set
			{
				if ((this._name != value))
				{
					this.OnnameChanging(value);
					this.SendPropertyChanging();
					this._name = value;
					this.SendPropertyChanged("name");
					this.OnnameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_part_number", DbType="VarChar(MAX)")]
		public string part_number
		{
			get
			{
				return this._part_number;
			}
			set
			{
				if ((this._part_number != value))
				{
					this.Onpart_numberChanging(value);
					this.SendPropertyChanging();
					this._part_number = value;
					this.SendPropertyChanged("part_number");
					this.Onpart_numberChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_sup_price", DbType="Float")]
		public float sup_price
		{
			get
			{
				return this._sup_price;
			}
			set
			{
				if ((this._sup_price != value))
				{
					this.Onsup_priceChanging(value);
					this.SendPropertyChanging();
					this._sup_price = value;
					this.SendPropertyChanged("sup_price");
					this.Onsup_priceChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_sup_id", DbType="Int")]
		public int sup_id
		{
			get
			{
				return this._sup_id;
			}
			set
			{
				if ((this._sup_id != value))
				{
					this.Onsup_idChanging(value);
					this.SendPropertyChanging();
					this._sup_id = value;
					this.SendPropertyChanged("sup_id");
					this.Onsup_idChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_price", DbType="Float")]
		public float price
		{
			get
			{
				return this._price;
			}
			set
			{
				if ((this._price != value))
				{
					this.OnpriceChanging(value);
					this.SendPropertyChanging();
					this._price = value;
					this.SendPropertyChanged("price");
					this.OnpriceChanged();
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
