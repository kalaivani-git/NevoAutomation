using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using NUnit.Framework.Interfaces;
using Nevo.Settings;
using Nevo.HelperUtilities;
using Nevo.PageObject;
namespace Nevo.Base
{
    
    public class BaseClass 
    {
        public static IWebDriver? driver;
        public static Verification verification = new Verification();
        public static ExtentReports _extent = new ExtentReports();
        public static ExtentHtmlReporter? _extentHtmlReporter;
        public static ExtentTest _test;
        
        private static bool isSystemInfoAdded = false;
       
        public static String nevoUrl;
        public String filename = "Scrrenshot_" + DateTime.Now.ToString("h_mm_ss") + ".png";
        public static ConfigSettings config;
        static string configPath = Directory.GetParent(@"../../../").FullName +
             Path.DirectorySeparatorChar + "Configuration/configuration.json";
        //static string configPath = "Configuration/configuration.json";   
        static string reportPath = Directory.GetParent(@"../../../").FullName +
            Path.DirectorySeparatorChar + "Reports/";
        static string sourceFile = reportPath + "index.html";
        private static Dictionary<string, ExtentTest> _suiteTests = new Dictionary<string, ExtentTest>();

        static string destinationFile = reportPath + "TestReport_" + DateTime.Now.ToString("yyyyMMddHHmm") + ".html";
       public static string downloadPath = Directory.GetParent(@"../../../").FullName + Path.DirectorySeparatorChar + "Data" + Path.DirectorySeparatorChar + "Downloads" + Path.DirectorySeparatorChar;
        public static HomePage homepage;


        [OneTimeSetUp]
        public void SetupTest()
        {

            config = new ConfigSettings();
            ConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddJsonFile(configPath, false, true);
            IConfigurationRoot configuration = builder.Build();
            configuration.Bind(config);
           
            nevoUrl = config.NevoUrl;
          
            try
            {
                _extentHtmlReporter = new ExtentHtmlReporter(sourceFile);
                _extent.AttachReporter(_extentHtmlReporter);
                // Add system info only if it hasn't been added yet
                if (!isSystemInfoAdded)
                {
                    _extent.AddSystemInfo("Host Name", "Local Host");
                    _extent.AddSystemInfo("Environment", config.NevoUrl);
                    isSystemInfoAdded = true;
                }
                 }
            catch (Exception e)
            {
                NetLogger.Log(NetLogger.Severity.Exception, "Exception: Unable to initialize test report : " + e.Message);
                throw;
            }

           


            try
            {
                //Set Default Download Directory Configuration
                var chromeOptions = new ChromeOptions();
                chromeOptions.AddArgument("--disable-extensions");
     
                CreateDirectoryIfNotExists(downloadPath);
                chromeOptions.AddUserProfilePreference("download.default_directory", downloadPath);
                
                if (config.headlessMode == true)
                {
                    
                    chromeOptions.AddArguments("headless");
                    chromeOptions.AddArgument("--window-size=1920,1080");
                    chromeOptions.AddArgument("--disable-gpu"); // Additional argument to improve compatibility
                    chromeOptions.AddArgument("--no-sandbox");
                    chromeOptions.AddArgument("--disable-dev-shm-usage");

                     driver = new ChromeDriver(chromeOptions);
               
                }
                else if (config.headlessMode == false)
                {
                    driver = new ChromeDriver(chromeOptions);
                }

                driver.Manage().Window.Maximize();
                driver.Navigate().GoToUrl(nevoUrl);
               homepage=new HomePage(driver);
                homepage.handleCookies("Allow all");
            }



            catch (Exception e)
            {
                NetLogger.Log(NetLogger.Severity.Exception, "Exception: Unable to initialize web driver : " + e.Message);
                throw;
            }
        }
       

        public string GetTimestamp()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmss");
        }



        [SetUp]
        public void baseSetUp()
        {
            // Create a test node under the suite
            var fixtureName = TestContext.CurrentContext.Test.ClassName;
            var testName = TestContext.CurrentContext.Test.Name;

            if (!_suiteTests.ContainsKey(fixtureName))
            {
                var suiteTest = _extent.CreateTest(fixtureName).Info("Starting test fixture: " + fixtureName);
                _suiteTests[fixtureName] = suiteTest;
            }

            _test = _suiteTests[fixtureName].CreateNode(testName).Info("Starting test case: " + testName);
        }
        [TearDown]
        public void BaseTearDown()
        {
            // Print log at the end of a test case
            try
            {
                var status = TestContext.CurrentContext.Result.Outcome.Status;
                var stacktrace = string.IsNullOrEmpty(TestContext.CurrentContext.Result.StackTrace) ? "" : string.Format("<pre>{0}</pre>", TestContext.CurrentContext.Result.StackTrace);
                var errorMsg = TestContext.CurrentContext.Result.Message;
                Status logStatus;

                switch (status)
                {
                    case TestStatus.Failed:
                        logStatus = Status.Fail;
                        _test.Fail("Test Case " + logStatus + stacktrace);
                        break;
                    case TestStatus.Skipped:
                        logStatus = Status.Skip;
                        _test.Skip("Test Case " + logStatus + stacktrace);
                        break;
                    case TestStatus.Passed:
                        logStatus = Status.Pass;
                        _test.Pass("Test Case " + logStatus);
                        break;
                }
            }
            catch (Exception ex)
            {
                NetLogger.Log(NetLogger.Severity.Exception, "Exception in endTest method : " + ex.Message);
            }
        }
        [OneTimeTearDown]
        public void TearDownTest()
        {

            try
            {
                ClearBrowserCache();
                driver.Quit();
                driver.Dispose();
            }
            catch (Exception e)
            {
                NetLogger.Log(NetLogger.Severity.Exception, "Exception in quit driver instance : " + e.Message);
                throw;
            }
            
            try
            {
                NetLogger.Log(NetLogger.Severity.Information, "Copy test report... ");
                _extent.Flush();//
                File.Copy(sourceFile, destinationFile, true);
            }
            catch (Exception e)
            {
                NetLogger.Log(NetLogger.Severity.Exception, "Exception while flush extent report : " + e.Message);
                throw;
            }
        }

        private void ClearBrowserCache()
        {
            try
            {
                driver.Manage().Cookies.DeleteAllCookies(); // Clear cookies
                driver.Navigate().GoToUrl("chrome://settings/clearBrowserData"); // Open the clear browsing data settings
                driver.FindElement(By.CssSelector("* /deep/ #clearBrowsingDataConfirm")).Click(); // Confirm clear browsing data
                Thread.Sleep(5000); // Wait for the cache clearing to complete
            }
            catch (Exception e)
            {
                NetLogger.Log(NetLogger.Severity.Exception, "Exception while clearing browser cache : " + e.Message);
            }
        }

  
        public static MediaEntityModelProvider captureScreenshot(IWebDriver driver, string name)
        {
            ITakesScreenshot ts = (ITakesScreenshot)driver;
            var ss = ts.GetScreenshot().AsBase64EncodedString;
            return MediaEntityBuilder.CreateScreenCaptureFromBase64String(ss, "Scrrenshot_" + DateTime.Now.ToString("h_mm_ss") + ".png").Build();
        }
        public static void captureScreenshotForCI(IWebDriver driver)
        {
            try
            {
                // Construct the file path for the screenshot
                var testDirectory = TestContext.CurrentContext.TestDirectory;
                var testName = TestContext.CurrentContext.Test.MethodName;
                var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                var filePath = Path.Combine(testDirectory, $"{testName}_{timestamp}.jpg");

                // Ensure the directory exists
                Directory.CreateDirectory(testDirectory);

                // Take the screenshot and save it to the specified file path
                var screenshot = ((ITakesScreenshot)driver).GetScreenshot();
                //screenshot.SaveAsFile(filePath, ScreenshotImageFormat.Jpeg);

                // Add the screenshot as an attachment to the NUnit test context
                TestContext.AddTestAttachment(filePath);

                // Log the screenshot path for debugging
                Console.WriteLine($"Screenshot saved to: {filePath}");
            }
            catch (Exception ex)
            {
                // Log the error for debugging
                Console.WriteLine($"Error capturing screenshot: {ex.Message}");
            }
        }

        public void CreateDirectoryIfNotExists(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                Console.WriteLine("Directory created: " + path);
            }
            else
            {
                Console.WriteLine("Directory already exists: " + path);
            }
        }


       
        }
}

