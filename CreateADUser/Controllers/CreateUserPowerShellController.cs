using System;
using System.Management.Automation;
using System.Web.Mvc;
using CreateADUser.Models;

namespace CreateADUser.Controllers
{
    public class CreateUserPowerShellController : Controller
    {
        public ActionResult Index()
        {
            var model = new CreateUser();
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(CreateUser userData)
        {
            if (ModelState.IsValid)
            {
                if (userData != null)
                {
                    // PowerShell class implements IDisposable, this way the instance is automatically disposed of
                    using (PowerShell powerShellInstance = PowerShell.Create())
                    {
                        // Add the script to the pipeline
                        powerShellInstance.AddCommand(AppDomain.CurrentDomain.BaseDirectory + "\\Powershell\\New-User.ps1");

                        // Add the parameters to the script based on the values entered by the user
                        powerShellInstance.AddParameter("FirstName", userData.FirstName);
                        powerShellInstance.AddParameter("LastName", userData.LastName);
                        powerShellInstance.AddParameter("Department", userData.Department);

                        try
                        {
                            // Attempt to invoke the pipeline
                            var results = powerShellInstance.Invoke();
                        }
                        catch (Exception e)
                        {
                            // Catch exception and display within validation error summary
                            ModelState.AddModelError("", "Error received when calling New-User.ps1 " + e);
                            return View(userData);
                        }

                        // Check for PowerShell errors - these errors will not be caught within the Try-Catch
                        if (powerShellInstance.Streams.Error.Count > 0)
                        {
                            foreach (var error in powerShellInstance.Streams.Error)
                            {
                                // Add each error to the validation error summary
                                ModelState.AddModelError("", error.ToString());
                            }

                            return View(userData);
                        }

                        // If no errors then redirect to the completed page
                        return RedirectToAction("Completed");
                    }
                }
            }

            return View(userData);
        }

        public ActionResult Completed()
        {
            return View();
        }
    }
}
