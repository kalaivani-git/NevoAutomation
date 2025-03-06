using Nevo.Base;
using OpenQA.Selenium;


namespace Nevo.PageObject
{
    public class HomePage : BaseClass
    {

        IWebDriver driver;
        CommonUtils commonMethod = new CommonUtils();

        public HomePage(IWebDriver driver)
        {
            this.driver = driver;
        }

        public void clickBrowseVehicles()
        {
            IWebElement browseVehiclesButton = driver.FindElement(By.XPath("//a[contains(text(),'Browse Vehicles')]"));
            browseVehiclesButton.Click();
        }
        public void handleCookies(String options)
        {
            By cookieOptions = By.XPath($"//button[text()='{options}']");
            commonMethod.clickElement(driver, cookieOptions, "Clicking cookie options");
        }

        public void selectNavigationHeader(String headerName)
        {
            By navigationheader = By.XPath($"//span//a[text()='{headerName}']");
            commonMethod.clickElement(driver, navigationheader, "");
        }

    }
}
