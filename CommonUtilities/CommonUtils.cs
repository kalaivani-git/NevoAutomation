#region File Header
/*  ----------------------------------------------------------------------------
 *  NibrasUTTest Automation : ConcertIDC
 *  ----------------------------------------------------------------------------
 *  ----------------------------------------------------------------------------
 *  File:
 *  Author:    Nagabhushana A
 *  Reworker: 
 *  Description : Common utility Utility created
 *  Creation Date: 17-11-2023
 *  Modified Date: 
 *  ----------------------------------------------------------------------------
 */
#endregion
using AventStack.ExtentReports;
using Nevo.HelperUtilities;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using Nevo.Base;

namespace Nevo
{
    public class CommonUtils : BaseClass
    {
     
        string elementText = null;
        Actions action = new Actions(driver);
        IList<IWebElement> listOfwebElements;

        public void clickElement(IWebDriver driver, By element, string stepDescription)
        {
            try
            {
                for (int i = 0; i < 8; i++)
                {
                    try
                    {
                        PerformClick(driver, element, stepDescription);
                        return;
                    }
                    catch (StaleElementReferenceException)
                    {
                        // Handle stale element exception
                        Console.WriteLine("Stale element exception occurred. Retrying...");
                    }
                    catch (ElementClickInterceptedException e)
                    {
                        Console.WriteLine("Element Click Intercepted Exception occurred. Retrying...");

                    }
                    catch (ElementNotInteractableException)
                    {
                        Console.WriteLine("Element Click Element Not Interactable Exception occurred. Retrying...");

                    }
                    Thread.Sleep(2000);
                }
                throw new ElementClickInterceptedException("Failed to click element after retries."); // Custom exception

            }


            catch (Exception e)
            {
                // Handle and log other exceptions
                handleException(e, "Error clicking element", stepDescription);
            }
        }

        private void PerformClick(IWebDriver driver, By element, string stepDescription)
        {
            // Wait for the element to be present and visible

            IWebElement foundElement = WaitUtils.FindElement(driver, element, 40);
            if (foundElement != null)
            {
                foundElement.Click();
            }
            else
            {
                throw new NoSuchElementException("Element not found: " + element.ToString());
            }

            // Log success
            if (_test != null)
            {
                _test.Pass("Passed : " + stepDescription);
            }
        }


        public void enterKeys(IWebDriver driver, By element, string keys, String stepDescription)
        {
            try
            {
                for (int i = 0; i < 5; i++)
                {
                    try
                    {
                        WaitUtils.FindElement(driver, element, 60);
                        driver.FindElement(element).Clear();
                        driver.FindElement(element).SendKeys(keys);
                        if (_test != null)
                        {
                            _test.Pass("Passed : " + stepDescription);
                        }
                        return;
                    }
                    catch (JavaScriptException)
                    {
                        Console.WriteLine("JavaScriptException has occured, retrying in process");
                    }
                    catch (ElementNotInteractableException)
                    {
                        Console.WriteLine("ElementNotInteractableException has occured, retrying in process");
                    }
                    Thread.Sleep(1000); // time interval before retrying
                }
            }
            catch (Exception e)
            {
                captureScreenshotForCI(driver);
                if (_test != null)
                {
                    _test.Fail("Failed : " + stepDescription + e.Message + e.StackTrace, captureScreenshot(driver, filename));
                }
                NetLogger.Log(NetLogger.Severity.Exception, "Exception: at enterKeys : " + e.Message);
                throw new InvalidOperationException("Error in enter Keys object." + Environment.NewLine + e.ToString());
            }
        }

        public string getElementText(IWebDriver driver, By element, String stepDescription)
        {
            WaitUtils.FindElement(driver, element, 60);

            try
            {
                Thread.Sleep(3000); // necessary to wait if we remove this then it returns null value
                elementText = driver.FindElement(element).Text;
                if (_test != null)
                {
                    _test.Pass("Passed : " + stepDescription);
                }
            }
            catch (Exception e)
            {
                captureScreenshotForCI(driver);
                _test.Fail("Failed : " + stepDescription + e.Message + e.StackTrace, captureScreenshot(driver, filename));
                NetLogger.Log(NetLogger.Severity.Exception, "Exception: at getElementText : " + e.Message);
                throw new InvalidOperationException("Error in get element object." + Environment.NewLine + e.ToString());
            }
            return elementText;
        }



        public void handleException(Exception ex, string errorMessage, string description)
        {
            Console.WriteLine($"{errorMessage}: {ex.Message}");
            captureScreenshotForCI(driver);
            string filename = $"{DateTime.Now:yyyyMMddHHmmss}_Error.png";
            if (_test != null)
            {
                _test.Fail($"Failed: {description} - {ex.Message}", captureScreenshot(driver, filename));
                NetLogger.Log(NetLogger.Severity.Exception, $"{errorMessage}: {ex.Message} {Environment.NewLine}{ex.StackTrace}");

            }

            NetLogger.Log(NetLogger.Severity.Exception, $"{errorMessage}: {ex.Message} {Environment.NewLine}{ex.StackTrace}");
            throw new InvalidOperationException($"{errorMessage}: {description} {Environment.NewLine}{ex.ToString()}", ex);
        }

    }

}



