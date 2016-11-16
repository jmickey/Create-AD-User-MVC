# Create-AD-User-MVC
Demo for creating a new AD user within an ASP.NET MVC application with both PowerShell and native C#

# Usage
- Computer or server will need to be a domain member
- Computer or server will need to have the Active Directory RSAT installed
- The user account used to run the application will require local admin and permission to add users to the OU that you specify
- Clone the repo and open the solution in Visual Studio (created in 2015)
- Edit domain and OU variables within the PowerShell\New-User.ps1 file
- Edit domain and OU variables within the Controllers\CreateUserCSharp.cs file
- Run solution or deploy to IIS
