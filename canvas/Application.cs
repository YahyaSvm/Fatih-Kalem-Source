using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Windows;
using Microsoft.VisualBasic.ApplicationServices;

namespace canvas;

public class Application : System.Windows.Application
{
	[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
	[SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
	internal AssemblyInfo Info
	{
		[DebuggerHidden]
		get
		{
			return new AssemblyInfo(Assembly.GetExecutingAssembly());
		}
	}

	public Application()
	{
		Module1.Splash1.Show(autoClose: false);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public void InitializeComponent()
	{
		base.StartupUri = new Uri("MainWindow.xaml", UriKind.Relative);
	}

	[STAThread]
	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public static void Main()
	{
		Application application = new Application();
		application.InitializeComponent();
		application.Run();
	}
}
