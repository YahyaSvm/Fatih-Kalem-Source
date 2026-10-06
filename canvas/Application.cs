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
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public static void Main()
	{
		try
		{
			Application application = new Application();
			application.InitializeComponent();
			application.Run();
		}
		catch (Exception ex)
		{
			string logPath = WriteCrashLog(ex);
			System.Windows.MessageBox.Show(
				"Fatih Kalem beklenmeyen bir hata nedeniyle kapandı.\n\nAyrıntılar şu dosyaya kaydedildi:\n" + logPath +
				"\n\nSorunu bildirmek için: https://github.com/YahyaSvm/Fatih-Kalem-Source/issues",
				"Fatih Kalem", MessageBoxButton.OK, MessageBoxImage.Error);
		}
	}

	private static string WriteCrashLog(Exception ex)
	{
		string text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\r\n" + ex;
		try
		{
			string dir = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Fatih Kalem");
			System.IO.Directory.CreateDirectory(dir);
			string path = System.IO.Path.Combine(dir, "hata.log");
			System.IO.File.AppendAllText(path, text + "\r\n\r\n");
			return path;
		}
		catch (Exception)
		{
			return "(günlük dosyası yazılamadı)";
		}
	}
}
