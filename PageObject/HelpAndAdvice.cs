using Nevo.Base;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nevo.PageObject
{
    public class HelpAndAdvice : BaseClass
    {
        IWebDriver driver;
        CommonUtils commonMethod = new CommonUtils();


        public HelpAndAdvice(IWebDriver driver)
        {
            this.driver = driver;

        }
        public String helpAndAdvice()
        {
            By helpHeaderXpath = By.XPath("//h1[text()='Help & Advice']");
            String headerName = commonMethod.getElementText(driver, helpHeaderXpath, "Getting Help&Advice Header");
            return headerName;
        }
    }
}
