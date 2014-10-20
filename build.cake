var target = Argument("Target", "Unit-Tests");							// Task to run
var configuration = Argument("Configuration", "Release");				// Build configuration (Debug/Release)
var assemblyVersion = Argument("AssemblyVersion", "0.0.0.0");			// Sets the assembly version
var assemblyFileVersion = Argument("FileVersion", "0.0.0.0");			// Sets the assembly file version
var prereleaseVersion = Argument("PrereleaseVersion", string.Empty);	// Sets the assembly informational version postfix
var nugetApiKey = Argument("NuGetKey", string.Empty);					// NuGet API Key

var buildDir = "./src/bin/" + configuration;
var nugetPath = "./.nuget/NuGet.exe";
var nuspecsDir = @"./nuspecs";
var version =  assemblyVersion + prereleaseVersion;

Task("Clean")
	.Does(() =>
{	
	CleanDirectory(buildDir);
	DeleteFiles(nuspecsDir + "/*.nupkg");
});

Task("Restore-NuGet-Packages")
	.IsDependentOn("Clean")
	.Does(context =>
{
	NuGetRestore("./EchoNest-Sharp.sln", new NuGetRestoreSettings
	{
		ToolPath = nugetPath
	});
});

Task("Patch-Assembly-Info")
	.Description("Patches the AssemblyInfo files.")
	.IsDependentOn("Restore-NuGet-Packages")
	.Does(() =>
{
	var file = "./src/EchoNest-Sharp/Properties/AssemblyInfo.cs";
	CreateAssemblyInfo(file, new AssemblyInfoSettings {
		Product = "EchoNest-Sharp",
		Version = assemblyVersion,
		FileVersion = assemblyFileVersion,
		InformationalVersion = version,
		Copyright = "Copyright (c) Patrik Svensson 2014"
	});
});

Task("Build")
	.IsDependentOn("Patch-Assembly-Info")
	.Description("Compiles the solution")
	.Does(() =>
{
	MSBuild("./EchoNest-Sharp.sln", s => s
		.SetConfiguration(configuration)
		.WithProperty("TreatWarningsAsErrors", "true")
		.WithProperty("StyleCopTreatErrorsAsWarnings", "false")
		.WithProperty("AllowedReferenceRelatedFileExtensions", ".pdb")); 
});

Task("Unit-Tests")
	.IsDependentOn("Build")
	.Description("Runs all unit tests")
	.Does(() =>
{
	MSTest("./src/**/bin/" + configuration + "/*.Tests.dll");
});

Task("Create-NuGet-Package")
	.IsDependentOn("Unit-Tests")
	.Description("Create NuGet package")
	.Does(() =>
{
	NuGetPack("./nuspecs/EchoNest-Sharp.nuspec", new NuGetPackSettings 
	{
		ToolPath = nugetPath,
		Version = version,
		OutputDirectory = nuspecsDir,
		NoPackageAnalysis = true
	});
});

Task("Publish-NuGet")
	.IsDependentOn("Create-NuGet-Package")
	.WithCriteria(() => !string.IsNullOrWhiteSpace(nugetApiKey))
	.Description("Push NuGet package to server")
	.Does(() =>
{
	var packages = GetFiles(nuspecsDir + "/*.nupkg");
	foreach(var package in packages)
	{
		Information("NuGet Push - " + package);
		
		// Push the package.
		NuGetPush(package, new NuGetPushSettings 
		{
			ToolPath = nugetPath,
			ApiKey = nugetApiKey
		});
	}
});

Task("Publish")
	.IsDependentOn("Unit-Tests")
	.IsDependentOn("Publish-NuGet");

//////////////////////////////////////////////////////////////////////

RunTarget(target);
