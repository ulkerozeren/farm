using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using testfarmweb.Models;
using Microsoft.Extensions.Configuration;
using System.Threading;

namespace testfarmweb.Controllers
{
    public class HomeController : Controller
    {
        public delegate void delegate1();
        public event delegate1 doit;

        private readonly IConfiguration _configuration;
        private string AnimalType = null;
        private string GestationPeriod = null;
        private string LifeExpectancy = null;
        private string ChildhoodTimeForFemale = null;
        private string ChildhoodTimeForMale = null;
        private string AnnualBirths = null;
        private string FemaleNumberOfPuppies = null;
        private string MaleNumberOfPuppies = null;
        private string StartDate = null;
        private int femaleAnimalNumber = 1;
        private int maleAnimalNumber = 1;
        int intGestationPeriod = 0;
        int currentMonth = 1;

        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
            AnimalType = _configuration.GetSection("FarmSettings").GetSection("AnimalType").Value;
            GestationPeriod = _configuration.GetSection("FarmSettings").GetSection("GestationPeriod").Value;
            LifeExpectancy = _configuration.GetSection("FarmSettings").GetSection("LifeExpectancy").Value;
            ChildhoodTimeForFemale = _configuration.GetSection("FarmSettings").GetSection("ChildhoodTimeForFemale").Value;
            ChildhoodTimeForMale = _configuration.GetSection("FarmSettings").GetSection("ChildhoodTimeForMale").Value;
            AnnualBirths = _configuration.GetSection("FarmSettings").GetSection("AnnualBirths").Value;
            FemaleNumberOfPuppies = _configuration.GetSection("FarmSettings").GetSection("FemaleNumberOfPuppies").Value;
            MaleNumberOfPuppies = _configuration.GetSection("FarmSettings").GetSection("MaleNumberOfPuppies").Value;
            StartDate = _configuration.GetSection("FarmSettings").GetSection("StartDate").Value;
            intGestationPeriod = Convert.ToInt32(GestationPeriod);
        }

        public IActionResult Index()
        {           
            string[] words = StartDate.Split('/');

            string day =   words[0];
            string month = words[1];
            string year =  words[2];

            int passedMonth = (DateTime.Now.Year - Convert.ToInt32(year)) * 12 +Convert.ToInt32(month);//şu ana kadar geçen ay 
            int passedYear = passedMonth / 12;                   

            int femaleAdultAnimalNumber = 1;
          
            List<ResultViewModel> results = new List<ResultViewModel>();

            //her gebelik bir thread
            while (currentMonth<= passedMonth)
            {
                //doğum yapabilir durumdaki dişilerin sayısı kadar thread yarat
                for (int i = 1; i <= femaleAdultAnimalNumber; i++)
                {
                    doit += new delegate1(GiveBirth);
                    femaleAnimalNumber = femaleAnimalNumber + Convert.ToInt32(FemaleNumberOfPuppies);
                    maleAnimalNumber = maleAnimalNumber + Convert.ToInt32(MaleNumberOfPuppies);
                }

                femaleAdultAnimalNumber = femaleAnimalNumber; //doğumdan sonra her dişi birey erişkinliğe ulaşınca femaleAdultAnimalNumber sayısını arttırmak gerek.
                                                               // ama bunu hesaplayamadım


                ResultViewModel resultViewModel = new ResultViewModel()
                {
                     NumberOfFemale=femaleAnimalNumber,
                     NumberOfMale=maleAnimalNumber,
                     NumberOfMonth= currentMonth
                };

                currentMonth++;
                results.Add(resultViewModel);

            }
          
            return View("Index",results);
        }

        public void GiveBirth()
        {
            Thread.Sleep(100); //gün*saniye şeklinde simule ediyorum.
        }
    }
}
