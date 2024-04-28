using DAL.Models;
using DAL.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class MealService
    {
        public static object Get()
        {
           return MealRepo.Get();
        }
        public static Meal Get(int id)
        {
            return MealRepo.Get(id);
        }
        public static bool Create(Meal meal)
        {
            return MealRepo.Create(meal);
        }
        public static bool Update(Meal meal)
        {
            return MealRepo.Update(meal);
        }
        public static bool Delete(int id)
        {
            return MealRepo.Delete(id);
        }
    }
}
