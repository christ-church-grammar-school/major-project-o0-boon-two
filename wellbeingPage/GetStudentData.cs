using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Chrome;

namespace wellbeingPage
{
    class GetStudentData
    {
        
        public static void Start()
        {
            var driverService = ChromeDriverService.CreateDefaultService();
            driverService.HideCommandPromptWindow = true;

            var Options = new ChromeOptions();
            //Options.AddArgument("--window-position=-32000,-32000");  //sets chrome window to out of screen (-32000, -320000)
            Options.AddAdditionalCapability("useAutomationExtension", false);
            //Options.AddArgument("--headless");
            //Options.AddArgument("--hide"); 
            ChromeDriver driver = new ChromeDriver(driverService,Options);

            driver.Navigate().GoToUrl("https://parentportal.ccgs.wa.edu.au/");
            System.Threading.Thread.Sleep(5000);
        }
    }
}
    