/* 项目:不用部署就运行网站
 * 日期:2009.10.1
   来源:奎宇工作室 整理
 */
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;

namespace StartExamples
{
	internal static class Program
	{
		private static string GetDevServerExecutable()
		{
			string str = string.Format("{0}\\Microsoft Shared\\DevServer\\10.0\\WebDev.WebServer40.exe", Environment.GetEnvironmentVariable("CommonProgramFiles"));
			if (!File.Exists(str))
			{
				str = string.Format("{0}\\Microsoft Shared\\DevServer\\10.0\\WebDev.WebServer40.exe", Environment.GetEnvironmentVariable("CommonProgramFiles(x86)"));
			}
			if (!File.Exists(str))
			{
				str = string.Format("{0}\\Microsoft Shared\\DevServer\\11.0\\WebDev.WebServer40.exe", Environment.GetEnvironmentVariable("CommonProgramFiles"));
				if (!File.Exists(str))
				{
					str = string.Format("{0}\\Microsoft Shared\\DevServer\\11.0\\WebDev.WebServer40.exe", Environment.GetEnvironmentVariable("CommonProgramFiles(x86)"));
				}
                if (!File.Exists(str))
                {
                    str = string.Format("{0}\\IIS Express\\iisexpress.exe", Environment.GetEnvironmentVariable("ProgramFiles(x86)"));
                }
				return str;
			}
			else
			{
				return str;
			}
		}

		private static string GetPortNumber()
		{
			TcpListener tcpListener;
			bool flag = false;
			int port = 0;
			try
			{
				tcpListener = new TcpListener(IPAddress.Any, 0x206d);
				tcpListener.ExclusiveAddressUse = true;
				tcpListener.Start();
				port = ((IPEndPoint)tcpListener.LocalEndpoint).Port;
				tcpListener.Stop();
			}
			catch (SocketException socketException)
			{
				flag = true;
			}
			if (flag)
			{
				try
				{
					tcpListener = new TcpListener(IPAddress.Any, 0);
					tcpListener.Start();
					port = ((IPEndPoint)tcpListener.LocalEndpoint).Port;
					tcpListener.Stop();
				}
				catch (Exception exception)
				{
				}
			}
			if (port == 0)
			{
				port = 0x206d;
			}
			return port.ToString();
		}

		[STAThread]
		private static void Main(string[] args)
		{
			string currentDirectory = Environment.CurrentDirectory;
			string str = "/";
			Program.ProcessParameters(args, ref currentDirectory, ref str);
			if (!Path.IsPathRooted(currentDirectory))
			{
				currentDirectory = Path.Combine(Environment.CurrentDirectory, currentDirectory);
			}
			string devServerExecutable = Program.GetDevServerExecutable();
			string portNumber = Program.GetPortNumber();
			if (File.Exists(devServerExecutable))
			{
				Program.StartProcess(devServerExecutable, portNumber, currentDirectory, str);
				return;
			}
			else
			{
				Console.WriteLine("ASP.NET 服务环境没有找到!");
				return;
			}
		}

		private static void ProcessParameters(string[] args, ref string physPath, ref string appName)
		{
			string[] strArrays = args;
			for (int i = 0; i < (int)strArrays.Length; i++)
			{
				string str = strArrays[i];
				if (!str.StartsWith("-path:", StringComparison.OrdinalIgnoreCase))
				{
					if (!str.StartsWith("-appname:", StringComparison.OrdinalIgnoreCase))
					{
						if (str.StartsWith("-help", StringComparison.OrdinalIgnoreCase) || str.StartsWith("-?", StringComparison.OrdinalIgnoreCase))
						{
							Program.ShowHelp();
						}
					}
					else
					{
						appName = str.Remove(0, 9);
					}
				}
				else
				{
					string str1 = "\"";
					string str2 = "'";
					physPath = str.Remove(0, 6);
					if (physPath.EndsWith(str1) && physPath.StartsWith(str1) || physPath.EndsWith(str2) && physPath.StartsWith(str2))
					{
						physPath = physPath.Remove(0, 1);
						physPath = physPath.Remove(physPath.Length - 1);
					}
				}
			}
		}

		private static void ShowHelp()
		{
			MessageBox.Show("Usage: {0} [-path:<path to root of web>] [-appname:/<name of web application>]", Application.ExecutablePath.Substring(Application.ExecutablePath.LastIndexOf("\\") + 1));
		}

		private static void StartProcess(string cassiniExecutable, string portNumber, string physPath, string appName)
		{
			Process process = new Process();
			process.StartInfo.FileName = cassiniExecutable;
			process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
			process.StartInfo.Arguments = string.Format("/port:{0} /path:\"{1}\" /vpath:\"{2}\"", portNumber, physPath, appName);
			process.Start();
			WaitMessageForm waitMessageForm = new WaitMessageForm();
			waitMessageForm.ShowDialog();
			Process process1 = new Process();
			process1.StartInfo.UseShellExecute = true;
			process1.StartInfo.FileName = string.Format("http://localhost:{0}{1}", portNumber, appName);
			process1.Start();
		}
	}
}