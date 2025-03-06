using Nevo.Base;
using Nevo.PageObject;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;

namespace Nevo.Test
{
    [TestFixture]
    public class NevoWebsiteTests : BaseClass
    {

        BrowseVehicles browseVehicles;
        JourneyPlanner journeyPlanner;
        HelpAndAdvice helpAndAdvice;
        

        [Test, Order(1)]
        public void TC001_launch_Nevo_Site_And_Verify_The_Title()
        {
            // Verify the page title
            string expectedTitle = "Nevo";
            string actualTitle = driver.Title;
            verification.validatePageTitle(driver, actualTitle, "Expected Title matches with Actual Title");
        }

       [Test, Order(2)]
        public void TC002_click_BrowseVehicles_And_Verify_the_page_is_navigated_correctly()
        {
            browseVehicles = new BrowseVehicles(driver);
            homepage.selectNavigationHeader("Browse Vehicles");
            String head=browseVehicles.browseVehiclesHeader();
            verification.validateTwoStringsAreEqual(head, "It's time. Let's Go Electric.", "Header Matches with Browser Vehicles page");
           
        }

      [Test, Order(3)]
        public void TC003_verify_the_count_of_Both_BEV_PHEV()
        {
            browseVehicles = new BrowseVehicles(driver);
            homepage.selectNavigationHeader("Browse Vehicles");
            int count=browseVehicles.getResultCount();
            Assert.AreEqual(count, 201 , "The two numbers are not equal!");
            
        }

        [Test, Order(4)]
        public void  TC004_verify_journey_details_isDisplayed_for_valid_start_point_and_end_point()
        {
           
            journeyPlanner = new JourneyPlanner(driver);
            journeyPlanner.planJourneyPage();    

        }

        [Test, Order(5)]
        public void TC005_verify_User_Is_Navigated_To_Help_And_Advice()
        {
            helpAndAdvice = new HelpAndAdvice(driver);
            homepage.selectNavigationHeader("Help & Advice");
            String headerName = helpAndAdvice.helpAndAdvice();
            verification.validateTwoStringsAreEqual(headerName, "Help & Advice","User is navigated to Help and Advice page successfully");
        }



        [OneTimeTearDown]
        public void TearDown()
        {
            if (driver != null)
            {
                driver.Quit();  // Closes all browser windows and safely ends the session
                driver.Dispose(); // Releases WebDriver resources
            }
        }
    }
}