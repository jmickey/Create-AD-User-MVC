[CmdletBinding()]
param (
    [Parameter(Mandatory=$True)]
            [String]$FirstName,
    [Parameter(Mandatory=$True)]
        [String]$LastName,
    [Parameter(Mandatory=$True)]
        [String]$Department
)

if (!(Get-Module ActiveDirectory)) {
    Import-Module ActiveDirectory
}

$DisplayName = "$FirstName $LastName"
$Username = "$FirstName.$LastName"
$DomainName = "contoso.com"
$Password = ConvertTo-SecureString -AsPlainText "P@ssw0rd@1" -Force

New-ADUser -Name $DisplayName -DisplayName $DisplayName -GivenName $FirstName -Surname $LastName -Department $Department -Path "OU=Users,OU=Contoso,DC=contoso,DC=com" -UserPrincipalName $Username@$DomainName -SamAccountName $Username -AccountPassword $Password