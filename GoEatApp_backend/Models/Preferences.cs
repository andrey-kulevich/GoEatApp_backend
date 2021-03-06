﻿
namespace GoEatapp_backend.Models
{
    public class Preferences
    {
        public int Id { get; set; }
        public string CiusineNationality { get; set; }
        public string Interior { get; set; }
        public int TipsPercentage { get; set; }
        public bool IsVegan { get; set; }
        public bool IsRawFood { get; set; }
        public Dish BestDrink { get; set; }
        public Dish BestFirstMeal { get; set; }
        public Dish BestSecondMeal { get; set; }
        public Dish BestDessert { get; set; }
        public string Other { get; set; }
    }

    public class PreferencesIntFieldUpdate
    {
        public int PreferencesId { get; set; }
        public string FieldToUpdate { get; set; }
        public int IntValue { get; set; }
    }

    public class PreferencesOtherUpate
    {
        public int PreferencesId { get; set; }
        public string OtherValue { get; set; }
    }
}
