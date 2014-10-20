@echo off
cd %~dp0

set EnableNuGetPackageRestore=true
.nuget\NuGet.exe install Cake -ExcludeVersion -OutputDirectory packages
packages\Cake\Cake.exe build.cake -verbosity=diagnostic %*