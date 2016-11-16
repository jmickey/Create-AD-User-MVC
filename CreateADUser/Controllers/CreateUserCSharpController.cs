using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.DirectoryServices.AccountManagement;
using CreateADUser.Models;
using System.DirectoryServices;

namespace CreateADUser.Controllers
{
    public class CreateUserCSharpController : Controller
    {
        public ActionResult Index()
        {
            var model = new CreateUser();
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(CreateUser model)
        {
            // NetBIOS name of domain. E.g. CONTOSO
            var domainName = "CONTOSO";
            // Full distinguished name of OU to create user in. E.g. OU=Users,OU=Perth,DC=Contoso,DC=com
            var userOU = "OU=Users,OU=Contoso,DC=contoso,DC=com";

            // Source: http://stackoverflow.com/a/2305871
            using (var pc = new PrincipalContext(ContextType.Domain, domainName, userOU))
            {
                using (var up = new UserPrincipal(pc))
                {
                    // Create username and display name from firstname and lastname
                    var userName = model.FirstName + "." + model.LastName;
                    var displayName = model.FirstName + " " + model.LastName;
                    // In a real scenario a randomised password would be preferred
                    var password = "P@ssw0rd#1";

                    // Set the values for new user account
                    up.Name = displayName;
                    up.DisplayName = displayName;
                    up.GivenName = model.FirstName;
                    up.Surname = model.LastName;
                    up.SamAccountName = userName;
                    up.SetPassword(password);
                    up.Enabled = true;
                    up.ExpirePasswordNow();

                    try
                    {
                        // Attempt to save the account to AD
                        up.Save();
                    }
                    catch (Exception e)
                    {
                        // Display exception(s) within validation summary
                        ModelState.AddModelError("","Exception creating user object. " + e);
                        return View(model);
                    }

                    // Add the department to the newly created AD user
                    // Get the directory entry object for the user
                    DirectoryEntry de = up.GetUnderlyingObject() as DirectoryEntry;
                    // Set the department property to the value entered by the user
                    de.Properties["department"].Value = model.Department;
                    try
                    {
                        // Try to commit changes
                        de.CommitChanges();
                    } 
                    catch (Exception e)
                    {
                        // Display exception(s) within validation summary
                        ModelState.AddModelError("", "Exception adding department. " + e);
                        return View(model);
                    }
                }
            }

            // Redirect to completed page if successful
            return RedirectToAction("Completed");
        }

        public ActionResult Completed()
        {
            return View();
        }
    }
}
