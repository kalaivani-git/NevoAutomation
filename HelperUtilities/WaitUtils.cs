using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
namespace Nevo.HelperUtilities
{
    public static class WaitUtils
    {
        public static IWebElement FindElement(this IWebDriver driver, By by, int timeoutInSeconds)
        {
            if (timeoutInSeconds > 0)
            {
                try
                {
                    // Define the fluent wait
                    DefaultWait<IWebDriver> fluentWait = new DefaultWait<IWebDriver>(driver)
                    {
                        Timeout = TimeSpan.FromSeconds(timeoutInSeconds),
                        PollingInterval = TimeSpan.FromMilliseconds(500)
                    };

                    // Ignore specific exceptions during the wait
                    fluentWait.IgnoreExceptionTypes(typeof(NoSuchElementException));

                    // Use the fluent wait to find the element
                    return fluentWait.Until(drv => drv.FindElement(by));
                }
                catch (WebDriverTimeoutException)
                {
                    // Log the issue and return null
                    Console.WriteLine($"Element with locator '{by}' was not found within {timeoutInSeconds} seconds.");
                    return null;
                }
            }
            try
            {
                // Try to find the element without waiting
                return driver.FindElement(by);
            }
            catch (NoSuchElementException)
            {
                // Log the issue and return null
                Console.WriteLine($"Element with locator '{by}' was not found.");
                return null;
            }
        }

        public static IWebElement waitUntilElementIsVisible(IWebDriver driver, By locator, int seconds)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(seconds));

            try
            {
                // Wait for the element to become visible
                return wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(locator));
            }
            catch (WebDriverTimeoutException ex)
            {
                Console.WriteLine("Timeout waiting for element to become visible: " + ex.Message);
                return null; // Return null to indicate failure to wait for element visibility
            }
        }
      

        
    }
}
