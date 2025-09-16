using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using Microsoft.VisualBasic.CompilerServices;

namespace canvas;

[CompilerGenerated]
[GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "14.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
internal sealed class MySettings : ApplicationSettingsBase
{
	private static MySettings defaultInstance = (MySettings)SettingsBase.Synchronized(new MySettings());

	public static MySettings Default => defaultInstance;

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("1")]
	public byte PenStyle
	{
		get
		{
			return Conversions.ToByte(this["PenStyle"]);
		}
		set
		{
			this["PenStyle"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("2")]
	public byte ColorNo
	{
		get
		{
			return Conversions.ToByte(this["ColorNo"]);
		}
		set
		{
			this["ColorNo"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("3")]
	public byte InkSize
	{
		get
		{
			return Conversions.ToByte(this["InkSize"]);
		}
		set
		{
			this["InkSize"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("True")]
	public bool isStartupPositionDefault
	{
		get
		{
			return Conversions.ToBoolean(this["isStartupPositionDefault"]);
		}
		set
		{
			this["isStartupPositionDefault"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("0")]
	public int Left
	{
		get
		{
			return Conversions.ToInteger(this["Left"]);
		}
		set
		{
			this["Left"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("0")]
	public int Top
	{
		get
		{
			return Conversions.ToInteger(this["Top"]);
		}
		set
		{
			this["Top"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("False")]
	public bool AutoStart
	{
		get
		{
			return Conversions.ToBoolean(this["AutoStart"]);
		}
		set
		{
			this["AutoStart"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("#FF00009D")]
	public Color InkColor
	{
		get
		{
			object obj = this["InkColor"];
			if (obj == null)
			{
				return default(Color);
			}
			return (Color)obj;
		}
		set
		{
			this["InkColor"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("True")]
	public bool isSideArrowsEnabled
	{
		get
		{
			return Conversions.ToBoolean(this["isSideArrowsEnabled"]);
		}
		set
		{
			this["isSideArrowsEnabled"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("True")]
	public bool isShortGestureEnabled
	{
		get
		{
			return Conversions.ToBoolean(this["isShortGestureEnabled"]);
		}
		set
		{
			this["isShortGestureEnabled"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("False")]
	public bool isCLoseConfirmationEnabled
	{
		get
		{
			return Conversions.ToBoolean(this["isCLoseConfirmationEnabled"]);
		}
		set
		{
			this["isCLoseConfirmationEnabled"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("True")]
	public bool isGestureEnabled
	{
		get
		{
			return Conversions.ToBoolean(this["isGestureEnabled"]);
		}
		set
		{
			this["isGestureEnabled"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("")]
	public string Fav1
	{
		get
		{
			return Conversions.ToString(this["Fav1"]);
		}
		set
		{
			this["Fav1"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("")]
	public string Fav2
	{
		get
		{
			return Conversions.ToString(this["Fav2"]);
		}
		set
		{
			this["Fav2"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("")]
	public string Fav3
	{
		get
		{
			return Conversions.ToString(this["Fav3"]);
		}
		set
		{
			this["Fav3"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("")]
	public string Fav4
	{
		get
		{
			return Conversions.ToString(this["Fav4"]);
		}
		set
		{
			this["Fav4"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("")]
	public string Fav5
	{
		get
		{
			return Conversions.ToString(this["Fav5"]);
		}
		set
		{
			this["Fav5"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("")]
	public string Fav6
	{
		get
		{
			return Conversions.ToString(this["Fav6"]);
		}
		set
		{
			this["Fav6"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("")]
	public string Fav7
	{
		get
		{
			return Conversions.ToString(this["Fav7"]);
		}
		set
		{
			this["Fav7"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("")]
	public string Fav8
	{
		get
		{
			return Conversions.ToString(this["Fav8"]);
		}
		set
		{
			this["Fav8"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("")]
	public string Fav9
	{
		get
		{
			return Conversions.ToString(this["Fav9"]);
		}
		set
		{
			this["Fav9"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("")]
	public string Fav10
	{
		get
		{
			return Conversions.ToString(this["Fav10"]);
		}
		set
		{
			this["Fav10"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("")]
	public string Fav11
	{
		get
		{
			return Conversions.ToString(this["Fav11"]);
		}
		set
		{
			this["Fav11"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("")]
	public string Fav12
	{
		get
		{
			return Conversions.ToString(this["Fav12"]);
		}
		set
		{
			this["Fav12"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("0")]
	public byte iFav
	{
		get
		{
			return Conversions.ToByte(this["iFav"]);
		}
		set
		{
			this["iFav"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("")]
	public string Fav13
	{
		get
		{
			return Conversions.ToString(this["Fav13"]);
		}
		set
		{
			this["Fav13"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("")]
	public string Fav14
	{
		get
		{
			return Conversions.ToString(this["Fav14"]);
		}
		set
		{
			this["Fav14"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("")]
	public string Fav15
	{
		get
		{
			return Conversions.ToString(this["Fav15"]);
		}
		set
		{
			this["Fav15"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("")]
	public string Fav16
	{
		get
		{
			return Conversions.ToString(this["Fav16"]);
		}
		set
		{
			this["Fav16"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("")]
	public string Fav17
	{
		get
		{
			return Conversions.ToString(this["Fav17"]);
		}
		set
		{
			this["Fav17"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("")]
	public string Fav18
	{
		get
		{
			return Conversions.ToString(this["Fav18"]);
		}
		set
		{
			this["Fav18"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("")]
	public string Fav19
	{
		get
		{
			return Conversions.ToString(this["Fav19"]);
		}
		set
		{
			this["Fav19"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("")]
	public string Fav20
	{
		get
		{
			return Conversions.ToString(this["Fav20"]);
		}
		set
		{
			this["Fav20"] = value;
		}
	}
}
