using Microsoft.Net.Http.Headers;
using Nevo.Base;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nevo.PageObject
{
    public class BrowseVehicles : BaseClass
    {
        IWebDriver driver;
        CommonUtils commonMethod = new CommonUtils();


        public BrowseVehicles(IWebDriver driver)
        {
            this.driver = driver;

        }

        public int getResultCount()
        {
            By getresult = By.XPath("//h1[contains(@class,'styles_countText')]/child::span");
            String count = commonMethod.getElementText(driver, getresult, "Getting count of EVs");
            string str = count;
            int num = int.Parse(str);
            return num;
        }
        public String browseVehiclesHeader()
        {
            By BvHeaderXpath = By.XPath("//div[contains(@class,'styles_pageHeaderContent')]//h1");
            String headerName = commonMethod.getElementText(driver, BvHeaderXpath, "Getting Browse Vehicles Header");
            return headerName;
        }



    }
}
