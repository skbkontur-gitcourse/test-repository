using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools.V133.Input;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace first_test;

public class Tests
{
  public IWebDriver driver;
  public WebDriverWait wait;
    [SetUp]
    public void Setup()
    {
      driver = new  ChromeDriver();
      driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
      wait = new WebDriverWait(driver, TimeSpan.FromSeconds(3));
    }

    [TearDown]
    public void TeartDown()
    {
      driver.Quit();
      driver.Dispose();
    }

    private void Authorize()
    {
        driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru/");
        //ввести логин
        var login = driver.FindElement(By.Id("Username"));
        login.SendKeys("slovesnov.v@skbkontur.ru");
        // ввести пароль
        var password = driver.FindElement(By.Id("Password"));
        password.SendKeys("dF3qn!ZZgEBgVDu");
        //нажать кнопку "Войти"
        var enter=driver.FindElement(By.Name("button"));
        enter.Click();

        wait.Until(ExpectedConditions.UrlToBe("https://staff-testing.testkontur.ru/news"));
    }

    [Test]
  public void AuthorizationTest()
  {
    Authorize();
    Assert.That(driver.Url.Equals("https://staff-testing.testkontur.ru/news"), "Некорректный url");
      }

    [Test]
    public void CorrectLogOutFromBurgerMenu()
    {
       Authorize();
        wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("[data-tid='Title']")));
        var enter=driver.FindElement(By.CssSelector("[data-tid='SidebarMenuButton']"));
        enter.Click();
        wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("[data-tid='SidePageBody'] [data-tid='LogoutButton']")));
        enter=driver.FindElement(By.CssSelector("[data-tid='SidePageBody'] [data-tid='LogoutButton']"));
        enter.Click();
        Assert.That(driver.Url.Contains("https://staff-testing.testkontur.ru/Account/Logout"), "Не тот URL");
    }
    [Test]
    public void Second_Email_Change()
    {
      Authorize();
      driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru/profile/settings/edit");
      var enter=driver.FindElement(By.CssSelector("[data-tid='AdditionalEmail'] [data-tid='Input']"));
      enter.SendKeys(Keys.Control + 'a');
      enter.SendKeys(Keys.Backspace);
      enter.SendKeys("AdditionalDog@mail.ru");
      enter=driver.FindElement(By.CssSelector("[class='sc-eYWepU klKumv'][class='sc-eYWepU klKumv']"));
      enter.Click();
      var ContactInformation = driver.FindElement(By.CssSelector("[data-tid='ContactCard']"));
      Assert.That(ContactInformation.Text, Does.Contain("AdditionalDog@mail.ru"), "Почта не изменилась");
    }

    [Test]

    public void FIleSearch()
    {
      Authorize();
      driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru/files");
      var enter=driver.FindElement(By.CssSelector("[data-tid='Search']"));
      enter.Click();
      enter=driver.FindElement(By.CssSelector("[placeholder='Введите название файла или папки']"));
      enter.Click();
      enter.SendKeys("Диск админа для гусевой");
      var folders = driver.FindElement(By.CssSelector("[data-tid='Folders']"));
      Assert.That(folders.Text, Does.Contain("ДИСК"), "файл не найден");
    }
    [Test]

   public void CreateFolder()
   {
    Authorize();
    driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru/files");
    var enter=driver.FindElement(By.CssSelector("[data-tid='DropdownButton'] [class='react-ui-1jhn7yl']"));
    enter.Click();
    enter=driver.FindElement(By.CssSelector("[data-tid='CreateFolder']"));
    enter.Click();
    enter=driver.FindElement(By.CssSelector("[data-tid='Input']"));
    enter.Click();
    enter.SendKeys("NewTestFolder");
    enter=driver.FindElement(By.CssSelector("[data-tid='SaveButton']"));
    enter.Click();
    var folders = driver.FindElement(By.CssSelector("[data-tid='Folders']"));
    Assert.That(folders.Text, Does.Contain("NewTestFolder"), "Папка не найдена");  
   }

}