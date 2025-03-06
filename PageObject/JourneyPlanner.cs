using Nevo.Base;
using Nevo.HelperUtilities;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nevo.PageObject
{
    public class JourneyPlanner : BaseClass
    {

        IWebDriver driver;
        CommonUtils commonMethod = new CommonUtils();
        public JourneyPlanner(IWebDriver driver)
        {
            this.driver = driver;
        }

        public void planJourneyPage()
        {
            IWebElement toolsMenu = driver.FindElement(By.XPath("//a[contains(text(),'Tools')]"));
            Actions actions = new Actions(driver);
            actions.MoveToElement(toolsMenu).Perform();
            IWebElement journeyPlanner1 = driver.FindElement(By.XPath("//a[contains(text(),'Journey Planner')]"));
            journeyPlanner1.Click();
            By startPointField = By.XPath("//input[@placeholder='Start Point']");
            WaitUtils.waitUntilElementIsVisible(driver, startPointField, 30);
            commonMethod.enterKeys(driver, startPointField, "Dublin", "Entering start point");
            By destinationField = By.XPath("//input[@placeholder='Destination']");
            WaitUtils.waitUntilElementIsVisible(driver, destinationField, 30);
            commonMethod.enterKeys(driver, destinationField, "Cork", "Entering Destination Point");
            By planJourney = By.XPath("//button[contains(text(),'Plan Journey')]");
            commonMethod.clickElement(driver, planJourney, "Plan journey clicked");
            By journey = By.XPath("//div[text()='Vehicle Model']");
            verification.validateElementExistsInBrowser(driver, journey, "yes");
        }

        public String journeyPlanner()
        {
            By helpHeaderXpath = By.XPath("//h1[text()='Journey Planner']");
            String headerName = commonMethod.getElementText(driver, helpHeaderXpath, "getting header");
            Console.WriteLine(headerName);
            return headerName;
        }






    }

}
