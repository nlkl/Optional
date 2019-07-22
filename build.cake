var target = Argument("target", "Default");

Task("Default")
    .IsDependentOn("Build")
    .IsDependentOn("Test")
    .Does(() =>
    {
    });

Task("Build")
    .Does(() =>
    {
        MSBuild("./src/Optional.sln", new MSBuildSettings 
        {
            Verbosity = Verbosity.Minimal,
            ToolVersion = MSBuildToolVersion.VS2017,
            Configuration = "Release",
            PlatformTarget = PlatformTarget.MSIL
        });
    });

Task("Test")
    .IsDependentOn("Build")
    .Does(() =>
    {
        MSTest("./src/Optional.Tests/bin/release/**/Optional.Tests.dll");
    });

Task("Pack")
    .IsDependentOn("Build")
    .IsDependentOn("Test")
    .Does(() =>
    {
        Pack("Optional", new [] { "net35", "net45", "netstandard1.0", "netstandard2.0" });
    });
    
RunTarget(target);

public void Pack(string projectName, string[] targets) 
{
    var nuGetPackSettings   = new NuGetPackSettings 
    {
        NoPackageAnalysis = true,
        BasePath = "./src/" + projectName + "/bin/release",
        OutputDirectory = "./nuget/" + projectName,
        Files = targets
            .SelectMany(target => new []
            {
                new NuSpecContent { Source = target + "/" + projectName + ".dll", Target = "lib/" + target },
                new NuSpecContent { Source = target + "/" + projectName + ".xml", Target = "lib/" + target }
            })
            .ToArray()
    };
    NuGetPack("./nuget/" + projectName + ".nuspec", nuGetPackSettings);
}