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

namespace PDMWebService.TaskSystem.Data
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
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="TaskDataBase")]
	public partial class DbModelDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    partial void InsertVibroInsertion(VibroInsertion instance);
    partial void UpdateVibroInsertion(VibroInsertion instance);
    partial void DeleteVibroInsertion(VibroInsertion instance);
    partial void InsertFlap(Flap instance);
    partial void UpdateFlap(Flap instance);
    partial void DeleteFlap(Flap instance);
    partial void InsertPanel(Panel instance);
    partial void UpdatePanel(Panel instance);
    partial void DeletePanel(Panel instance);
    partial void InsertRoof(Roof instance);
    partial void UpdateRoof(Roof instance);
    partial void DeleteRoof(Roof instance);
    partial void InsertTaskInstance(TaskInstance instance);
    partial void UpdateTaskInstance(TaskInstance instance);
    partial void DeleteTaskInstance(TaskInstance instance);
    partial void InsertTaskType(TaskType instance);
    partial void UpdateTaskType(TaskType instance);
    partial void DeleteTaskType(TaskType instance);
    #endregion
		
		public DbModelDataContext() : 
				base(global::PDMWebService.Properties.Settings.Default.TaskDataBaseConnectionString1, mappingSource)
		{
			OnCreated();
		}
		
		public DbModelDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public DbModelDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public DbModelDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public DbModelDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<VibroInsertion> VibroInsertions
		{
			get
			{
				return this.GetTable<VibroInsertion>();
			}
		}
		
		public System.Data.Linq.Table<Flap> Flaps
		{
			get
			{
				return this.GetTable<Flap>();
			}
		}
		
		public System.Data.Linq.Table<Panel> Panels
		{
			get
			{
				return this.GetTable<Panel>();
			}
		}
		
		public System.Data.Linq.Table<Roof> Roofs
		{
			get
			{
				return this.GetTable<Roof>();
			}
		}
		
		public System.Data.Linq.Table<TaskInstance> TaskInstances
		{
			get
			{
				return this.GetTable<TaskInstance>();
			}
		}
		
		public System.Data.Linq.Table<TaskSystemLog> TaskSystemLogs
		{
			get
			{
				return this.GetTable<TaskSystemLog>();
			}
		}
		
		public System.Data.Linq.Table<TaskType> TaskTypes
		{
			get
			{
				return this.GetTable<TaskType>();
			}
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.CreateFlap", IsComposable=true)]
		public object CreateFlap([global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Int")] System.Nullable<int> type, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Int")] System.Nullable<int> wight, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Int")] System.Nullable<int> height, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Int")] System.Nullable<int> userId, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Int")] System.Nullable<int> status, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="DateTime")] System.Nullable<System.DateTime> timeStart, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Int")] System.Nullable<int> typeTask, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Int")] System.Nullable<int> materialId, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Int")] System.Nullable<int> isOuter, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Float")] System.Nullable<double> thickness)
		{
			return ((object)(this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), type, wight, height, userId, status, timeStart, typeTask, materialId, isOuter, thickness).ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.ExistWaitingTasks")]
		public int ExistWaitingTasks()
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())));
			return ((int)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.CreatePanel")]
		public int CreatePanel([global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Int")] System.Nullable<int> status, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Int")] System.Nullable<int> typeTask, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Int")] System.Nullable<int> userId, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="DateTime")] System.Nullable<System.DateTime> timeStart, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Int")] System.Nullable<int> panelProfil, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Int")] System.Nullable<int> panelType, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Int")] System.Nullable<int> wight, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Int")] System.Nullable<int> height, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Int")] System.Nullable<int> outerMaterial, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Int")] System.Nullable<int> innerMaterial, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Float")] System.Nullable<double> thicknessOuterMaterial, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Float")] System.Nullable<double> thicknessInnerMaterial)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), status, typeTask, userId, timeStart, panelProfil, panelType, wight, height, outerMaterial, innerMaterial, thicknessOuterMaterial, thicknessInnerMaterial);
			return ((int)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.CreateTaskRoof")]
		public int CreateTaskRoof([global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Int")] System.Nullable<int> type, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Int")] System.Nullable<int> wight, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Int")] System.Nullable<int> height, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Int")] System.Nullable<int> userId, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Int")] System.Nullable<int> status, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="DateTime")] System.Nullable<System.DateTime> timeStart, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Int")] System.Nullable<int> typeTask)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), type, wight, height, userId, status, timeStart, typeTask);
			return ((int)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.CreateTaskVibroInserion")]
		public int CreateTaskVibroInserion([global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Int")] System.Nullable<int> type, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Int")] System.Nullable<int> wight, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Int")] System.Nullable<int> height, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Int")] System.Nullable<int> userId, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Int")] System.Nullable<int> status, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="DateTime")] System.Nullable<System.DateTime> timeStart, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Int")] System.Nullable<int> taskType)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), type, wight, height, userId, status, timeStart, taskType);
			return ((int)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.ExistTaskToExecute")]
		public int ExistTaskToExecute()
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())));
			return ((int)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.LastTaskId")]
		public int LastTaskId()
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())));
			return ((int)(result.ReturnValue));
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.VibroInsertions")]
	public partial class VibroInsertion : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _Id;
		
		private int _Height;
		
		private int _Width;
		
		private int _TypeVibroInsert;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIdChanging(int value);
    partial void OnIdChanged();
    partial void OnHeightChanging(int value);
    partial void OnHeightChanged();
    partial void OnWidthChanging(int value);
    partial void OnWidthChanged();
    partial void OnTypeVibroInsertChanging(int value);
    partial void OnTypeVibroInsertChanged();
    #endregion
		
		public VibroInsertion()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Id", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int Id
		{
			get
			{
				return this._Id;
			}
			set
			{
				if ((this._Id != value))
				{
					this.OnIdChanging(value);
					this.SendPropertyChanging();
					this._Id = value;
					this.SendPropertyChanged("Id");
					this.OnIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Height", DbType="Int NOT NULL")]
		public int Height
		{
			get
			{
				return this._Height;
			}
			set
			{
				if ((this._Height != value))
				{
					this.OnHeightChanging(value);
					this.SendPropertyChanging();
					this._Height = value;
					this.SendPropertyChanged("Height");
					this.OnHeightChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Width", DbType="Int NOT NULL")]
		public int Width
		{
			get
			{
				return this._Width;
			}
			set
			{
				if ((this._Width != value))
				{
					this.OnWidthChanging(value);
					this.SendPropertyChanging();
					this._Width = value;
					this.SendPropertyChanged("Width");
					this.OnWidthChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_TypeVibroInsert", DbType="Int NOT NULL")]
		public int TypeVibroInsert
		{
			get
			{
				return this._TypeVibroInsert;
			}
			set
			{
				if ((this._TypeVibroInsert != value))
				{
					this.OnTypeVibroInsertChanging(value);
					this.SendPropertyChanging();
					this._TypeVibroInsert = value;
					this.SendPropertyChanged("TypeVibroInsert");
					this.OnTypeVibroInsertChanged();
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
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.Flaps")]
	public partial class Flap : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _Id;
		
		private int _Height;
		
		private int _Width;
		
		private int _FlapType;
		
		private int _MaterialId;
		
		private int _isOuter;
		
		private int _Thickness;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIdChanging(int value);
    partial void OnIdChanged();
    partial void OnHeightChanging(int value);
    partial void OnHeightChanged();
    partial void OnWidthChanging(int value);
    partial void OnWidthChanged();
    partial void OnFlapTypeChanging(int value);
    partial void OnFlapTypeChanged();
    partial void OnMaterialIdChanging(int value);
    partial void OnMaterialIdChanged();
    partial void OnisOuterChanging(int value);
    partial void OnisOuterChanged();
    partial void OnThicknessChanging(int value);
    partial void OnThicknessChanged();
    #endregion
		
		public Flap()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Id", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int Id
		{
			get
			{
				return this._Id;
			}
			set
			{
				if ((this._Id != value))
				{
					this.OnIdChanging(value);
					this.SendPropertyChanging();
					this._Id = value;
					this.SendPropertyChanged("Id");
					this.OnIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Height", DbType="Int NOT NULL")]
		public int Height
		{
			get
			{
				return this._Height;
			}
			set
			{
				if ((this._Height != value))
				{
					this.OnHeightChanging(value);
					this.SendPropertyChanging();
					this._Height = value;
					this.SendPropertyChanged("Height");
					this.OnHeightChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Width", DbType="Int NOT NULL")]
		public int Width
		{
			get
			{
				return this._Width;
			}
			set
			{
				if ((this._Width != value))
				{
					this.OnWidthChanging(value);
					this.SendPropertyChanging();
					this._Width = value;
					this.SendPropertyChanged("Width");
					this.OnWidthChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_FlapType", DbType="Int NOT NULL")]
		public int FlapType
		{
			get
			{
				return this._FlapType;
			}
			set
			{
				if ((this._FlapType != value))
				{
					this.OnFlapTypeChanging(value);
					this.SendPropertyChanging();
					this._FlapType = value;
					this.SendPropertyChanged("FlapType");
					this.OnFlapTypeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_MaterialId", DbType="Int NOT NULL")]
		public int MaterialId
		{
			get
			{
				return this._MaterialId;
			}
			set
			{
				if ((this._MaterialId != value))
				{
					this.OnMaterialIdChanging(value);
					this.SendPropertyChanging();
					this._MaterialId = value;
					this.SendPropertyChanged("MaterialId");
					this.OnMaterialIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_isOuter", DbType="Int NOT NULL")]
		public int isOuter
		{
			get
			{
				return this._isOuter;
			}
			set
			{
				if ((this._isOuter != value))
				{
					this.OnisOuterChanging(value);
					this.SendPropertyChanging();
					this._isOuter = value;
					this.SendPropertyChanged("isOuter");
					this.OnisOuterChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Thickness", DbType="Int NOT NULL")]
		public int Thickness
		{
			get
			{
				return this._Thickness;
			}
			set
			{
				if ((this._Thickness != value))
				{
					this.OnThicknessChanging(value);
					this.SendPropertyChanging();
					this._Thickness = value;
					this.SendPropertyChanged("Thickness");
					this.OnThicknessChanged();
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
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.Panels")]
	public partial class Panel : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _Id;
		
		private int _PanelProfil;
		
		private int _PanelType;
		
		private int _Height;
		
		private int _Width;
		
		private int _OuterMaterial;
		
		private int _InnerMaterial;
		
		private double _ThicknessOuterMaterial;
		
		private double _ThicknessInnerMaterial;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIdChanging(int value);
    partial void OnIdChanged();
    partial void OnPanelProfilChanging(int value);
    partial void OnPanelProfilChanged();
    partial void OnPanelTypeChanging(int value);
    partial void OnPanelTypeChanged();
    partial void OnHeightChanging(int value);
    partial void OnHeightChanged();
    partial void OnWidthChanging(int value);
    partial void OnWidthChanged();
    partial void OnOuterMaterialChanging(int value);
    partial void OnOuterMaterialChanged();
    partial void OnInnerMaterialChanging(int value);
    partial void OnInnerMaterialChanged();
    partial void OnThicknessOuterMaterialChanging(double value);
    partial void OnThicknessOuterMaterialChanged();
    partial void OnThicknessInnerMaterialChanging(double value);
    partial void OnThicknessInnerMaterialChanged();
    #endregion
		
		public Panel()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Id", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int Id
		{
			get
			{
				return this._Id;
			}
			set
			{
				if ((this._Id != value))
				{
					this.OnIdChanging(value);
					this.SendPropertyChanging();
					this._Id = value;
					this.SendPropertyChanged("Id");
					this.OnIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_PanelProfil", DbType="Int NOT NULL")]
		public int PanelProfil
		{
			get
			{
				return this._PanelProfil;
			}
			set
			{
				if ((this._PanelProfil != value))
				{
					this.OnPanelProfilChanging(value);
					this.SendPropertyChanging();
					this._PanelProfil = value;
					this.SendPropertyChanged("PanelProfil");
					this.OnPanelProfilChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_PanelType", DbType="Int NOT NULL")]
		public int PanelType
		{
			get
			{
				return this._PanelType;
			}
			set
			{
				if ((this._PanelType != value))
				{
					this.OnPanelTypeChanging(value);
					this.SendPropertyChanging();
					this._PanelType = value;
					this.SendPropertyChanged("PanelType");
					this.OnPanelTypeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Height", DbType="Int NOT NULL")]
		public int Height
		{
			get
			{
				return this._Height;
			}
			set
			{
				if ((this._Height != value))
				{
					this.OnHeightChanging(value);
					this.SendPropertyChanging();
					this._Height = value;
					this.SendPropertyChanged("Height");
					this.OnHeightChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Width", DbType="Int NOT NULL")]
		public int Width
		{
			get
			{
				return this._Width;
			}
			set
			{
				if ((this._Width != value))
				{
					this.OnWidthChanging(value);
					this.SendPropertyChanging();
					this._Width = value;
					this.SendPropertyChanged("Width");
					this.OnWidthChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_OuterMaterial", DbType="Int NOT NULL")]
		public int OuterMaterial
		{
			get
			{
				return this._OuterMaterial;
			}
			set
			{
				if ((this._OuterMaterial != value))
				{
					this.OnOuterMaterialChanging(value);
					this.SendPropertyChanging();
					this._OuterMaterial = value;
					this.SendPropertyChanged("OuterMaterial");
					this.OnOuterMaterialChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_InnerMaterial", DbType="Int NOT NULL")]
		public int InnerMaterial
		{
			get
			{
				return this._InnerMaterial;
			}
			set
			{
				if ((this._InnerMaterial != value))
				{
					this.OnInnerMaterialChanging(value);
					this.SendPropertyChanging();
					this._InnerMaterial = value;
					this.SendPropertyChanged("InnerMaterial");
					this.OnInnerMaterialChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ThicknessOuterMaterial", DbType="Float NOT NULL")]
		public double ThicknessOuterMaterial
		{
			get
			{
				return this._ThicknessOuterMaterial;
			}
			set
			{
				if ((this._ThicknessOuterMaterial != value))
				{
					this.OnThicknessOuterMaterialChanging(value);
					this.SendPropertyChanging();
					this._ThicknessOuterMaterial = value;
					this.SendPropertyChanged("ThicknessOuterMaterial");
					this.OnThicknessOuterMaterialChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ThicknessInnerMaterial", DbType="Float NOT NULL")]
		public double ThicknessInnerMaterial
		{
			get
			{
				return this._ThicknessInnerMaterial;
			}
			set
			{
				if ((this._ThicknessInnerMaterial != value))
				{
					this.OnThicknessInnerMaterialChanging(value);
					this.SendPropertyChanging();
					this._ThicknessInnerMaterial = value;
					this.SendPropertyChanged("ThicknessInnerMaterial");
					this.OnThicknessInnerMaterialChanged();
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
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.Roofs")]
	public partial class Roof : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _Id;
		
		private int _Height;
		
		private int _Width;
		
		private int _RoofType;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIdChanging(int value);
    partial void OnIdChanged();
    partial void OnHeightChanging(int value);
    partial void OnHeightChanged();
    partial void OnWidthChanging(int value);
    partial void OnWidthChanged();
    partial void OnRoofTypeChanging(int value);
    partial void OnRoofTypeChanged();
    #endregion
		
		public Roof()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Id", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int Id
		{
			get
			{
				return this._Id;
			}
			set
			{
				if ((this._Id != value))
				{
					this.OnIdChanging(value);
					this.SendPropertyChanging();
					this._Id = value;
					this.SendPropertyChanged("Id");
					this.OnIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Height", DbType="Int NOT NULL")]
		public int Height
		{
			get
			{
				return this._Height;
			}
			set
			{
				if ((this._Height != value))
				{
					this.OnHeightChanging(value);
					this.SendPropertyChanging();
					this._Height = value;
					this.SendPropertyChanged("Height");
					this.OnHeightChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Width", DbType="Int NOT NULL")]
		public int Width
		{
			get
			{
				return this._Width;
			}
			set
			{
				if ((this._Width != value))
				{
					this.OnWidthChanging(value);
					this.SendPropertyChanging();
					this._Width = value;
					this.SendPropertyChanged("Width");
					this.OnWidthChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_RoofType", DbType="Int NOT NULL")]
		public int RoofType
		{
			get
			{
				return this._RoofType;
			}
			set
			{
				if ((this._RoofType != value))
				{
					this.OnRoofTypeChanging(value);
					this.SendPropertyChanging();
					this._RoofType = value;
					this.SendPropertyChanged("RoofType");
					this.OnRoofTypeChanged();
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
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.TaskInstance")]
	public partial class TaskInstance : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _Id;
		
		private int _TypeTask;
		
		private int _Status;
		
		private System.Nullable<System.DateTime> _TimeStart;
		
		private int _UserId;
		
		private int _DataTaskId;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIdChanging(int value);
    partial void OnIdChanged();
    partial void OnTypeTaskChanging(int value);
    partial void OnTypeTaskChanged();
    partial void OnStatusChanging(int value);
    partial void OnStatusChanged();
    partial void OnTimeStartChanging(System.Nullable<System.DateTime> value);
    partial void OnTimeStartChanged();
    partial void OnUserIdChanging(int value);
    partial void OnUserIdChanged();
    partial void OnDataTaskIdChanging(int value);
    partial void OnDataTaskIdChanged();
    #endregion
		
		public TaskInstance()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Id", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int Id
		{
			get
			{
				return this._Id;
			}
			set
			{
				if ((this._Id != value))
				{
					this.OnIdChanging(value);
					this.SendPropertyChanging();
					this._Id = value;
					this.SendPropertyChanged("Id");
					this.OnIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_TypeTask", DbType="Int NOT NULL")]
		public int TypeTask
		{
			get
			{
				return this._TypeTask;
			}
			set
			{
				if ((this._TypeTask != value))
				{
					this.OnTypeTaskChanging(value);
					this.SendPropertyChanging();
					this._TypeTask = value;
					this.SendPropertyChanged("TypeTask");
					this.OnTypeTaskChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Status", DbType="Int NOT NULL")]
		public int Status
		{
			get
			{
				return this._Status;
			}
			set
			{
				if ((this._Status != value))
				{
					this.OnStatusChanging(value);
					this.SendPropertyChanging();
					this._Status = value;
					this.SendPropertyChanged("Status");
					this.OnStatusChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_TimeStart", DbType="DateTime")]
		public System.Nullable<System.DateTime> TimeStart
		{
			get
			{
				return this._TimeStart;
			}
			set
			{
				if ((this._TimeStart != value))
				{
					this.OnTimeStartChanging(value);
					this.SendPropertyChanging();
					this._TimeStart = value;
					this.SendPropertyChanged("TimeStart");
					this.OnTimeStartChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_UserId", DbType="Int NOT NULL")]
		public int UserId
		{
			get
			{
				return this._UserId;
			}
			set
			{
				if ((this._UserId != value))
				{
					this.OnUserIdChanging(value);
					this.SendPropertyChanging();
					this._UserId = value;
					this.SendPropertyChanged("UserId");
					this.OnUserIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DataTaskId", DbType="Int NOT NULL")]
		public int DataTaskId
		{
			get
			{
				return this._DataTaskId;
			}
			set
			{
				if ((this._DataTaskId != value))
				{
					this.OnDataTaskIdChanging(value);
					this.SendPropertyChanging();
					this._DataTaskId = value;
					this.SendPropertyChanged("DataTaskId");
					this.OnDataTaskIdChanged();
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
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.TaskSystemLog")]
	public partial class TaskSystemLog
	{
		
		private int _Id;
		
		private int _TaskId;
		
		private int _Type;
		
		private string _Coments;
		
		public TaskSystemLog()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Id", AutoSync=AutoSync.Always, DbType="Int NOT NULL IDENTITY", IsDbGenerated=true)]
		public int Id
		{
			get
			{
				return this._Id;
			}
			set
			{
				if ((this._Id != value))
				{
					this._Id = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_TaskId", DbType="Int NOT NULL")]
		public int TaskId
		{
			get
			{
				return this._TaskId;
			}
			set
			{
				if ((this._TaskId != value))
				{
					this._TaskId = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Type", DbType="Int NOT NULL")]
		public int Type
		{
			get
			{
				return this._Type;
			}
			set
			{
				if ((this._Type != value))
				{
					this._Type = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Coments", DbType="NVarChar(MAX) NOT NULL", CanBeNull=false)]
		public string Coments
		{
			get
			{
				return this._Coments;
			}
			set
			{
				if ((this._Coments != value))
				{
					this._Coments = value;
				}
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.TaskType")]
	public partial class TaskType : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _Id;
		
		private string _Title;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIdChanging(int value);
    partial void OnIdChanged();
    partial void OnTitleChanging(string value);
    partial void OnTitleChanged();
    #endregion
		
		public TaskType()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Id", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int Id
		{
			get
			{
				return this._Id;
			}
			set
			{
				if ((this._Id != value))
				{
					this.OnIdChanging(value);
					this.SendPropertyChanging();
					this._Id = value;
					this.SendPropertyChanged("Id");
					this.OnIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Title", DbType="Text NOT NULL", CanBeNull=false, UpdateCheck=UpdateCheck.Never)]
		public string Title
		{
			get
			{
				return this._Title;
			}
			set
			{
				if ((this._Title != value))
				{
					this.OnTitleChanging(value);
					this.SendPropertyChanging();
					this._Title = value;
					this.SendPropertyChanged("Title");
					this.OnTitleChanged();
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
