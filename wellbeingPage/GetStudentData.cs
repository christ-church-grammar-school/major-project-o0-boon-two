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

            Options.AddAdditionalCapability("useAutomationExtension", false);

            var Driver = new ChromeDriver(driverService,Options);
        }
    }
}
