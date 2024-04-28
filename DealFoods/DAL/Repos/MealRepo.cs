using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repos
{
    public class MealRepo
    {
        static MealContext mealContext;
        static MealRepo()
        {
            mealContext = new MealContext();
        }
        public static List<Meal> Get()
        {
            return mealContext.Meals.ToList();
        }
        public static Meal Get(int id) { return mealContext.Meals.Find(id); }
        public static bool Create(Meal meal) 
        { 
            mealContext.Meals.Add(meal);
            return mealContext.SaveChanges() > 0;
        
        }
        public static bool Update(Meal meal) 
        {
            var exmeal = mealContext.Meals.Find(meal.Id);
            mealContext.Entry(exmeal).CurrentValues.SetValues(meal);
            return mealContext.SaveChanges() > 0;
        }
        public static bool Delete(int id) 
        {
            var exmeal = Get(id);
            mealContext.Meals.Remove(exmeal);
            return mealContext.SaveChanges() > 0;

        
        }


    }
}
