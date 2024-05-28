namespace DependencyExample
{
    using RestWrapper;
    using System;
    using System.IO;
    using System.Reflection;
    using System.Runtime.Loader;

    internal class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Write(Environment.NewLine + "URL: ");
                string url = Console.ReadLine();
                if (String.IsNullOrEmpty(url)) continue;

                try
                {
                    #region Local

                    Console.WriteLine("");
                    Console.WriteLine("Local");
                    foreach (Assembly asm in AssemblyLoadContext.Default.Assemblies)
                    {
                        if (asm.FullName.StartsWith("RestWrapper")
                            || asm.FullName.StartsWith("Timestamps"))
                        {
                            Console.WriteLine("| " + asm.FullName);
                        }
                    }

                    using (RestRequest req = new RestRequest(url))
                    {
                        using (RestResponse resp = req.Send())
                        {
                            Console.WriteLine("| Status (runner) : " + resp.StatusCode + " " + resp.ContentLength + " bytes");
                        }
                    }

                    #endregion

                    #region Module1

                    Console.WriteLine("");
                    Console.WriteLine("Module 1");
                    AssemblyLoadContext ctx1 = new AssemblyLoadContext("module1", false);
                    Assembly asm1 = ctx1.LoadFromAssemblyPath(Path.GetFullPath("module1/Module1.dll"));

                    ctx1.Resolving += (ctx, assemblyName) =>
                    {
                        Console.WriteLine("| Module 1 loading assembly " + assemblyName.FullName);
                        var parts = assemblyName.FullName.Split(',');
                        string name = parts[0];
                        var version = Version.Parse(parts[1].Split('=')[1]);
                        string filename = new FileInfo(Path.GetFullPath("module1/" + name + ".dll")).FullName;
                        Console.WriteLine("| Module 1 loading from file " + filename);
                        Console.WriteLine("| Exists: " + File.Exists(filename));

                        Assembly asm = ctx.LoadFromAssemblyPath(filename);
                        Console.WriteLine("| Module 1 loaded assembly " + filename);
                        return asm;
                    };

                    Type type1 = asm1.GetType("Module1.Processor", true);
                    dynamic instance1 = Activator.CreateInstance(type1);
                    instance1.Process(url);

                    #endregion

                    #region Module2

                    Console.WriteLine("");
                    Console.WriteLine("Module 2");
                    AssemblyLoadContext ctx2 = new AssemblyLoadContext("module2", false);
                    Assembly asm2 = ctx2.LoadFromAssemblyPath(Path.GetFullPath("module2/Module2.dll"));

                    ctx2.Resolving += (ctx, assemblyName) =>
                    {
                        Console.WriteLine("| Module 2 loading assembly " + assemblyName.FullName);
                        var parts = assemblyName.FullName.Split(',');
                        string name = parts[0];
                        var version = Version.Parse(parts[1].Split('=')[1]);
                        string filename = new FileInfo(Path.GetFullPath("module2/" + name + ".dll")).FullName;
                        Console.WriteLine("| Module 2 loading from file " + filename);
                        Console.WriteLine("| Exists: " + File.Exists(filename));

                        Assembly asm = ctx.LoadFromAssemblyPath(filename);
                        Console.WriteLine("| Module 2 loaded assembly " + filename);
                        return asm;
                    };

                    Type type2 = asm2.GetType("Module2.Processor", true);
                    dynamic instance2 = Activator.CreateInstance(type2);
                    instance2.Process(url);

                    #endregion

                    Console.WriteLine("Finished URL " + url);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
        }
    }
}
