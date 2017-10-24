
#addin nuget:https://www.nuget.org/api/v2/?package=Cake.VersionReader

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

var solution = File("./src/PressureTransientAnalysis.sln");
var buildDir = Directory("./src/Team76.PTA/bin") + Directory(configuration);
var assemblyPath = MakeAbsolute(File((string)buildDir + @"/Team76.PTA.dll"));

Task("Clean")
	.Does(() =>
	{
		CleanDirectories(string.Format("./src/**/obj/{0}", configuration));
		CleanDirectories(string.Format("./src/**/bin/{0}", configuration));		
	});

Task("Restore")
    .IsDependentOn("Clean")
	.Does(() => NuGetRestore(solution));

Task("Build")
    .IsDependentOn("Restore")
    .Does(() => DotNetBuild(solution, settings => settings.SetConfiguration(configuration)
														  .SetVerbosity(Verbosity.Minimal)));

Task("Tests")
    .IsDependentOn("Build")
    .Does(() =>  NUnit3("./src/**/bin/" + configuration + "/*.Tests.dll", 
		new NUnit3Settings { NoResults = true, X86 = true, Verbose = false, Full = false }));

Task("Pack")
     .IsDependentOn("Tests")
	 .Does(() =>
{

	 var version = GetVersionNumber(assemblyPath);
	 Information("Version: {0}", version);
	 
	 var nuGetPackSettings   = new NuGetPackSettings {
                                     Id                      = "Team76.PTA",
                                     Version                 = version,
                                     Title                   = "Team76.PTA",
                                     Authors                 = new[] {"Anton Baluev"},
                                     Owners                  = new[] {"Team 76 Ltd"},
                                     Description             = "Pressure Transient Analysis",
                                     Summary                 = "Pressure transient analysis library for oil and gas",
                                     ProjectUrl              = new Uri("https://github.com/AntonBaluev/PressureTransientAnalysis"),
                                     LicenseUrl              = new Uri("https://github.com/AntonBaluev/PressureTransientAnalysis/blob/master/LICENSE"),
                                     Copyright               = "Team 76 Ltd",
                                     ReleaseNotes            = new [] {"Release"},
                                     Tags                    = new [] {"PTA"},
                                     RequireLicenseAcceptance= false,
                                     Symbols                 = false,
                                     NoPackageAnalysis       = true,
                                     Files                   = new [] {
                                                                          new NuSpecContent {Source = "Team76.PTA.dll", Target = "lib/net452"},
																		  new NuSpecContent {Source = "Team76.PTA.xml", Target = "lib/net452"}
                                                                       },
									 Dependencies            = new List<NuSpecDependency>
												{ 
													new NuSpecDependency { Id= "CuttingEdge.Conditions", Version="1.2.0.0"},
													new NuSpecDependency { Id= "MathNet.Numerics", Version="3.20.0"}
												},

                                     BasePath                = buildDir,
                                     OutputDirectory         = buildDir
                                 };

     NuGetPack(nuGetPackSettings);
});


Task("Default")
    .IsDependentOn("Pack")
	;

	
RunTarget(target);