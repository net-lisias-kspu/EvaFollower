using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("EVA Follower /L Unleashed")]
[assembly: AssemblyDescription("Allows you to order Kerbals around, make them follow you, or make them patrol the area.")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany(EvaFollower.LegalMamboJambo.Company)]
[assembly: AssemblyProduct(EvaFollower.LegalMamboJambo.Product)]
[assembly: AssemblyCopyright(EvaFollower.LegalMamboJambo.Copyright)]
[assembly: AssemblyTrademark(EvaFollower.LegalMamboJambo.Trademark)]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("3c084a49-f116-491d-839a-d3aab10d3899")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion(EvaFollower.Version.Number)]
[assembly: AssemblyFileVersion(EvaFollower.Version.Number)]
[assembly: KSPAssembly("EvaFollower", EvaFollower.Version.major, EvaFollower.Version.minor)]

[assembly: KSPAssemblyDependency("KSPe", 2, 4)]
[assembly: KSPAssemblyDependency("KSPe.UI", 2, 4)]
