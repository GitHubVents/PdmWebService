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

namespace DataBaseDomian.Orm
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
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="SWPlusDB")]
	public partial class AVTypeSizeDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    partial void InsertStandardSize(StandardSize instance);
    partial void UpdateStandardSize(StandardSize instance);
    partial void DeleteStandardSize(StandardSize instance);
    partial void InsertDimensionType(DimensionType instance);
    partial void UpdateDimensionType(DimensionType instance);
    partial void DeleteDimensionType(DimensionType instance);
    partial void InsertDimension(Dimension instance);
    partial void UpdateDimension(Dimension instance);
    partial void DeleteDimension(Dimension instance);
    partial void InsertProfil(Profil instance);
    partial void UpdateProfil(Profil instance);
    partial void DeleteProfil(Profil instance);
    #endregion
		
		public AVTypeSizeDataContext() : 
				base(global::DataBaseDomian.Properties.Settings.Default.SWPlusDBConnectionString1, mappingSource)
		{
			OnCreated();
		}
		
		public AVTypeSizeDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public AVTypeSizeDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public AVTypeSizeDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public AVTypeSizeDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<StandardSize> StandardSizes
		{
			get
			{
				return this.GetTable<StandardSize>();
			}
		}
		
		public System.Data.Linq.Table<DimensionType> DimensionTypes
		{
			get
			{
				return this.GetTable<DimensionType>();
			}
		}
		
		public System.Data.Linq.Table<Dimension> Dimensions
		{
			get
			{
				return this.GetTable<Dimension>();
			}
		}
		
		public System.Data.Linq.Table<Profil> Profils
		{
			get
			{
				return this.GetTable<Profil>();
			}
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.GetAV_TypeSize")]
		public int GetAV_TypeSize([global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Int")] System.Nullable<int> sizeId, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="Width", DbType="Int")] ref System.Nullable<int> width, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="Height", DbType="Int")] ref System.Nullable<int> height)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), sizeId, width, height);
			width = ((System.Nullable<int>)(result.GetParameterValue(1)));
			height = ((System.Nullable<int>)(result.GetParameterValue(2)));
			return ((int)(result.ReturnValue));
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="AirVents.StandardSize")]
	public partial class StandardSize : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _SizeID;
		
		private string _Type;
		
		private EntitySet<DimensionType> _DimensionTypes;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnSizeIDChanging(int value);
    partial void OnSizeIDChanged();
    partial void OnTypeChanging(string value);
    partial void OnTypeChanged();
    #endregion
		
		public StandardSize()
		{
			this._DimensionTypes = new EntitySet<DimensionType>(new Action<DimensionType>(this.attach_DimensionTypes), new Action<DimensionType>(this.detach_DimensionTypes));
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_SizeID", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int SizeID
		{
			get
			{
				return this._SizeID;
			}
			set
			{
				if ((this._SizeID != value))
				{
					this.OnSizeIDChanging(value);
					this.SendPropertyChanging();
					this._SizeID = value;
					this.SendPropertyChanged("SizeID");
					this.OnSizeIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Type", DbType="NVarChar(6) NOT NULL", CanBeNull=false)]
		public string Type
		{
			get
			{
				return this._Type;
			}
			set
			{
				if ((this._Type != value))
				{
					this.OnTypeChanging(value);
					this.SendPropertyChanging();
					this._Type = value;
					this.SendPropertyChanged("Type");
					this.OnTypeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="StandardSize_DimensionType", Storage="_DimensionTypes", ThisKey="SizeID", OtherKey="SizeID")]
		public EntitySet<DimensionType> DimensionTypes
		{
			get
			{
				return this._DimensionTypes;
			}
			set
			{
				this._DimensionTypes.Assign(value);
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
		
		private void attach_DimensionTypes(DimensionType entity)
		{
			this.SendPropertyChanging();
			entity.StandardSize = this;
		}
		
		private void detach_DimensionTypes(DimensionType entity)
		{
			this.SendPropertyChanging();
			entity.StandardSize = null;
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="AirVents.DimensionType")]
	public partial class DimensionType : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _DimensionTypeID;
		
		private System.Nullable<int> _SizeID;
		
		private System.Nullable<int> _ProfilID;
		
		private System.Nullable<int> _DimensionID;
		
		private System.Nullable<int> _SEID;
		
		private EntityRef<StandardSize> _StandardSize;
		
		private EntityRef<Dimension> _Dimension;
		
		private EntityRef<Profil> _Profil;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnDimensionTypeIDChanging(int value);
    partial void OnDimensionTypeIDChanged();
    partial void OnSizeIDChanging(System.Nullable<int> value);
    partial void OnSizeIDChanged();
    partial void OnProfilIDChanging(System.Nullable<int> value);
    partial void OnProfilIDChanged();
    partial void OnDimensionIDChanging(System.Nullable<int> value);
    partial void OnDimensionIDChanged();
    partial void OnSEIDChanging(System.Nullable<int> value);
    partial void OnSEIDChanged();
    #endregion
		
		public DimensionType()
		{
			this._StandardSize = default(EntityRef<StandardSize>);
			this._Dimension = default(EntityRef<Dimension>);
			this._Profil = default(EntityRef<Profil>);
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DimensionTypeID", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int DimensionTypeID
		{
			get
			{
				return this._DimensionTypeID;
			}
			set
			{
				if ((this._DimensionTypeID != value))
				{
					this.OnDimensionTypeIDChanging(value);
					this.SendPropertyChanging();
					this._DimensionTypeID = value;
					this.SendPropertyChanged("DimensionTypeID");
					this.OnDimensionTypeIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_SizeID", DbType="Int")]
		public System.Nullable<int> SizeID
		{
			get
			{
				return this._SizeID;
			}
			set
			{
				if ((this._SizeID != value))
				{
					if (this._StandardSize.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.OnSizeIDChanging(value);
					this.SendPropertyChanging();
					this._SizeID = value;
					this.SendPropertyChanged("SizeID");
					this.OnSizeIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ProfilID", DbType="Int")]
		public System.Nullable<int> ProfilID
		{
			get
			{
				return this._ProfilID;
			}
			set
			{
				if ((this._ProfilID != value))
				{
					if (this._Profil.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.OnProfilIDChanging(value);
					this.SendPropertyChanging();
					this._ProfilID = value;
					this.SendPropertyChanged("ProfilID");
					this.OnProfilIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DimensionID", DbType="Int")]
		public System.Nullable<int> DimensionID
		{
			get
			{
				return this._DimensionID;
			}
			set
			{
				if ((this._DimensionID != value))
				{
					if (this._Dimension.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.OnDimensionIDChanging(value);
					this.SendPropertyChanging();
					this._DimensionID = value;
					this.SendPropertyChanged("DimensionID");
					this.OnDimensionIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_SEID", DbType="Int")]
		public System.Nullable<int> SEID
		{
			get
			{
				return this._SEID;
			}
			set
			{
				if ((this._SEID != value))
				{
					this.OnSEIDChanging(value);
					this.SendPropertyChanging();
					this._SEID = value;
					this.SendPropertyChanged("SEID");
					this.OnSEIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="StandardSize_DimensionType", Storage="_StandardSize", ThisKey="SizeID", OtherKey="SizeID", IsForeignKey=true)]
		public StandardSize StandardSize
		{
			get
			{
				return this._StandardSize.Entity;
			}
			set
			{
				StandardSize previousValue = this._StandardSize.Entity;
				if (((previousValue != value) 
							|| (this._StandardSize.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._StandardSize.Entity = null;
						previousValue.DimensionTypes.Remove(this);
					}
					this._StandardSize.Entity = value;
					if ((value != null))
					{
						value.DimensionTypes.Add(this);
						this._SizeID = value.SizeID;
					}
					else
					{
						this._SizeID = default(Nullable<int>);
					}
					this.SendPropertyChanged("StandardSize");
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="Dimension_DimensionType", Storage="_Dimension", ThisKey="DimensionID", OtherKey="DimensionID", IsForeignKey=true)]
		public Dimension Dimension
		{
			get
			{
				return this._Dimension.Entity;
			}
			set
			{
				Dimension previousValue = this._Dimension.Entity;
				if (((previousValue != value) 
							|| (this._Dimension.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._Dimension.Entity = null;
						previousValue.DimensionTypes.Remove(this);
					}
					this._Dimension.Entity = value;
					if ((value != null))
					{
						value.DimensionTypes.Add(this);
						this._DimensionID = value.DimensionID;
					}
					else
					{
						this._DimensionID = default(Nullable<int>);
					}
					this.SendPropertyChanged("Dimension");
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="Profil_DimensionType", Storage="_Profil", ThisKey="ProfilID", OtherKey="ProfilID", IsForeignKey=true)]
		public Profil Profil
		{
			get
			{
				return this._Profil.Entity;
			}
			set
			{
				Profil previousValue = this._Profil.Entity;
				if (((previousValue != value) 
							|| (this._Profil.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._Profil.Entity = null;
						previousValue.DimensionTypes.Remove(this);
					}
					this._Profil.Entity = value;
					if ((value != null))
					{
						value.DimensionTypes.Add(this);
						this._ProfilID = value.ProfilID;
					}
					else
					{
						this._ProfilID = default(Nullable<int>);
					}
					this.SendPropertyChanged("Profil");
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
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="AirVents.Dimension")]
	public partial class Dimension : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _DimensionID;
		
		private int _Wight;
		
		private int _Hight;
		
		private EntitySet<DimensionType> _DimensionTypes;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnDimensionIDChanging(int value);
    partial void OnDimensionIDChanged();
    partial void OnWightChanging(int value);
    partial void OnWightChanged();
    partial void OnHightChanging(int value);
    partial void OnHightChanged();
    #endregion
		
		public Dimension()
		{
			this._DimensionTypes = new EntitySet<DimensionType>(new Action<DimensionType>(this.attach_DimensionTypes), new Action<DimensionType>(this.detach_DimensionTypes));
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DimensionID", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int DimensionID
		{
			get
			{
				return this._DimensionID;
			}
			set
			{
				if ((this._DimensionID != value))
				{
					this.OnDimensionIDChanging(value);
					this.SendPropertyChanging();
					this._DimensionID = value;
					this.SendPropertyChanged("DimensionID");
					this.OnDimensionIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Wight", DbType="Int NOT NULL")]
		public int Wight
		{
			get
			{
				return this._Wight;
			}
			set
			{
				if ((this._Wight != value))
				{
					this.OnWightChanging(value);
					this.SendPropertyChanging();
					this._Wight = value;
					this.SendPropertyChanged("Wight");
					this.OnWightChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Hight", DbType="Int NOT NULL")]
		public int Hight
		{
			get
			{
				return this._Hight;
			}
			set
			{
				if ((this._Hight != value))
				{
					this.OnHightChanging(value);
					this.SendPropertyChanging();
					this._Hight = value;
					this.SendPropertyChanged("Hight");
					this.OnHightChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="Dimension_DimensionType", Storage="_DimensionTypes", ThisKey="DimensionID", OtherKey="DimensionID")]
		public EntitySet<DimensionType> DimensionTypes
		{
			get
			{
				return this._DimensionTypes;
			}
			set
			{
				this._DimensionTypes.Assign(value);
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
		
		private void attach_DimensionTypes(DimensionType entity)
		{
			this.SendPropertyChanging();
			entity.Dimension = this;
		}
		
		private void detach_DimensionTypes(DimensionType entity)
		{
			this.SendPropertyChanging();
			entity.Dimension = null;
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="AirVents.Profil")]
	public partial class Profil : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _ProfilID;
		
		private string _Description;
		
		private string _Code;
		
		private EntitySet<DimensionType> _DimensionTypes;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnProfilIDChanging(int value);
    partial void OnProfilIDChanged();
    partial void OnDescriptionChanging(string value);
    partial void OnDescriptionChanged();
    partial void OnCodeChanging(string value);
    partial void OnCodeChanged();
    #endregion
		
		public Profil()
		{
			this._DimensionTypes = new EntitySet<DimensionType>(new Action<DimensionType>(this.attach_DimensionTypes), new Action<DimensionType>(this.detach_DimensionTypes));
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ProfilID", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int ProfilID
		{
			get
			{
				return this._ProfilID;
			}
			set
			{
				if ((this._ProfilID != value))
				{
					this.OnProfilIDChanging(value);
					this.SendPropertyChanging();
					this._ProfilID = value;
					this.SendPropertyChanged("ProfilID");
					this.OnProfilIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Description", DbType="NVarChar(15) NOT NULL", CanBeNull=false)]
		public string Description
		{
			get
			{
				return this._Description;
			}
			set
			{
				if ((this._Description != value))
				{
					this.OnDescriptionChanging(value);
					this.SendPropertyChanging();
					this._Description = value;
					this.SendPropertyChanged("Description");
					this.OnDescriptionChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Code", DbType="NVarChar(5) NOT NULL", CanBeNull=false)]
		public string Code
		{
			get
			{
				return this._Code;
			}
			set
			{
				if ((this._Code != value))
				{
					this.OnCodeChanging(value);
					this.SendPropertyChanging();
					this._Code = value;
					this.SendPropertyChanged("Code");
					this.OnCodeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="Profil_DimensionType", Storage="_DimensionTypes", ThisKey="ProfilID", OtherKey="ProfilID")]
		public EntitySet<DimensionType> DimensionTypes
		{
			get
			{
				return this._DimensionTypes;
			}
			set
			{
				this._DimensionTypes.Assign(value);
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
		
		private void attach_DimensionTypes(DimensionType entity)
		{
			this.SendPropertyChanging();
			entity.Profil = this;
		}
		
		private void detach_DimensionTypes(DimensionType entity)
		{
			this.SendPropertyChanging();
			entity.Profil = null;
		}
	}
}
#pragma warning restore 1591
